using System;
using System.Diagnostics;

namespace Game.Graphics.Renderer.OpenGL
{
    public class Texture : IDisposable
    {
        protected Texture(TextureTarget target)
        {
            this.target = target;
            this.texture = GL.GenTexture();

            GL.BindTexture(target, texture);
        }

        public void Dispose()
        {
            GL.DeleteTexture(texture);
            texture = 0;
            Device.Current.Dispose(this);
        }

        public void SetFilter(TextureMagFilter mag, TextureMinFilter min)
        {
            Bind();
            GL.TexMagFilter(target, mag);
            GL.TexMinFilter(target, min);
        }

        public void SetFilterAnisotropy(float anisotropy)
        {
            // GL_EXT_texture_filter_anisotropic

            Bind();
            GL.TexMagFilter(target, TextureMagFilter.Linear);
            GL.TexMinFilter(target, TextureMinFilter.LinearMipmapLinear);
            GL.TexMaxAnisotropy(target, anisotropy);
        }

        public void SetCompareMode(TextureCompareMode mode)
        {
            Bind();
            GL.TexCompareMode(target, mode);
        }

        public void SetCompareFunc(TextureCompareFunc func)
        {
            Bind();
            GL.TexCompareFunc(target, func);
        }

        internal void Bind()
        {
            GL.BindTexture(target, texture);
        }

        internal uint Handle
        {
            get
            {
                if (texture == 0)
                {
                    throw new ObjectDisposedException("Texture");
                }
                return texture;
            }
        }

        uint texture;
        TextureTarget target;

        protected static PixelFormat GetInternalFormatFormat(InternalFormat format)
        {
            switch (format)
            {
            case InternalFormat.Alpha:
            case InternalFormat.Alpha4:
            case InternalFormat.Alpha8:
            case InternalFormat.Alpha12:
            case InternalFormat.Alpha16:
            case InternalFormat.Alpha16F:
            case InternalFormat.Alpha32F:
                return PixelFormat.Alpha;

            case InternalFormat.Luminance:
            case InternalFormat.Luminance4:
            case InternalFormat.Luminance8:
            case InternalFormat.Luminance12:
            case InternalFormat.Luminance16:
            case InternalFormat.Luminance16F:
            case InternalFormat.Luminance32F:
                return PixelFormat.Luminance;

            case InternalFormat.LuminanceAlpha:
            case InternalFormat.Luminance4Alpha4:
            case InternalFormat.Luminance6Alpha2:
            case InternalFormat.Luminance8Alpha8:
            case InternalFormat.Luminance12Alpha4:
            case InternalFormat.Luminance12Alpha12:
            case InternalFormat.Luminance16Alpha16:
            case InternalFormat.LuminanceAlpha16F:
            case InternalFormat.LuminanceAlpha32F:
                return PixelFormat.LuminanceAlpha;

            case InternalFormat.Intensity:
            case InternalFormat.Intensity4:
            case InternalFormat.Intensity8:
            case InternalFormat.Intensity12:
            case InternalFormat.Intensity16:
            case InternalFormat.Intensity16F:
            case InternalFormat.Intensity32F:
                return PixelFormat.Intensity;

            case InternalFormat.RGB:
            case InternalFormat.R3G3B2:
            case InternalFormat.RGB4:
            case InternalFormat.RGB5:
            case InternalFormat.RGB8:
            case InternalFormat.RGB10:
            case InternalFormat.RGB12:
            case InternalFormat.RGB16:
            case InternalFormat.RGB16F:
            case InternalFormat.RGB32F:
                return PixelFormat.BGR;

            case InternalFormat.RGBA:
            case InternalFormat.RGBA2:
            case InternalFormat.RGBA4:
            case InternalFormat.RGB5A1:
            case InternalFormat.RGBA8:
            case InternalFormat.RGB10A2:
            case InternalFormat.RGBA12:
            case InternalFormat.RGBA16:
            case InternalFormat.RGBA16F:
            case InternalFormat.RGBA32F:
                return PixelFormat.BGRA;

            case InternalFormat.DepthComponent:
            case InternalFormat.DepthComponent16:
            case InternalFormat.DepthComponent24:
            case InternalFormat.DepthComponent32:
            case InternalFormat.DepthComponent32F:
            case InternalFormat.DepthComponent32F_NV:
                return PixelFormat.DepthComponent;

            case InternalFormat.DepthComponent32F_Stencil8:
            case InternalFormat.DepthComponent32F_Stencil8_NV:
                return PixelFormat.DepthStencil;

            case InternalFormat.DepthStencil:
            case InternalFormat.Depth24Stencil8:
                return PixelFormat.DepthStencil;

            case InternalFormat.StencilIndex:
            case InternalFormat.StencilIndex1:
            case InternalFormat.StencilIndex4:
            case InternalFormat.StencilIndex8:
            case InternalFormat.StencilIndex16:
                return PixelFormat.StencilIndex;

            default:
                Debug.Assert(false);
                return (PixelFormat)0;
            }

        }

