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
    sealed partial class Game : GameWindow
    {
        public Game() : base(1024, 768, 0, "Labyrinth2", false, false)
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

        static Game()
        {
        }

        public override void OnRender(float deltaTime)
        {
            time += deltaTime;

            float zNear = 0.1f;
            float zFar = 100.0f;

            Matrix4 projectionMatrix = Matrix4.Perspective(45.0f, (float)device.Width / (float)device.Height, zNear, zFar);

            renderer.Render(RenderObjects, projectionMatrix, camera.ViewMatrix);
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
            Create();
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
    }
}
