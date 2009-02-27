using System;
using System.Collections.Generic;
using System.Text;
using Game.Graphics.Renderer.OpenGL;

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

        }

    }
}
