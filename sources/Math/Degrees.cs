using System;

namespace Game
{
    public static class Degrees
    {
        public static float ToRadians(float degrees)
        {
            return degrees * (float)Math.PI / 180.0f;
        }

        public static double ToRadians(double degrees)
        {
            return degrees * Math.PI / 180.0;
        }

        public static float Sin(float x)
        {
            return (float)Math.Sin(ToRadians(x));
        }

        public static double Sin(double x)
        {
            return Math.Sin(ToRadians(x));
        }

        public static float Cos(float x)
        {
            return (float)Math.Cos(ToRadians(x));
        }

        public static double Cos(double x)
        {
            return Math.Cos(ToRadians(x));
        }

        public static float Tan(float x)
        {
            return (float)Math.Tan(ToRadians(x));
        }

        public static double Tan(double x)
        {
            return Math.Tan(ToRadians(x));
        }


        public static float Atan2(float y, float x)
        {
            return Radians.ToDegrees((float)Math.Atan2(y, x));
        }

        public static double Atan2(double y, double x)
        {
            return Normalize(Radians.ToDegrees(Math.Atan2(y, x)));
        }

        public static double Normalize(double x)
        {
            double result = Math.IEEERemainder(x, 360.0);
            if (result < 0.0)
            {
                result += 360.0;
            }
            return result;
        }
    }
}
