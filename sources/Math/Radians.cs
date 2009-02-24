using System;

namespace Game
{
    public static class Radians
    {
        public const float PI = (float)Math.PI;

        public static float ToDegrees(float radians)
        {
            return radians * 180.0f / (float)Math.PI;
        }

        public static double ToDegrees(double radians)
        {
            return radians * 180.0 / Math.PI;
        }

        public static float Sin(float x)
        {
            return (float)Math.Sin(x);
        }

        public static double Sin(double x)
        {
            return Math.Sin(x);
        }

        public static float Cos(float x)
        {
            return (float)Math.Cos(x);
        }

        public static double Cos(double x)
        {
            return Math.Cos(x);
        }

        public static float Atan2(float y, float x)
        {
            return (float)Math.Atan2(y, x);
        }

        public static double Atan2(double y, double x)
        {
            return Math.Atan2(y, x);
        }

        public static double Normalize(double x)
        {
            double result = Math.IEEERemainder(x, Math.PI);
            if (result < 0.0)
            {
                result += Math.PI;
            }
            return result;
        }

    }
}
