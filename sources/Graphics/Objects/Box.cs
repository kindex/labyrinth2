using Game.Graphics.Renderer.OpenGL;
using Game.Graphics;

namespace Game
{
    public sealed class Box
    {
        public Material material { get; private set; }
        public Physics.Newton.Body physic_body { get; private set; }
        public Vector3 inertia { get; private set; }

        Matrix4 physicOffsetMatrix;

        public Box(Vector3 min_point, Vector3 max_point, Material material, Physics.Newton.World world)
        {
            Vector3 box_size = max_point - min_point;
            Physics.Newton.ConvexCollision collision = world.CreateBox(box_size);
            physic_body = new Physics.Newton.Body(world, collision);

            Vector3 origin;
            Vector3 inertia;
            collision.CalculateInertialMatrix(out inertia, out origin);
            this.inertia = inertia;
            physic_body.Matrix = Matrix4.Translation((min_point + max_point)/2);
            physic_body.CentreOfMass = origin;

            physicOffsetMatrix = Matrix4.Scale(box_size);

            collision.Release();

            this.material = material;
        }

        public void Render(Matrix4 viewMatrix)
        {
            Device device = Device.Current;
            device.SetMatrix(MatrixModeEnum.ModelView, physicOffsetMatrix * physic_body.Matrix * viewMatrix);
            device.DrawElements(BeginMode.Triangles, 0, 6 * 6);
        }
    }
}
