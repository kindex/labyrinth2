using System;

namespace Game.Graphics.Renderer.OpenGL
{
    static class Loader
    {
        static IntPtr handle;
        static int count = 0;

        public static void Load()
        {
            if (count++ == 0)
            {
                Console.WriteLine("Loading opengl32.dll");
                handle = NativeLoader.Load("opengl32.dll");
                NativeLoader.LoadDelegates(handle, typeof(GL), Window.Windows.NativeAPI.wglGetProcAddress);
            }
        }

        public static void Unload()
        {
            if (--count == 0)
            {
                Console.WriteLine("Unloading opengl32.dll");
                NativeLoader.Unload(handle);
            }
        }
    }
}
