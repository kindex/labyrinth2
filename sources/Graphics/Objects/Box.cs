using Game.Graphics.Renderer.OpenGL;

namespace Game
{
    public sealed class Box
    {
        Matrix4 matrix;

        public Box(Matrix4 matrix)
        {
            this.matrix = matrix;
        }

        public Box(Vector3 min_point, Vector3 max_point)
        {
            matrix =
                Matrix4.Scale(max_point - min_point)*
                Matrix4.Translation(min_point);
        }

        public void Render(Matrix4 viewMatrix)
        {
            Device device = Device.Current;

            device.SetMatrix(MatrixModeEnum.ModelView, matrix * viewMatrix);

            device.DrawElements(BeginMode.Triangles, 0, 6 * 6);
        }
    }
}
