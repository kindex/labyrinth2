using System;
using System.Collections.Generic;
using System.Text;
using Game.Graphics.Renderer.OpenGL;

namespace Game
{
    sealed partial class Game
    {
        void Create()
        {
            labyrinth_matrix = Labyrinth.Generator.Generator.Generate(5, 5, 0);

            Vector3[] quadN = { 
                new Vector3(1,0,0), new Vector3(-1, 0, 0),
                new Vector3(0,1,0), new Vector3( 0,-1, 0),
                new Vector3(0,0,1), new Vector3( 0, 0,-1),
            };

            Vector3[] quadX = {
                new Vector3( 0,0,1), new Vector3( 0, 0,-1),
                new Vector3( 1,0,0), new Vector3(-1, 0, 0),
                new Vector3(-1,0,0), new Vector3( 1, 0, 0),
            };

            ushort[] indices = new ushort[6 * 6];

            VF.PositionTexcoordNT[] boxDataVB = new VF.PositionTexcoordNT[6 * 4];
            for (int quad = 0; quad < quadN.Length; quad++)
            {
                boxDataVB[4 * quad + 0].Normal = quadN[quad];
                boxDataVB[4 * quad + 1].Normal = quadN[quad];
                boxDataVB[4 * quad + 2].Normal = quadN[quad];
                boxDataVB[4 * quad + 3].Normal = quadN[quad];

                boxDataVB[4 * quad + 0].Position = Vector3.Half + quadN[quad] * 0.5f - quadX[quad] * 0.5f - Vector3.Cross(quadN[quad], quadX[quad]) * 0.5f;
                boxDataVB[4 * quad + 1].Position = Vector3.Half + quadN[quad] * 0.5f + quadX[quad] * 0.5f - Vector3.Cross(quadN[quad], quadX[quad]) * 0.5f;
                boxDataVB[4 * quad + 2].Position = Vector3.Half + quadN[quad] * 0.5f + quadX[quad] * 0.5f + Vector3.Cross(quadN[quad], quadX[quad]) * 0.5f;
                boxDataVB[4 * quad + 3].Position = Vector3.Half + quadN[quad] * 0.5f - quadX[quad] * 0.5f + Vector3.Cross(quadN[quad], quadX[quad]) * 0.5f;

                boxDataVB[4 * quad + 0].Texcoord = new Vector2(0, 0);
                boxDataVB[4 * quad + 1].Texcoord = new Vector2(1, 0);
                boxDataVB[4 * quad + 2].Texcoord = new Vector2(1, 1);
                boxDataVB[4 * quad + 3].Texcoord = new Vector2(0, 1);

                indices[6 * quad + 0] = (ushort)(4 * quad + 0);
                indices[6 * quad + 1] = (ushort)(4 * quad + 1);
                indices[6 * quad + 2] = (ushort)(4 * quad + 2);
                indices[6 * quad + 3] = (ushort)(4 * quad + 0);
                indices[6 * quad + 4] = (ushort)(4 * quad + 2);
                indices[6 * quad + 5] = (ushort)(4 * quad + 3);
            }

            Utils.calculate_TB(boxDataVB, indices);

            boxVB = device.CreateVertexBuffer(BufferUsage.StaticDraw, boxDataVB);
            boxIB = device.CreateIndexBuffer(BufferUsage.StaticDraw, indices);

            boxTexture = Loaders.LoadTexture2D_RGBA("texcol3_001.jpg", true);
            boxTexture.SetWrap(TextureWrap.Repeat, TextureWrap.Repeat);
            boxTexture.SetFilterAnisotropy(4.0f);

            boxTextureNMap = Loaders.LoadTexture2D_RGBA("texcol3_001n.png", true);
            boxTextureNMap.SetWrap(TextureWrap.Repeat, TextureWrap.Repeat);
            boxTextureNMap.SetFilterAnisotropy(4.0f);


            boxes.Add(new Box(new Vector3(0, -0.1f, 0), new Vector3(labyrinth_matrix.dim_x, 0, labyrinth_matrix.dim_y))); // floor
            boxes.Add(new Box(new Vector3(0, 0, 0), new Vector3(0.1f, 3, 0.1f))); // start
            boxes.Add(new Box(new Vector3(labyrinth_matrix.dim_x, 0, labyrinth_matrix.dim_y), new Vector3(labyrinth_matrix.dim_x + 0.1f, 5, labyrinth_matrix.dim_y + 0.1f))); // finish

            for (int x = -1; x <= labyrinth_matrix.dim_x; x++)
            {
                for (int y = -1; y <= labyrinth_matrix.dim_y; y++)
                {
                    if (labyrinth_matrix.isUpBorder(x, y))
                    {
                        boxes.Add(new Box(new Vector3(x, 0, y+1), new Vector3(x + 1, 1, y + 1.1f)));
                    }
                    if (labyrinth_matrix.isRightBorder(x, y))
                    {
                        boxes.Add(new Box(new Vector3(x+1, 0, y), new Vector3(x + 1.1f, 1, y + 1)));
                    }
                }
            }

            for (int i = 0; i < pointlights; i++)
            {
                lights.Add(new PointLight()
                {
                    Ambient = 0.2f,
                    Diffuse = 0.4f,
                    Specular = 0.5f,
                    Shininess = 40.0f,
                    Radius = 2.0f,
                    Position = Vector3.Zero,
                    Color = new Vector3((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble())
                });
            }

            Vector3 cameraPosition = new Vector3(0, 0, 0);
            Vector3 cameraTarget = new Vector3(1, 0, 0);
            camera.SetPosition(cameraPosition, cameraTarget, Vector3.UnitY);
        }
    }
}
