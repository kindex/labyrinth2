using System;
using System.IO;
using System.Runtime.InteropServices;

using System.Collections.Generic;

using Game.Graphics.Window;
using Game.Graphics.Window.InputEvents;
using Game.Graphics.Renderer.OpenGL;
using Game.Physics.Newton;

namespace Game
{
    public sealed class Game : GameWindow
    {
        FPS fps = new FPS();
        Vector3 move = Vector3.Zero;

        Device device;

        public Game() : base(800, 600, 0, "Test", false, false)
        {
        }

        public override void OnClose()
        {
            Close();
        }

        public override void OnResize(int width, int height)
        {
            device.Init();
            device.Resize(width, height);
        }
        
        bool visualize_volume = false;
           
        public override void OnUpdate(float deltaTime)
        {
            InputEvent ev;
            while ((ev = GetNextInputEvent()) != null)
            {
                if (ev is MouseButtonPressedEvent)
                {
                    MouseButtonPressedEvent buttonEvent = (MouseButtonPressedEvent)ev;
                    
                    if (buttonEvent.Button == MouseButton.Left)
                    {
                        MouseCaptured = true;
                    }
                }
                else if (ev is KeyPressedEvent)
                {
                    KeyPressedEvent keyEvent = (KeyPressedEvent)ev;
                    //if (keyEvent.Key == Key.M)
                    //{
                    //    Samples = 4 - Samples;
                    //    OnResize(Width, Height);
                    //}
                    //else
                    if (keyEvent.Key == Key.Escape)
                    {
                        if (MouseCaptured)
                        {
                            MouseCaptured = false;
                        }
                        else
                        {
                            Close();
                        }
                    }
                    else if (keyEvent.Key == Key.W)
                    {
                        move.Z += 1.0f;
                    }
                    else if (keyEvent.Key == Key.S)
                    {
                        move.Z -= 1.0f;
                    }
                    else if (keyEvent.Key == Key.A)
                    {
                        move.X -= 1.0f;
                    }
                    else if (keyEvent.Key == Key.D)
                    {
                        move.X += 1.0f;
                    }
                    else if (keyEvent.Key == Key.Q)
                    {
                        move.Y += 1.0f;
                    }
                    else if (keyEvent.Key == Key.Z)
                    {
                        move.Y -= 1.0f;
                    }
                    else if (keyEvent.Key == Key.Space)
                    {
                        visualize_volume = !visualize_volume;
                    }
                }
                else if (ev is KeyReleasedEvent)
                {
                    KeyReleasedEvent keyEvent = (KeyReleasedEvent)ev;
                    if (keyEvent.Key == Key.W)
                    {
                        move.Z -= 1.0f;
                    }
                    else if (keyEvent.Key == Key.S)
                    {
                        move.Z += 1.0f;
                    }
                    else if (keyEvent.Key == Key.A)
                    {
                        move.X += 1.0f;
                    }
                    else if (keyEvent.Key == Key.D)
                    {
                        move.X -= 1.0f;
                    }
                    else if (keyEvent.Key == Key.Q)
                    {
                        move.Y -= 1.0f;
                    }
                    else if (keyEvent.Key == Key.Z)
                    {
                        move.Y += 1.0f;
                    }

                }
            }

            if (MouseCaptured)
            {
                Point mousePos = this.MousePosition;
                camera.Rotate(mousePos.X * Radians.PI / Height, mousePos.Y * Radians.PI / Width);
            }

            Vector3 moveDirection = 3.0f * move;
            camera.Move(moveDirection, deltaTime);
        }


        void CreateObjects()
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
        }

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

        void DisposeObjects()
        {
            floorVB.Dispose();
            floorIB.Dispose();
            floorTexture.Dispose();
            floorTextureNMap.Dispose();

            boxVB.Dispose();
            boxIB.Dispose();
            boxTexture.Dispose();
            boxTextureNMap.Dispose();
        }

        const int boxCount = 5;
        Box[] boxes;

        Graphics.Renderer.OpenGL.Buffer floorVB;
        Graphics.Renderer.OpenGL.Buffer floorIB;
        Graphics.Renderer.OpenGL.Buffer boxVB;
        Graphics.Renderer.OpenGL.Buffer boxIB;

        Texture2D floorTexture;
        Texture2D floorTextureNMap;
        Texture2D boxTexture;
        Texture2D boxTextureNMap;

        const int pointlights = 15;
        const int spotlights = 5;

        static Vector3[] lightColors;
        static Vector3[] spotlightColors;
        
