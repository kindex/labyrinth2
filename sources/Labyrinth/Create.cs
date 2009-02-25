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
            floorTexture = Loaders.LoadTexture2D_RGBA("texcol4_032.jpg", true);
            floorTexture.SetWrap(TextureWrap.Repeat, TextureWrap.Repeat);
            floorTexture.SetFilterAnisotropy(4.0f);

            floorTextureNMap = Loaders.LoadTexture2D_RGBA("texcol4_032n.png", true);
            floorTextureNMap.SetWrap(TextureWrap.Repeat, TextureWrap.Repeat);
            floorTextureNMap.SetFilterAnisotropy(4.0f);

            VF.PositionTexcoordNT[] dataVB = new VF.PositionTexcoordNT[4];
            dataVB[0] = new VF.PositionTexcoordNT() { Position = new Vector3(-10, 0, -10), Texcoord = new Vector2(0, 0), Normal = Vector3.UnitY };
            dataVB[1] = new VF.PositionTexcoordNT() { Position = new Vector3(-10, 0, +10), Texcoord = new Vector2(0, 20), Normal = Vector3.UnitY };
            dataVB[2] = new VF.PositionTexcoordNT() { Position = new Vector3(+10, 0, +10), Texcoord = new Vector2(20, 20), Normal = Vector3.UnitY };
            dataVB[3] = new VF.PositionTexcoordNT() { Position = new Vector3(+10, 0, -10), Texcoord = new Vector2(20, 0), Normal = Vector3.UnitY };

            ushort[] dataIB = new ushort[] { 0, 1, 2, 0, 2, 3 };
            Utils.calculate_TB(dataVB, dataIB);

            floorVB = device.CreateVertexBuffer(BufferUsage.StaticDraw, dataVB);
            floorIB = device.CreateIndexBuffer(BufferUsage.StaticDraw, dataIB);


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
                boxDataVB[4 * quad + 0].Position = quadN[quad] * 0.5f - quadX[quad] * 0.5f - Vector3.Cross(quadN[quad], quadX[quad]) * 0.5f;
                boxDataVB[4 * quad + 1].Position = quadN[quad] * 0.5f + quadX[quad] * 0.5f - Vector3.Cross(quadN[quad], quadX[quad]) * 0.5f;
                boxDataVB[4 * quad + 2].Position = quadN[quad] * 0.5f + quadX[quad] * 0.5f + Vector3.Cross(quadN[quad], quadX[quad]) * 0.5f;
                boxDataVB[4 * quad + 3].Position = quadN[quad] * 0.5f - quadX[quad] * 0.5f + Vector3.Cross(quadN[quad], quadX[quad]) * 0.5f;

                for (int t = 0; t < 4; t++)
                {
                    boxDataVB[4 * quad + t].Position.X *= 0.4f;
                    //boxDataVB[4 * quad + t].Position.Y *= 0.2f;
                    //boxDataVB[4 * quad + t].Position.Y -= 0.3f;
                    boxDataVB[4 * quad + t].Position.Z *= 0.4f;
                }

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

            boxes = new Box[boxCount];
            for (int i = 0; i < boxCount; i++)
            {
                boxes[i] = new Box(Matrix4.Translation(new Vector3(-1.0f, 0.5f, (i - boxCount / 3) * 1.5f)));
            }
            //for (int i = boxCount; i < 2*boxCount; i++)
            //{
            //    boxes[i] = new Box(Matrix4.Translation(new Vector3(1.0f, 0.5f, (i - boxCount - boxCount / 3) * 1.5f)));
            //}

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
        }
    }
}
