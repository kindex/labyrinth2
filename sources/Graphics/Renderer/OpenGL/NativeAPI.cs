using System;
using System.Text;
using System.Security;
using System.Runtime.InteropServices;

namespace Game.Graphics.Renderer.OpenGL
{
    public static class GL
    {
        // drawing-control commands

        [SuppressUnmanagedCodeSecurity]
        public delegate void glClipPlane(ClipPlaneName plane, [In, MarshalAs(UnmanagedType.LPArray, SizeConst = 4)] double[] equation);
        public static glClipPlane ClipPlane;

        [SuppressUnmanagedCodeSecurity]
        public delegate void glCullFace(CullFaceMode mode);
        public static glCullFace CullFace;

        [SuppressUnmanagedCodeSecurity]
        public delegate void glFrontFace(FrontFaceDirection mode);
        public static glFrontFace FrontFace;

        [SuppressUnmanagedCodeSecurity]
        public delegate void glPolygonMode(PolygonFace face, PolygonMode mode);
        public static glPolygonMode PolygonMode;

        [SuppressUnmanagedCodeSecurity]
        public delegate void glPolygonOffset(float factor, float units);
        public static glPolygonOffset PolygonOffset;

        [SuppressUnmanagedCodeSecurity]
        public delegate void glScissor(int x, int y, int width, int height);
        public static glScissor Scissor;

        public static partial class Private
        {
            public enum TextureParameterName : int
            {
                MagFilter = 0x2800,
                MinFilter = 0x2801,
                WrapS = 0x2802,
                WrapT = 0x2803,

                // GL_ARB_depth_texture
                DepthTextureMode = 0x884B,

                // GL_ARB_shadow
                CompareMode = 0x884C,
                CompareFunc = 0x884D,

                // GL_EXT_texture3D
                WrapR = 0x8072,

                // GL_EXT_texture_filter_anisotropic,
                TextureMaxAnisotropy = 0x84FE,
            }

            [SuppressUnmanagedCodeSecurity]
            public delegate void glTexParameterf(TextureTarget target, TextureParameterName pname, float param);
            public static glTexParameterf TexParameterf;

            [SuppressUnmanagedCodeSecurity]
            public delegate void glTexParameterfv(TextureTarget target, TextureParameterName pname, [In] ref Vector4 param);
            public static glTexParameterfv TexParameterfv;

            [SuppressUnmanagedCodeSecurity]
            public delegate void glTexParameteriv(TextureTarget target, TextureParameterName pname, [In] ref int param);
            public static glTexParameteriv TexParameteri;
        }

        public static void TexMagFilter(TextureTarget target, TextureMagFilter param)
        {
            int i = (int)param;
            Private.TexParameteri(target, Private.TextureParameterName.MagFilter, ref i);
        }

        public static void TexMinFilter(TextureTarget target, TextureMinFilter param)
        {
            int i = (int)param;
            Private.TexParameteri(target, Private.TextureParameterName.MinFilter, ref i);
        }

        public static void TexWrapS(TextureTarget target, TextureWrap param)
        {
            int i = (int)param;
            Private.TexParameteri(target, Private.TextureParameterName.WrapS, ref i);
        }

        public static void TexWrapT(TextureTarget target, TextureWrap param)
        {
            int i = (int)param;
            Private.TexParameteri(target, Private.TextureParameterName.WrapT, ref i);
        }

        public static void TexWrapR(TextureTarget target, TextureWrap param)
        {
            int i = (int)param;
            Private.TexParameteri(target, Private.TextureParameterName.WrapR, ref i);
        }

        public static void TexDepthMode(TextureTarget target, TextureDepthMode param)
        {
            int i = (int)param;
            Private.TexParameteri(target, Private.TextureParameterName.DepthTextureMode, ref i);
        }

        public static void TexCompareMode(TextureTarget target, TextureCompareMode param)
        {
            int i = (int)param;
            Private.TexParameteri(target, Private.TextureParameterName.CompareMode, ref i);
        }

