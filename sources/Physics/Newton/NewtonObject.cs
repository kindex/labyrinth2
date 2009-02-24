using System;

namespace Game.Physics.Newton
{
    public abstract class NewtonObject : IDisposable
    {
        protected NewtonObject(IntPtr handle)
        {
            this.handle = handle;
        }

        public virtual void Dispose()
        {
            if (handle == IntPtr.Zero)
            {
                return;
            }
            
            ReleaseHandle();
            handle = IntPtr.Zero;
        }

        protected abstract void ReleaseHandle();

        internal IntPtr handle;
    }
}
