using System;

namespace Game.Graphics.Window
{
    public interface IPlatformContext : IDisposable
    {
        void ShareWith(IPlatformContext newContext);
        void MakeCurrent();
        void SwapBuffers();

        bool VSync { get; set; }
        int Samples { get; }

        long SupportedSamples { get; }
    }
}
