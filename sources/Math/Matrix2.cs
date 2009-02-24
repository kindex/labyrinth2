using System;

namespace Game
{
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct Matrix2 : IEquatable<Matrix2>
    {
        public Vector2 Row0;
        public Vector2 Row1;

        public static Matrix2 Zero = new Matrix2(Vector2.Zero, Vector2.Zero);
        public static Matrix2 Identity = new Matrix2(Vector2.UnitX, Vector2.UnitY);

        public Matrix2(Vector2 row0, Vector2 row1)
        {
            this.Row0 = row0;
            this.Row1 = row1;
        }

        public Vector2 Column0
        {
            get
            {
                return new Vector2(Row0.X, Row1.X);
            }
        }

        public Vector2 Column1
        {
            get
            {
                return new Vector2(Row0.Y, Row1.Y);
            }
        }

        public static Matrix2 Scale(Vector2 scale)
        {
            return new Matrix2(new Vector2(scale.X, 0.0f),
                               new Vector2(0.0f, scale.Y));
        }

        public static Matrix2 Rotate(float angle)
        {
            float cos = (float)Math.Cos(angle);
            float sin = (float)Math.Sin(angle);

            return new Matrix2(new Vector2(cos, sin),
                               new Vector2(-sin, cos));
        }

        public static Matrix2 Multiply(Matrix2 a, Matrix2 b)
        {
            Vector2 col0 = b.Column0;
            Vector2 col1 = b.Column1;

            return new Matrix2(new Vector2(Vector2.Dot(a.Row0, col0), Vector2.Dot(a.Row0, col1)),
                               new Vector2(Vector2.Dot(a.Row1, col0), Vector2.Dot(a.Row1, col1)));
        }

        public static void Multiply(ref Matrix2 a, ref Matrix2 b, out Matrix2 c)
        {
            Vector2 col0 = b.Column0;
            Vector2 col1 = b.Column1;

            Vector2 row0 = new Vector2(Vector2.Dot(a.Row0, col0), Vector2.Dot(a.Row0, col1));
            Vector2 row1 = new Vector2(Vector2.Dot(a.Row1, col0), Vector2.Dot(a.Row1, col1));

            c.Row0 = row0;
            c.Row1 = row1;
        }

        public static Matrix2 Transpose(Matrix2 m)
        {
            return new Matrix2(m.Column0, m.Column1);
        }

        public static void Transpose(ref Matrix2 m, out Matrix2 result)
        {
            Vector2 col0 = m.Column0;
            Vector2 col1 = m.Column1;
            result.Row0 = col0;
            result.Row1 = col1;
        }

        public override string ToString()
        {
            return String.Format("{0}\n{1}", Row0, Row1);
        }

        public override int GetHashCode()
        {
            return Row0.GetHashCode() ^ Row1.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is Matrix2)
            {
                return Equals((Matrix2)obj);
            }
            return false;
        }

        public bool Equals(Matrix2 other)
        {
            return Row0 == other.Row0 && Row1 == other.Row1;
        }

        public static bool operator ==(Matrix2 a, Matrix2 b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Matrix2 a, Matrix2 b)
        {
            return !a.Equals(b);
        }

        public static Matrix2 operator *(Matrix2 a, Matrix2 b)
        {
            return Multiply(a, b);
        }
    }
}
