using System;
using System.Diagnostics;

namespace Game
{
    public sealed class FPS
    {
        public void Update()
        {
            totalFrames++;
            frames++;

            long nowTime = Stopwatch.GetTimestamp();
            long elapsedMs = (nowTime - lastTime) * 1000 / Stopwatch.Frequency;
            if (elapsedMs > 1000)
            {
                Framerate = (uint)(frames * 1000 / elapsedMs);

                frames = 0;
                lastTime = nowTime;
            }
        }

        public uint Framerate { get; private set; }

        uint frames = 0;
        long totalFrames = 0;
        long lastTime = Stopwatch.GetTimestamp();
    }
}
