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
            device.CurrentShader.GetSampler("texture").Set(floorTexture);
            device.CurrentShader.GetSampler("texture_nmap").Set(floorTextureNMap);

            device.SetVertexBuffer(floorVB, 0);
            device.SetIndexBuffer(floorIB);

            device.SetMatrix(MatrixModeEnum.ModelView, viewMatrix);
            device.DrawElements(BeginMode.Triangles, 0, 6);

            device.CurrentShader.GetSampler("texture").Set(boxTexture);
            device.CurrentShader.GetSampler("texture_nmap").Set(boxTextureNMap);

            device.SetVertexBuffer(boxVB, 0);
            device.SetIndexBuffer(boxIB);

            for (int i = 0; i < boxes.Length; i++)
            {
                boxes[i].Render(viewMatrix);
            }
        }

    }
}
