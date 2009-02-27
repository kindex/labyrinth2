using System;
using System.IO;
using System.Drawing;
using System.Runtime.InteropServices;

using Game.Graphics.Renderer.OpenGL;

namespace Game
{
    static class Loaders
    {
        const string BaseDir = "Resources";

        struct DDSCaps2
        {
            public UInt32 dwCaps1;
            public UInt32 dwCaps2;
            public UInt32 Reserved1;
            public UInt32 Reserved2;

            public void Read(BinaryReader reader)
            {
                dwCaps1 = reader.ReadUInt32();
                dwCaps2 = reader.ReadUInt32();
                Reserved1 = reader.ReadUInt32();
                Reserved2 = reader.ReadUInt32();
            }
        }

        struct DDPixelFormat
        {
            public UInt32 dwSize;
            public UInt32 dwFlags;
            public UInt32 dwFourCC;
            public UInt32 dwRGBBitCount;
            public UInt32 dwRBitMask;
            public UInt32 dwGBitMask;
            public UInt32 dwBBitMask;
            public UInt32 dwRGBAlphaBitMask;

            public void Read(BinaryReader reader)
            {
                dwSize = reader.ReadUInt32();
                dwFlags = reader.ReadUInt32();
                dwFourCC = reader.ReadUInt32();
                dwRGBBitCount = reader.ReadUInt32();
                dwRBitMask = reader.ReadUInt32();
                dwGBitMask = reader.ReadUInt32();
                dwBBitMask = reader.ReadUInt32();
                dwRGBAlphaBitMask = reader.ReadUInt32();
            }
        }

        struct DDSurfaceDesc2
        {
            public UInt32 dwSize;
            public UInt32 dwFlags;
            public UInt32 dwHeight;
            public UInt32 dwWidth;
            public UInt32 dwPitchOrLinearSize;
            public UInt32 dwDepth;
            public UInt32 dwMipMapCount;
            public UInt32 dwReserved1_1;
            public UInt32 dwReserved1_2;
            public UInt32 dwReserved1_3;
            public UInt32 dwReserved1_4;
            public UInt32 dwReserved1_5;
            public UInt32 dwReserved1_6;
            public UInt32 dwReserved1_7;
            public UInt32 dwReserved1_8;
            public UInt32 dwReserved1_9;
            public UInt32 dwReserved1_10;
            public UInt32 dwReserved1_11;
            public DDPixelFormat ddpfPixelFormat;
            public DDSCaps2 ddsCaps;
            public UInt32 dwReserved2;

            public void Read(BinaryReader reader)
            {
                dwSize = reader.ReadUInt32();
                dwFlags = reader.ReadUInt32();
                dwHeight = reader.ReadUInt32();
                dwWidth = reader.ReadUInt32();
                dwPitchOrLinearSize = reader.ReadUInt32();
                dwDepth = reader.ReadUInt32();
                dwMipMapCount = reader.ReadUInt32();
                dwReserved1_1 = reader.ReadUInt32();
                dwReserved1_2 = reader.ReadUInt32();
                dwReserved1_3 = reader.ReadUInt32();
                dwReserved1_4 = reader.ReadUInt32();
                dwReserved1_5 = reader.ReadUInt32();
                dwReserved1_6 = reader.ReadUInt32();
                dwReserved1_7 = reader.ReadUInt32();
                dwReserved1_8 = reader.ReadUInt32();
                dwReserved1_9 = reader.ReadUInt32();
                dwReserved1_10 = reader.ReadUInt32();
                dwReserved1_11 = reader.ReadUInt32();
                ddpfPixelFormat.Read(reader);
                ddsCaps.Read(reader);
                dwReserved2 = reader.ReadUInt32();
            }
        }