        public static void TexCompareFunc(TextureTarget target, TextureCompareFunc param)
        {
            int i = (int)param;
            Private.TexParameteri(target, Private.TextureParameterName.CompareFunc, ref i);
        }

        // GL_EXT_texture_filter_anisotropic
        public static void TexMaxAnisotropy(TextureTarget target, float anisotropy)
        {
            Private.TexParameterf(target, Private.TextureParameterName.TextureMaxAnisotropy, anisotropy);
        }


        [SuppressUnmanagedCodeSecurity]
        public delegate void glTexImage1D(TextureTarget target, int level, InternalFormat internalformat, int width, int border, PixelFormat format, PixelType type, IntPtr pixels);
        public static glTexImage1D TexImage1D;

        [SuppressUnmanagedCodeSecurity]
        public delegate void glTexImage2D(TextureTarget target, int level, InternalFormat internalformat, int width, int height, int border, PixelFormat format, PixelType type, IntPtr pixels);
        public static glTexImage2D TexImage2D;

        // framebuf commands

        [SuppressUnmanagedCodeSecurity]
        public delegate void glDrawBuffer(DrawBufferMode mode);
        public static glDrawBuffer DrawBuffer;

        [SuppressUnmanagedCodeSecurity]
        public delegate void glClear(BufferMask mask);
        public static glClear Clear;

        [SuppressUnmanagedCodeSecurity]
        public delegate void glClearColor(float red, float green, float blue, float alpha);
        public static glClearColor ClearColor;

        [SuppressUnmanagedCodeSecurity]
        public delegate void glClearStencil(float s);
        public static glClearStencil ClearStencil;

        [SuppressUnmanagedCodeSecurity]
        public delegate void glClearDepth(double depth);
        public static glClearDepth ClearDepth;

        [SuppressUnmanagedCodeSecurity]
        public delegate void glStencilMask(uint mask);
        public static glStencilMask StencilMask;

        [SuppressUnmanagedCodeSecurity]
        public delegate void glColorMask(int red, int green, int blue, int alpha);
        public static glColorMask ColorMask;

        [SuppressUnmanagedCodeSecurity]
        public delegate void glDepthMask(int flag);
        public static glDepthMask DepthMask;

        // misc commands

        [SuppressUnmanagedCodeSecurity]
        public delegate void glDisable(EnableCap cap);
        public static glDisable Disable;

        [SuppressUnmanagedCodeSecurity]
        public delegate void glEnable(EnableCap cap);
        public static glEnable Enable;

        [SuppressUnmanagedCodeSecurity]
        public delegate void glHint(int target, int mode);
        public static glHint Hint;

        // pixel-op commands

        [SuppressUnmanagedCodeSecurity]
        public delegate void glAlphaFunc(AlphaFunction func, float @ref);
        public static glAlphaFunc AlphaFunc;

        [SuppressUnmanagedCodeSecurity]
        public delegate void glBlendFunc(BlendingFactorSrc sfactor, BlendingFactorDest dfactor);
        public static glBlendFunc BlendFunc;

        [SuppressUnmanagedCodeSecurity]
        public delegate void glStencilFunc(StencilFunction func, int @ref, uint mask);
        public static glStencilFunc StencilFunc;

        [SuppressUnmanagedCodeSecurity]
        public delegate void glStencilOp(StencilOpEnum fail, StencilOpEnum zfail, StencilOpEnum zpass);
        public static glStencilOp StencilOp;

        [SuppressUnmanagedCodeSecurity]
        public delegate void glDepthFunc(DepthFunction func);
        public static glDepthFunc DepthFunc;

        // pixel-rw commands

        [SuppressUnmanagedCodeSecurity]
        public delegate void glPixelStorei(PixelStoreParameter pname, int param);
        public static glPixelStorei PixelStore;

