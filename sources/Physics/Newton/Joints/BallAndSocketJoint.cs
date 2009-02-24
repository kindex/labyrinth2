using System;

namespace Game.Physics.Newton.Joints
{
    public sealed class BallAndSocketJoint : CustomJoint
    {
        private const float MIN_JOINT_PIN_LENGTH = 50.0f;

        public BallAndSocketJoint(Matrix4 pinsAndPivotFrame, Body child)
            : this(pinsAndPivotFrame, child, null)
        {
        }

        public BallAndSocketJoint(Matrix4 pinsAndPivotFrame, Body child, Body parent)
            : base(6, child, parent)
        {
            CalculateLocalMatrix(pinsAndPivotFrame, out localMatrix0, out localMatrix1);
        }

        protected override void GetInfo(ref JointRecord info)
        {
            info.DescriptionType = "ballsocket";

            info.AttachedBody0 = Body0;
            info.AttachedBody1 = Body1;

            info.MinLinearDof = Vector3.Zero;
            info.MaxLinearDof = Vector3.Zero;

            info.MinAngularDof = new Vector3(-float.MaxValue, -float.MaxValue, -float.MaxValue);
            info.MaxAngularDof = new Vector3(+float.MaxValue, -float.MinValue, -float.MinValue);

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
        }

        Matrix4 localMatrix0;
        Matrix4 localMatrix1;
    }
}
