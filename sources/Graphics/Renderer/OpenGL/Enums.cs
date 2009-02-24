using System;

namespace Game.Graphics.Renderer.OpenGL
{
    public enum ClipPlaneName : int
    {
        ClipPlane0 = 0x3000,
        ClipPlane1 = 0x3001,
        ClipPlane2 = 0x3002,
        ClipPlane3 = 0x3003,
        ClipPlane4 = 0x3004,
        ClipPlane5 = 0x3005,
    }

    public enum CullFaceMode : int
    {
        Front = 0x0404,
        Back = 0x0405,
        FrontAndBack = 0x0408,
    }

    public enum FrontFaceDirection : int
    {
        CW = 0x0900,
        CCW = 0x0901,
    }

    public enum PolygonFace : int
    {
        Front = 0x0404,
        Back = 0x0405,
        FrontAndBack = 0x0408,
    }

    public enum PolygonMode : int
    {
        Point = 0x1B00,
        Line = 0x1B01,
        Fill = 0x1B02,
    }

    public enum TextureTarget : int
    {
        Texture1D = 0x0DE0,
        Texture2D = 0x0DE1,

        // GL_EXT_texture3D
        Texture3D = 0x806F,

        // GL_ARB_texture_cube_map
        TextureCubeMap = 0x8513,
        TextureCubeMapPositiveX = 0x8515,
        TextureCubeMapNegativeX = 0x8516,
        TextureCubeMapPositiveY = 0x8517,
        TextureCubeMapNegativeY = 0x8518,
        TextureCubeMapPositiveZ = 0x8519,
        TextureCubeMapNegativeZ = 0x851A,
    }

    // GL_ARB_depth_texture
    public enum TextureDepthMode : int
    {
        Luminance = 0x1909,
        Intensity = 0x8049,
        Alpha = 0x1906,
    }

    // GL_ARB_shadow
    public enum TextureCompareMode : int
    {
        None = 0,
        CompareRToTexture = 0x884E,
    }

    // GL_ARB_shadow
    public enum TextureCompareFunc : int
    {
        LEqual = 0x0203,
        GEqual = 0x0206,
    }

    public enum TextureMagFilter : int
    {
        Nearest = 0x2600,
        Linear = 0x2601,
    }

    public enum TextureMinFilter : int
    {
        Nearest = 0x2600,
        Linear = 0x2601,
        NearestMipmapNearest = 0x2700,
        LinearMipmpNearest = 0x2701,
        NearestMipmapLinear = 0x2702,
        LinearMipmapLinear = 0x2703,
    }

    public enum TextureWrap : int
    {
        Clamp = 0x2900,
        Repeat = 0x2901,

        // GL_SGIS_texture_edge_clamp
        ClampToEdge = 0x812F,
    }

    public enum InternalFormat : int
    {
        StencilIndex = 0x1901,
        DepthComponent = 0x1902,
        Alpha = 0x1906,
        RGB = 0x1907,
        RGBA = 0x1908,
        Luminance = 0x1909,
        LuminanceAlpha = 0x190A,

        Alpha4 = 0x803B,
        Alpha8 = 0x803C,
        Alpha12 = 0x803D,
        Alpha16 = 0x803E,

        Luminance4 = 0x803F,
        Luminance8 = 0x8040,
        Luminance12 = 0x8041,
        Luminance16 = 0x8042,

        Luminance4Alpha4 = 0x8043,
        Luminance6Alpha2 = 0x8044,
        Luminance8Alpha8 = 0x8045,
        Luminance12Alpha4 = 0x8046,
        Luminance12Alpha12 = 0x8047,
        Luminance16Alpha16 = 0x8048,

        Intensity = 0x8049,
        Intensity4 = 0x804A,
        Intensity8 = 0x804B,
        Intensity12 = 0x804C,
        Intensity16 = 0x804D,

        R3G3B2 = 0x2A10,

        RGB4 = 0x804F,
        RGB5 = 0x8050,
        RGB8 = 0x8051,
        RGB10 = 0x8052,
        RGB12 = 0x8053,
        RGB16 = 0x8054,

        RGBA2 = 0x8055,
        RGBA4 = 0x8056,
        RGB5A1 = 0x8057,
        RGBA8 = 0x8058,
        RGB10A2 = 0x8059,
        RGBA12 = 0x805A,
        RGBA16 = 0x805B,

        // GL_ARB_depth_texture
        DepthComponent16 = 0x81A5,
        DepthComponent24 = 0x81A6,
        DepthComponent32 = 0x81A7,

