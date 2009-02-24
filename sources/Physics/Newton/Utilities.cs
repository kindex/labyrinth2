using System;

namespace Game.Physics.Newton
{
    public static class Utilities
    {
        public static Vector3 GetEulerAngles(this Matrix4 matrix)
        {
            Vector3 result;
            NativeAPI.GetEulerAngle(ref matrix, out result);
            return result;
        }

        public static void GetEulerAngle(Matrix4 matrix, out Vector3 eulerAngles)
        {
            NativeAPI.GetEulerAngle(ref matrix, out eulerAngles);
        }

        public static Matrix4 FromEulerAngles(this Vector3 eulerAngles)
        {
            Matrix4 result;
            NativeAPI.SetEulerAngle(ref eulerAngles, out result);
            return result;
        }

        public static void GetEulerAngle(Vector3 eulerAngles, out Matrix4 matrix)
        {
            NativeAPI.SetEulerAngle(ref eulerAngles, out matrix);
        }

        public static float CalculateSpringDamperAcceleration(float dt, float ks, float x, float kd, float s)
        {
            return NativeAPI.CalculateSpringDamperAcceleration(dt, ks, x, kd, s);
        }

    }
}
