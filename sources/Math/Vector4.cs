using System;

namespace Game
{
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct Vector4 : IEquatable<Vector4>
    {
        public float X;
        public float Y;
        public float Z;
        public float W;

        public static Vector4 Zero = new Vector4(0, 0, 0, 0);
        public static Vector4 UnitX = new Vector4(1, 0, 0, 0);
        public static Vector4 UnitY = new Vector4(0, 1, 0, 0);
        public static Vector4 UnitZ = new Vector4(0, 0, 1, 0);
        public static Vector4 UnitW = new Vector4(0, 0, 0, 1);

        public Vector4(float x, float y, float z, float w)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.W = w;
        }

        public Vector4(Vector3 v, float w)
        {
            this.X = v.X;
            this.Y = v.Y;
            this.Z = v.Z;
            this.W = w;
        }

        public void Normalize()
        {
            float len = (float)(1.0 / Math.Sqrt(X * X + Y * Y + Z * Z + W * W));
            X *= len;
            Y *= len;
            Z *= len;
            W *= len;
        }

        public static Vector4 Normalize(Vector4 v)
        {
            Vector4 result = v;
            result.Normalize();
            return result;
        }


        public float Length
        {
            get
            {
                return (float)Math.Sqrt(X * X + Y * Y + Z * Z + W * W);
            }
        }

        public float Length2
        {
            get
            {
                return X * X + Y * Y + Z * Z + W * W;
            }
        }

        public static Vector4 Lerp(Vector4 a, Vector4 b, float t)
        {
            return new Vector4(a.X + (b.X - a.X) * t,
                               a.Y + (b.Y - a.Y) * t,
                               a.Z + (b.Z - a.Z) * t,
                               a.W + (b.W - a.W) * t);
        }

        public static void Lerp(ref Vector4 a, ref Vector4 b, float t, out Vector4 c)
        {
            c.X = a.X + (b.X - a.X) * t;
            c.Y = a.Y + (b.Y - a.Y) * t;
            c.Z = a.Z + (b.Z - a.Z) * t;
            c.W = a.W + (b.W - a.W) * t;
        }
        
        public static float Dot(Vector4 a, Vector4 b)
        {
            return a.X * b.X + a.Y * b.Y + a.Z * b.Z + a.W * b.W;
        }

        public static Vector4 Transform(Vector4 v, Matrix4 m)
        {
            return new Vector4(Vector4.Dot(v, m.Column0),
                               Vector4.Dot(v, m.Column1),
                               Vector4.Dot(v, m.Column2),
                               Vector4.Dot(v, m.Column3));
        }

        public override string ToString()
        {
            return String.Format("{0}, {1}, {2}, {3}", X, Y, Z, W);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode() ^ W.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is Vector4)
            {
                return Equals((Vector4)obj);
            }
            return false;
        }

        public bool Equals(Vector4 other)
        {
            return X == other.X && Y == other.Y && Z == other.Z && W == other.W;
        }

        public static bool operator ==(Vector4 a, Vector4 b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Vector4 a, Vector4 b)
        {
            return !a.Equals(b);
        }

        public static Vector4 operator +(Vector4 a, Vector4 b)
        {
            return new Vector4(a.X + b.X, a.Y + b.Y, a.Z + b.Z, a.W + b.W);
        }

        public static Vector4 operator -(Vector4 a, Vector4 b)
        {
            return new Vector4(a.X - b.X, a.Y - b.Y, a.Z - b.Z, a.W - b.W);
        }

        public static Vector4 operator -(Vector4 a)
        {
            return new Vector4(-a.X, -a.Y, -a.Z, -a.W);
        }

        public static Vector4 operator *(Vector4 a, float b)
        {
            return new Vector4(a.X * b, a.Y * b, a.Z * b, a.W * b);
        }

        public static Vector4 operator *(float a, Vector4 b)
        {
            return new Vector4(a * b.X, a * b.Y, a * b.Z, a * b.W);
        }

        public static Vector4 operator /(Vector4 a, float b)
        {
            return new Vector4(a.X / b, a.Y / b, a.Z / b, a.W / b);
        }
    }
}