        // GL_ARB_texture_float
        RGBA32F = 0x8814,
        RGB32F = 0x8815,
        Alpha32F = 0x8816,
        Intensity32F = 0x8817,
        Luminance32F = 0x8818,
        LuminanceAlpha32F = 0x8819,
        RGBA16F = 0x881A,
        RGB16F = 0x881B,
        Alpha16F = 0x881C,
        Intensity16F = 0x881D,
        Luminance16F = 0x881E,
        LuminanceAlpha16F = 0x881F,

        // GL_EXT_packed_depth_stencil
        DepthStencil = 0x84F9,
        Depth24Stencil8 = 0x88F0,

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
        StencilIndex8 = 0x8D48,
        StencilIndex16 = 0x8D49,

        // ARB_depth_buffer_float
        DepthComponent32F = 0x8CAC,
        DepthComponent32F_Stencil8 = 0x8CAD,

        // NV_depth_buffer_float
        DepthComponent32F_NV = 0x8DAB,
        DepthComponent32F_Stencil8_NV = 0x8DAC,
    }

    public enum PixelFormat : int
    {
        StencilIndex = 0x1901,
        DepthComponent = 0x1902,
        Red = 0x1903,
        Green = 0x1904,
        Blue = 0x1905,
        Alpha = 0x1906,
        RGB = 0x1907,
        RGBA = 0x1908,
        Luminance = 0x1909,
        LuminanceAlpha = 0x190A,

        Intensity = 0x8049,

        // GL_EXT_bgra
        BGR = 0x80E0,
        BGRA = 0x80E1,

        // GL_EXT_packed_depth_stencil
        DepthStencil = 0x84F9,
    }

    public enum PixelType : int
    {
        Bitmap = 0x1A00,
        Byte = 0x1400,
        UnsignedByte = 0x1401,
        Short = 0x1402,
        UnsignedShort = 0x1403,
        Int = 0x1404,
        UnsignedInt = 0x1405,
        Float = 0x1406,

        // GL_EXT_packed_pixels
        UnsignedByte_3_3_2 = 0x8032,
        UnsignedShort_4_4_4_4 = 0x8033,
        UnsignedShort_5_5_5_1 = 0x8034,
        UnsignedInt_8_8_8_8 = 0x8035,
        UnsignedInt_10_10_10_2 = 0x8036,
        UnsignedByte_2_3_3_Rev = 0x8362,
        UnsignedShort_5_6_5 = 0x8363,
        UnsignedShort_5_6_5_Rev = 0x8364,
        UnsignedShort_4_4_4_4_Rev = 0x8365,
        UnsignedShort_1_5_5_5_Rev = 0x8366,
        UnsignedInt_8_8_8_8_Rev = 0x8367,
        UnsignedInt_2_10_10_10_Rev = 0x8368,

        // GL_EXT_packed_depth_stencil
        UsignedInt_24_8 = 0x84FA,

        // ARB_depth_buffer_float
        Float32_UnsignedInt_24_8_Rev = 0x8CAC,

        // NV_depth_buffer_float
        Float32_UnsignedInt_24_8_Rev_NV = 0x8DAD,
    }

    public enum EnableCap : int
    {
        CullFace = 0x0b44,
        AlphaTest = 0x0bc0,
        Blend = 0x0be2,
        StencilTest = 0x0b90,
        DepthTest = 0x0b71,
        ClipPlane0 = 0x3000,
        ClipPlane1 = 0x3001,
        ClipPlane2 = 0x3002,
        ClipPlane3 = 0x3003,
        ClipPlane4 = 0x3004,
        ClipPlane5 = 0x3005,
        ScissorTest = 0x0c11,
        PolygonOffset = 0x8037,

        // GL_ARB_multisample
        Multisample = 0x809d,
        SampleAlphaToCoverage = 0x809e,
        SampleAlphaToOne = 0x809f,
        SampleCoverage = 0x80a0,

        // GL_EXT_stencil_two_side
        StencilTestTwoSide = 0x8910,
    }

    public enum DrawBufferMode : int
    {
        None = 0,
        Front = 0x0404,
        Back = 0x0405,
        FrontAndBack = 0x0408,

        // GL_ARB_draw_buffers
        Color0 = 0x8CE0,
        Color1 = 0x8CE1,
        Color2 = 0x8CE2,
        Color3 = 0x8CE3,
        Color4 = 0x8CE4,
        Color5 = 0x8CE5,
        Color6 = 0x8CE6,
        Color7 = 0x8CE7,
        Color8 = 0x8CE8,
        Color9 = 0x8CE9,
        Color10 = 0x8CEA,
        Color11 = 0x8CEB,
        Color12 = 0x8CEC,
        Color13 = 0x8CED,
        Color14 = 0x8CEE,
        Color15 = 0x8CEF,
    }

