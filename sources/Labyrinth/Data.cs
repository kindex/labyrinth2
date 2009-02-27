using System;
using System.Collections.Generic;
using System.Text;
using Game.Graphics.Renderer.OpenGL;

namespace Game
{
    sealed partial class Game
    {
        FPS fps = new FPS();
        Vector3 move = Vector3.Zero;
        Device device;
        bool visualize_volume = false;
        List<Box> boxes = new List<Box>();

        Graphics.Renderer.OpenGL.Buffer boxVB;
        Graphics.Renderer.OpenGL.Buffer boxIB;

        Texture2D boxTexture;
        Texture2D boxTextureNMap;

        const int pointlights = 10;
        Random random = new Random();
        float time = 0;
        List<Light> lights = new List<Light>();
        Shader single_color;
        DeferredRenderer renderer;
        SpectatorCamera camera = new SpectatorCamera();

        // Labyrinth
        Labyrinth.Generator.Matrix labyrinth_matrix;
        Vector3 ceil_size;
    }
}
