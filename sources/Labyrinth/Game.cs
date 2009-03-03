using System;
using System.IO;
using System.Runtime.InteropServices;

using System.Collections.Generic;

using Game.Graphics.Window;
using Game.Graphics.Window.InputEvents;
using Game.Graphics.Renderer.OpenGL;
using Game.Physics.Newton;
using Game.Graphics;

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
            boxVB.Dispose();
            boxIB.Dispose();
            //boxTexture.Dispose();
            //boxTextureNMap.Dispose();
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
            RenderDebug(projectionMatrix, camera.ViewMatrix);

            renderer.Finish();

            fps.Update();

            Title = fps.Framerate.ToString();
        }

        public override void OnLoad()
        {
            this.device = new Device(Width, Height);
            device.RenderState.DepthTest = true;
            device.ApplyRenderState();

            OnResize(Width, Height);

            renderer = new DeferredRenderer(device);
            Create();
            single_color = Loaders.LoadShader<VF.Position3>("single_color");

            wire = new WireframeVisualizer(device);
        }

        public override void OnUnload()
        {
            renderer.Dispose();

            DisposeObjects();

            device.Dispose();
        }
    }
}
