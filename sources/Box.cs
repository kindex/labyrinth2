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

        public void Render(Matrix4 viewMatrix)
        {
            Device device = Device.Current;

            device.SetMatrix(MatrixModeEnum.ModelView, matrix * viewMatrix);

            device.DrawElements(BeginMode.Triangles, 0, 6 * 6);
        }
    }
}
