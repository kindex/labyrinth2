using System;

namespace Game.Physics.Newton
{
    public struct MaterialCollision
    {
        internal MaterialCollision(IntPtr handle)
        {
            this.handle = handle;
        }

        public uint GetBodyCollisionID(Body body)
        {
            return NativeAPI.MaterialGetBodyCollisionID(handle, body.handle);
        }

        public void GetContactPositionAndNormal(out Vector3 position, out Vector3 normal)
        {
            NativeAPI.MaterialGetContactPositionAndNormal(handle, out position, out normal);
        }

        public void GetContactTangentDirections(out Vector3 direction0, out Vector3 direction1)
        {
            NativeAPI.MaterialGetContactTangentDirections(handle, out direction0, out direction1);
        }

        public float GetContactTangentSpeed(int index)
        {
            if (index != 0 && index != 1)
            {
                throw new ArgumentOutOfRangeException("index");
            }
            return NativeAPI.MaterialGetContactTangentSpeed(handle, index);
        }

        public void SetContactSoftness(float softness)
        {
            NativeAPI.MaterialSetContactSoftness(handle, softness);
        }

        public void SetContactElasticity(float restitution)
        {
            NativeAPI.MaterialSetContactElasticity(handle, restitution);
        }

        public void SetContactFrictionState(int state, int index)
        {
            if (index != 0 && index != 1)
            {
                throw new ArgumentOutOfRangeException("index");
            }
            NativeAPI.MaterialSetContactFrictionState(handle, state, index);
        }

        public void SetContactFrictionCoef(float staticFrictionCoef, float kineticFrictionCoef, int index)
        {
            if (index != 0 && index != 1)
            {
                throw new ArgumentOutOfRangeException("index");
            }
            NativeAPI.MaterialSetContactFrictionCoef(handle, staticFrictionCoef, kineticFrictionCoef, index);
        }

        public void SetContactNormalAcceleration(float acceleration)
        {
            NativeAPI.MaterialSetContactNormalAcceleration(handle, acceleration);
        }

        public void SetContactNormalDirection(Vector3 direction)
        {
            NativeAPI.MaterialSetContactNormalDirection(handle, ref direction);
        }

        public void SetContactTangentAcceleration(float acceleration, int index)
        {
            if (index != 0 && index != 1)
            {
                throw new ArgumentOutOfRangeException("index");
            }
            NativeAPI.MaterialSetContactTangentAcceleration(handle, acceleration, index);
        }

        public void SetContactRotateTangentDirections(Vector3 direction)
        {
            NativeAPI.MaterialContactRotateTangentDirections(handle, ref direction);
        }

        public uint ContactFaceAttribute
        {
            get
            {
                return NativeAPI.MaterialGetContactFaceAttribute(handle);
            }
        }

        public float ContactNormalSpeed
        {
            get
            {
                return NativeAPI.MaterialGetContactNormalSpeed(handle);
            }
        }

        public Vector3 ContactForce
        {
            get
            {
                Vector3 result;
                NativeAPI.MaterialGetContactForce(handle, out result);
                return result;
            }
        }

        IntPtr handle;
    }
}
