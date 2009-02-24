using System;
using System.Configuration;

namespace Game.Audio
{
    public sealed class AudioManager : IDisposable
    {
        public AudioManager()
        {
        }

        ~AudioManager()
        {
            Dispose(false);
        }

        public void Restart()
        {
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        bool disposed = false;

        void Dispose(bool disposing)
        {
            if (!disposed)
            {
                disposed = true;
            }
        }

    }
}
