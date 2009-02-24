using System;


namespace Game.Graphics.Renderer.OpenGL
{
    public sealed class Caps
    {
        public Caps(Extensions extensions)
        {
            this.RedBits = GL.GetInteger(IntegerName.RedBits);
            this.GreenBits = GL.GetInteger(IntegerName.GreenBits);
            this.BlueBits = GL.GetInteger(IntegerName.BlueBits);
            this.AlphaBits = GL.GetInteger(IntegerName.AlphaBits);
            this.DepthBits = GL.GetInteger(IntegerName.DepthBits);
            this.StencilBits = GL.GetInteger(IntegerName.StencilBits);
            this.MaxClipPlanes = GL.GetInteger(IntegerName.MaxClipPlanes);
            this.MaxTextureSize = GL.GetInteger(IntegerName.MaxTextureSize);
            
            int x, y;
            GL.GetInteger(Integer2Name.MaxViewportDims, out x, out y);
            this.MaxViewportWidth = x;
            this.MaxViewportHeight = y;

            if (extensions.ARB_draw_buffers)
            {
                this.MaxDrawBuffers = GL.GetInteger(IntegerName.MaxDrawBuffers);
            }

            if (extensions.ARB_texture_cube_map)
            {
                this.MaxCubeMapTextureSize = GL.GetInteger(IntegerName.MaxCubeMapTextureSize);
            }

            if (extensions.ARB_multisample)
            {
                this.Samples = GL.GetInteger(IntegerName.Samples);
            }

            if (extensions.ARB_fragment_shader)
            {
                this.MaxFragmentUniformComponents = GL.GetInteger(IntegerName.MaxFragmentUniformComponents);
                this.MaxTextureImageUnits = GL.GetInteger(IntegerName.MaxTextureImageUnits);
            }

            if (extensions.ARB_vertex_shader)
            {
                this.MaxVertexUniformComponents = GL.GetInteger(IntegerName.MaxVertexUniformComponents);
                this.MaxVaryingFloats = GL.GetInteger(IntegerName.MaxVaryingFloats);
                this.MaxVertexAttribs = GL.GetInteger(IntegerName.MaxVertexAttribs);
                this.MaxVertexTextureImageUnits = GL.GetInteger(IntegerName.MaxVertexTextureImageUnits);
                this.MaxCombinedTextureImageUnits = GL.GetInteger(IntegerName.MaxCombinedTextureImageUnits);
            }

            if (extensions.EXT_draw_range_elements)
            {
                this.MaxElementsVertices = GL.GetInteger(IntegerName.MaxElementsVertices);
                this.MaxElementsIndices = GL.GetInteger(IntegerName.MaxElementsIndices);
            }

            if (extensions.EXT_framebuffer_object)
            {
                this.MaxColorAttachments = GL.GetInteger(IntegerName.MaxColorAttachments);
                this.MaxRenderbufferSize = GL.GetInteger(IntegerName.MaxRenderbufferSize);
            }

            if (extensions.EXT_framebuffer_multisample)
            {
                this.MaxFramebufferSamples = GL.GetInteger(IntegerName.MaxSamples);
            }

            if (extensions.EXT_texture3D)
            {
                this.Max3DTextureSize = GL.GetInteger(IntegerName.Max3DTextureSize);
            }

            if (extensions.EXT_texture_filter_anisotropic)
            {
                this.MaxTextureMaxAnisotropy = GL.GetInteger(IntegerName.MaxTextureMaxAnisotropy);
            }
        }

        public int RedBits { get; private set; }
        public int GreenBits { get; private set; }
        public int BlueBits { get; private set; }
        public int AlphaBits { get; private set; }
        public int DepthBits { get; private set; }
        public int StencilBits { get; private set; }

        public int MaxClipPlanes { get; private set; }
        public int MaxTextureSize { get; private set; }
        public int MaxViewportWidth { get; private set; }
        public int MaxViewportHeight { get; private set; }

        // GL_ARB_draw_buffers
        public int MaxDrawBuffers { get; private set; }

        // GL_ARB_texture_cube_map
        public int MaxCubeMapTextureSize { get; private set; }

        // GL_ARB_multisample
        public int Samples { get; private set; }

        // GL_ARB_fragment_shader
        public int MaxFragmentUniformComponents { get; private set; }
        public int MaxTextureImageUnits { get; private set; }

        // GL_ARB_vertex_shader
        public int MaxVertexUniformComponents { get; private set; }
        public int MaxVaryingFloats { get; private set; }
        public int MaxVertexAttribs { get; private set; }
        public int MaxVertexTextureImageUnits { get; private set; }
        public int MaxCombinedTextureImageUnits { get; private set; }

        // GL_EXT_draw_range_elements
        public int MaxElementsVertices { get; private set; }
        public int MaxElementsIndices { get; private set; }

        // GL_EXT_framebuffer_object
        public int MaxColorAttachments { get; private set; }
        public int MaxRenderbufferSize { get; private set; }

        // GL_EXT_framebuffer_multisample
        public int MaxFramebufferSamples { get; private set; }

        // GL_EXT_texture3D
        public int Max3DTextureSize { get; private set; }

        // GL_EXT_texture_filter_anisotropic
        public int MaxTextureMaxAnisotropy { get; private set; }
    }
}
