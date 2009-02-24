using Game.Graphics.Renderer.OpenGL;

namespace Game
{
    static class Utils
    {
        public static void calculate_TB(VF.PositionTexcoordNT[] dataVB, ushort[] dataIB)
        {
            Vector3[] tan1 = new Vector3[dataVB.Length];
            Vector3[] tan2 = new Vector3[dataVB.Length];

            for (int i = 0; i < dataIB.Length; i += 3)
            {
                int i1 = dataIB[i + 0];
                int i2 = dataIB[i + 1];
                int i3 = dataIB[i + 2];

                Vector3 v1 = dataVB[i1].Position;
                Vector3 v2 = dataVB[i2].Position;
                Vector3 v3 = dataVB[i3].Position;
                Vector2 w1 = dataVB[i1].Texcoord;
                Vector2 w2 = dataVB[i2].Texcoord;
                Vector2 w3 = dataVB[i3].Texcoord;

                float x1 = v2.X - v1.X;
                float x2 = v3.X - v1.X;
                float y1 = v2.Y - v1.Y;
                float y2 = v3.Y - v1.Y;
                float z1 = v2.Z - v1.Z;
                float z2 = v3.Z - v1.Z;

                float s1 = w2.X - w1.X;
                float s2 = w3.X - w1.X;
                float t1 = w2.Y - w1.Y;
                float t2 = w3.Y - w1.Y;

                float r = 1.0f / (s1 * t2 - s2 * t1);
                Vector3 sdir = new Vector3((t2 * x1 - t1 * x2) * r, (t2 * y1 - t1 * y2) * r, (t2 * z1 - t1 * z2) * r);
                Vector3 tdir = new Vector3((s2 * x2 - s2 * x1) * r, (s1 * y2 - s2 * y1) * r, (s1 * z2 - s2 * z1) * r);

                tan1[i1] += sdir;
                tan1[i2] += sdir;
                tan1[i3] += sdir;

                tan2[i1] += tdir;
                tan2[i2] += tdir;
                tan2[i3] += tdir;
            }

            for (int i = 0; i < dataVB.Length; i++)
            {
                Vector3 n = dataVB[i].Normal;
                Vector3 t = tan1[i];

                dataVB[i].Tangent = new Vector4(Vector3.Normalize(t - n * Vector3.Dot(n, t)), 1.0f);

                if (Vector3.Dot(t, tan2[i]) < 0.0f)
                {
                    dataVB[i].Tangent.W = -1.0f;
                }
            }
        }

        public static int generateLightSphere(int stacks, int slices, out Buffer vbuf, out Buffer ibuf)
        {
            VF.Position3[] vertices = new VF.Position3[(stacks + 1) * (slices + 1)];
            ushort[] indices = new ushort[stacks * (slices + 1) * 2];

            int verticesIdx = 0;

            for (int stackNumber = 0; stackNumber <= stacks; ++stackNumber)
            {
                for (int sliceNumber = 0; sliceNumber <= slices; ++sliceNumber)
                {
                    float theta = stackNumber * Radians.PI / stacks;
                    float phi = sliceNumber * 2 * Radians.PI / slices + Radians.PI / 2;
                    float sinTheta = Radians.Sin(theta);
                    float sinPhi = Radians.Sin(phi);
                    float cosTheta = Radians.Cos(theta);
                    float cosPhi = Radians.Cos(phi);

                    vertices[verticesIdx++] = new VF.Position3()
                    {
                        Position = new Vector3(cosPhi * sinTheta, sinPhi * sinTheta, cosTheta)
                    };
                }
            }

            int indicesIdx = 0;

            for (int stackNumber = 0; stackNumber < stacks; ++stackNumber)
            {
                for (int sliceNumber = 0; sliceNumber <= slices; ++sliceNumber)
                {
                    indices[indicesIdx++] = (ushort)(stackNumber * (slices + 1) + sliceNumber);
                    indices[indicesIdx++] = (ushort)((stackNumber + 1) * (slices + 1) + sliceNumber);
                }
            }

            vbuf = Device.Current.CreateVertexBuffer(BufferUsage.StaticDraw, vertices);
            ibuf = Device.Current.CreateIndexBuffer(BufferUsage.StaticDraw, indices);

            return indicesIdx;
        }

        public static int generateLightCone(int stacks, int slices, out Buffer vbuf, out Buffer ibuf)
        {
            VF.Position3[] vertices = new VF.Position3[(1 + slices + 1) + (stacks + 1) * (slices + 1)];
            ushort[] indices = new ushort[(slices + 1) * 2 + 2 + stacks * (slices + 1) * 2];

            int verticesIdx = 0;

            // bottom 
            vertices[verticesIdx++] = new VF.Position3()
            {
                Position = Vector3.UnitZ
            };
            for (int sliceNumber = 0; sliceNumber <= slices; ++sliceNumber)
            {
                float phi = sliceNumber * 2 * Radians.PI / slices + Radians.PI / 2;
                float sinPhi = Radians.Sin(phi);
                float cosPhi = Radians.Cos(phi);

                vertices[verticesIdx++] = new VF.Position3()
                {
                    Position = new Vector3(cosPhi, sinPhi, 1.0f)
                };
            }

            // cone
            for (int stackNumber = 0; stackNumber <= stacks; ++stackNumber)
            {
                for (int sliceNumber = slices; sliceNumber >= 0; --sliceNumber)
                {
                    float phi = sliceNumber * 2 * Radians.PI / slices + Radians.PI / 2;
                    float sinPhi = Radians.Sin(phi);
                    float cosPhi = Radians.Cos(phi);
                    float t = 1.0f - (float)stackNumber / (float)stacks;

                    vertices[verticesIdx++] = new VF.Position3()
                    {
                        Position = new Vector3(cosPhi * t, sinPhi * t, t)
                    };
                }
            }

            int indicesIdx = 0;

            // bottom
            for (int sliceNumber = 0; sliceNumber <= slices; ++sliceNumber)
            {
                indices[indicesIdx++] = (ushort)(0);
                indices[indicesIdx++] = (ushort)(1 + sliceNumber);
            }
            indices[indicesIdx++] = (ushort)(1 + slices);
            indices[indicesIdx++] = (ushort)(1 + slices);

            for (int stackNumber = 0; stackNumber < stacks; ++stackNumber)
            {
                for (int sliceNumber = 0; sliceNumber <= slices; ++sliceNumber)
                {
                    indices[indicesIdx++] = (ushort)((1 + slices + 1) + (stackNumber + 1) * (slices + 1) + sliceNumber);
                    indices[indicesIdx++] = (ushort)((1 + slices + 1) + stackNumber * (slices + 1) + sliceNumber);
                }
            }

            vbuf = Device.Current.CreateVertexBuffer(BufferUsage.StaticDraw, vertices);
            ibuf = Device.Current.CreateIndexBuffer(BufferUsage.StaticDraw, indices);

            return indicesIdx;
        }

    }
}