        [SuppressUnmanagedCodeSecurity]
        public delegate void glReadBuffer(ReadBufferMode mode);
        public static glReadBuffer ReadBuffer;
        
        [SuppressUnmanagedCodeSecurity]
        public delegate void glReadPixels(int x, int y, int width, int height, PixelFormat format, PixelType type, IntPtr pixels);
        public static glReadPixels ReadPixels;

        // state-req commands

        [SuppressUnmanagedCodeSecurity]
        public delegate int glGetError();
        public static glGetError GetError;

        public static partial class Private
        {
            [SuppressUnmanagedCodeSecurity]
            public delegate void glGetIntegerv(int pname, IntPtr param);
            public static glGetIntegerv GetInteger;
        }

        public static int GetInteger(IntegerName pname)
        {
            Object result = new int();
            GCHandle handle = GCHandle.Alloc(result, GCHandleType.Pinned);
            try
            {
                Private.GetInteger((int)pname, handle.AddrOfPinnedObject());
            }
            finally
            {
                handle.Free();
            }
            return (int)result;
        }

        public static void GetInteger(Integer2Name pname, out int x, out int y)
        {
            int[] values = new int[2];
            GCHandle handle = GCHandle.Alloc(values, GCHandleType.Pinned);
            try
            {
                Private.GetInteger((int)pname, handle.AddrOfPinnedObject());
            }
            finally
            {
                handle.Free();
            }

            x = values[0];
            y = values[1];
        }

        public static partial class Private
        {
            [SuppressUnmanagedCodeSecurity]
            public delegate IntPtr glGetString(StringName name);
            public static glGetString GetString;
        }
         
        public static string GetString(StringName name)
        {
            return Marshal.PtrToStringAnsi(Private.GetString(name));
        }

        [SuppressUnmanagedCodeSecurity]
        public delegate void glGetTexImage(TextureTarget target, int level, PixelFormat format, PixelType type, IntPtr pixels);
        public static glGetTexImage GetTexImage;

        // xform commands

        [SuppressUnmanagedCodeSecurity]
        public delegate void glLoadMatrixf([In] ref Matrix4 m);
        public static glLoadMatrixf LoadMatrix;

        [SuppressUnmanagedCodeSecurity]
        public delegate void glMatrixMode(MatrixModeEnum mode);
        public static glMatrixMode MatrixMode;

        [SuppressUnmanagedCodeSecurity]
        public delegate void glViewport(int x, int y, int width, int height);
        public static glViewport Viewport;

        // OpenGL 1.1 commands

        [SuppressUnmanagedCodeSecurity]
        public delegate void glDrawArrays(BeginMode mode, int first, int count);
        public static glDrawArrays DrawArrays;

        [SuppressUnmanagedCodeSecurity]
        public delegate void glDrawElements(BeginMode mode, int count, DrawElementsType type, IntPtr indices);
        public static glDrawElements DrawElements;

        [SuppressUnmanagedCodeSecurity]
        public delegate void glTexSubImage1D(TextureTarget target, int level, int xoffset, int width, PixelFormat format, PixelType type, IntPtr pixels);
        public static glTexSubImage1D TexSubImage1D;

        [SuppressUnmanagedCodeSecurity]
        public delegate void glTexSubImage2D(TextureTarget target, int level, int xoffset, int yoffset, int width, int height, PixelFormat format, PixelType type, IntPtr pixels);
        public static glTexSubImage2D TexSubImage2D;

        [SuppressUnmanagedCodeSecurity]
        public delegate void glBindTexture(TextureTarget target, uint texture);
        public static glBindTexture BindTexture;

        public static partial class Private
        {
            [SuppressUnmanagedCodeSecurity]
            public delegate void glDeleteTextures(int n, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] uint[] textures);
            public static glDeleteTextures DeleteTextures;
            
            [SuppressUnmanagedCodeSecurity]
            public delegate void glGenTextures(int n, [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] uint[] textures);
            public static glGenTextures GenTextures;
        }

