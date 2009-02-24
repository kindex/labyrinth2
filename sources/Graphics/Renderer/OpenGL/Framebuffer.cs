using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Game.Graphics.Renderer.OpenGL
{
    public sealed class RenderBuffer : IDisposable
    {
        internal RenderBuffer(InternalFormat internalFormat, int samples, int width, int height)
        {
            this.renderbuffer = GL.GenRenderbuffer();
            GL.BindRenderbuffer(RenderbufferType.Renderbuffer, renderbuffer);
            if (samples == 0)
            {
                GL.RenderbufferStorage(RenderbufferType.Renderbuffer, internalFormat, width, height);
            }
            else
            {
                GL.RenderbufferStorageMultisample(RenderbufferType.Renderbuffer, samples, internalFormat, width, height);
            }
        }

        public void Dispose()
        {
            GL.DeleteRenderbuffer(renderbuffer);
            renderbuffer = 0;
            Device.Current.Dispose(this);
        }

        public int Width
        {
            get
            {
                int param;
                GL.GetRenderbufferParameter(RenderbufferType.Renderbuffer, RenderbufferParameter.Width, out param);
                return param;
            }
        }

        public int Height
        {
            get
            {
                int param;
                GL.GetRenderbufferParameter(RenderbufferType.Renderbuffer, RenderbufferParameter.Height, out param);
                return param;
            }
        }

        public InternalFormat InternalFormat
        {
            get
            {
                int param;
                GL.GetRenderbufferParameter(RenderbufferType.Renderbuffer, RenderbufferParameter.InternalFormat, out param);
                return (InternalFormat)param;
            }
        }

        public int RedSize
        {
            get
            {
                int param;
                GL.GetRenderbufferParameter(RenderbufferType.Renderbuffer, RenderbufferParameter.RedSize, out param);
                return param;
            }
        }

        public int GreenSize
        {
            get
            {
                int param;
                GL.GetRenderbufferParameter(RenderbufferType.Renderbuffer, RenderbufferParameter.GreenSize, out param);
                return param;
            }
        }

        public int BlueSize
        {
            get
            {
                int param;
                GL.GetRenderbufferParameter(RenderbufferType.Renderbuffer, RenderbufferParameter.BlueSize, out param);
                return param;
            }
        }

        public int AlphaSize
        {
            get
            {
                int param;
                GL.GetRenderbufferParameter(RenderbufferType.Renderbuffer, RenderbufferParameter.AlphaSize, out param);
                return param;
            }
        }

        public int DepthSize
        {
            get
            {
                int param;
                GL.GetRenderbufferParameter(RenderbufferType.Renderbuffer, RenderbufferParameter.DepthSize, out param);
                return param;
            }
        }

        public int StencilSize
        {
            get
            {
                int param;
                GL.GetRenderbufferParameter(RenderbufferType.Renderbuffer, RenderbufferParameter.StencilSize, out param);
                return param;
            }
        }
        
        public int Samples
        {
            get
            {
                int param;
                GL.GetRenderbufferParameter(RenderbufferType.Renderbuffer, RenderbufferParameter.Samples, out param);
                return param;
            }
        }

        internal void Bind()
        {
            GL.BindRenderbuffer(RenderbufferType.Renderbuffer, renderbuffer);
        }

        public static long SupportedSamples(InternalFormat internalFormat)
        {
            while (GL.GetError() != 0)
            {
            }

            uint rb = GL.GenRenderbuffer();
            GL.BindRenderbuffer(RenderbufferType.Renderbuffer, rb);

            if (GL.GetError() != 0)
            {
                GL.BindRenderbuffer(RenderbufferType.Renderbuffer, 0);
                GL.DeleteRenderbuffer(rb);
                return 0;
            }

            long samples = 0;

            for (int i = 0; i <= 32; ++i)
            {
                GL.RenderbufferStorageMultisample(RenderbufferType.Renderbuffer, i, internalFormat, 4, 4);
                if (GL.GetError() == 0)
                {
                    int param;
                    GL.GetRenderbufferParameter(RenderbufferType.Renderbuffer, RenderbufferParameter.Samples, out param);
                    if (i == param)
                    {
                        samples |= (long)1 << i;
                    }
                }
            }

            GL.BindRenderbuffer(RenderbufferType.Renderbuffer, 0);
            GL.DeleteRenderbuffer(rb);

            return samples;
        }

        internal uint Handle
        {
            get
            {
                if (renderbuffer == 0)
                {
                    throw new ObjectDisposedException("Renderbuffer");
                }
                return renderbuffer;
            }
        }

        uint renderbuffer;
    }

    public sealed class FrameBuffer : IDisposable
    {
        internal FrameBuffer(int width, int height)
        {
            this.width = width;
            this.height = height;
            this.framebuffer = GL.GenFramebuffer();
            
            GL.BindFramebuffer(FramebufferType.Framebuffer, framebuffer);
        }

        public void Dispose()
        {
            GL.DeleteFramebuffer(framebuffer);
            framebuffer = 0;
            Device.Current.Dispose(this);
        }
        
        public void SetTexture1D(FramebufferAttachment attachment, int miplevel, Texture1D texture)
        {
            GL.BindFramebuffer(FramebufferType.Framebuffer, framebuffer);
            GL.FramebufferTexture1D(FramebufferType.Framebuffer, attachment, TextureTarget.Texture1D, texture == null ? 0 : texture.Handle, miplevel);
            SetTextureAttachment(attachment, texture);
        }

        public void SetTexture2D(FramebufferAttachment attachment, int miplevel, Texture2D texture)
        {
            GL.BindFramebuffer(FramebufferType.Framebuffer, framebuffer);
            GL.FramebufferTexture2D(FramebufferType.Framebuffer, attachment, TextureTarget.Texture2D, texture == null ? 0 : texture.Handle, miplevel);
            SetTextureAttachment(attachment, texture);
        }

        public void SetTexture3D(FramebufferAttachment attachment, int miplevel, int zOffset, Texture3D texture)
        {
            GL.BindFramebuffer(FramebufferType.Framebuffer, framebuffer);
            GL.FramebufferTexture3D(FramebufferType.Framebuffer, attachment, TextureTarget.Texture3D, texture == null ? 0 : texture.Handle, miplevel, zOffset);
            SetTextureAttachment(attachment, texture);
        }

        public void SetTextureCubeMap(FramebufferAttachment attachment, TextureTarget face, int miplevel, TextureCubeMap texture)
        {
            GL.BindFramebuffer(FramebufferType.Framebuffer, framebuffer);
            GL.FramebufferTexture2D(FramebufferType.Framebuffer, attachment, face, texture == null ? 0 : texture.Handle, miplevel);
            SetTextureAttachment(attachment, texture);
        }

        public void SetRenderBuffer(FramebufferAttachment attachment, RenderBuffer buffer)
        {
            GL.BindFramebuffer(FramebufferType.Framebuffer, framebuffer);
            GL.FramebufferRenderbuffer(FramebufferType.Framebuffer, attachment, RenderbufferType.Renderbuffer, buffer == null ? 0 : buffer.Handle);

            if (attachment == FramebufferAttachment.DepthAttachment)
            {
                buffers[16] = buffer;
            }
            else if (attachment == FramebufferAttachment.StencilAttachment)
            {
                buffers[17] = buffer;
            }
            else
            {
                int idx = attachment - FramebufferAttachment.Color0;
                buffers[idx] = buffer;
                if (buffer == null)
                {
                    attachments &= ~(1 << idx);
                }
                else
                {
                    attachments |= 1 << idx;
                }
            }
        }

        internal void Bind()
        {
            GL.BindFramebuffer(FramebufferType.Framebuffer, framebuffer);
            GL.Viewport(0, 0, width, height);

            int attachmentCount = 0;
            int temp = attachments;
            while (temp != 0)
            {
                attachmentCount++;
                temp &= temp - 1;
            }

            if (attachmentCount == 0)
            {
                GL.DrawBuffer(DrawBufferMode.None);
            }
            else if (attachmentCount == 1)
            {
                DrawBufferMode drawBuffer = DrawBufferMode.Color0;
                temp = attachments;
                while (temp != 1)
                {
                    temp >>= 1;
                    drawBuffer++;

                }
                GL.DrawBuffer(drawBuffer);
            }
            else
            {
                DrawBufferMode[] buffers = new DrawBufferMode[attachmentCount];

                DrawBufferMode drawBuffer = DrawBufferMode.Color0;
                temp = attachments;

                for (int i = 0; i < attachmentCount; i++)
                {
                    while ((temp & 1) != 1)
                    {
                        temp >>= 1;
                    }
                    temp >>= 1;

                    buffers[i] = drawBuffer++;
                }

                GL.DrawBuffers(buffers);
            }
        }

        public FramebufferStatus Status
        {
            get
            {
                //GL.DrawBuffer(DrawBufferMode.None);
                //GL.ReadBuffer(ReadBufferMode.None);
                GL.BindFramebuffer(FramebufferType.Framebuffer, framebuffer);
                try
                {
                    return GL.CheckFramebufferStatus(FramebufferType.Framebuffer);
                }
                finally
                {
                    GL.BindFramebuffer(FramebufferType.Framebuffer, 0);
                    //GL.DrawBuffer(DrawBufferMode.Back);
                    //GL.ReadBuffer(ReadBufferMode.Back);
                }
            }
        }

        void SetTextureAttachment(FramebufferAttachment attachment, Texture texture)
        {
            if (attachment == FramebufferAttachment.DepthAttachment)
            {
                textures[16] = texture;
            }
            else if (attachment == FramebufferAttachment.StencilAttachment)
            {
                textures[17] = texture;
            }
            else
            {
                int idx = attachment - FramebufferAttachment.Color0;
                textures[idx] = texture;
                if (texture == null)
                {
                    attachments &= ~(1 << idx);
                }
                else
                {
                    attachments |= 1 << idx;
                }
            }
        }

        internal uint Handle
        {
            get
            {
                if (framebuffer == 0)
                {
                    throw new ObjectDisposedException("Framebuffer");
                }
                return framebuffer;
            }
        }

        uint framebuffer;
        internal int width;
        internal int height;
        Texture[] textures = new Texture[16 + 2];
        RenderBuffer[] buffers = new RenderBuffer[16 + 2];
        int attachments = 0;
    }
}
