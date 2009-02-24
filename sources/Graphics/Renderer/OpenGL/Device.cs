using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Game.Graphics.Renderer.OpenGL
{
    public sealed class Device : IDisposable
    {
        public Device(int width, int height)
        {
            Current = this;
            
            this.Width = width;
            this.Height = height;
            this.Extensions = new Extensions();

            this.currentRenderState = RenderState.Default;

            Init();
        }

        public void Init()
        {
            RenderState = RenderState.Default;
            RenderState.FrontFace = FrontFaceDirection.CW;
            RenderState.CullFace = true;
            RenderState.DepthTest = true;
            ApplyRenderState();

            Capabilities = new Caps(Extensions);

            lastMatrixMode = (MatrixModeEnum)0;
            CurrentShader = null;
            currentVertexBuffer = null;
            currentIndexBuffer = null;
            activeVertexAttributes = 0;
            lastMatrixMode = (MatrixModeEnum)0;
        }

        public void Dispose(IDisposable disposable)
        {
            if (disposables != null)
            {
                disposables.Remove(disposable);
            }
        }

        public void Dispose()
        {
            if (disposables.Count > 0)
            {
                Debug.WriteLine("WARNING: Some of OpenGL objects has not been disposed!");

                HashSet<IDisposable> objs = disposables;
                disposables = null;
                foreach (var obj in objs)
                {
                    obj.Dispose();
                }
            }

            Current = null;
        }

        public void Resize(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            GL.Viewport(0, 0, width, height);
        }

        // create

        public Texture1D CreateTexture1D(InternalFormat internalFormat, int width, PixelFormat format, PixelType type, IntPtr pixels)
        {
            Texture1D texture = new Texture1D(internalFormat, width, format, type, pixels);
            disposables.Add(texture);
            return texture;
        }

        public Texture1D CreateTexture1D(InternalFormat internalFormat, int width)
        {
            Texture1D texture = new Texture1D(internalFormat, width);
            disposables.Add(texture);
            return texture;
        }

        public Texture2D CreateTexture2D(InternalFormat internalFormat, int width, int height, PixelFormat format, PixelType type, IntPtr pixels)
        {
            Texture2D texture = new Texture2D(internalFormat, width, height, format, type, pixels);
            disposables.Add(texture);
            return texture;
        }

        public Texture2D CreateTexture2D(InternalFormat internalFormat, int width, int height, int[] mipSizes, IntPtr data)
        {
            Texture2D texture = new Texture2D(internalFormat, width, height, mipSizes, data);
            disposables.Add(texture);
            return texture;
        }

        public Texture2D CreateTexture2D(InternalFormat internalFormat, int width, int height)
        {
            Texture2D texture = new Texture2D(internalFormat, width, height);
            disposables.Add(texture);
            return texture;
        }

        public Texture3D CreateTexture3D(InternalFormat internalFormat, int width, int height, int depth, PixelFormat format, PixelType type, IntPtr pixels)
        {
            Texture3D texture = new Texture3D(internalFormat, width, height, depth, format, type, pixels);
            disposables.Add(texture);
            return texture;
        }

        public Texture3D CreateTexture3D(InternalFormat internalFormat, int width, int height, int depth)
        {
            Texture3D texture = new Texture3D(internalFormat, width, height, depth);
            disposables.Add(texture);
            return texture;
        }

        public TextureCubeMap CreateTextureCubeMap(int size)
        {
            TextureCubeMap texture = new TextureCubeMap(size);
            disposables.Add(texture);
            return texture;
        }

        public TextureCubeMap CreateTextureCubeMap(InternalFormat internalFormat, int size)
        {
            TextureCubeMap texture = new TextureCubeMap(internalFormat, size);
            disposables.Add(texture);
            return texture;
        }

        public VertexBuffer<T> CreateVertexBuffer<T>(BufferUsage usage, T[] data) where T : struct
        {
            Debug.Assert(typeof(T).IsLayoutSequential);

            VertexBuffer<T> buffer = new VertexBuffer<T>(usage, data);
            disposables.Add(buffer);
            return buffer;
        }

        public VertexBuffer<T> CreateVertexBuffer<T>(BufferUsage usage, int count) where T : struct
        {
            Debug.Assert(typeof(T).IsLayoutSequential);

            VertexBuffer<T> buffer = new VertexBuffer<T>(usage, count);
            disposables.Add(buffer);
            return buffer;
        }

        public IndexBuffer<T> CreateIndexBuffer<T>(BufferUsage usage, T[] data) where T : struct
        {
            Debug.Assert(typeof(T).IsLayoutSequential);

            IndexBuffer<T> buffer = new IndexBuffer<T>(usage, data);
            currentIndexBuffer = null;
            SetIndexBuffer(buffer);
            disposables.Add(buffer);
            return buffer;
        }

        public IndexBuffer<T> CreateIndexBuffer<T>(BufferUsage usage, int count) where T : struct
        {
            Debug.Assert(typeof(T).IsLayoutSequential);

            IndexBuffer<T> buffer = new IndexBuffer<T>(usage, count);
            currentIndexBuffer = null;
            SetIndexBuffer(buffer);
            disposables.Add(buffer);
            return buffer;
        }

        public PixelPackBuffer<T> CreatePixelPackBuffer<T>(BufferUsage usage, int count) where T : struct
        {
            Debug.Assert(typeof(T).IsLayoutSequential);

            PixelPackBuffer<T> buffer = new PixelPackBuffer<T>(usage, count);
            disposables.Add(buffer);
            return buffer;
        }

        public PixelUnpackBuffer<T> CreatePixelUnpackBuffer<T>(BufferUsage usage, int count) where T : struct
        {
            Debug.Assert(typeof(T).IsLayoutSequential);

            PixelUnpackBuffer<T> buffer = new PixelUnpackBuffer<T>(usage, count);
            disposables.Add(buffer);
            return buffer;
        }

        public RenderBuffer CreateRenderBuffer(InternalFormat internalFormat, int samples, int width, int height)
        {
            RenderBuffer renderbuffer = new RenderBuffer(internalFormat, samples, width, height);
            disposables.Add(renderbuffer);
            return renderbuffer;
        }

        public FrameBuffer CreateFrameBuffer(int width, int height)
        {
            FrameBuffer framebuffer = new FrameBuffer(width, height);
            disposables.Add(framebuffer);
            return framebuffer;
        }

        public Shader CreateShader(VertexFormat vertexFormat, params string[] shaders)
        {
            Shader shader = new Shader(vertexFormat, shaders);
            CurrentShader = shader;
            disposables.Add(shader);
            return shader;
        }

        // state

        public void SetScissor(int x, int y, int width, int height)
        {
            GL.Scissor(x, y, width, height);
        }

        public void SetClipPlane(ClipPlaneName plane, Vector4 equation)
        {
            double[] e = { equation.X, equation.Y, equation.Z, equation.W };
            GL.ClipPlane(plane, e);
        }

        public void SetMatrix(MatrixModeEnum mode, Matrix4 matrix)
        {
            if (lastMatrixMode != mode)
            {
                GL.MatrixMode(mode);
                lastMatrixMode = mode;
            }
            GL.LoadMatrix(ref matrix);
        }

        // framebuffer

        public void Clear(BufferMask mask)
        {
            GL.Clear(mask);
        }

        public void SetFramebuffer(FrameBuffer framebuffer)
        {
            if (framebuffer == null)
            {
                GL.BindFramebuffer(FramebufferType.Framebuffer, 0);
                GL.DrawBuffer(DrawBufferMode.Back);
                GL.Viewport(0, 0, Width, Height);
            }
            else
            {
                framebuffer.Bind();
            }
        }

        public void Blit(FrameBuffer src, FrameBuffer dst, BufferMask mask, BlitFramebufferFilter filter)
        {
            GL.BindFramebuffer(FramebufferType.ReadFramebuffer, src == null ? 0 : src.Handle);
            GL.BindFramebuffer(FramebufferType.DrawFramebuffer, dst == null ? 0 : dst.Handle);
            GL.BlitFramebuffer(0, 0, src == null ? Width : src.width, src == null ? Height : src.height, 0, 0, dst == null ? Width : dst.width, dst == null ? Height : dst.height, mask, filter);
            GL.BindFramebuffer(FramebufferType.Framebuffer, 0);
        }

        // shader

        public void SetShader(Shader shader)
        {
            shader.Bind();
        }

        // buffers

        public void SetVertexBuffer(Buffer buffer, int offset)
        {
            Debug.Assert(CurrentShader != null);

            buffer.Bind(CurrentShader.VertexFormat.attributes, offset);
        }

        public void SetIndexBuffer(Buffer buffer)
        {
            if (currentIndexBuffer != buffer)
            {
                if (buffer.elementSize == 1)
                {
                    currentIndexBufferDrawType = DrawElementsType.UnsignedByte;
                    currentIndexBufferSize = 1;
                }
                else if (buffer.elementSize == 2)
                {
                    currentIndexBufferDrawType = DrawElementsType.UnsignedShort;
                    currentIndexBufferSize = 2;
                }
                else if (buffer.elementSize == 4)
                {
                    currentIndexBufferDrawType = DrawElementsType.UnsignedInt;
                    currentIndexBufferSize = 4;
                }
                else
                {
                    Debug.Assert(false);
                }

                buffer.Bind();
            }
        }

        public void SetPackPixelBuffer(Buffer buffer)
        {
            if (buffer == null)
            {
                GL.BindBuffer(BufferTarget.PixelPackBuffer, 0);
            }
            else
            {
                buffer.Bind();
            }
        }

        public void SetUnpackPixelBuffer(Buffer buffer)
        {
            if (buffer == null)
            {
                GL.BindBuffer(BufferTarget.PixelUnpackBuffer, 0);
            }
            else
            {
                buffer.Bind();
            }
        }

        // draw methods

        public RenderStateBinder ChangeRenderState()
        {
            renderStateBinder.oldState = RenderState;
            return renderStateBinder;
        }

        public void ApplyRenderState()
        {
            RenderState.Apply(currentRenderState);
            currentRenderState = RenderState;
        }

        public void DrawArrays(BeginMode mode, int first, int count)
        {
            GL.DrawArrays(mode, first, count);
        }

        public void DrawElements(BeginMode mode, int first, int count)
        {
            GL.DrawElements(mode, count, currentIndexBufferDrawType, new IntPtr(first * currentIndexBufferSize));
        }

        public void DrawRangeElements(BeginMode mode, int first, int start, int end, int count)
        {
            GL.DrawRangeElements(mode, start, end, count, currentIndexBufferDrawType, new IntPtr(first * currentIndexBufferSize));
        }

        public Extensions Extensions { get; private set; }
        public Caps Capabilities { get; private set; }
        public RenderState RenderState;
        public Shader CurrentShader { get; internal set; }

        public int Width { get; private set; }
        public int Height { get; private set; }

        // private stuff

        HashSet<IDisposable> disposables = new HashSet<IDisposable>();
        RenderState currentRenderState;
        MatrixModeEnum lastMatrixMode;

        internal Buffer currentVertexBuffer;
        internal Buffer currentIndexBuffer;
        internal DrawElementsType currentIndexBufferDrawType;
        internal int currentIndexBufferSize;
        internal int activeVertexAttributes;

        static RenderStateBinder renderStateBinder = new RenderStateBinder();

        public static Device Current { get; private set; }
    }
}