        public static uint GenTexture()
        {
            uint[] tex = new uint[1];
            Private.GenTextures(1, tex);
            return tex[0];
        }

        public static uint[] GenTextures(int n)
        {
            uint[] tex = new uint[n];
            Private.GenTextures(n, tex);
            return tex;
        }

        public static void DeleteTexture(uint texture)
        {
            uint[] tex = { texture };
            Private.DeleteTextures(1, tex);
        }

        public static void DeleteTextures(uint[] textures)
        {
            Private.DeleteTextures(textures.Length, textures);
        }

        // GL_ARB_depth_texture
        //x

        // GL_ARB_draw_buffers

        public static partial class Private
        {
            [SuppressUnmanagedCodeSecurity]
            public delegate void glDrawBuffersARB(int n, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] DrawBufferMode[] bufs);
            public static glDrawBuffersARB DrawBuffers;
        }

        public static void DrawBuffers(params DrawBufferMode[] bufs)
        {
            Private.DrawBuffers(bufs.Length, bufs);
        }

        // GL_ARB_multisample
        [SuppressUnmanagedCodeSecurity]
        public delegate void glSampleCoverageARB(float value, bool invert);
        public static glSampleCoverageARB SampleCoverage;
        
        // GL_ARB_multitexture

        [SuppressUnmanagedCodeSecurity]
        public delegate void glActiveTextureARB(TextureUnit texture);
        public static glActiveTextureARB ActiveTexture;
        
        // GL_ARB_pixel_buffer_object
        //x

        //GL_ARB_shadow
        //x

        //GL_ARB_texture_compression

        [SuppressUnmanagedCodeSecurity]
        public delegate void glCompressedTexImage3DARB(TextureTarget target, int level, InternalFormat internalformat, int width, int height, int depth, int border, int imageSize, IntPtr data);
        public static glCompressedTexImage3DARB CompressedTexImage3D;

        [SuppressUnmanagedCodeSecurity]
        public delegate void glCompressedTexImage2DARB(TextureTarget target, int level, InternalFormat internalformat, int width, int height, int border, int imageSize, IntPtr data);
        public static glCompressedTexImage2DARB CompressedTexImage2D;

        [SuppressUnmanagedCodeSecurity]
        public delegate void glCompressedTexImage1DARB(TextureTarget target, int level, InternalFormat internalformat, int width, int border, int imageSize, IntPtr data);
        public static glCompressedTexImage1DARB CompressedTexImage1D;

        //GL_ARB_texture_cube_map
        //x

        //GL_ARB_texture_float
        //x

        //GL_ARB_texture_non_power_of_two
        //x

        // GL_ARB_vertex_buffer_object

        [SuppressUnmanagedCodeSecurity]
        public delegate void glBindBufferARB(BufferTarget target, uint buffer);
        public static glBindBufferARB BindBuffer;

        public static partial class Private
        {
            [SuppressUnmanagedCodeSecurity]
            public delegate void glDeleteBuffersARB(int n, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] uint[] ids);
            public static glDeleteBuffersARB DeleteBuffers;

            [SuppressUnmanagedCodeSecurity]
            public delegate void glGenBuffersARB(int n, [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] uint[] ids);
            public static glGenBuffersARB GenBuffers;
        }

        public static uint GenBuffer()
        {
            uint[] buffer = new uint[1];
            Private.GenBuffers(1, buffer);
            return buffer[0];
        }

        public static uint[] GenBuffers(int n)
        {
            uint[] buffers = new uint[n];
            Private.GenBuffers(n, buffers);
            return buffers;
        }

        public static void DeleteBuffer(uint buffer)
        {
            uint[] buffers = { buffer };
            Private.DeleteBuffers(1, buffers);
        }

        public static void DeleteBuffers(uint[] buffers)
        {
            Private.DeleteBuffers(buffers.Length, buffers);
        }

        [SuppressUnmanagedCodeSecurity]
        public delegate void glBufferDataARB(BufferTarget target, int size, IntPtr data, BufferUsage usage);
        public static glBufferDataARB BufferData;

        [SuppressUnmanagedCodeSecurity]
        public delegate void glBufferSubDataARB(BufferTarget target, int offset, int size, IntPtr data);
        public static glBufferSubDataARB BufferSubData;

        [SuppressUnmanagedCodeSecurity]
        public delegate void glGetBufferSubDataARB(BufferTarget target, int offset, int size, IntPtr data);
        public static glGetBufferSubDataARB GetBufferSubData;

        [SuppressUnmanagedCodeSecurity]
        public delegate IntPtr glMapBufferARB(BufferTarget target, BufferAccess access);
        public static glMapBufferARB MapBuffer;

        [SuppressUnmanagedCodeSecurity]
        public delegate bool glUnmapBufferARB(BufferTarget target);
        public static glUnmapBufferARB UnmapBuffer;

        //GL_ARB_shading_language_100
        //x

        // GL_ARB_shader_objects

        [SuppressUnmanagedCodeSecurity]
        public delegate void glDeleteObjectARB(uint obj);
        public static glDeleteObjectARB DeleteObject;

        [SuppressUnmanagedCodeSecurity]
        public delegate uint glCreateShaderObjectARB(ShaderObjectType shaderType);
        public static glCreateShaderObjectARB CreateShaderObject;

        [SuppressUnmanagedCodeSecurity]
        public delegate void glShaderSourceARB(uint shaderObj, int count, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] string[] @string, IntPtr length);
        public static glShaderSourceARB ShaderSource;

        [SuppressUnmanagedCodeSecurity]
        public delegate void glCompileShaderARB(uint shaderObj);
        public static glCompileShaderARB CompileShader;

        [SuppressUnmanagedCodeSecurity]
        public delegate uint glCreateProgramObjectARB();
        public static glCreateProgramObjectARB CreateProgramObject;

        [SuppressUnmanagedCodeSecurity]
        public delegate void glAttachObjectARB(uint containerObj, uint obj);
        public static glAttachObjectARB AttachObject;

        [SuppressUnmanagedCodeSecurity]
        public delegate void glLinkProgramARB(uint programObj);
        public static glLinkProgramARB LinkProgram;

        [SuppressUnmanagedCodeSecurity]
        public delegate void glUseProgramObjectARB(uint programObj);
        public static glUseProgramObjectARB UseProgramObject;

        public static partial class Private
        {
            [SuppressUnmanagedCodeSecurity]
            public delegate void glUniform1fARB(int location, float v0);
            public static glUniform1fARB Uniform1f;

            [SuppressUnmanagedCodeSecurity]
            public delegate void glUniform1iARB(int location, int v0);
            public static glUniform1iARB Uniform1i;

            [SuppressUnmanagedCodeSecurity]
            public delegate void glUniform2iARB(int location, int v0, int v1);
            public static glUniform2iARB Uniform2i;

            [SuppressUnmanagedCodeSecurity]
            public delegate void glUniform3iARB(int location, int v0, int v1, int v2);
            public static glUniform3iARB Uniform3i;

            [SuppressUnmanagedCodeSecurity]
            public delegate void glUniform4iARB(int location, int v0, int v1, int v2, int v3);
            public static glUniform4iARB Uniform4i;

            [SuppressUnmanagedCodeSecurity]
            public delegate void glUniform2fvARB(int location, int count, [In] ref Vector2 value);
            public static glUniform2fvARB Uniform2f;

            [SuppressUnmanagedCodeSecurity]
            public delegate void glUniform3fvARB(int location, int count, [In] ref Vector3 value);
            public static glUniform3fvARB Uniform3f;

            [SuppressUnmanagedCodeSecurity]
            public delegate void glUniform4fvARB(int location, int count, [In] ref Vector4 value);
            public static glUniform4fvARB Uniform4f;

            [SuppressUnmanagedCodeSecurity]
            public delegate void glUniformMatrix2fvARB(int location, int count, bool transpose, [In] ref Matrix2 value);
            public static glUniformMatrix2fvARB UniformMatrix2;

            [SuppressUnmanagedCodeSecurity]
            public delegate void glUniformMatrix3fvARB(int location, int count, bool transpose, [In] ref Matrix3 value);
            public static glUniformMatrix3fvARB UniformMatrix3;

            [SuppressUnmanagedCodeSecurity]
            public delegate void glUniformMatrix4fvARB(int location, int count, bool transpose, [In] ref Matrix4 value);
            public static glUniformMatrix4fvARB UniformMatrix4;
        }

        public static void Uniform(int location, int v0)
        {
            Private.Uniform1i(location, v0);
        }

        public static void Uniform(int location, int v0, int v1)
        {
            Private.Uniform2i(location, v0, v1);
        }

        public static void Uniform(int location, int v0, int v1, int v2)
        {
            Private.Uniform3i(location, v0, v1, v2);
        }

        public static void Uniform(int location, int v0, int v1, int v2, int v3)
        {
            Private.Uniform4i(location, v0, v1, v2, v3);
        }

        public static void Uniform(int location, float v0)
        {
            Private.Uniform1f(location, v0);
        }

        public static void Uniform(int location, Vector2 v)
        {
            Private.Uniform2f(location, 1, ref v);
        }

        public static void Uniform(int location, Vector3 v)
        {
            Private.Uniform3f(location, 1, ref v);
        }

        public static void Uniform(int location, Vector4 v)
        {
            Private.Uniform4f(location, 1, ref v);
        }

        public static void Uniform(int location, bool transpose, Matrix2 m)
        {
            Private.UniformMatrix2(location, 1, transpose, ref m);
        }

        public static void Uniform(int location, bool transpose, Matrix3 m)
        {
            Private.UniformMatrix3(location, 1, transpose, ref m);
        }

        public static void Uniform(int location, bool transpose, Matrix4 m)
        {
            Private.UniformMatrix4(location, 1, transpose, ref m);
        }

        [SuppressUnmanagedCodeSecurity]
        public delegate void glGetObjectParameterivARB(uint obj, ObjectParameter pname, out int param);
        public static glGetObjectParameterivARB GetObjectParameter;

        public static partial class Private
        {
            [SuppressUnmanagedCodeSecurity]
            public delegate void glGetInfoLogARB(uint obj, int maxLength, out int length, [Out] StringBuilder infoLog);
            public static glGetInfoLogARB GetInfoLog;
        }

        public static string GetInfoLog(uint obj)
        {
            int length;
            GetObjectParameter(obj, ObjectParameter.InfoLogLength, out length);
            StringBuilder sb = new StringBuilder(length);
            Private.GetInfoLog(obj, length, out length, sb);
            return sb.ToString().Substring(0, length);
        }

        [SuppressUnmanagedCodeSecurity]
        public delegate int glGetUniformLocationARB(uint programObj, string name);
        public static glGetUniformLocationARB GetUniformLocation;

        [SuppressUnmanagedCodeSecurity]
        public delegate void glGetActiveUniformARB(uint programObj, int index, int maxLength, out int length, out int size, out UniformType type, [Out] StringBuilder name);
        public static glGetActiveUniformARB GetActiveUniform;

        //GL_ARB_fragment_shader
        //x

        //GL_ARB_vertex_shader

        [SuppressUnmanagedCodeSecurity]
        public delegate void glVertexAttribPointerARB(int index, int size, VertexAttributePointerType type, bool normalized, int stride, IntPtr pointer);
        public static glVertexAttribPointerARB VertexAttribPointer;

        [SuppressUnmanagedCodeSecurity]
        public delegate void glEnableVertexAttribArrayARB(int index);
        public static glEnableVertexAttribArrayARB EnableVertexAttribArray;
        
        [SuppressUnmanagedCodeSecurity]
        public delegate void glDisableVertexAttribArrayARB(int index);
        public static glDisableVertexAttribArrayARB DisableVertexAttribArray;

        [SuppressUnmanagedCodeSecurity]
        public delegate void glBindAttribLocationARB(uint programObj, int index, string name);
        public static glBindAttribLocationARB BindAttribLocation;

        [SuppressUnmanagedCodeSecurity]
        public delegate void glGetActiveAttribARB(uint programObj, int index, int maxLength, out int length, out int size, ActiveAttributeType type, [Out] StringBuilder name);
        public static glGetActiveAttribARB GetActiveAttrib;

        [SuppressUnmanagedCodeSecurity]
        public delegate uint glGetAttribLocationARB(uint programObj, string name);
        public static glGetAttribLocationARB GetAttribLocation;

        // GL_ATI_separate_stencil
        
        [SuppressUnmanagedCodeSecurity]
        public delegate void glStencilOpSeparateATI(StencilOpSeperateFace face, StencilOpEnum sfail, StencilOpEnum dpfail, StencilOpEnum dppass);
        public static glStencilOpSeparateATI StencilOpSeparate;

        [SuppressUnmanagedCodeSecurity]
        public delegate void glStencilFuncSeparateATI(StencilFunction frontfunc, StencilFunction backfunc, int @ref, uint mask);
        public static glStencilFuncSeparateATI StencilFuncSeparate;

        //GL_ATI_texture_compression_3dc
        //x

        //GL_EXT_bgra
        //x

        // GL_EXT_draw_range_elements
        
        [SuppressUnmanagedCodeSecurity]
        public delegate void glDrawRangeElementsEXT(BeginMode mode, int start, int end, int count, DrawElementsType type, IntPtr indices);
        public static glDrawRangeElementsEXT DrawRangeElements;
       
        // GL_EXT_framebuffer_blit

        [SuppressUnmanagedCodeSecurity]
        public delegate void glBlitFramebufferEXT(int srcX0, int srcY0, int srcX1, int srcY1, int dstX0, int dstY0, int dstX1, int dstY1, BufferMask mask, BlitFramebufferFilter filter);
        public static glBlitFramebufferEXT BlitFramebuffer;

        // GL_EXT_framebuffer_multisample
        
        [SuppressUnmanagedCodeSecurity]
        public delegate void glRenderbufferStorageMultisampleEXT(RenderbufferType target, int samples, InternalFormat internalformat, int width, int height);
        public static glRenderbufferStorageMultisampleEXT RenderbufferStorageMultisample;

        // GL_EXT_framebuffer_object

        [SuppressUnmanagedCodeSecurity]
        public delegate void glBindRenderbufferEXT(RenderbufferType target, uint renderbuffer);
        public static glBindRenderbufferEXT BindRenderbuffer;

        public static partial class Private
        {
            [SuppressUnmanagedCodeSecurity]
            public delegate void glDeleteRenderbuffersEXT(int n, [In] ref uint renderbuffers);
            public static glDeleteRenderbuffersEXT DeleteRenderbuffers;

            [SuppressUnmanagedCodeSecurity]
            public delegate void glGenRenderbuffersEXT(int n, out uint renderbuffers);
            public static glGenRenderbuffersEXT GenRenderbuffers;

            [SuppressUnmanagedCodeSecurity]
            public delegate void glDeleteFramebuffersEXT(int n, [In] ref uint framebuffers);
            public static glDeleteFramebuffersEXT DeleteFramebuffers;

            [SuppressUnmanagedCodeSecurity]
            public delegate void glGenFramebuffersEXT(int n, out uint framebuffers);
            public static glGenFramebuffersEXT GenFramebuffers;
        }

        public static uint GenRenderbuffer()
        {
            uint rb;
            Private.GenRenderbuffers(1, out rb);
            return rb;
        }

        public static uint GenFramebuffer()
        {
            uint fb;
            Private.GenFramebuffers(1, out fb);
            return fb;
        }

        public static void DeleteRenderbuffer(uint rb)
        {
            Private.DeleteRenderbuffers(1, ref rb);
        }

        public static void DeleteFramebuffer(uint fb)
        {
            Private.DeleteFramebuffers(1, ref fb);
        }

        [SuppressUnmanagedCodeSecurity]
        public delegate void glRenderbufferStorageEXT(RenderbufferType target, InternalFormat internalformat, int width, int height);
        public static glRenderbufferStorageEXT RenderbufferStorage;

        [SuppressUnmanagedCodeSecurity]
        public delegate void glGetRenderbufferParameterivEXT(RenderbufferType target, RenderbufferParameter pname, out int param);
        public static glGetRenderbufferParameterivEXT GetRenderbufferParameter;

        [SuppressUnmanagedCodeSecurity]
        public delegate void glBindFramebufferEXT(FramebufferType target, uint framebuffer);
        public static glBindFramebufferEXT BindFramebuffer;
        
        [SuppressUnmanagedCodeSecurity]
        public delegate FramebufferStatus glCheckFramebufferStatusEXT(FramebufferType target);
        public static glCheckFramebufferStatusEXT CheckFramebufferStatus;

        [SuppressUnmanagedCodeSecurity]
        public delegate void glFramebufferTexture1DEXT(FramebufferType target, FramebufferAttachment attachment, TextureTarget textarget, uint texture, int level);
        public static glFramebufferTexture1DEXT FramebufferTexture1D;

        [SuppressUnmanagedCodeSecurity]
        public delegate void glFramebufferTexture2DEXT(FramebufferType target, FramebufferAttachment attachment, TextureTarget textarget, uint texture, int level);
        public static glFramebufferTexture2DEXT FramebufferTexture2D;

        [SuppressUnmanagedCodeSecurity]
        public delegate void glFramebufferTexture3DEXT(FramebufferType target, FramebufferAttachment attachment, TextureTarget textarget, uint texture, int level, int zoffset);
        public static glFramebufferTexture3DEXT FramebufferTexture3D;

        [SuppressUnmanagedCodeSecurity]
        public delegate void glFramebufferRenderbufferEXT(FramebufferType target, FramebufferAttachment attachment, RenderbufferType renderbuffertarget, uint renderbuffer);
        public static glFramebufferRenderbufferEXT FramebufferRenderbuffer;

        [SuppressUnmanagedCodeSecurity]
        public delegate void glGenerateMipmapEXT(TextureTarget target);
        public static glGenerateMipmapEXT GenerateMipmap;

        //GL_EXT_packed_depth_stencil
        //x

        // GL_EXT_stencil_two_side

        [SuppressUnmanagedCodeSecurity]
        public delegate void glActiveStencilFaceEXT(StencilFaceDirection face);
        public static glActiveStencilFaceEXT ActiveStencilFace;

        //GL_EXT_stencil_wrap
        //x

        //GL_EXT_texture3D

        [SuppressUnmanagedCodeSecurity]
        public delegate void glTexImage3DEXT(TextureTarget target, int level, InternalFormat internalformat, int width, int height, int depth, int border, PixelFormat format, PixelType type, IntPtr pixels);
        public static glTexImage3DEXT TexImage3D;

        // GL_EXT_texture_compression_s3tc
        //x

        // GL_EXT_texture_compression_latc
        //x

        // GL_EXT_texture_filter_anisotropic
        //x

        //GL_SGIS_generate_mipmap
        //x

        //GL_SGIS_texture_edge_clamp
        //x


        // temp
        [SuppressUnmanagedCodeSecurity]
        public delegate void glLineWidth(float width);
        public static glLineWidth LineWidth;
    }
}
