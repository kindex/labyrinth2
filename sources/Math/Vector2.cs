using System;

namespace Game
{
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct Vector2 : IEquatable<Vector2>
    {
        public float X;
        public float Y;

        public static Vector2 Zero = new Vector2(0, 0);
        public static Vector2 UnitX = new Vector2(1, 0);
        public static Vector2 UnitY = new Vector2(0, 1);

        public Vector2(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }

        public Vector2(Vector3 v)
        {
            this.X = v.X;
            this.Y = v.Y;
        }

        public void Normalize()
        {
            float len = (float)(1.0 / Math.Sqrt(X * X + Y * Y));
            X *= len;
            Y *= len;
        }

        public static Vector2 Normalize(Vector2 v)
        {
            Vector2 result = v;
            result.Normalize();
            return result;
        }

        public float Length
        {
            get
            {
                return (float)Math.Sqrt(X * X + Y * Y);
            }
        }

        public float Length2
        {
            get
            {
                return X * X + Y * Y;
            }
        }

        public static Vector2 Lerp(Vector2 a, Vector2 b, float t)
        {
            return new Vector2(a.X + (b.X - a.X) * t,
                               a.Y + (b.Y - a.Y) * t);
        }

        public static void Lerp(ref Vector2 a, ref Vector2 b, float t, out Vector2 c)
        {
            c.X = a.X + (b.X - a.X) * t;
            c.Y = a.Y + (b.Y - a.Y) * t;
        }

        public static float Dot(Vector2 a, Vector2 b)
        {
            return a.X * b.X + a.Y * b.Y;
        }

        public static Vector2 TransformVector(Vector2 v, Matrix2 m)
        {
            return new Vector2(Vector2.Dot(v, m.Column0),
                               Vector2.Dot(v, m.Column1));
        }

        public static Vector2 TransformVector(Vector2 v, Matrix3 m)
        {
            return new Vector2(Vector2.Dot(v, new Vector2(m.Column0)),
                               Vector2.Dot(v, new Vector2(m.Column1)));
        }

        public static Vector2 TransformPosition(Vector2 v, Matrix3 m)
        {
            return new Vector2(Vector2.Dot(v, new Vector2(m.Column0)) + m.Row2.X,
                               Vector2.Dot(v, new Vector2(m.Column1)) + m.Row2.Y);
        }

        public override string ToString()
        {
            return String.Format("{0}, {1}", X, Y);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is Vector2)
            {
                return Equals((Vector2)obj);
            }
            return false;
        }

        public bool Equals(Vector2 other)
        {
            return X == other.X && Y == other.Y;
        }

        public static bool operator ==(Vector2 a, Vector2 b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Vector2 a, Vector2 b)
        {
            return !a.Equals(b);
        }

        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X + b.X, a.Y + b.Y);
        }

        public static Vector2 operator -(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X - b.X, a.Y - b.Y);
        }

        public static Vector2 operator -(Vector2 a)
        {
            return new Vector2(-a.X, -a.Y);
        }

        public static Vector2 operator *(Vector2 a, float b)
        {
            return new Vector2(a.X * b, a.Y * b);
        }

        public static Vector2 operator *(float a, Vector2 b)
        {
            return new Vector2(a * b.X, a * b.Y);
        }

        public static Vector2 operator /(Vector2 a, float b)
        {
            return new Vector2(a.X / b, a.Y / b);
        }
    }
}