    [Flags]
    public enum BufferMask : int
    {
        ColorBuffer = 0x00004000,
        StencilBuffer = 0x00000400,
        DepthBuffer = 0x00000100,
    }

    public enum AlphaFunction : int
    {
        Never = 0x0200,
        Less = 0x0201,
        Equal = 0x0202,
        LEqual = 0x0203,
        Greater = 0x0204,
        NotEqual = 0x0205,
        GEqual = 0x0206,
        Always = 0x0207,
    }

    public enum BlendingFactorSrc : int
    {
        Zero = 0,
        One = 1,
        SrcColor = 0x0300,
        OneMinusSrcColor = 0x0301,
        SrcAlpha = 0x0302,
        OneMinusSrcAlpha = 0x0303,
        DstAlpha = 0x0304,
        OneMinusDstAlpha = 0x0305,
        DstColor = 0x0306,
        OneMinusDstColor = 0x0307,
        SrcAlphaSaturate = 0x0308,
    }

    public enum BlendingFactorDest : int
    {
        Zero = 0,
        One = 1,
        SrcColor = 0x0300,
        OneMinusSrcColor = 0x0301,
        SrcAlpha = 0x0302,
        OneMinusSrcAlpha = 0x0303,
        DstAlpha = 0x0304,
        OneMinusDstAlpha = 0x0305,
        DstColor = 0x0306,
        OneMinusDstColor = 0x0307,
        SrcAlphaSaturate = 0x0308,
    }

    public enum StencilFunction : int
    {
        Never = 0x0200,
        Less = 0x0201,
        Equal = 0x0202,
        LEqual = 0x0203,
        Greater = 0x0204,
        NotEqual = 0x0205,
        GEqual = 0x0206,
        Always = 0x0207,
    }

    public enum StencilOpEnum : int
    {
        Zero = 0,
        Keep = 0x1E00,
        Replace = 0x1E01,
        Incr = 0x1E02,
        Decr = 0x1E03,
        Invert = 0x150A,

        // GL_EXT_stencil_wrap
        IncrWrap = 0x8507,
        DecrWrap = 0x8508,
    }

    public enum DepthFunction : int
    {
        Never = 0x0200,
        Less = 0x0201,
        Equal = 0x0202,
        LEqual = 0x0203,
        Greater = 0x0204,
        NotEqual = 0x0205,
        GEqual = 0x0206,
        Always = 0x0207,
    }

    public enum PixelStoreParameter : int
    {
        UnpackSwapBytes = 0x0CF0,
        UnpackLSBFirst = 0x0CF1,
        UnpackRowLength = 0x0CF2,
        UnpackSkipRows = 0x0CF3,
        UnpackSkipPixels = 0x0CF4,
        UnpackAlignment = 0x0CF5,

        PackSwapBytes = 0x0DF0,
        PackLSBFirst = 0x0DF1,
        PackRowLength = 0x0DF2,
        PackSkipRows = 0x0DF3,
        PackSkipPixels = 0x0DF4,
        PackAlignment = 0x0DF5,

        // GL_EXT_texture3D
        PackSkipImages = 0x806B,
        PackImageHeight = 0x806C,
        UnpackSkipImages = 0x806D,
        UnpackImageHeight = 0x806E,
    }

    public enum ReadBufferMode : int
    {
        None = 0,
        Front = 0x0404,
        Back = 0x0405,
    }

    public enum IntegerName : int
    {
        MaxClipPlanes = 0x0D32,
        MaxTextureSize = 0x0D33,
        RedBits = 0x0D52,
        GreenBits = 0x0D53,
        BlueBits = 0x0D54,
        AlphaBits = 0x0D55,
        DepthBits = 0x0D56,
        StencilBits = 0x0D57,

        // GL_ARB_draw_buffers
        MaxDrawBuffers = 0x8824,

        // GL_ARB_texture_cube_map
        MaxCubeMapTextureSize = 0x851C,

        // GL_ARB_multisample
        Samples = 0x80A9,

        // GL_ARB_fragment_shader
        MaxFragmentUniformComponents = 0x8B49,
        MaxTextureImageUnits = 0x8872,

