using Game.Graphics.Renderer.OpenGL;
using Game.Graphics;

namespace Game
{
    public sealed class Box
    {
        Matrix4 matrix;
        public Material material { get; private set; }

        public Box(Matrix4 matrix, Material material)
        {
            this.matrix = matrix;
            this.material = material;
        }

        public Box(Vector3 min_point, Vector3 max_point, Material material)
        {
            matrix =
                Matrix4.Scale(max_point - min_point)*
                Matrix4.Translation(min_point);
            this.material = material;
        }

        public void Render(Matrix4 viewMatrix)
        {
            Device device = Device.Current;

            device.SetMatrix(MatrixModeEnum.ModelView, matrix * viewMatrix);

            device.DrawElements(BeginMode.Triangles, 0, 6 * 6);
        }
    }
}
