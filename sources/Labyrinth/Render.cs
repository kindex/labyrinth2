using System;
using System.Collections.Generic;
using System.Text;
using Game.Graphics.Renderer.OpenGL;
using Game.Labyrinth.Character;
using Game.Graphics;

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
                device.CurrentShader.GetSampler("texture").Set(character.Body.material.Texture);
                device.CurrentShader.GetSampler("texture_nmap").Set(character.Body.material.TextureNMap);
                character.Body.Render(viewMatrix);
            }
        }

        void RenderDebug(Matrix4 projectionMatrix, Matrix4 viewMatrix)
        {
            if (gameFlags.debugWifreframe)
            {
                using (RenderStateBinder binder = device.ChangeRenderState())
                {
                    device.RenderState.PolygonMode = Graphics.Renderer.OpenGL.PolygonMode.Line;
                    device.RenderState.PolygonOffset = false;
                    device.RenderState.DepthTest = false;
                    device.RenderState.DepthFunction = DepthFunction.Always;

                    device.ApplyRenderState();

                    wire.Begin();
                    foreach (Box box in boxes)
                    {
                        box.physic_body.Collision.ForEachPolygonDo(
                            box.physic_body.Matrix, new Physics.Newton.CollisionIterator(delegate(Vector3[] vertices, int faceId)
                            {
                                wire.AddPolygon(vertices);
                            }));
                    }

                    foreach (Character character in characters)
                    {
                        character.Body.physic_body.Collision.ForEachPolygonDo(
                            character.Body.physic_body.Matrix, new Physics.Newton.CollisionIterator(delegate(Vector3[] vertices, int faceId)
                        {
                            wire.AddPolygon(vertices);
                        }));
                    }
                    wire.End();
                    wire.Render(viewMatrix);
                }
            }
        }
    }
}