        // GL_ARB_vertex_shader
        MaxVertexUniformComponents = 0x8B4A,
        MaxVaryingFloats = 0x8B4B,
        MaxVertexAttribs = 0x8869,
        MaxVertexTextureImageUnits = 0x8B4C,
        MaxCombinedTextureImageUnits = 0x8B4D,

        // GL_EXT_draw_range_elements
        MaxElementsVertices = 0x80E8,
        MaxElementsIndices = 0x80E9,

        // GL_EXT_framebuffer_object
        MaxColorAttachments = 0x8CDF,
        MaxRenderbufferSize = 0x84E8,

        // GL_EXT_framebuffer_multisample
        MaxSamples = 0x8D57,

        // GL_EXT_texture3D
        Max3DTextureSize = 0x8073,

        // GL_EXT_texture_filter_anisotropic
        MaxTextureMaxAnisotropy = 0x84FF,
    }

    public enum Integer2Name : int
    {
        MaxViewportDims = 0x0D3A,
    }

    public enum StringName : int
    {
        Vendor = 0x1F00,
        Renderer = 0x1F01,
        Version = 0x1F02,
        Extensions = 0x1F03,

        // GL_ARB_shading_language_100
        ShadingLanguageVersion = 0x8B8C,
    }

    public enum MatrixModeEnum : int
    {
        ModelView = 0x1700,
        Projection = 0x1701,
        Texture = 0x1702,
    }

    public enum BeginMode : int
    {
        Points = 0x0000,
        Lines = 0x0001,
        LineLoop = 0x0002,
        LineStrip = 0x0003,
        Triangles = 0x0004,
        TriangleStrip = 0x0005,
        TriangleFan = 0x0006,
        Quads = 0x0007,
        QuadStrip = 0x0008,
        Polygon = 0x0009,
    }

    public enum DrawElementsType : int
    {
        UnsignedByte = 0x1401,
        UnsignedShort = 0x1403,
        UnsignedInt = 0x1405,
    }

    // GL_ARB_multitexture
    public enum TextureUnit : int
    {
        Texture0 = 0x84C0,
        Texture1 = 0x84C1,
        Texture2 = 0x84C2,
        Texture3 = 0x84C3,
        Texture4 = 0x84C4,
        Texture5 = 0x84C5,
        Texture6 = 0x84C6,
        Texture7 = 0x84C7,
        Texture8 = 0x84C8,
        Texture9 = 0x84C9,
        Texture10 = 0x84CA,
        Texture11 = 0x84CB,
        Texture12 = 0x84CC,
        Texture13 = 0x84CD,
        Texture14 = 0x84CE,
        Texture15 = 0x84CF,
        Texture16 = 0x84D0,
        Texture17 = 0x84D1,
        Texture18 = 0x84D2,
        Texture19 = 0x84D3,
        Texture20 = 0x84D4,
        Texture21 = 0x84D5,
        Texture22 = 0x84D6,
        Texture23 = 0x84D7,
        Texture24 = 0x84D8,
        Texture25 = 0x84D9,
        Texture26 = 0x84DA,
        Texture27 = 0x84DB,
        Texture28 = 0x84DC,
        Texture29 = 0x84DD,
        Texture30 = 0x84DE,
        Texture31 = 0x84DF,
    }

    // GL_ARB_vertex_buffer_object
    public enum BufferTarget : int
    {
        ArrayBuffer = 0x8892,
        ElementArrayBuffer = 0x8893,

        // GL_ARB_pixel_buffer_object
        PixelPackBuffer = 0x88EB,
        PixelUnpackBuffer = 0x88EC,
    }

    public enum BufferUsage : int
    {
        StreamDraw = 0x88E0,
        StreamRead = 0x88E1,
        StreamCopy = 0x88E2,
        StaticDraw = 0x88E4,
        StaticRead = 0x88E5,
        StaticCopy = 0x88E6,
        DynamicDraw = 0x88E8,
        DynamicRead = 0x88E9,
        DynamicCopy = 0x88EA,
    }

    public enum BufferAccess : int
    {
        ReadOnly = 0x88B8,
        WriteOnly = 0x88B9,
        ReadWrite = 0x88BA,
    }

    // GL_EXT_stencil_two_side
    public enum StencilFaceDirection : int
    {
        Front = 0x0404,
        Back = 0x0405,
    }

    // GL_ATI_separate_stencil
    public enum StencilOpSeperateFace : int
    {
        Front = 0x0404,
        Back = 0x0405,
        FrontAndBack = 0x0408,
    }

    // GL_ARB_shader_objects
    public enum ShaderObjectType : int
    {
        FragmentShader = 0x8B30,
        VertexShader = 0x8B31,
    }

