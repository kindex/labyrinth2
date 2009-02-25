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
            device.CurrentShader.GetSampler("texture").Set(boxTexture);
            device.CurrentShader.GetSampler("texture_nmap").Set(boxTextureNMap);

            device.SetVertexBuffer(boxVB, 0);
            device.SetIndexBuffer(boxIB);

            foreach (Box box in boxes)
            {
                box.Render(viewMatrix);
            }
        }

    }
}