        static Game()
        {
            lightColors = new Vector3[pointlights];
            spotlightColors = new Vector3[spotlights];
            Random rnd = new Random();

            for (int i = 0; i < pointlights; i++)
            {
                lightColors[i] = new Vector3((float)rnd.NextDouble(),
                                             (float)rnd.NextDouble(),
                                             (float)rnd.NextDouble());

                lightColors[i] = lightColors[i] * 0.4f + new Vector3(0.6f);
            }

            for (int i = 0; i < spotlights; i++)
            {
                spotlightColors[i] = new Vector3((i + 1) % 4 == 0 ? 1 : 0,
                                                 (i + 1) % 2 == 0 ? 1 : 0,
                                                 (i + 1) % 3 == 0 ? 1 : 0);
            }
            spotlightColors[0].X = 1;
        }

        float time = 0;

        public override void OnRender(float deltaTime)
        {
            time += deltaTime;

            float zNear = 0.1f;
            float zFar = 100.0f;

            Matrix4 projectionMatrix = Matrix4.Perspective(45.0f, (float)device.Width / (float)device.Height, zNear, zFar);

            renderer.Render(RenderObjects, projectionMatrix, camera.ViewMatrix);
            
            List<Light> lights = new List<Light>();

            //float x = -0.7f * Radians.Sin(time / 2);
            //float y = 0.5f;
            //float z = -0.75f * Radians.Cos(time / 2);

            //float x = camera.Position.X;
            //float y = camera.Position.Y;
            //float z = camera.Position.Z;

            //float x = 0.2f;
            //float y = 0.5f;
            //float z = -0.75f;

            //lights.Add(new PointLight()
            //{
            //    Ambient = 0.2f,
            //    Diffuse = 0.6f,
            //    Specular = 0.5f,
            //    Shininess = 40.0f,
            //    Radius = 1.0f,
            //    Position = new Vector3(x, y, z),
            //    Color = Vector3.UnitX
            //});

            /*
            for (int i = 0; i < spotlights; i++)
            {
                SpotLight light = new SpotLight()
                {
                    Ambient = 0.2f,
                    Diffuse = 0.6f,
                    Specular = 0.5f,
                    Shininess = 40.0f,
                    Length = 6.0f,
                    Exponent = 1.0f,
                    Angle = 30.0f,
                    Color = spotlightColors[i]
                };
                light.SetPosAndDir(new Vector3(0.2f, 1.5f, i - 2), new Vector3(-1.0f, -1.5f, Radians.Sin(time * (0.51123f * i * i + 1) / 10)));
                lights.Add(light);
            }
            */
            for (int i = 0; i < pointlights; i++)
            {
                float x = 2.0f * Radians.Sin(time * (0.21718f * i + 1) / 10);
                float y = 0.1f;
                float z = 2.0f * Radians.Cos(time * (0.31434f * i + 1) / 10);
                
                lights.Add(new PointLight()
                {
                    Ambient = 0.2f,
                    Diffuse = 0.4f,
                    Specular = 0.5f,
                    Shininess = 40.0f,
                    Radius = 2.0f,
                    Position = new Vector3(x, y, z),
                    Color = lightColors[i]
                });
            }

            renderer.DrawLights(RenderObjects, lights, projectionMatrix, zNear, camera);

            renderer.Finish();

            fps.Update();

            Title = fps.Framerate.ToString();
        }

        public override void OnLoad()
        {
            this.device = new Device(Width, Height);
            device.RenderState.DepthTest = true;
            device.ApplyRenderState();

            Vector3 cameraPosition = new Vector3(-1.0f, 3.0f, 5.0f);
            Vector3 cameraTarget = new Vector3(0.0f, 0.0f, 0.0f);
            camera.SetPosition(cameraPosition, cameraTarget, Vector3.UnitY);

            OnResize(Width, Height);

            renderer = new DeferredRenderer(device);
            CreateObjects();
            single_color = Loaders.LoadShader<VF.Position3>("single_color");
        }

        public override void OnUnload()
        {
            floorVB.Dispose();
            floorIB.Dispose();

            renderer.Dispose();

            DisposeObjects();

            device.Dispose();
        }

        Shader single_color;

        DeferredRenderer renderer;

        SpectatorCamera camera = new SpectatorCamera();

        static void Main()
        {
            //StreamWriter output = new StreamWriter("output.txt");
            //output.AutoFlush = true;
            //Console.SetOut(output);

            NativeLoader.AutoLoad();

            using (Game game = new Game())
            {
                game.Run();
            }
        }
    }
}