        protected static PixelType GetInternalFormatType(InternalFormat format)
        {
            switch (format)
            {
            case InternalFormat.Alpha8:
            case InternalFormat.Luminance8:
            case InternalFormat.Luminance8Alpha8:
            case InternalFormat.Intensity8:
            case InternalFormat.RGB8:
            case InternalFormat.StencilIndex8:
                return PixelType.UnsignedByte;

            case InternalFormat.RGBA8:
                return PixelType.UnsignedInt_8_8_8_8_Rev;

            case InternalFormat.Alpha16:
            case InternalFormat.Luminance16:
            case InternalFormat.Luminance16Alpha16:
            case InternalFormat.Intensity16:
            case InternalFormat.RGB16:
            case InternalFormat.RGBA16:
            case InternalFormat.DepthComponent16:
            case InternalFormat.StencilIndex16:
                return PixelType.UnsignedShort;

            case InternalFormat.DepthComponent24:
            case InternalFormat.DepthComponent32:
                return PixelType.UnsignedInt;

            case InternalFormat.Alpha32F:
            case InternalFormat.Luminance32F:
            case InternalFormat.LuminanceAlpha32F:
            case InternalFormat.Intensity32F:
            case InternalFormat.RGB32F:
            case InternalFormat.RGBA32F:

            case InternalFormat.Alpha16F:
            case InternalFormat.Luminance16F:
            case InternalFormat.LuminanceAlpha16F:
            case InternalFormat.Intensity16F:
            case InternalFormat.RGB16F:
            case InternalFormat.RGBA16F:

            case InternalFormat.DepthComponent32F:
            case InternalFormat.DepthComponent32F_NV:
                return PixelType.Float;

            case InternalFormat.Depth24Stencil8:
                return PixelType.UsignedInt_24_8;

            case InternalFormat.R3G3B2:
                return PixelType.UnsignedByte_2_3_3_Rev;

            case InternalFormat.RGB5A1:
                return PixelType.UnsignedShort_1_5_5_5_Rev;

            case InternalFormat.RGB10A2:
                return PixelType.UnsignedInt_2_10_10_10_Rev;

            case InternalFormat.RGBA4:
                return PixelType.UnsignedShort_4_4_4_4_Rev;

            case InternalFormat.DepthComponent32F_Stencil8:
                return PixelType.Float32_UnsignedInt_24_8_Rev;

            case InternalFormat.DepthComponent32F_Stencil8_NV:
                return PixelType.Float32_UnsignedInt_24_8_Rev_NV;

            default:
                Debug.Assert(false);
                return (PixelType)0;
/*            
            StencilIndex = 0x1901,
            DepthComponent = 0x1902,
            Alpha = 0x1906,
            RGB = 0x1907,
            RGBA = 0x1908,
            Luminance = 0x1909,
            LuminanceAlpha = 0x190A,

            Alpha4 = 0x803B,
            Alpha12 = 0x803D,

            Luminance4 = 0x803F,
            Luminance12 = 0x8041,

            Luminance4Alpha4 = 0x8043,
            Luminance6Alpha2 = 0x8044,
            Luminance12Alpha4 = 0x8046,
            Luminance12Alpha12 = 0x8047,

            Intensity = 0x8049,
            Intensity4 = 0x804A,
            Intensity12 = 0x804C,

            RGB4 = 0x804F,
            RGB5 = 0x8050,
            RGB10 = 0x8052,
            RGB12 = 0x8053,

            RGBA2 = 0x8055,
            RGBA4 = 0x8056,
            RGBA12 = 0x805A,

            // GL_EXT_packed_depth_stencil
            DepthStencil = 0x84F9,

            // GL_ARB_texture_compression
            CompressedAlpha = 0x84E9,
            CompressedLuminance = 0x84EA,
            CompressedLuminanceAlpha = 0x84EB,
            CompressedIntensity = 0x84EC,
            CompressedRGB = 0x84ED,
            CompressedRGBA = 0x84EE,

            // GL_EXT_texture_compression_s3tc
            CompressedRGB_S3TC_DXT1 = 0x83F0,
            CompressedRGBA_S3TC_DXT1 = 0x83F1,
            CompressedRGBA_S3TC_DXT3 = 0x83F2,
            CompressedRGBA_S3TC_DXT5 = 0x83F3,

            // GL_ATI_texture_compression_3dc
            CompressedLuminanceAlpha3DC = 0x8837,

            // GL_EXT_texture_compression_latc
            CompressedLuminanceLATC1 = 0x8C70,
            CompressedSignedLuminanceLATC1 = 0x8C71,
            CompressedLuminanceAlphaLATC2 = 0x8C72,
            CompressedSignedLuminanceAlphaLATC2 = 0x8C73,

            // GL_EXT_framebuffer_object
            StencilIndex1 = 0x8D46,
            StencilIndex4 = 0x8D47,
*/
            }
        }
    }

