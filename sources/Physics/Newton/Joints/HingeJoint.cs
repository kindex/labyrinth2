using System;

namespace Game.Physics.Newton.Joints
{
    public sealed class HingeJoint : CustomJoint
    {
        private const float MIN_JOINT_PIN_LENGTH = 50.0f;

        public HingeJoint(Matrix4 pinsAndPivotFrame, Body child)
            : this(pinsAndPivotFrame, child, null)
        {
        }

        public HingeJoint(Matrix4 pinsAndPivotFrame, Body child, Body parent)
            : base(6, child, parent)
        {
            LimitsOn = false;
            minAngle = Degrees.ToRadians(-45.0f);
            maxAngle = Degrees.ToRadians(45.0f);

            CalculateLocalMatrix(pinsAndPivotFrame, out localMatrix0, out localMatrix1);
        }

        public bool LimitsOn { get; set; }

        public void SetLimits(float minAngle, float maxAngle)
        {
            this.minAngle = minAngle;
            this.maxAngle = maxAngle;
        }

        protected override void GetInfo(ref JointRecord info)
        {
            info.DescriptionType = "hinge";

            info.AttachedBody0 = Body0;
            info.AttachedBody1 = Body1;

            info.MinLinearDof = Vector3.Zero;
            info.MaxLinearDof = Vector3.Zero;

            if (LimitsOn)
            {
                Matrix4 matrix0;
                Matrix4 matrix1;
                CalculateGlobalMatrix(localMatrix0, localMatrix1, out matrix0, out matrix1);
                
                float sinAngle = Vector3.Dot(Vector3.Cross(matrix0.Up, matrix1.Up), matrix0.Front);
                float cosAngle = Vector3.Dot(matrix0.Up, matrix1.Up);
                float angle = (float)Math.Atan2(sinAngle, cosAngle);
                
                info.MinAngularDof = new Vector3(Degrees.ToRadians(minAngle - angle), 0.0f, 0.0f);
                info.MaxAngularDof = new Vector3(Degrees.ToRadians(maxAngle - angle), 0.0f, 0.0f);
            }
            else
            {
                info.MinAngularDof = new Vector3(-float.MaxValue, 0.0f, 0.0f);
                info.MaxAngularDof = new Vector3(+float.MaxValue, 0.0f, 0.0f);
            }

            info.AttachmentMatrix0 = localMatrix0;
            info.AttachmentMatrix1 = localMatrix1;
        }

        protected override void SubmitConstraint(float timestep, int threadIndex)
        {
            Matrix4 matrix0;
            Matrix4 matrix1;

            // calculate the position of the pivot point and the Jacobian direction vectors, in global space. 
            CalculateGlobalMatrix(localMatrix0, localMatrix1, out matrix0, out matrix1);

            // Restrict the movement on the pivot point along all tree orthonormal direction
            AddLinearRow(matrix0.Posit, matrix1.Posit, matrix0.Front);
            AddLinearRow(matrix0.Posit, matrix1.Posit, matrix0.Up);
            AddLinearRow(matrix0.Posit, matrix1.Posit, matrix0.Right);
	
            // get a point along the pin axis at some reasonable large distance from the pivot
            Vector3 q0 = matrix0.Posit + matrix0.Front * MIN_JOINT_PIN_LENGTH;
            Vector3 q1 = matrix1.Posit + new Vector3(matrix1.Row2) * MIN_JOINT_PIN_LENGTH;

            // two constraints row perpendicular to the pin vector
            AddLinearRow(q0, q1, matrix0.Up);
            AddLinearRow(q0, q1, matrix0.Right);

            // if limit are enable ...
	        if (LimitsOn)
            {
		        // the joint angle can be determine by getting the angle between any two non parallel vectors
                float sinAngle = Vector3.Dot(Vector3.Cross(matrix0.Up, matrix1.Up), matrix0.Front);
                float cosAngle = Vector3.Dot(matrix0.Up, matrix1.Up);
                float angle = (float)Math.Atan2(sinAngle, cosAngle);
					
		        if (angle < minAngle)
                {
                    float relAngle = angle - minAngle;

                    // tell joint error will minimize the exceeded angle error
                    AddAngularRow(relAngle, matrix0.Front);

			        // need high stiffness here
			        SetRowStiffness(1.0f);

			        // allow the joint to move back freely 
			        SetRowMaximumFriction(0.0f);
                }
                else if (angle > maxAngle)
                {
			        float relAngle = angle - maxAngle;
			
                    // tell joint error will minimize the exceeded angle error
                    AddAngularRow(relAngle, matrix0.Front);

			        // need high stiffness here
			        SetRowStiffness(1.0f);

			        // allow the joint to move back freely
			        SetRowMinimumFriction(0.0f);
		        }
            }
        }

        Matrix4 localMatrix0;
        Matrix4 localMatrix1;

        float minAngle;
        float maxAngle;
    }
}