        public static Texture2D LoadTexture2D_DDS(string name)
        {
            DDSurfaceDesc2 ddsd;
            byte[] data;
            InternalFormat format;
            int width;
            int height;
            int[] mipSizes;

            using (BinaryReader reader = new BinaryReader(File.OpenRead(Path.Combine(BaseDir, "textures/" + name))))
            {
                int magic = reader.ReadInt32();
                if (magic != 'D' + ('D' << 8) + ('S' << 16) + (' ' << 24))
                {
                    throw new ApplicationException("Invalid DDS format");
                }

                ddsd = new DDSurfaceDesc2();
                ddsd.Read(reader);

                int blockSize;

                if (ddsd.ddpfPixelFormat.dwFourCC == 'D' + ('X' << 8) + ('T' << 16) + ('1' << 24))
                {
                    format = InternalFormat.CompressedRGBA_S3TC_DXT1;
                    blockSize = 8;
                }
                else if (ddsd.ddpfPixelFormat.dwFourCC == 'D' + ('X' << 8) + ('T' << 16) + ('3' << 24))
                {
                    format = InternalFormat.CompressedRGBA_S3TC_DXT3;
                    blockSize = 16;
                }
                else if (ddsd.ddpfPixelFormat.dwFourCC == 'D' + ('X' << 8) + ('T' << 16) + ('5' << 24))
                {
                    format = InternalFormat.CompressedRGBA_S3TC_DXT5;
                    blockSize = 16;
                }
                else
                {
                    throw new ApplicationException("Unsupported DDS format");
                }

                width = (int)ddsd.dwWidth;
                height = (int)ddsd.dwHeight;
                mipSizes = new int[Math.Max(ddsd.dwMipMapCount, 1)];

                int bufsize = 0;


                int w = width;
                int h = height;
                for (int i = 0; i < mipSizes.Length; i++)
                {
                    mipSizes[i] = ((w + 3) / 4) * ((h + 3) / 4) * blockSize;
                    bufsize += mipSizes[i];
                    w = Math.Max(1, w / 2);
                    h = Math.Max(1, h / 2);
                }

                data = reader.ReadBytes(bufsize);
            }

            Texture2D texture;
            GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            try
            {
                texture = Device.Current.CreateTexture2D(format, width, height, mipSizes, handle.AddrOfPinnedObject());
            }
            finally
            {
                handle.Free();
            }

            if (mipSizes.Length > 1)
            {
                texture.SetFilter(TextureMagFilter.Linear, TextureMinFilter.LinearMipmapLinear);
            }
            else
            {
                texture.SetFilter(TextureMagFilter.Linear, TextureMinFilter.Linear);
            }

            return texture;
        }

        public static Texture2D LoadTexture2D_RGBA(string name, bool mipmap)
        {
            Texture2D texture =  null;

            string path = Path.Combine("../../../res", name);
            string[] extensions = { "bmp", "gif", "exig", "jpg", "png", "tiff"};

            foreach (string ext in extensions)
            {
                try
                {
                    using (Bitmap bmp = new System.Drawing.Bitmap(path + "." + ext))
                    {
                        int width = bmp.Width;
                        int height = bmp.Height;

                        System.Drawing.Imaging.BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                        try
                        {
                            texture = Device.Current.CreateTexture2D(InternalFormat.RGBA8, width, height, PixelFormat.BGRA, PixelType.UnsignedByte, data.Scan0);
                        }
                        finally
                        {
                            bmp.UnlockBits(data);
                        }

                        break;
                    }
                }
                catch (ArgumentException)
                {
                    texture = null;
                }
            }

            if (mipmap)
            {
                texture.SetFilter(TextureMagFilter.Linear, TextureMinFilter.LinearMipmapLinear);
                texture.GenerateMipmap();
            }
            else
            {
                texture.SetFilter(TextureMagFilter.Linear, TextureMinFilter.Linear);
            }

            return texture;
        }

        public static Shader LoadShader<T>(string name) where T : struct
        {
            return Loaders.LoadShader(name, VF.GetVertexFormat<T>());
        }

        public static Shader LoadShader(string name, VertexFormat vf)
        {
            Shader shader = Device.Current.CreateShader(vf,
                File.ReadAllText(Path.Combine(BaseDir, "shaders/common.glsl")),
                File.ReadAllText(Path.Combine(BaseDir, "shaders/" + name + ".glsl"))
            );

            if (shader.Link() == false)
            {
                throw new ApplicationException(name + " shader failed to link:\n" + shader.ErrorLog);
            }

            return shader;
        }

    }
}