    public sealed class Texture1D : Texture
    {
        internal Texture1D(InternalFormat internalFormat, int width, PixelFormat format, PixelType type, IntPtr pixels)
            : base(TextureTarget.Texture1D)
        {
            this.Width = width;
            GL.TexImage1D(TextureTarget.Texture1D, 0, internalFormat, width, 0, format, type, pixels);
        }

        internal Texture1D(InternalFormat internalFormat, int width)
            : base(TextureTarget.Texture1D)
        {
            this.Width = width;

            PixelFormat format = GetInternalFormatFormat(internalFormat);
            PixelType type = GetInternalFormatType(internalFormat);

            GL.TexImage1D(TextureTarget.Texture1D, 0, internalFormat, width, 0, format, type, IntPtr.Zero);
        }

        public void SetWrap(TextureWrap s)
        {
            Bind();
            GL.TexWrapS(TextureTarget.Texture1D, s);
        }

        public void SetSubImage(int level, int xOffset, int width, PixelFormat format, PixelType type, IntPtr pixels)
        {
            Bind();
            GL.TexSubImage1D(TextureTarget.Texture1D, level, xOffset, width, format, type, pixels);
        }

        public void GetImage(int level, PixelFormat format, PixelType type, IntPtr pixels)
        {
            Bind();
            GL.GetTexImage(TextureTarget.Texture1D, level, format, type, pixels);
        }

        public void GenerateMipmap()
        {
            // GL_EXT_framebuffer_object

            Bind();
            GL.GenerateMipmap(TextureTarget.Texture1D);
        }

        public int Width { get; private set; }
    }

    public sealed class Texture2D : Texture
    {
        internal Texture2D(InternalFormat internalFormat, int width, int height, PixelFormat format, PixelType type, IntPtr pixels)
            : base(TextureTarget.Texture2D)
        {
            this.Width = width;
            this.Height = height;

            GL.TexImage2D(TextureTarget.Texture2D, 0, internalFormat, width, height, 0, format, type, pixels);
        }

        internal Texture2D(InternalFormat internalFormat, int width, int height, int[] mipSizes, IntPtr data)
            : base(TextureTarget.Texture2D)
        {
            this.Width = width;
            this.Height = height;

            IntPtr offset = data;
            for (int i = 0; i < mipSizes.Length; i++)
            {
                GL.CompressedTexImage2D(TextureTarget.Texture2D, i, internalFormat, width, height, 0, mipSizes[i], offset);
                offset = (IntPtr)((long)offset + mipSizes[i]);
                width = Math.Max(1, width / 2);
                height = Math.Max(1, height / 2);
            }
        }

        internal Texture2D(InternalFormat internalFormat, int width, int height)
            : base(TextureTarget.Texture2D)
        {
            this.Width = width;
            this.Height = height;
            
            PixelFormat format = GetInternalFormatFormat(internalFormat);
            PixelType type = GetInternalFormatType(internalFormat);

            GL.TexImage2D(TextureTarget.Texture2D, 0, internalFormat, width, height, 0, format, type, IntPtr.Zero);
        }

