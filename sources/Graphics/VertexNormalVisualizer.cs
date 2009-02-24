using System.IO;

using Game.Graphics.Renderer.OpenGL;

namespace Game.Graphics
{
    public delegate Vector3 GetPositionDelegate<T>(T vertex);
    public delegate Vector3 GetNormalDelegate<T>(T vertex);

    public sealed class VertexNormalVisualizer<T> : System.IDisposable
    {
        public VertexNormalVisualizer(Device device, T[] vbuf, float scale, GetPositionDelegate<T> getPosition, GetNormalDelegate<T> getNormal)
        {
            Vector3[] normals = new Vector3[vbuf.Length * 2];
            for (int i = 0; i < vbuf.Length; i++)
            {
                normals[2 * i + 0] = getPosition(vbuf[i]);
                normals[2 * i + 1] = normals[2 * i + 0] + getNormal(vbuf[i]) * scale;
            }

            this.primitiveCount = vbuf.Length * 2;
            this.vbuf = device.CreateVertexBuffer(BufferUsage.StaticDraw, normals);

            shader = Loaders.LoadShader<VF.Position3>("only_color");
            SetColor(new Vector3(1, 1, 1));
        }

        public void Dispose()
        {
            shader.Dispose();
            vbuf.Dispose();
        }

        public void SetColor(Vector3 color)
        {
            shader.GetUniform("fragment_color").Set(color);
        }

        public void Render(Matrix4 viewMatrix)
        {
            Device device = Device.Current;

            device.SetShader(shader);
            device.SetVertexBuffer(vbuf, 0);

            device.SetMatrix(MatrixModeEnum.ModelView, viewMatrix);
            device.DrawArrays(BeginMode.Lines, 0, primitiveCount);
        }

        Shader shader;
        Buffer vbuf;
        int primitiveCount;
    }
}