    public enum ObjectParameter : int
    {
        Type = 0x8B4E,
        Subtype = 0x8B4F,
        DeleteStatus = 0x8B80,
        CompileStatus = 0x8B81,
        LinkStatus = 0x8B82,
        ValidateStatus = 0x8B83,
        InfoLogLength = 0x8B84,
        //AttachedObjects = 0x8B85,
        ActiveUniforms = 0x8B86,
        ActiveUniformMaxLength = 0x8B87,
        //ShaderSourceLength = 0x8B88,

        // GL_ARB_vertex_shader
        ActiveAttributes = 0x8B89,
        ActiveAttributeMaxLength = 0x8B8A,
    }

    public enum UniformType : int
    {
        Float = 0x1406,
        Float2 = 0x8B50,
        Float3 = 0x8B51,
        Float4 = 0x8B52,
        Int = 0x1404,
        Int2 = 0x8B53,
        Int3 = 0x8B54,
        Int4 = 0x8B55,
        Bool = 0x8B56,
        Bool2 = 0x8B57,
        Bool3 = 0x8B58,
        Bool4 = 0x8B59,
        Matrix2 = 0x8B5A,
        Matrix3 = 0x8B5B,
        Matrix4 = 0x8B5C,
        Sampler1D = 0x8B5D,
        Sampler2D = 0x8B5E,
        Sampler3D = 0x8B5F,
        SamplerCube = 0x8B60,
        Sampler1DShadow = 0x8B61,
        Sampler2DShadow = 0x8B62,
        Sampler2DRect = 0x8B63,
        Sampler2DRectShadow = 0x8B64,
    }

    // GL_ARB_vertex_shader
    public enum VertexAttributePointerType : int
    {
        Byte = 0x1400,
        UnsignedByte = 0x1401,
        Short = 0x1402,
        UnsignedShort = 0x1403,
        Int = 0x1404,
        UnsignedInt = 0x1405,
        Float = 0x1406,
        Double = 0x140A,
    }

    public enum ActiveAttributeType : int
    {
        Float = 0x1406,
        Float2 = 0x8B50,
        Float3 = 0x8B51,
        Float4 = 0x8B52,
        Matrix2 = 0x8B5A,
        Matrix3 = 0x8B5B,
        Matrix4 = 0x8B5C,
    }

    // GL_EXT_framebuffer_blit
    public enum BlitFramebufferFilter : int
    {
        Nearest = 0x2600,
        Linear = 0x2601,
    }

    // GL_EXT_framebuffer_object
    public enum RenderbufferType : int
    {
        Renderbuffer = 0x8D41,
    }

    public enum RenderbufferParameter : int
    {
        Width = 0x8D42,
        Height = 0x8D43,
        InternalFormat = 0x8D44,
        RedSize = 0x8D50,
        GreenSize = 0x8D51,
        BlueSize = 0x8D52,
        AlphaSize = 0x8D53,
        DepthSize = 0x8D54,
        StencilSize = 0x8D55,

        // GL_EXT_framebuffer_multisample
        Samples = 0x8CAB,
    }

    public enum FramebufferType : int
    {
        Framebuffer = 0x8D40,

        // GL_EXT_framebuffer_blit
        ReadFramebuffer = 0x8CA8,
        DrawFramebuffer = 0x8CA9,
    }

    public enum FramebufferAttachment : int
    {
        Color0 = 0x8CE0,
        Color1 = 0x8CE1,
        Color2 = 0x8CE2,
        Color3 = 0x8CE3,
        Color4 = 0x8CE4,
        Color5 = 0x8CE5,
        Color6 = 0x8CE6,
        Color7 = 0x8CE7,
        Color8 = 0x8CE8,
        Color9 = 0x8CE9,
        Color10 = 0x8CEA,
        Color11 = 0x8CEB,
        Color12 = 0x8CEC,
        Color13 = 0x8CED,
        Color14 = 0x8CEE,
        Color15 = 0x8CEF,
        DepthAttachment = 0x8D00,
        StencilAttachment = 0x8D20,
    }

    public enum FramebufferStatus : int
    {
        Complete = 0x8CD5,
        IncompleteAttachment = 0x8CD6,
        IncompleteMissingAttachment = 0x8CD7,
        IncompleteDimensions = 0x8CD9,
        IncompleteFormats = 0x8CDA,
        IncompleteDrawBuffer = 0x8CDB,
        IncompleteReadBuffer = 0x8CDC,
        Unsupported = 0x8CDD,
    }
}
