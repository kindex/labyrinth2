using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Game.Physics.Newton
{
    public delegate void ContactJointIterator(ContactJoint contactJoint);
    public delegate void SetForceAndTorqueEventHandler(Body body, float timestep, int threadIndex);
    public delegate void SetTransformEventHandler(Body body, Matrix4 matrix, int threadIndex);

    public sealed class Body : NewtonObject
    {
        internal static Body FromNewtonBody(IntPtr newtonBody)
        {
            return (Body)GCHandle.FromIntPtr(NativeAPI.BodyGetUserData(newtonBody)).Target;
        }

        public Body(World world, Collision collision)
            : base(NativeAPI.CreateBody(world.handle, collision.handle))
        {
            this.World = world;
            this.collision = collision;

            IntPtr ptr = GCHandle.ToIntPtr(GCHandle.Alloc(this));
            NativeAPI.BodySetUserData(handle, ptr);

            NativeAPI.BodySetDestructorCallback(handle, NativeDestructorHandler);
        }

        static void DestructorHandler(IntPtr newtonBody)
        {
            IntPtr userData = NativeAPI.BodyGetUserData(newtonBody);
            Body body = (Body)GCHandle.FromIntPtr(userData).Target;
            GCHandle.FromIntPtr(userData).Free();

            body.handle = IntPtr.Zero;
        }

        protected override void ReleaseHandle()
        {
            NativeAPI.BodySetDestructorCallback(handle, null);
            NativeAPI.DestroyBody(World.handle, handle);
        }

        public void AddForce(Vector3 force)
        {
            NativeAPI.BodyAddForce(handle, ref force);
        }

        public void AddTorque(Vector3 torque)
        {
            NativeAPI.BodyAddTorque(handle, ref torque);
        }

        public void AddImpulse(Vector3 pointDeltaVelocity, Vector3 pointPosition)
        {
            NativeAPI.BodyAddImpulse(handle, ref pointDeltaVelocity, ref pointPosition);
        }

        public Vector3 CalculateInverseDynamicsForce(float timestep, Vector3 desiredVelocity)
        {
            Vector3 result;
            NativeAPI.BodyCalculateInverseDynamicsForce(handle, timestep, ref desiredVelocity, out result);
            return result;
        }

        public void SetMatrixRecursive(Matrix4 matrix)
        {
            NativeAPI.BodySetMatrixRecursive(handle, ref matrix);
        }

        public void SetMass(float mass, Vector3 inertia)
        {
            NativeAPI.BodySetMassMatrix(handle, mass, inertia.X, inertia.Y, inertia.Z);
        }

        public void GetMass(out float mass, out Vector3 inertia)
        {
            NativeAPI.BodyGetMassMatrix(handle, out mass, out inertia.X, out inertia.Y, out inertia.Z);
        }

        public void GetInvMass(out float invMass, out Vector3 invInertia)
        {
            NativeAPI.BodyGetInvMass(handle, out invMass, out invInertia.X, out invInertia.Y, out invInertia.Z);
        }

        public void GetAABB(out Vector3 p0, out Vector3 p1)
        {
            NativeAPI.BodyGetAABB(handle, out p0, out p1);
        }

        public IEnumerable<ContactJoint> ContactJoints
        {
            get
            {
                IntPtr newtonContactJoint = NativeAPI.BodyGetFirstContactJoint(handle);
                while (newtonContactJoint != IntPtr.Zero)
                {
                    yield return new ContactJoint(newtonContactJoint);
                    newtonContactJoint = NativeAPI.BodyGetNextContactJoint(handle, newtonContactJoint);
                }
            }
        }

        public Matrix4 Matrix
        {
            get
            {
                Matrix4 matrix;
                NativeAPI.BodyGetMatrix(handle, out matrix);
                return matrix;
            }
            set
            {
                NativeAPI.BodySetMatrix(handle, ref value);
            }
        }

        public Quaternion Rotation
        {
            get
            {
                Quaternion q;
                NativeAPI.BodyGetRotation(handle, out q);
                return q;
            }
        }

        public int MaterialGroupID
        {
            get
            {
                return NativeAPI.BodyGetMaterialGroupID(handle);
            }
            set
            {
                NativeAPI.BodySetMaterialGroupID(handle, value);
            }
        }

        public bool ContinuousCollision
        {
            get
            {
                return NativeAPI.BodyGetContinuousCollisionMode(handle) != 0;
            }
            set
            {
                NativeAPI.BodySetContinuousCollisionMode(handle, (uint)(value ? 1 : 0));
            }
        }

        public bool JointRecursiveCollision
        {
            get
            {
                return NativeAPI.BodyGetJointRecursiveCollision(handle) != 0;
            }
            set
            {
                NativeAPI.BodySetJointRecursiveCollision(handle, (uint)(value ? 1 : 0));
            }
        }

        public Vector3 Omega
        {
            get
            {
                Vector3 result;
                NativeAPI.BodyGetOmega(handle, out result);
                return result;
            }
            set
            {
                NativeAPI.BodySetOmega(handle, ref value);
            }
        }

        public Vector3 Velocity
        {
            get
            {
                Vector3 result;
                NativeAPI.BodyGetVelocity(handle, out result);
                return result;
            }
            set
            {
                NativeAPI.BodySetVelocity(handle, ref value);
            }
        }

        public Vector3 Force
        {
            get
            {
                Vector3 result;
                NativeAPI.BodyGetForce(handle, out result);
                return result;
            }
            set
            {
                NativeAPI.BodySetForce(handle, ref value);
            }
        }

        public Vector3 Torque
        {
            get
            {
                Vector3 result;
                NativeAPI.BodyGetTorque(handle, out result);
                return result;
            }
            set
            {
                NativeAPI.BodySetTorque(handle, ref value);
            }
        }

        public Vector3 CentreOfMass
        {
            get
            {
                Vector3 result;
                NativeAPI.BodyGetCentreOfMass(handle, out result);
                return result;
            }
            set
            {
                NativeAPI.BodySetCentreOfMass(handle, ref value);
            }
        }

        public float LinearDamping
        {
            get
            {
                return NativeAPI.BodyGetLinearDamping(handle);
            }
            set
            {
                NativeAPI.BodySetLinearDamping(handle, value);
            }
        }

        public Vector3 AngularDamping
        {
            get
            {
                Vector3 result;
                NativeAPI.BodyGetAngularDamping(handle, out result);
                return result;
            }
            set
            {
                NativeAPI.BodySetAngularDamping(handle, ref value);
            }
        }

        public Collision Collision
        {
            get
            {
                return collision;
            }
            set
            {
                this.collision = value;
                NativeAPI.BodySetCollision(handle, value.handle);
            }
        }

        public bool Sleeping
        {
            get
            {
                return NativeAPI.BodyGetSleepState(handle) != 0;
            }
        }

        public bool AutoSleep
        {
            get
            {
                return NativeAPI.BodyGetAutoSleep(handle) != 0;
            }
            set
            {
                NativeAPI.BodySetAutoSleep(handle, value ? 1 : 0);
            }
        }

        public bool Freezed
        {
            get
            {
                return NativeAPI.BodyGetFreezeState(handle) != 0;
            }
            set
            {
                NativeAPI.BodySetFreezeState(handle, value ? 1 : 0);
            }
        }

        public Vector3 ForceAcc
        {
            get
            {
                Vector3 result;
                NativeAPI.BodyGetForceAcc(handle, out result);
                return result;
            }
        }

        public Vector3 TorqueAcc
        {
            get
            {
                Vector3 result;
                NativeAPI.BodyGetTorqueAcc(handle, out result);
                return result;
            }
        }

        public World World { get; private set; }
       
        public event SetForceAndTorqueEventHandler SetForceAndTorqueEvent
        {
            add
            {
                if (SetForceAndTorque == null)
                {
                    NativeAPI.BodySetForceAndTorqueCallback(handle, NativeSetForceAndTorqueHandler);
                }

                SetForceAndTorque += value;
            }
            remove
            {
                SetForceAndTorque -= value;

                if (SetForceAndTorque == null)
                {
                    NativeAPI.BodySetForceAndTorqueCallback(handle, null);
                }
            }
        }

        public event SetTransformEventHandler SetTransformEvent
        {
            add
            {
                if (SetTransform == null)
                {
                    NativeAPI.BodySetTransformCallback(handle, NativeSetTransformHandler);
                }

                SetTransform += value;
            }
            remove
            {
                SetTransform -= value;

                if (SetTransform == null)
                {
                    NativeAPI.BodySetTransformCallback(handle, null);
                }
            }
        }

        event SetForceAndTorqueEventHandler SetForceAndTorque;
        event SetTransformEventHandler SetTransform;

        static void SetForceAndTorqueHandler(IntPtr newtonBody, float timestep, int threadIndex)
        {
            Body body = FromNewtonBody(newtonBody);
            body.SetForceAndTorque(body, timestep, threadIndex);
        }

        static void SetTransformHandler(IntPtr newtonBody, ref Matrix4 matrix, int threadIndex)
        {
            Body body = FromNewtonBody(newtonBody);
            body.SetTransform(body, matrix, threadIndex);
        }

        static NativeAPI.NewtonApplyForceAndTorque NativeSetForceAndTorqueHandler = new NativeAPI.NewtonApplyForceAndTorque(SetForceAndTorqueHandler);
        static NativeAPI.NewtonSetTransform NativeSetTransformHandler = new NativeAPI.NewtonSetTransform(SetTransformHandler);
        static NativeAPI.NewtonBodyDestructor NativeDestructorHandler = new NativeAPI.NewtonBodyDestructor(DestructorHandler);

        Collision collision;
    }
}
