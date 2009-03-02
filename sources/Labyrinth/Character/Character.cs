using System;
using System.Collections.Generic;
using System.Text;
using Game.Graphics;

namespace Game.Labyrinth.Character
{
    class Character
    {
        readonly Vector3 box_size = Vector3.Half;
        const float mass = 1.0f;

        public string Name { get; private set; }
        public Box Graph { get; private set; }

        Material material;

        public Character(Material material)
        {
            this.material = material;
        }

        public void PlaceToScene(Vector3 position, Physics.Newton.World world)
        {
            Graph = new Box(position - box_size / 2, position + box_size / 2, material, world);

            Graph.physic_body.SetMass(mass, mass*Graph.inertia);
            Graph.physic_body.SetForceAndTorqueEvent += delegate(Physics.Newton.Body body, float timestep, int threadIndex)
                {
                    body.AddForce(new Vector3(0, -9.8f, 0) * mass);
                };
        }
    }
}
