using System;

namespace Game
{
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct Matrix3 : IEquatable<Matrix3>
    {
        public Vector3 Row0;
        public Vector3 Row1;
        public Vector3 Row2;

        public static Matrix3 Zero = new Matrix3(Vector3.Zero, Vector3.Zero, Vector3.Zero);
        public static Matrix3 Identity = new Matrix3(Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ);

        public Matrix3(Vector3 row0, Vector3 row1, Vector3 row2)
        {
            this.Row0 = row0;
            this.Row1 = row1;
            this.Row2 = row2;
        }

        public Matrix3(Matrix4 m)
        {
            this.Row0 = new Vector3(m.Row0);
            this.Row1 = new Vector3(m.Row1);
            this.Row2 = new Vector3(m.Row2);
        }

        public Matrix3(Quaternion q)
        {
            float qx2 = q.XYZ.X + q.XYZ.X;
            float qy2 = q.XYZ.Y + q.XYZ.Y;
            float qz2 = q.XYZ.Z + q.XYZ.Z;

            float qxqx2 = q.XYZ.X * qx2;
            float qxqy2 = q.XYZ.X * qy2;
            float qxqz2 = q.XYZ.X * qz2;

            float qxqw2 = q.W * qx2;
            float qyqy2 = q.XYZ.Y * qy2;
            float qyqz2 = q.XYZ.Y * qz2;

            float qyqw2 = q.W * qy2;
            float qzqz2 = q.XYZ.Z * qz2;
            float qzqw2 = q.W * qz2;

            this.Row0 = new Vector3(1.0f - qyqy2 - qzqz2, qxqy2 - qzqw2, qxqz2 + qyqw2);
            this.Row1 = new Vector3(qxqy2 + qzqw2, 1.0f - qxqx2 - qzqz2, qyqz2 - qxqw2);
            this.Row2 = new Vector3(qxqz2 - qyqw2, qyqz2 + qxqw2, qxqx2 - qyqy2);
        }

        public Vector3 Column0
        {
            get
            {
                return new Vector3(Row0.X, Row1.X, Row2.X);
            }
        }

        public Vector3 Column1
        {
            get
            {
                return new Vector3(Row0.Y, Row1.Y, Row2.Y);
            }
        }

        public Vector3 Column2
        {
            get
            {
                return new Vector3(Row0.Z, Row1.Z, Row2.Z);
            }
        }

        public static Matrix3 Scale(Vector3 scale)
        {
            return new Matrix3(new Vector3(scale.X, 0.0f, 0.0f),
                               new Vector3(0.0f, scale.Y, 0.0f),
                               new Vector3(0.0f, 0.0f, scale.Z));
        }

        public static Matrix3 RotateX(float angle)
        {
            float cos = (float)Math.Cos(angle);
            float sin = (float)Math.Sin(angle);

            return new Matrix3(Vector3.UnitX,
                               new Vector3(0.0f, cos, sin),
                               new Vector3(0.0f, -sin, cos));
        }

        public static Matrix3 RotateY(float angle)
        {
            float cos = (float)Math.Cos(angle);
            float sin = (float)Math.Sin(angle);

            return new Matrix3(new Vector3(cos, 0.0f, -sin),
                               Vector3.UnitY,
                               new Vector3(sin, 0.0f, cos));
        }

        public static Matrix3 RotateZ(float angle)
        {
            float cos = (float)Math.Cos(angle);
            float sin = (float)Math.Sin(angle);

            return new Matrix3(new Vector3(cos, sin, 0.0f),
                               new Vector3(-sin, cos, 0.0f),
                               Vector3.UnitZ);
        }


        public static Matrix3 Rotate(Vector3 axis, float angle)
        {
            float cos = (float)Math.Cos(-angle);
            float sin = (float)Math.Sin(-angle);
            float t = 1.0f - cos;

            axis.Normalize();

            return new Matrix3(new Vector3(t * axis.X * axis.X + cos, t * axis.X * axis.Y - sin * axis.Z, t * axis.X * axis.Z + sin * axis.Y),
                               new Vector3(t * axis.X * axis.Y + sin * axis.Z, t * axis.Y * axis.Y + cos, t * axis.Y * axis.Z - sin * axis.X),
                               new Vector3(t * axis.X * axis.Z - sin * axis.Y, t * axis.Y * axis.Z + sin * axis.X, t * axis.Z * axis.Z + cos));
        }

        public static Matrix3 Translation(Vector2 translation)
        {
            return new Matrix3(Vector3.UnitX, Vector3.UnitY, new Vector3(translation, 1.0f));
        }

        public static Matrix3 Multiply(Matrix3 a, Matrix3 b)
        {
            Vector3 col0 = b.Column0;
            Vector3 col1 = b.Column1;
            Vector3 col2 = b.Column2;

            return new Matrix3(new Vector3(Vector3.Dot(a.Row0, col0), Vector3.Dot(a.Row0, col1), Vector3.Dot(a.Row0, col2)),
                               new Vector3(Vector3.Dot(a.Row1, col0), Vector3.Dot(a.Row1, col1), Vector3.Dot(a.Row1, col2)),
                               new Vector3(Vector3.Dot(a.Row2, col0), Vector3.Dot(a.Row2, col1), Vector3.Dot(a.Row2, col2)));
        }

        public static void Multiply(ref Matrix3 a, ref Matrix3 b, out Matrix3 c)
        {
            Vector3 col0 = b.Column0;
            Vector3 col1 = b.Column1;
            Vector3 col2 = b.Column2;

            Vector3 row0 = new Vector3(Vector3.Dot(a.Row0, col0), Vector3.Dot(a.Row0, col1), Vector3.Dot(a.Row0, col2));
            Vector3 row1 = new Vector3(Vector3.Dot(a.Row1, col0), Vector3.Dot(a.Row1, col1), Vector3.Dot(a.Row1, col2));
            Vector3 row2 = new Vector3(Vector3.Dot(a.Row2, col0), Vector3.Dot(a.Row2, col1), Vector3.Dot(a.Row2, col2));

            c.Row0 = row0;
            c.Row1 = row1;
            c.Row2 = row2;
        }

        public static Matrix3 Transpose(Matrix3 m)
        {
            return new Matrix3(m.Column0, m.Column1, m.Column2);
        }

        public static void Transpose(ref Matrix3 m, out Matrix3 result)
        {
            Vector3 col0 = m.Column0;
            Vector3 col1 = m.Column1;
            Vector3 col2 = m.Column2;
            result.Row0 = col0;
            result.Row1 = col1;
            result.Row2 = col2;
        }

        public float Determinant()
        {
            return (Row0.X * Row1.Y * Row2.Z + Row0.Y * Row1.Z * Row2.X + Row0.Z * Row1.X * Row2.Y) -
                (Row0.Y * Row1.X * Row2.Z + Row0.X * Row1.Z * Row2.Y + Row0.Z * Row1.Y * Row2.X);
        }

        public override string ToString()
        {
            return String.Format("{0}, {1}, {2}", Row0, Row1, Row2);
        }

        public override int GetHashCode()
        {
            return Row0.GetHashCode() ^ Row1.GetHashCode() ^ Row2.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is Matrix3)
            {
                return Equals((Matrix3)obj);
            }
            return false;
        }

        public bool Equals(Matrix3 other)
        {
            return Row0 == other.Row0 && Row1 == other.Row1 && Row2 == other.Row2;
        }

        public static bool operator ==(Matrix3 a, Matrix3 b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Matrix3 a, Matrix3 b)
        {
            return !a.Equals(b);
        }

        public static Matrix3 operator *(Matrix3 a, Matrix3 b)
        {
            return Multiply(a, b);
        }
    }
}
