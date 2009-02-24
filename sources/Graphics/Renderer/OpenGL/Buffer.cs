using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Game.Graphics.Renderer.OpenGL
{
    public class Buffer : IDisposable
    {
        protected Buffer(BufferTarget target, BufferUsage usage, int count, int elementSize) : this(target)
        {
            this.elementSize = elementSize;
            GL.BufferData(target, count * elementSize, IntPtr.Zero, usage);
        }

        protected Buffer(BufferTarget target)
        {
            this.target = target;
            this.buffer = GL.GenBuffer();

            GL.BindBuffer(target, buffer);
        }

        protected void SetData<T>(BufferUsage usage, T[] data) where T : struct
        {
            Bind();
            this.elementSize = Marshal.SizeOf(typeof(T));
            GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            try
            {
                GL.BufferData(target, data.Length * elementSize, handle.AddrOfPinnedObject(), usage);
            }
            finally
            {
                handle.Free();
            }
        }

        public void Dispose()
        {
            GL.DeleteBuffer(buffer);
            buffer = 0;
            Device.Current.Dispose(this);
        }

        public IntPtr Map(BufferAccess access)
        {
            Bind();
            return GL.MapBuffer(target, access);
        }

        public void Unmap()
        {
            GL.UnmapBuffer(target);
        }

        public void SetSubData<T>(int offset, int count, T[] data) where T : struct
        {
            Debug.Assert(elementSize == Marshal.SizeOf(typeof(T)) && typeof(T).IsLayoutSequential);

            Bind();
            GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            try
            {
                GL.BufferSubData(target, offset * elementSize, count * elementSize, handle.AddrOfPinnedObject());
            }
            finally
            {
                handle.Free();
            }
        }

        public void GetSubData<T>(int offset, int count, T[] data) where T : struct
        {
            Debug.Assert(elementSize == Marshal.SizeOf(typeof(T)) && typeof(T).IsLayoutSequential);

            Bind();
            GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            try
            {
                GL.GetBufferSubData(target, offset * elementSize, count * elementSize, handle.AddrOfPinnedObject());
            }
            finally
            {
                handle.Free();
            }
        }

        internal void Bind()
        {
            if (target == BufferTarget.ElementArrayBuffer)
            {
                if (Device.Current.currentVertexBuffer != this)
                {
                    Device.Current.currentVertexBuffer = null;
                    GL.BindBuffer(target, buffer);
                }
            }
            else if (target == BufferTarget.ElementArrayBuffer)
            {
                if (Device.Current.currentIndexBuffer != this)
                {
                    Device.Current.currentIndexBuffer = null;
                    GL.BindBuffer(target, buffer);
                }
            }
            else
            {
                GL.BindBuffer(target, buffer);
            }
        }

        internal void Bind(VertexAttribute[] attributes, int offset)
        {
            if (Device.Current.currentVertexBuffer == this)
            {
                return;
            }
            Device.Current.currentVertexBuffer = this;

            GL.BindBuffer(target, buffer);
            for (int i = 0; i < attributes.Length; i++)
            {
                GL.VertexAttribPointer(i, attributes[i].count, attributes[i].type, true, elementSize, new IntPtr(offset + attributes[i].offset));
            }

            int activeAttributes = Device.Current.activeVertexAttributes;
            if (activeAttributes < attributes.Length)
            {
                for (int i = activeAttributes; i < attributes.Length; i++)
                {
                    GL.EnableVertexAttribArray(i);
                }
            }
            else if (activeAttributes > attributes.Length)
            {
                for (int i = attributes.Length; i < activeAttributes; i++)
                {
                    GL.DisableVertexAttribArray(i);
                }
            }
            Device.Current.activeVertexAttributes = attributes.Length;
        }

        BufferTarget target;
        uint buffer;
        internal int elementSize;
    }

    public sealed class VertexBuffer<T> : Buffer where T : struct
    {
        internal VertexBuffer(BufferUsage usage, T[] data)
            : base(BufferTarget.ArrayBuffer)
        {
            SetData(usage, data);
        }

        internal VertexBuffer(BufferUsage usage, int count)
            : base(BufferTarget.ArrayBuffer, usage, count, Marshal.SizeOf(typeof(T)))
        {
        }
    }

    public sealed class IndexBuffer<T> : Buffer where T : struct
    {
        internal IndexBuffer(BufferUsage usage, T[] data)
            : base(BufferTarget.ElementArrayBuffer)
        {
            SetData(usage, data);
        }

        internal IndexBuffer(BufferUsage usage, int count)
            : base(BufferTarget.ElementArrayBuffer, usage, count, Marshal.SizeOf(typeof(T)))
        {
        }
    }

    public sealed class PixelPackBuffer<T> : Buffer where T : struct
    {
        internal PixelPackBuffer(BufferUsage usage, int count)
            : base(BufferTarget.PixelPackBuffer, usage, count, Marshal.SizeOf(typeof(T)))
        {
        }
    }

    public sealed class PixelUnpackBuffer<T> : Buffer where T : struct
    {
        internal PixelUnpackBuffer(BufferUsage usage, int count)
            : base(BufferTarget.PixelUnpackBuffer, usage, count, Marshal.SizeOf(typeof(T)))
        {
        }
    }
}
