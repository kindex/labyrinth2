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
        FPS fps = new FPS();
        Vector3 move = Vector3.Zero;
        Device device;
        bool visualize_volume = false;
        List<Box> boxes = new List<Box>();

        Graphics.Renderer.OpenGL.Buffer boxVB;
        Graphics.Renderer.OpenGL.Buffer boxIB;

        const int pointlights = 4;
        Random random = new Random();
        float time = 0;
        List<Light> lights = new List<Light>();
        Shader single_color;
        DeferredRenderer renderer;
        WireframeVisualizer wire;

        Camera camera;

        // Labyrinth
        Labyrinth.Generator.Matrix labyrinth_matrix;
        Vector3 ceil_size;

        // Characters
        List<Character> characters = new List<Character>();
        Character active_character;

        // Physic
        Physics.Newton.World physic_world;

        // Game
        enum CameraMode
        {
            Free,
            CharacterBind
        }
    }
}
