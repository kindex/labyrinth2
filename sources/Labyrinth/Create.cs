﻿using System;
using System.Collections.Generic;
using System.Text;
using Game.Graphics.Renderer.OpenGL;
using Game.Labyrinth.Character;
using Game.Graphics;

namespace Game
{
    sealed partial class Game
    {
        //bool ContactProcessEventHandler(Physics.Newton.MaterialCollision material, Physics.Newton.Body body0, Physics.Newton.Body body1, float timestep, int threadIndex)
        //{
        //    return true;
        //}

        void Create()
        {
            // Physic
            physic_world = new Physics.Newton.World();
            //material_pair = new Physics.Newton.MaterialPair(physic_world, physic_world.DefaultMaterialID, physic_world.DefaultMaterialID);
            //material_pair.SetCollisionCallback(null, ContactProcessEventHandler);


            // create box graph
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

            Material box_m = new Material("wood_box");
            Material floor_m = new Material("metal");

            // create world
            labyrinth_matrix = Labyrinth.Generator.Generator.Generate(7, 7, 0);
            ceil_size = new Vector3(2, 1, 3);

            Box floor = new Box(new Vector3(0, -0.1f, 0), new Vector3(labyrinth_matrix.dim_x, 0, labyrinth_matrix.dim_y).MemberMul(ceil_size), floor_m, physic_world);

            boxes.Add(floor);
            boxes.Add(new Box(new Vector3(0, 0, 0), new Vector3(0.1f, 3, 0.1f), box_m, physic_world)); // start
            boxes.Add(new Box(new Vector3(labyrinth_matrix.dim_x, 0, labyrinth_matrix.dim_y).MemberMul(ceil_size), new Vector3(labyrinth_matrix.dim_x + 0.1f, 5, labyrinth_matrix.dim_y + 0.1f).MemberMul(ceil_size), box_m, physic_world)); // finish

            for (int x = -1; x <= labyrinth_matrix.dim_x; x++)
            {
                for (int y = -1; y <= labyrinth_matrix.dim_y; y++)
                {
                    if (labyrinth_matrix.isUpBorder(x, y))
                    {
                        boxes.Add(new WallBox(new Vector3(x, 0, y + 1).MemberMul(ceil_size), new Vector3(x + 1, 1, y + 1.1f).MemberMul(ceil_size), box_m, physic_world));
                    }
                    if (labyrinth_matrix.isRightBorder(x, y))
                    {
                        boxes.Add(new WallBox(new Vector3(x + 1, 0, y).MemberMul(ceil_size), new Vector3(x + 1.1f, 1, y + 1).MemberMul(ceil_size), box_m, physic_world));
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
                    Position = ceil_size * 0.5f,
                    Color = new Vector3((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble()).GetNormalized()
                });
            }

            // Characters
            Character character = new Character(box_m);
            character.PlaceToScene(ceil_size * 0.5f, physic_world);
            characters.Add(character);
            active_character = character;

            character = new Character(box_m);
            character.PlaceToScene(ceil_size * 0.5f + new Vector3(0, 0, 1), physic_world);
            characters.Add(character);

            // Camera
            MouseCaptured = true;

            Vector3 cameraPosition = new Vector3(0.5f, 2, 0.5f);
            Vector3 cameraTarget = new Vector3(2, 0, 2);

            //camera = new SpectatorCamera(cameraPosition, cameraTarget);
            camera = new ThirdPersonCamera(active_character.Position, Vector3.UnitY, maxOrbitRadius);

            gameFlags.debugWifreframe = false;
        }
    }
}
