using System;

namespace Game.Graphics.Renderer.OpenGL
{
    public sealed class RenderStateBinder : IDisposable
    {
        internal RenderState oldState;

        internal RenderStateBinder()
        {
        }

        public void Dispose()
        {
            Device.Current.RenderState = oldState;
        }
    }

    public struct RenderState
    {
        public static readonly RenderState Default = new RenderState()
        {
            CullFace = false,
            CullFaceMode = CullFaceMode.Back,
            FrontFace = FrontFaceDirection.CCW,

            PolygonOffset = false,
            PolygonOffsetFactor = 0,
            PolygonOffsetUnits = 0,
            PolygonMode = PolygonMode.Fill,

            ClearColor = Vector4.Zero,
            ClearStencil = 0,
            ClearDepth = 1,

            StencilMask = 0xFFFFFFFF,
            ColorWrite = true,
            DepthWrite = true,

            AlphaTest = false,
            AlphaFunc = AlphaFunction.Always,
            AlphaFuncReference = 0.0f,

            Blend = false,
            BlendingFuncSrc = BlendingFactorSrc.One,
            BlendingFuncDst = BlendingFactorDest.Zero,

            StencilTest = false,
            StencilTestTwoSide = false,
            StencilFunc = StencilFunction.Always,
            StencilFuncReference = 0,
            StencilFuncMask = 0xFFFFFFFF,
            StencilFail = StencilOpEnum.Keep,
            StencilZFail = StencilOpEnum.Keep,
            StencilZPass = StencilOpEnum.Keep,

            DepthTest = false,
            DepthFunction = DepthFunction.Less,

            ScissorTest = false,

            ClipPlane0 = false,
            ClipPlane1 = false,
            ClipPlane2 = false,
            ClipPlane3 = false,
            ClipPlane4 = false,
            ClipPlane5 = false,

            Multisample = false,
        };

        public bool CullFace;
        public CullFaceMode CullFaceMode;
        public FrontFaceDirection FrontFace;

        public bool PolygonOffset;
        public float PolygonOffsetFactor;
        public float PolygonOffsetUnits;
        public PolygonMode PolygonMode;
        
        public Vector4 ClearColor;
        public float ClearStencil;
        public float ClearDepth;

        public uint StencilMask;
        public bool ColorWrite;
        public bool DepthWrite;

        public bool AlphaTest;
        public AlphaFunction AlphaFunc;
        public float AlphaFuncReference;

        public bool Blend;
        public BlendingFactorSrc BlendingFuncSrc;
        public BlendingFactorDest BlendingFuncDst;

        public bool StencilTest;
        public bool StencilTestTwoSide;
        public StencilFunction StencilFunc;
        public int StencilFuncReference;
        public uint StencilFuncMask;
        public StencilOpEnum StencilFail;
        public StencilOpEnum StencilZFail;
        public StencilOpEnum StencilZPass;

        public bool DepthTest;
        public DepthFunction DepthFunction;

        public bool ScissorTest;

        public bool ClipPlane0;
        public bool ClipPlane1;
        public bool ClipPlane2;
        public bool ClipPlane3;
        public bool ClipPlane4;
        public bool ClipPlane5;

        public bool Multisample;

