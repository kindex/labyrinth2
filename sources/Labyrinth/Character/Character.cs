using System;
using System.Collections.Generic;
using System.Text;
using Game.Graphics;

namespace Game.Labyrinth.Character
{
    class Character
    {
        public readonly Vector3 box_size = Vector3.Half;
        const float mass = 1.0f;

        public string Name { get; private set; }
        public Box Body { get; private set; }

        Material material;

        public Character(Material material)
        {
            this.material = material;
        }

        public void PlaceToScene(Vector3 position, Physics.Newton.World world)
        {
            Body = new Box(position - box_size / 2, position + box_size / 2, material, world);

            Body.physic_body.SetMass(mass, mass*Body.inertia);

            // global gravity
            Body.physic_body.SetForceAndTorqueEvent += delegate(Physics.Newton.Body body, float timestep, int threadIndex)
                {
                    body.AddForce(new Vector3(0, -9.8f, 0) * mass);
                };
        }

        public Vector3 Position
        {
            get
            {
                return Body.physic_body.Matrix.Posit;
            }
        }
    }
}
