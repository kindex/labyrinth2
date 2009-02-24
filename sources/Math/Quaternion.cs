using System;

namespace Game
{
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct Quaternion : IEquatable<Quaternion>
    {
        public Vector3 XYZ;
        public float W;

        public static Quaternion Identity = new Quaternion(0, 0, 0, 1);

        public Quaternion(Vector3 v, float w)
        {
            this.XYZ = v;
            this.W = w;
        }

        public Quaternion(float x, float y, float z, float w)
        {
            this.XYZ = new Vector3(x, y, z);
            this.W = w;
        }

        public Quaternion(Matrix3 m)
        {
            float xx = m.Row0.X;
            float xy = m.Row0.Y;
            float xz = m.Row0.Z;
            float yx = m.Row1.X;
            float yy = m.Row1.Y;
            float yz = m.Row1.Z;
            float zx = m.Row2.X;
            float zy = m.Row2.Y;
            float zz = m.Row2.Z;

            float trace = xx + yy + zz;
            bool isTraceNeg = trace < 0.0f;
            bool isZgtX = zz > xx;
            bool isZgtY = zz > yy;
            bool isYgtX = yy > xx;
            bool largestXorY = (!isZgtX || !isZgtY) && isTraceNeg;
            bool largestYorZ = (isYgtX || isZgtX) && isTraceNeg;
            bool largestZorX = (isZgtY || !isYgtX) && isTraceNeg;
            
            if (largestXorY)
            {
                zz = -zz;
                xy = -xy;
            }

            if (largestYorZ)
            {
                xx = -xx;
                yz = -yz;
            }

            if (largestZorX)
            {
                yy = -yy;
                zx = -zx;
            }

            float diagSum = xx + yy + zz + 1.0f;
            float scale = 0.5f / (float)Math.Sqrt(diagSum);

            XYZ.X = (zy - yz) * scale;
            XYZ.Y = (xz - zx) * scale;
            XYZ.Z = (yx - xy) * scale;
            W = diagSum * scale;
            
            if (largestXorY)
            {
                float tmpx = XYZ.X;
                float tmpy = XYZ.Y;
                XYZ.X = W;
                XYZ.Y = XYZ.Z;
                XYZ.Z = tmpy;
                W = tmpx;
            }

            if (largestYorZ)
            {
                float tmpx = XYZ.X;
                float tmpz = XYZ.Z;
                XYZ.X = XYZ.Y;
                XYZ.Y = tmpx;
                XYZ.Z = W;
                W = tmpz;
            }
        }

        public Quaternion(Matrix4 m) : this(new Matrix3(m))
        {
        }

        public void Normalize()
        {
            float len = (float)(1.0 / Math.Sqrt(W * W + XYZ.Length2));
            XYZ *= len;
            W *= len;
        }

        public static Quaternion Normalize(Quaternion v)
        {
            Quaternion result = v;
            result.Normalize();
            return result;
        }

        public float Length
        {
            get
            {
                return (float)Math.Sqrt(W * W + XYZ.Length2);
            }
        }

        public float Length2
        {
            get
            {
                return W * W + XYZ.Length2;
            }
        }

        public Quaternion Conjugate()
        {
            return new Quaternion(-XYZ, W);
        }

        public static Quaternion Multiply(Quaternion a, Quaternion b)
        {
            return new Quaternion(b.W * a.XYZ + a.W * b.XYZ + Vector3.Cross(a.XYZ, b.XYZ),
                                  b.W * a.W - Vector3.Dot(a.XYZ, b.XYZ));
        }

        public static void Multiply(ref Quaternion a, ref Quaternion b, out Quaternion c)
        {
            float w = b.W * a.W - Vector3.Dot(a.XYZ, b.XYZ);
            c.XYZ = b.W * a.XYZ + a.W * b.XYZ + Vector3.Cross(a.XYZ, b.XYZ);
            c.W = w;
        }

        public static Quaternion Invert(Quaternion q)
        {
            float len2 = q.Length2;
            if (len2 != 0)
            {
                len2 = 1.0f / len2;
                q.XYZ *= -len2;
                q.W *= len2;
            }
            return q;
        }

        public static Quaternion Slerp(Quaternion a, Quaternion b, float t)
        {
            Quaternion start;
            float cosAngle = a.W * b.W + Vector3.Dot(a.XYZ, b.XYZ);

            if (cosAngle < 0.0f)
            {
                cosAngle = -cosAngle;
                start = new Quaternion(-a.XYZ, -a.W);
            }
            else
            {
                start = b;
            }

            float s0, s1;
            if (cosAngle < 1.0f - 1e-5f)
            {
                float angle = (float)Math.Acos(cosAngle);
                float oneOverSinAngle = 1.0f / (float)Math.Sin(angle);
                s0 = (float)Math.Sin((1.0f - t) * angle) * oneOverSinAngle;
                s1 = (float)Math.Sin(t * angle) * oneOverSinAngle;
            }
            else
            {
                s0 = 1.0f - t;
                s1 = t;
            }

            return new Quaternion(s0 * start.XYZ + s1 * b.XYZ, s0 * start.W + s1 * b.W);
        }

        public override string ToString()
        {
            return String.Format("{0}, {1}", XYZ, W);
        }

        public override int GetHashCode()
        {
            return XYZ.GetHashCode() ^ W.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is Quaternion)
            {
                return Equals((Quaternion)obj);
            }
            return false;
        }

        public bool Equals(Quaternion other)
        {
            return XYZ == other.XYZ && W == other.W;
        }

        public static bool operator ==(Quaternion a, Quaternion b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Quaternion a, Quaternion b)
        {
            return !a.Equals(b);
        }

        public static Quaternion operator *(Quaternion a, Quaternion b)
        {
            return Multiply(a, b);
        }
    }
}
