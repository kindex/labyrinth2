using System;

namespace Game
{
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct Matrix4 : IEquatable<Matrix4>
    {
        public Vector4 Row0;
        public Vector4 Row1;
        public Vector4 Row2;
        public Vector4 Row3;

        public static Matrix4 Zero = new Matrix4(Vector4.Zero, Vector4.Zero, Vector4.Zero, Vector4.Zero);
        public static Matrix4 Identity = new Matrix4(Vector4.UnitX, Vector4.UnitY, Vector4.UnitZ, Vector4.UnitW);

        public Matrix4(Vector4 row0, Vector4 row1, Vector4 row2, Vector4 row3)
        {
            this.Row0 = row0;
            this.Row1 = row1;
            this.Row2 = row2;
            this.Row3 = row3;
        }

        public Matrix4(Matrix3 m)
        {
            this.Row0 = new Vector4(m.Row0, 0.0f);
            this.Row1 = new Vector4(m.Row1, 0.0f);
            this.Row2 = new Vector4(m.Row2, 0.0f);
            this.Row3 = Vector4.UnitW;
        }

        public Matrix4(Quaternion q) : this(new Matrix3(q))
        {
        }

        public Vector4 Column0
        {
            get
            {
                return new Vector4(Row0.X, Row1.X, Row2.X, Row3.X);
            }
        }

        public Vector4 Column1
        {
            get
            {
                return new Vector4(Row0.Y, Row1.Y, Row2.Y, Row3.Y);
            }
        }

        public Vector4 Column2
        {
            get
            {
                return new Vector4(Row0.Z, Row1.Z, Row2.Z, Row3.Z);
            }
        }

        public Vector4 Column3
        {
            get
            {
                return new Vector4(Row0.W, Row1.W, Row2.W, Row3.W);
            }
        }

        public static Matrix4 Scale(Vector3 scale)
        {
            return new Matrix4(new Vector4(scale.X, 0.0f, 0.0f, 0.0f),
                               new Vector4(0.0f, scale.Y, 0.0f, 0.0f),
                               new Vector4(0.0f, 0.0f, scale.Z, 0.0f),
                               Vector4.UnitW);
        }

        public static Matrix4 RotateX(float angle)
        {
            float cos = (float)Math.Cos(angle);
            float sin = (float)Math.Sin(angle);

            return new Matrix4(Vector4.UnitX,
                               new Vector4(0.0f, cos, sin, 0.0f),
                               new Vector4(0.0f, -sin, cos, 0.0f),
                               Vector4.UnitW);
        }

        public static Matrix4 RotateY(float angle)
        {
            float cos = (float)Math.Cos(angle);
            float sin = (float)Math.Sin(angle);

            return new Matrix4(new Vector4(cos, 0.0f, -sin, 0.0f),
                               Vector4.UnitY,
                               new Vector4(sin, 0.0f, cos, 0.0f),
                               Vector4.UnitW);
        }

        public static Matrix4 RotateZ(float angle)
        {
            float cos = (float)Math.Cos(angle);
            float sin = (float)Math.Sin(angle);

            return new Matrix4(new Vector4(cos, sin, 0.0f, 0.0f),
                               new Vector4(-sin, cos, 0.0f, 0.0f),
                               Vector4.UnitZ,
                               Vector4.UnitW);
        }


        public static Matrix4 Rotate(Vector3 axis, float angle)
        {
            float cos = (float)Math.Cos(-angle);
            float sin = (float)Math.Sin(-angle);
            float t = 1.0f - cos;

            axis.Normalize();

            return new Matrix4(new Vector4(t * axis.X * axis.X + cos, t * axis.X * axis.Y - sin * axis.Z, t * axis.X * axis.Z + sin * axis.Y, 0.0f),
                               new Vector4(t * axis.X * axis.Y + sin * axis.Z, t * axis.Y * axis.Y + cos, t * axis.Y * axis.Z - sin * axis.X, 0.0f),
                               new Vector4(t * axis.X * axis.Z - sin * axis.Y, t * axis.Y * axis.Z + sin * axis.X, t * axis.Z * axis.Z + cos, 0.0f),
                               Vector4.UnitW);
        }

        public static Matrix4 Translation(Vector3 translation)
        {
            return new Matrix4(Vector4.UnitX, Vector4.UnitY, Vector4.UnitZ, new Vector4(translation, 1.0f));
        }

		public static Matrix4 Multiply(Matrix4 a, Matrix4 b)
		{
			Vector4 col0 = b.Column0;
			Vector4 col1 = b.Column1;
			Vector4 col2 = b.Column2;
			Vector4 col3 = b.Column3;

            return new Matrix4(new Vector4(Vector4.Dot(a.Row0, col0), Vector4.Dot(a.Row0, col1), Vector4.Dot(a.Row0, col2), Vector4.Dot(a.Row0, col3)),
                               new Vector4(Vector4.Dot(a.Row1, col0), Vector4.Dot(a.Row1, col1), Vector4.Dot(a.Row1, col2), Vector4.Dot(a.Row1, col3)),
                               new Vector4(Vector4.Dot(a.Row2, col0), Vector4.Dot(a.Row2, col1), Vector4.Dot(a.Row2, col2), Vector4.Dot(a.Row2, col3)),
                               new Vector4(Vector4.Dot(a.Row3, col0), Vector4.Dot(a.Row3, col1), Vector4.Dot(a.Row3, col2), Vector4.Dot(a.Row3, col3)));
		}

        public static Matrix4 MultiplyTransform(Matrix4 a, Matrix4 b)
		{
			Vector4 col0 = b.Column0;
			Vector4 col1 = b.Column1;
			Vector4 col2 = b.Column2;

            return new Matrix4(new Vector4(Vector4.Dot(a.Row0, col0), Vector4.Dot(a.Row0, col1), Vector4.Dot(a.Row0, col2), 0.0f),
                               new Vector4(Vector4.Dot(a.Row1, col0), Vector4.Dot(a.Row1, col1), Vector4.Dot(a.Row1, col2), 0.0f),
                               new Vector4(Vector4.Dot(a.Row2, col0), Vector4.Dot(a.Row2, col1), Vector4.Dot(a.Row2, col2), 0.0f),
                               new Vector4(Vector4.Dot(a.Row3, col0), Vector4.Dot(a.Row3, col1), Vector4.Dot(a.Row3, col2), 1.0f));
		}       

        public static void Multiply(ref Matrix4 a, ref Matrix4 b, out Matrix4 c)
        {
            Vector4 col0 = b.Column0;
            Vector4 col1 = b.Column1;
            Vector4 col2 = b.Column2;
            Vector4 col3 = b.Column3;

            Vector4 row0 = new Vector4(Vector4.Dot(a.Row0, col0), Vector4.Dot(a.Row0, col1), Vector4.Dot(a.Row0, col2), Vector4.Dot(a.Row0, col3));
            Vector4 row1 = new Vector4(Vector4.Dot(a.Row1, col0), Vector4.Dot(a.Row1, col1), Vector4.Dot(a.Row1, col2), Vector4.Dot(a.Row1, col3));
            Vector4 row2 = new Vector4(Vector4.Dot(a.Row2, col0), Vector4.Dot(a.Row2, col1), Vector4.Dot(a.Row2, col2), Vector4.Dot(a.Row2, col3));
            Vector4 row3 = new Vector4(Vector4.Dot(a.Row3, col0), Vector4.Dot(a.Row3, col1), Vector4.Dot(a.Row3, col2), Vector4.Dot(a.Row3, col3));

            c.Row0 = row0;
            c.Row1 = row1;
            c.Row2 = row2;
            c.Row3 = row3;
        }

        public static Matrix4 Transpose(Matrix4 m)
        {
            return new Matrix4(m.Column0, m.Column1, m.Column2, m.Column3);
        }

        public static void Transpose(ref Matrix4 m, out Matrix4 result)
        {
            Vector4 col0 = m.Column0;
            Vector4 col1 = m.Column1;
            Vector4 col2 = m.Column2;
            Vector4 col3 = m.Column3;
            result.Row0 = col0;
            result.Row1 = col1;
            result.Row2 = col2;
            result.Row3 = col3;
        }

        public static Matrix4 InverseTransform(Matrix4 m)
        {
            return new Matrix4(
                new Vector4(m.Row0.X, m.Row1.X, m.Row2.X, 0.0f),
                new Vector4(m.Row0.Y, m.Row1.Y, m.Row2.Y, 0.0f),
                new Vector4(m.Row0.Z, m.Row1.Z, m.Row2.Z, 0.0f),
                new Vector4(
                    -Vector3.Dot(new Vector3(m.Row3), new Vector3(m.Row0)),
                    -Vector3.Dot(new Vector3(m.Row3), new Vector3(m.Row1)),
                    -Vector3.Dot(new Vector3(m.Row3), new Vector3(m.Row2)),
                    1.0f));
        }

        public static Matrix4 Invert(Matrix4 m)
        {
            Matrix4 result = new Matrix4();
            float num5 = m.Row0.X;
            float num4 = m.Row0.Y;
            float num3 = m.Row0.Z;
            float num2 = m.Row0.W;
            float num9 = m.Row1.X;
            float num8 = m.Row1.Y;
            float num7 = m.Row1.Z;
            float num6 = m.Row1.W;
            float num17 = m.Row2.X;
            float num16 = m.Row2.Y;
            float num15 = m.Row2.Z;
            float num14 = m.Row2.W;
            float num13 = m.Row3.X;
            float num12 = m.Row3.Y;
            float num11 = m.Row3.Z;
            float num10 = m.Row3.W;
            float num23 = num15 * num10 - num14 * num11;
            float num22 = num16 * num10 - num14 * num12;
            float num21 = num16 * num11 - num15 * num12;
            float num20 = num17 * num10 - num14 * num13;
            float num19 = num17 * num11 - num15 * num13;
            float num18 = num17 * num12 - num16 * num13;
            float num39 = num8 * num23 - num7 * num22 + num6 * num21;
            float num38 = -num9 * num23 + num7 * num20 - num6 * num19;
            float num37 = num9 * num22 - num8 * num20 + num6 * num18;
            float num36 = -num9 * num21 + num8 * num19 - num7 * num18;
            float num = 1.0f / (num5 * num39 + num4 * num38 + num3 * num37 + num2 * num36);
            result.Row0.X = num39 * num;
            result.Row1.X = num38 * num;
            result.Row2.X = num37 * num;
            result.Row3.X = num36 * num;
            result.Row0.Y = (-num4 * num23 + num3 * num22 - num2 * num21) * num;
            result.Row1.Y = (num5 * num23 - num3 * num20 + num2 * num19) * num;
            result.Row2.Y = (-num5 * num22 + num4 * num20 - num2 * num18) * num;
            result.Row3.Y = (num5 * num21 - num4 * num19 + num3 * num18) * num;
            float num35 = num7 * num10 - num6 * num11;
            float num34 = num8 * num10 - num6 * num12;
            float num33 = num8 * num11 - num7 * num12;
            float num32 = num9 * num10 - num6 * num13;
            float num31 = num9 * num11 - num7 * num13;
            float num30 = num9 * num12 - num8 * num13;
            result.Row0.Z = (num4 * num35 - num3 * num34 + num2 * num33) * num;
            result.Row1.Z = (-num5 * num35 + num3 * num32 - num2 * num31) * num;
            result.Row2.Z = (num5 * num34 - num4 * num32 + num2 * num30) * num;
            result.Row3.Z = (-num5 * num33 + num4 * num31 - num3 * num30) * num;
            float num29 = num7 * num14 - num6 * num15;
            float num28 = num8 * num14 - num6 * num16;
            float num27 = num8 * num15 - num7 * num16;
            float num26 = num9 * num14 - num6 * num17;
            float num25 = num9 * num15 - num7 * num17;
            float num24 = num9 * num16 - num8 * num17;
            result.Row0.W = (-num4 * num29 + num3 * num28 - num2 * num27) * num;
            result.Row1.W = (num5 * num29 - num3 * num26 + num2 * num25) * num;
            result.Row2.W = (-num5 * num28 + num4 * num26 - num2 * num24) * num;
            result.Row3.W = (num5 * num27 - num4 * num25 + num3 * num24) * num;

            return result;
        }

        public static Matrix4 GrammSchmidt(Vector3 dir)
        {
            Vector3 front = dir;
            front.Normalize();

            Vector3 right;
        	if (Math.Abs(front.Z) > 0.577f)
            {
                right = Vector3.Cross(front, new Vector3(-front.Y, front.Z, 0.0f));
	        }
            else
            {
	  	        right = Vector3.Cross(front, new Vector3(-front.Y, front.X, 0.0f));
	        }
  	        right.Normalize();
  	        
            Vector3 up = Vector3.Cross(right, front);

            return new Matrix4(
                new Vector4(right, 0.0f),
                new Vector4(up, 0.0f),
                new Vector4(front, 0.0f),
                Vector4.UnitW);
        }

        public static Matrix4 LookAt(Vector3 position, Vector3 target, Vector3 up)
        {
            Vector3 zAxis = Vector3.Normalize(target - position);
            Vector3 xAxis = Vector3.Normalize(Vector3.Cross(up, zAxis));
            Vector3 yAxis = Vector3.Normalize(Vector3.Cross(zAxis, xAxis));

            Matrix4 m = new Matrix4();
            m.Row0 = new Vector4(xAxis.X, yAxis.X, -zAxis.X, 0.0f);
            m.Row1 = new Vector4(xAxis.Y, yAxis.Y, -zAxis.Y, 0.0f);
            m.Row2 = new Vector4(xAxis.Z, yAxis.Z, -zAxis.Z, 0.0f);
            m.Row3.X = -Vector3.Dot(xAxis, position);
            m.Row3.Y = -Vector3.Dot(yAxis, position);
            m.Row3.Z = Vector3.Dot(zAxis, position);
            m.Row3.W = 1.0f;

            return m;
        }
        
        public static Matrix4 Perspective(float fovY, float aspect, float zNear, float zFar)
        {
            float f = 1.0f / Degrees.Tan(fovY / 2.0f);
            float a = (zFar + zNear) / (zNear - zFar);
            float b = (2.0f * zFar * zNear) / (zNear - zFar);

            return new Matrix4(new Vector4(f / aspect, 0.0f, 0.0f, 0.0f),
                               new Vector4(0.0f, f, 0.0f, 0.0f),
                               new Vector4(0.0f, 0.0f, a, -1.0f),
                               new Vector4(0.0f, 0.0f, b, 0.0f));
        }

        public static Matrix4 Orthographic(float left, float right, float top, float bottom, float zNear, float zFar)
        {
            float rl = 1.0f / (right - left);
            float tb = 1.0f / (top - bottom);
            float fn = 1.0f / (zFar - zNear);

            float tx = -(right + left) * rl;
            float ty = -(top + bottom) * tb;
            float tz = -(zFar + zNear) * fn;

            return new Matrix4(new Vector4(2.0f * rl, 0.0f, 0.0f, 0.0f),
                               new Vector4(0.0f, 2.0f * tb, 0.0f, 0.0f),
                               new Vector4(0.0f, 0.0f, -2.0f * fn, 0.0f),
                               new Vector4(tx, ty, tz, 1.0f));
        }

        public override string ToString()
        {
            return String.Format("{0}, {1}, {2}, {3}", Row0, Row1, Row2, Row3);
        }

        public override int GetHashCode()
        {
            return Row0.GetHashCode() ^ Row1.GetHashCode() ^ Row2.GetHashCode() ^ Row3.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is Matrix4)
            {
                return Equals((Matrix4)obj);
            }
            return false;
        }

        public bool Equals(Matrix4 other)
        {
            return Row0 == other.Row0 && Row1 == other.Row1 && Row2 == other.Row2 && Row3 == other.Row3;
        }

        public static bool operator ==(Matrix4 a, Matrix4 b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Matrix4 a, Matrix4 b)
        {
            return !a.Equals(b);
        }

        public static Matrix4 operator *(Matrix4 left, Matrix4 right)
        {
            return Multiply(left, right);
        }

        public Vector3 Right
        {
            get
            {
                return new Vector3(Row0);
            }
            set
            {
                Row0 = new Vector4(value, 0.0f);
            }
        }

        public Vector3 Up
        {
            get
            {
                return new Vector3(Row1);
            }
            set
            {
                Row1 = new Vector4(value, 0.0f);
            }
        }

        public Vector3 Front
        {
            get
            {
                return new Vector3(Row2);
            }
            set
            {
                Row2 = new Vector4(value, 0.0f);
            }
        }

        public Vector3 Posit
        {
            get
            {
                return new Vector3(Row3);
            }
            set
            {
                Row3 = new Vector4(value, 1.0f);
            }
        }
    }
}
