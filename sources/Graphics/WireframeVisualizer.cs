using System.Collections.Generic;
using System.IO;

using Game.Graphics.Renderer.OpenGL;

namespace Game.Graphics
{
    public sealed class WireframeVisualizer : System.IDisposable
    {
        public WireframeVisualizer(Device device)
        {
            vbuf = Device.Current.CreateVertexBuffer<Vector3>(BufferUsage.DynamicDraw, 1024 * 1024);
            ibuf = Device.Current.CreateIndexBuffer<uint>(BufferUsage.DynamicDraw, 1024 * 1024);

            shader = Loaders.LoadShader<VF.Position3>("only_color");
            SetColor(new Vector3(1, 1, 1));
        }

        public void Dispose()
        {
            shader.Dispose();
            vbuf.Dispose();
            ibuf.Dispose();
        }

        public void SetColor(Vector3 color)
        {
            shader.GetUniform("fragment_color").Set(color);
        }
        
        public void Begin()
        {
            positions.Clear();
            indices.Clear();
        }

        public void AddPolygon(params Vector3[] pos)
        {
            if (positions.Count + pos.Length < 1024 * 1024 && indices.Count + 2 * pos.Length < 1024 * 1024)
            {
                int first = positions.Count;
                for (int i = 0; i < pos.Length - 1; i++)
                {
                    indices.Add((uint)positions.Count);
                    indices.Add((uint)positions.Count + 1);
                    positions.Add(pos[i]);
                }
                indices.Add((uint)positions.Count);
                indices.Add((uint)first);
                positions.Add(pos[pos.Length - 1]);
            }
        }

        public void End()
        {
            vbuf.SetSubData(0, positions.Count, positions.ToArray());
            ibuf.SetSubData(0, indices.Count, indices.ToArray());
        }

        public void Render(Matrix4 viewMatrix)
        {
            Device device = Device.Current;

            device.SetShader(shader);
            device.SetVertexBuffer(vbuf, 0);
            device.SetIndexBuffer(ibuf);

            device.SetMatrix(MatrixModeEnum.ModelView, viewMatrix);
            device.DrawElements(BeginMode.Lines, 0, indices.Count);
        }

        List<Vector3> positions = new List<Vector3>();
        List<uint> indices = new List<uint>();

        Shader shader;
        Buffer vbuf;
        Buffer ibuf;
    }
}
