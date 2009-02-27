using System;
using System.Collections.Generic;
using System.Text;
using Game.Graphics;

namespace Game.Labyrinth.Character
{
    class Character
    {
        readonly Vector3 box_size = Vector3.Half;

        public string Name { get; private set; }
        public Box Graph { get; private set; }

        public Character(Vector3 position, Material material)
        {
            Graph = new Box(position - box_size / 2, position + box_size / 2, material);
        }
    }
}
