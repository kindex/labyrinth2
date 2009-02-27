using System;
using System.Collections.Generic;
using System.Text;
using Game.Graphics.Renderer.OpenGL;
using Game.Labyrinth.Character;

namespace Game
{
    sealed partial class Game
    {
        void RenderObjects(Matrix4 viewMatrix)
        {
            device.SetVertexBuffer(boxVB, 0);
            device.SetIndexBuffer(boxIB);

            foreach (Box box in boxes)
            {
                device.CurrentShader.GetSampler("texture").Set(box.material.Texture);
                device.CurrentShader.GetSampler("texture_nmap").Set(box.material.TextureNMap);
                box.Render(viewMatrix);
            }

            foreach (Character character in characters)
            {
                device.CurrentShader.GetSampler("texture").Set(character.Graph.material.Texture);
                device.CurrentShader.GetSampler("texture_nmap").Set(character.Graph.material.TextureNMap);
                character.Graph.Render(viewMatrix);
            }
        }
    }
}
