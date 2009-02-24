using System;
using System.Runtime.InteropServices;

using Game.Physics.Newton;

namespace Game.Physics.Newton
{
    public struct JointRecord
    {
        public Matrix4 AttachmentMatrix0;
        public Matrix4 AttachmentMatrix1;
        public Vector3 MinLinearDof;
        public Vector3 MaxLinearDof;
        public Vector3 MinAngularDof;
        public Vector3 MaxAngularDof;
        public Body AttachedBody0;
        public Body AttachedBody1;
        //public float[] ExtraParameters;
        public int BodiesCollisionOn;
        public string DescriptionType;
    }

    public abstract class CustomJoint : NewtonObject
    {
        internal static CustomJoint FromNewtonJoint(IntPtr newtonJoint)
        {
            return (CustomJoint)GCHandle.FromIntPtr(NativeAPI.JointGetUserData(newtonJoint)).Target;
        }

        public CustomJoint(int maxDOF, Body body0, Body body1)
            : base(NativeAPI.ConstraintCreateUserJoint(body0.World.handle, maxDOF, NativeSubmitConstraint, NativeGetInfo, body0.handle, (body1 == null ? IntPtr.Zero : body1.handle)))
        {
            this.Body0 = body0;
            this.Body1 = body1;
            this.world = body0.World;

            IntPtr ptr = GCHandle.ToIntPtr(GCHandle.Alloc(this));
            NativeAPI.JointSetUserData(handle, ptr);

            NativeAPI.JointSetDestructor(handle, NativeDestructorHandler);
        }

        protected sealed override void ReleaseHandle()
        {
            NativeAPI.JointSetDestructor(handle, null);
            NativeAPI.DestroyJoint(world.handle, handle);
        }

        public int CollisionState
        {
            get
            {
                return NativeAPI.JointGetCollisionState(handle);
            }
            set
            {
                NativeAPI.JointSetCollisionState(handle, value);
            }
        }

        public Body Body0 { get; private set; }
        public Body Body1 { get; private set; }

        protected abstract void GetInfo(ref JointRecord info);
        protected abstract void SubmitConstraint(float timestep, int threadIndex);

        protected World world;

        protected void CalculateGlobalMatrix(Matrix4 localMatrix0, Matrix4 localMatrix1, out Matrix4 matrix0, out Matrix4 matrix1)
        {
            Matrix4 body0matrix = Body0.Matrix;

            matrix0 = localMatrix0 * body0matrix;
            if (Body1 == null)
            {
                matrix1 = localMatrix1;
            }
            else
            {
                matrix1 = localMatrix1 * Body1.Matrix;
            }
        }

        protected void CalculateLocalMatrix(Matrix4 pinsAndPivotFrame, out Matrix4 localMatrix0, out Matrix4 localMatrix1)
        {
            Matrix4 matrix0 = Body0.Matrix;

            localMatrix0 = pinsAndPivotFrame * Matrix4.InverseTransform(matrix0);

            if (Body1 == null)
            {
                localMatrix1 = pinsAndPivotFrame;
            }
            else
            {
                localMatrix1 = pinsAndPivotFrame * Matrix4.InverseTransform(Body1.Matrix);
            }
        }



        protected void AddAngularRow(float relativeAngle, Vector3 dir)
        {
            NativeAPI.UserJointAddAngularRow(handle, relativeAngle, ref dir);
        }

        protected void AddLinearRow(Vector3 pivot0, Vector3 pivot1, Vector3 dir)
        {
            NativeAPI.UserJointAddLinearRow(handle, ref pivot0, ref pivot1, ref dir);
        }

        protected void SetRowAcceleration(float acceleration)
        {
            NativeAPI.UserJointSetRowAcceleration(handle, acceleration);
        }

        protected void SetRowStiffness(float stiffness)
        {
            NativeAPI.UserJointSetRowStiffness(handle, stiffness);
        }

        protected void SetRowMinimumFriction(float friction)
        {
            NativeAPI.UserJointSetRowMinimumFriction(handle, friction);
        }

        protected void SetRowMaximumFriction(float friction)
        {
            NativeAPI.UserJointSetRowMaximumFriction(handle, friction);
        }

        protected void AddGeneralRow(Vector3[] jacobian0, Vector3[] jacobian1)
        {
            System.Diagnostics.Debug.Assert(jacobian0.Length == 2 && jacobian1.Length == 2);
            NativeAPI.UserJointAddGeneralRow(handle, jacobian0, jacobian1);
        }

        static void DestructorHandler(IntPtr newtonJoint)
        {
            IntPtr userData = NativeAPI.JointGetUserData(newtonJoint);
            CustomJoint joint = (CustomJoint)GCHandle.FromIntPtr(userData).Target;
            GCHandle.FromIntPtr(userData).Free();

            joint.handle = IntPtr.Zero;
        }

        static void GetInfo(IntPtr userJoint, out NativeAPI.NewtonJointRecordTag info)
        {
            CustomJoint joint = FromNewtonJoint(userJoint);

            JointRecord infoRecord = new JointRecord();
            joint.GetInfo(ref infoRecord);

            info = new NativeAPI.NewtonJointRecordTag();
            info.m_attachmentMatrix_0 = infoRecord.AttachmentMatrix0;
            info.m_attachmentMatrix_1 = infoRecord.AttachmentMatrix1;
            info.m_minLinearDof = infoRecord.MinLinearDof;
            info.m_maxLinearDof = infoRecord.MinLinearDof;
            info.m_minAngularDof = infoRecord.MinAngularDof;
            info.m_maxAngularDof = infoRecord.MaxAngularDof;
            info.m_attachBody_0 = infoRecord.AttachedBody0.handle;
            info.m_attachBody_1 = (infoRecord.AttachedBody1 == null ? IntPtr.Zero : infoRecord.AttachedBody1.handle);
            //info.m_extraParameters;
            info.m_bodiesCollisionOn = infoRecord.BodiesCollisionOn;
            info.m_descriptionType = infoRecord.DescriptionType;
        }

        static void SubmitConstraint(IntPtr userJoint, float timestep, int threadIndex)
        {
            CustomJoint joint = FromNewtonJoint(userJoint);
            joint.SubmitConstraint(timestep, threadIndex);
        }

        static NativeAPI.NewtonConstraintDestructor NativeDestructorHandler = new NativeAPI.NewtonConstraintDestructor(DestructorHandler);
        static NativeAPI.NewtonUserBilateralGetInfoCallBack NativeGetInfo = new NativeAPI.NewtonUserBilateralGetInfoCallBack(GetInfo);
        static NativeAPI.NewtonUserBilateralCallBack NativeSubmitConstraint = new NativeAPI.NewtonUserBilateralCallBack(SubmitConstraint);
    }
}