        public void SetWrap(TextureWrap s, TextureWrap t)
        {
            Bind();
            GL.TexWrapS(TextureTarget.Texture2D, s);
            GL.TexWrapT(TextureTarget.Texture2D, t);
        }

        public void SetSubImage(int Level, int xOffset, int yOffset, int Width, int Height, PixelFormat format, PixelType type, IntPtr pixels)
        {
            Bind();
            GL.TexSubImage2D(TextureTarget.Texture2D, Level, xOffset, yOffset, Width, Height, format, type, pixels);
        }

        public void GetImage(int level, PixelFormat format, PixelType type, IntPtr pixels)
        {
            Bind();
            GL.GetTexImage(TextureTarget.Texture2D, level, format, type, pixels);
        }

        public void GenerateMipmap()
        {
            // GL_EXT_framebuffer_object

            Bind();
            
            // TODO: this is hack for ATI cards
            GL.Enable((EnableCap)0x0DE1);
            GL.Hint(0x8192, 0x1102);

            GL.GenerateMipmap(TextureTarget.Texture2D);
        }

        public int Width { get; private set; }
        public int Height { get; private set; }
    }

    public sealed class Texture3D : Texture
    {
        internal Texture3D(InternalFormat internalFormat, int width, int height, int depth, PixelFormat format, PixelType type, IntPtr pixels)
            : base(TextureTarget.Texture3D)
        {
            this.Width = width;
            this.Height = height;
            this.Depth = depth;

            GL.TexImage3D(TextureTarget.Texture3D, 0, internalFormat, width, height, depth, 0, format, type, pixels);
        }

        internal Texture3D(InternalFormat internalFormat, int width, int height, int depth)
            : base(TextureTarget.Texture3D)
        {
            this.Width = width;
            this.Height = height;
            this.Depth = depth;

            PixelFormat format = GetInternalFormatFormat(internalFormat);
            PixelType type = GetInternalFormatType(internalFormat);

            GL.TexImage3D(TextureTarget.Texture3D, 0, internalFormat, width, height, depth, 0, format, type, IntPtr.Zero);
        }

        public void SetWrap(TextureWrap s, TextureWrap t, TextureWrap r)
        {
            Bind();
            GL.TexWrapS(TextureTarget.Texture3D, s);
            GL.TexWrapT(TextureTarget.Texture3D, t);
            GL.TexWrapR(TextureTarget.Texture3D, r);
        }

        public int Width { get; private set; }
        public int Height { get; private set; }
        public int Depth { get; private set; }
    }

    public sealed class TextureCubeMap : Texture
    {
        internal TextureCubeMap(int size)
            : base(TextureTarget.TextureCubeMap)
        {
            this.Size = size;
        }

        internal TextureCubeMap(InternalFormat internalFormat, int size)
            : base(TextureTarget.TextureCubeMap)
        {
            this.Size = size;

            PixelFormat format = GetInternalFormatFormat(internalFormat);
            PixelType type = GetInternalFormatType(internalFormat);

            for (int i = 0; i < CubeMapFaces.Length; i++)
            {
                GL.TexImage2D(CubeMapFaces[i], 0, internalFormat, size, size, 0, format, type, IntPtr.Zero);
            }
        }

        public void SetWrap(TextureWrap s, TextureWrap t)
        {
            Bind();
            GL.TexWrapS(TextureTarget.TextureCubeMap, s);
            GL.TexWrapT(TextureTarget.TextureCubeMap, t);
        }

        public void SetImage(TextureTarget face, InternalFormat internalFormat, int size, PixelFormat format, PixelType type, IntPtr pixels)
        {
            Bind();
            GL.TexImage2D(face, 0, internalFormat, size, size, 0, format, type, pixels);
        }

        public void SetCompressedImage(TextureTarget face, InternalFormat internalFormat, int size, int imageSize, IntPtr pixels)
        {
            Bind();
            GL.CompressedTexImage2D(face, 0, internalFormat, size, size, 0, imageSize, pixels);
        }

        public int Size { get; private set; }

        static readonly TextureTarget[] CubeMapFaces = 
        {
            TextureTarget.TextureCubeMapPositiveX,
            TextureTarget.TextureCubeMapNegativeX,
            TextureTarget.TextureCubeMapPositiveY,
            TextureTarget.TextureCubeMapNegativeY,
            TextureTarget.TextureCubeMapPositiveZ,
            TextureTarget.TextureCubeMapNegativeZ,
        };
    }
}
