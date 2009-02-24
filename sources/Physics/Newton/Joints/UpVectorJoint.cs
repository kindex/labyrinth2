using System;

namespace Game.Physics.Newton.Joints
{
    public sealed class UpVectorJoint : CustomJoint
    {
        public UpVectorJoint(Vector3 pin, Body child) : base(2, child, null)
        {
            Matrix4 pivot = child.Matrix;

            Matrix4 matrix = Matrix4.GrammSchmidt(pin);
            matrix.Row3 = pivot.Row3;

	        CalculateLocalMatrix(matrix, out localMatrix0, out localMatrix1);
        }

        public void SetPinDir(Vector3 pin)
        {
            localMatrix1 = Matrix4.GrammSchmidt(pin);
        }

        protected override void GetInfo(ref JointRecord info)
        {
            info.DescriptionType = "upVector";

            info.AttachedBody0 = Body0;
            info.AttachedBody1 = Body1;

            info.MinLinearDof = new Vector3(-float.MaxValue, -float.MaxValue, -float.MaxValue);
            info.MaxLinearDof = new Vector3(+float.MaxValue, +float.MaxValue, +float.MaxValue);

            info.MinAngularDof = new Vector3(-float.MaxValue, -float.MaxValue, -float.MaxValue);
            info.MaxAngularDof = new Vector3(+float.MaxValue, +float.MaxValue, +float.MaxValue);

            info.BodiesCollisionOn = 1;

            info.AttachmentMatrix0 = info.AttachmentMatrix1 = localMatrix0;
        }

        protected override void SubmitConstraint(float timestep, int threadIndex)
        {
	        Matrix4 matrix0;
	        Matrix4 matrix1;

	        // calculate the position of the pivot point and the Jacobian direction vectors, in global space. 
	        CalculateGlobalMatrix(localMatrix0, localMatrix1, out matrix0, out matrix1);
          
	        // if the body ha rotated by some amount, the there will be a plane of rotation

            Vector3 lateralDir = Vector3.Cross(matrix0.Front, matrix1.Front);
	        float mag = lateralDir.Length2;
	        if (mag > 1.0e-6f)
            {
		        // if the side vector is not zero, it means the body has rotated
		        mag = (float)Math.Sqrt(mag);
		        lateralDir = lateralDir * (1.0f / mag);
		        
                float angle = (float)Math.Asin(mag);

		        // add an angular constraint to correct the error angle
		        AddAngularRow(angle, lateralDir);

		        // in theory only one correction is needed, but this produces instability as the body may move sideway.
		        // a lateral correction prevent this from happening.
                AddAngularRow(0.0f, Vector3.Cross(lateralDir, matrix1.Front));
 	        }
            else
            {
                AddAngularRow(0.0f, matrix0.Up);
                AddAngularRow(0.0f, matrix0.Right);
            }
        }

        Matrix4 localMatrix0;
	    Matrix4 localMatrix1;
    }
}