        internal void Apply(RenderState oldState)
        {
            if (CullFace != oldState.CullFace)
            {
                if (CullFace)
                {
                    GL.Enable(EnableCap.CullFace);
                }
                else
                {
                    GL.Disable(EnableCap.CullFace);
                }
            }

            if (CullFaceMode != oldState.CullFaceMode)
            {
                GL.CullFace(CullFaceMode);
            }

            if (FrontFace != oldState.FrontFace)
            {
                GL.FrontFace(FrontFace);
            }

            if (PolygonOffset != oldState.PolygonOffset)
            {
                if (PolygonOffset)
                {
                    GL.Enable(EnableCap.PolygonOffset);
                }
                else
                {
                    GL.Disable(EnableCap.PolygonOffset);
                }
            }

            if (PolygonOffsetFactor != oldState.PolygonOffsetFactor
                      || PolygonOffsetUnits != oldState.PolygonOffsetUnits)
            {
                if (PolygonOffset)
                {
                    GL.PolygonOffset(PolygonOffsetFactor, PolygonOffsetUnits);
                }
            }

            if (PolygonMode != oldState.PolygonMode)
            {
                GL.PolygonMode(PolygonFace.FrontAndBack, PolygonMode);
            }

            if (ClearColor != oldState.ClearColor)
            {
                GL.ClearColor(ClearColor.X, ClearColor.Y, ClearColor.Z, ClearColor.W);
            }

            if (ClearStencil != oldState.ClearStencil)
            {
                GL.ClearStencil(ClearStencil);
            }

            if (ClearDepth != oldState.ClearDepth)
            {
                GL.ClearDepth(ClearDepth);
            }

            if (StencilMask != oldState.StencilMask)
            {
                GL.StencilMask(StencilMask);
            }

            if (ColorWrite != oldState.ColorWrite)
            {
                if (ColorWrite)
                {
                    GL.ColorMask(1, 1, 1, 1);
                }
                else
                {
                    GL.ColorMask(0, 0, 0, 0);
                }
            }

            if (DepthWrite != oldState.DepthWrite)
            {
                GL.DepthMask(DepthWrite ? 1 : 0);
            }

            if (AlphaTest != oldState.AlphaTest)
            {
                if (AlphaTest)
                {
                    GL.Enable(EnableCap.AlphaTest);
                }
                else
                {
                    GL.Disable(EnableCap.AlphaTest);
                }
            }

            if (AlphaFunc != oldState.AlphaFunc
                      || AlphaFuncReference != oldState.AlphaFuncReference)
            {
                GL.AlphaFunc(AlphaFunc, AlphaFuncReference);
            }

            if (Blend != oldState.Blend)
            {
                if (Blend)
                {
                    GL.Enable(EnableCap.Blend);
                }
                else
                {
                    GL.Disable(EnableCap.Blend);
                }
            }

            if (BlendingFuncSrc != oldState.BlendingFuncSrc || BlendingFuncDst != oldState.BlendingFuncDst)
            {
                if (Blend)
                {
                    GL.BlendFunc(BlendingFuncSrc, BlendingFuncDst);
                }
            }

            if (StencilTest != oldState.StencilTest)
            {
                if (StencilTest)
                {
                    GL.Enable(EnableCap.StencilTest);
                }
                else
                {
                    GL.Disable(EnableCap.StencilTest);
                }
            }

            if (Device.Current.Extensions.EXT_stencil_two_side && (StencilTestTwoSide != oldState.StencilTestTwoSide))
            {
                if (StencilTestTwoSide)
                {
                    GL.Enable(EnableCap.StencilTestTwoSide);
                }
                else
                {
                    GL.Disable(EnableCap.StencilTestTwoSide);
                }
            }

            if (StencilFunc != oldState.StencilFunc
                || StencilFuncReference != oldState.StencilFuncReference
                || StencilFuncMask != oldState.StencilFuncMask)
            {
                if (StencilTest)
                {
                    GL.StencilFunc(StencilFunc, StencilFuncReference, StencilFuncMask);
                }
            }
            if (StencilFail != oldState.StencilFail
                || StencilZFail != oldState.StencilZFail
                || StencilZPass != oldState.StencilZPass)
            {
                if (StencilTest)
                {
                    GL.StencilOp(StencilFail, StencilZFail, StencilZPass);
                }
            }

            if (DepthTest != oldState.DepthTest)
            {
                if (DepthTest)
                {
                    GL.Enable(EnableCap.DepthTest);
                }
                else
                {
                    GL.Disable(EnableCap.DepthTest);
                }
            }

            if (DepthFunction != oldState.DepthFunction)
            {
                if (DepthTest)
                {
                    GL.DepthFunc(DepthFunction);
                }
            }

            if (ScissorTest != oldState.ScissorTest)
            {
                if (ScissorTest)
                {
                    GL.Enable(EnableCap.ScissorTest);
                }
                else
                {
                    GL.Disable(EnableCap.ScissorTest);
                }
            }

            if (ClipPlane0 != oldState.ClipPlane0)
            {
                if (ClipPlane0)
                {
                    GL.Enable(EnableCap.ClipPlane0);
                }
                else
                {
                    GL.Disable(EnableCap.ClipPlane0);
                }
            }

            if (ClipPlane1 != oldState.ClipPlane1)
            {
                if (ClipPlane1)
                {
                    GL.Enable(EnableCap.ClipPlane1);
                }
                else
                {
                    GL.Disable(EnableCap.ClipPlane1);
                }
            }

            if (ClipPlane2 != oldState.ClipPlane2)
            {
                if (ClipPlane2)
                {
                    GL.Enable(EnableCap.ClipPlane2);
                }
                else
                {
                    GL.Disable(EnableCap.ClipPlane2);
                }
            }

            if (ClipPlane3 != oldState.ClipPlane3)
            {
                if (ClipPlane3)
                {
                    GL.Enable(EnableCap.ClipPlane3);
                }
                else
                {
                    GL.Disable(EnableCap.ClipPlane3);
                }
            }

            if (ClipPlane4 != oldState.ClipPlane4)
            {
                if (ClipPlane4)
                {
                    GL.Enable(EnableCap.ClipPlane4);
                }
                else
                {
                    GL.Disable(EnableCap.ClipPlane4);
                }
            }

            if (ClipPlane5 != oldState.ClipPlane5)
            {
                if (ClipPlane5)
                {
                    GL.Enable(EnableCap.ClipPlane5);
                }
                else
                {
                    GL.Disable(EnableCap.ClipPlane5);
                }
            }

            if (Multisample != oldState.Multisample)
            {
                if (Multisample)
                {
                    GL.Enable(EnableCap.Multisample);
                }
                else
                {
                    GL.Disable(EnableCap.Multisample);
                }
            }
        }
    }
}
