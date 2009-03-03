using System;

namespace Game
{
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct Vector3 : IEquatable<Vector3>
    {
        public float X;
        public float Y;
        public float Z;

        public static Vector3 Zero = new Vector3(0, 0, 0);
        public static Vector3 UnitX = new Vector3(1, 0, 0);
        public static Vector3 UnitY = new Vector3(0, 1, 0);
        public static Vector3 UnitZ = new Vector3(0, 0, 1);
        public static Vector3 Half = new Vector3(0.5f, 0.5f, 0.5f);
        public static Vector3 One = new Vector3(1, 1, 1);

        public Vector3(float v)
        {
            this.X = v;
            this.Y = v;
            this.Z = v;
        }

        public Vector3(float x, float y, float z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public Vector3(Vector2 v, float z)
        {
            this.X = v.X;
            this.Y = v.Y;
            this.Z = z;
        }

        public Vector3(Vector4 v)
        {
            this.X = v.X;
            this.Y = v.Y;
            this.Z = v.Z;
        }

        public void Normalize()
        {
            float len = (float)(1.0 / Math.Sqrt(X * X + Y * Y + Z * Z));
            X *= len;
            Y *= len;
            Z *= len;
        }

        public Vector3 GetNormalized()
        {
            float len = (float)Math.Sqrt(X * X + Y * Y + Z * Z);
            if (len != 0)
            {
                return new Vector3(X / len, Y / len, Z / len);
            }
            else
            {
                return Zero;
            }
        }

        public static Vector3 Normalize(Vector3 v)
        {
            Vector3 result = v;
            result.Normalize();
            return result;
        }

        public float Length
        {
            get
            {
                return (float)Math.Sqrt(X * X + Y * Y + Z * Z);
            }
        }

        public float Length2
        {
            get
            {
                return X * X + Y * Y + Z * Z;
            }
        }

        public static Vector3 Lerp(Vector3 a, Vector3 b, float t)
        {
            return new Vector3(a.X + (b.X - a.X) * t,
                               a.Y + (b.Y - a.Y) * t,
                               a.Z + (b.Z - a.Z) * t);
        }

        public static void Lerp(ref Vector3 a, ref Vector3 b, float t, out Vector3 c)
        {
            c.X = a.X + (b.X - a.X) * t;
            c.Y = a.Y + (b.Y - a.Y) * t;
            c.Z = a.Z + (b.Z - a.Z) * t;
        }

        public static float Dot(Vector3 a, Vector3 b)
        {
            return a.X * b.X + a.Y * b.Y + a.Z * b.Z;
        }

        public static Vector3 Cross(Vector3 a, Vector3 b)
        {
            return new Vector3(a.Y * b.Z - a.Z * b.Y, a.Z * b.X - a.X * b.Z, a.X * b.Y - a.Y * b.X);
        }

        public static void Cross(ref Vector3 a, ref Vector3 b, out Vector3 c)
        {
            float x = a.Y * b.Z - a.Z * b.Y;
            float y = a.Z * b.X - a.X * b.Z;
            float z = a.X * b.Y - a.Y * b.X;

            c.X = x;
            c.Y = y;
            c.Z = z;
        }

        public static Vector3 TransformVector(Vector3 v, Matrix3 m)
        {
            return new Vector3(Vector3.Dot(v, m.Column0),
                               Vector3.Dot(v, m.Column1),
                               Vector3.Dot(v, m.Column2));
        }

        public static Vector3 TransformVector(Vector3 v, Matrix4 m)
        {
            return new Vector3(Vector3.Dot(v, new Vector3(m.Column0)),
                               Vector3.Dot(v, new Vector3(m.Column1)),
                               Vector3.Dot(v, new Vector3(m.Column2)));
        }

        public static Vector3 TransformPosition(Vector3 v, Matrix4 m)
        {
            return new Vector3(Vector3.Dot(v, new Vector3(m.Column0)) + m.Row3.X,
                               Vector3.Dot(v, new Vector3(m.Column1)) + m.Row3.Y,
                               Vector3.Dot(v, new Vector3(m.Column2)) + m.Row3.Z);
        }

        public static Vector3 UntransformVector(Vector3 v, Matrix4 m)
        {
            return new Vector3(Vector3.Dot(v, new Vector3(m.Row0)),
                               Vector3.Dot(v, new Vector3(m.Row1)),
                               Vector3.Dot(v, new Vector3(m.Row2)));
        }
        
        public static Vector3 UntransformPosition(Vector3 v, Matrix4 m)
        {
            return UntransformVector(v - new Vector3(m.Row3), m);
        }

        public override string ToString()
        {
            return String.Format("{0}, {1}, {2}", X, Y, Z);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is Vector3)
            {
                return Equals((Vector3)obj);
            }
            return false;
        }

        public bool Equals(Vector3 other)
        {
            return X == other.X && Y == other.Y && Z == other.Z;
        }

        public static bool operator ==(Vector3 a, Vector3 b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Vector3 a, Vector3 b)
        {
            return !a.Equals(b);
        }

        public static Vector3 operator +(Vector3 a, Vector3 b)
        {
            return new Vector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static Vector3 operator -(Vector3 a, Vector3 b)
        {
            return new Vector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public static Vector3 operator -(Vector3 a)
        {
            return new Vector3(-a.X, -a.Y, -a.Z);
        }

        public static Vector3 operator *(Vector3 a, float b)
        {
            return new Vector3(a.X * b, a.Y * b, a.Z * b);
        }

        public static Vector3 operator *(float a, Vector3 b)
        {
            return new Vector3(a * b.X, a * b.Y, a * b.Z);
        }

        public static Vector3 operator /(Vector3 a, float b)
        {
            return new Vector3(a.X / b, a.Y / b, a.Z / b);
        }

        public static Vector3 MaxComponents(Vector3 a, Vector3 b)
        {
            return new Vector3(Math.Max(a.X, b.X), Math.Max(a.Y, b.Y), Math.Max(a.Z, b.Z));
        }

        public static Vector3 MinComponents(Vector3 a, Vector3 b)
        {
            return new Vector3(Math.Min(a.X, b.X), Math.Min(a.Y, b.Y), Math.Min(a.Z, b.Z));
        }

        public Vector3 MemberMul(Vector3 a)
        {
            return new Vector3(a.X*this.X, a.Y*this.Y, a.Z*this.Z);
        }
    }
}
