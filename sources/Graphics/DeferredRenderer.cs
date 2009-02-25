using System.Collections.Generic;
using Game.Graphics.Renderer.OpenGL;

namespace Game
{
    public delegate void RenderObjects(Matrix4 viewMatrix);

    public sealed class DeferredRenderer : System.IDisposable
    {
        const int SHADOW_MAP_SIZE = 256;

        public DeferredRenderer(Device device)
        {
            this.device = device;

            CreateFramebuffers();
            CreateBuffers();
            CreateShaders();
        }

        public void Dispose()
        {
            unit_quad_VB.Dispose();

            point_light_VB.Dispose();
            point_light_IB.Dispose();
            spot_light_VB.Dispose();
            spot_light_IB.Dispose();

            deferred_fb.Dispose();
            lighting_fb.Dispose();
            shadow_fb.Dispose();

            shadow_texture.Dispose();
            shadow_texture_cube.Dispose();

            depth_texture.Dispose();
            normal_texture.Dispose();
            color_texture.Dispose();
            lighting_output.Dispose();

            deferred_geometry.Dispose();
            deferred_ambient.Dispose();
            deferred_pointlight.Dispose();
            deferred_spotlight.Dispose();
            generate_shadowmap.Dispose();
            generate_dp_shadowmap.Dispose();
        }

        public void Render(RenderObjects renderObjects, Matrix4 projectionMatrix, Matrix4 viewMatrix)
        {
            device.SetFramebuffer(deferred_fb);

            using (RenderStateBinder binder = device.ChangeRenderState())
            {
                device.RenderState.DepthWrite = true;
                device.RenderState.DepthTest = true;
                device.ApplyRenderState();
                device.Clear(BufferMask.ColorBuffer | BufferMask.DepthBuffer | BufferMask.StencilBuffer);
            }

            device.SetShader(deferred_geometry);

            device.SetMatrix(MatrixModeEnum.Projection, projectionMatrix);

            renderObjects(viewMatrix);

            // TODO: add this as last pass
            device.SetFramebuffer(lighting_fb);

            using (RenderStateBinder binder = device.ChangeRenderState())
            {
                device.RenderState.DepthTest = false;
                device.RenderState.DepthWrite = false;
                device.ApplyRenderState();

                device.SetShader(deferred_ambient);
                deferred_ambient.GetSampler("color_texture").Set(color_texture);
                deferred_ambient.GetUniform("ambient_light").Set(0.7f); // TODO: full diffuse color for directional light

                device.SetVertexBuffer(unit_quad_VB, 0);
                device.DrawArrays(BeginMode.TriangleStrip, 0, 4);
            }
        }

        void DrawLight(int idx, RenderObjects renderObjects, SpotLight light, Matrix4 m, Matrix4 projectionMatrix, Matrix4 invViewMatrix, Camera camera, float zNear)
        {
            using (RenderStateBinder binder = device.ChangeRenderState())
            {
                device.RenderState.DepthWrite = true;
                device.RenderState.DepthTest = true;
                device.RenderState.ColorWrite = false;
                device.RenderState.FrontFace = FrontFaceDirection.CCW;
                device.RenderState.PolygonOffset = true;
                device.RenderState.PolygonOffsetFactor = 0.0f;
                device.RenderState.PolygonOffsetUnits = -1.0f;
                device.ApplyRenderState();

                shadow_fb.SetTexture2D(FramebufferAttachment.DepthAttachment, 0, shadow_texture);
                device.SetFramebuffer(shadow_fb);

                device.Clear(BufferMask.DepthBuffer);

                device.SetShader(generate_shadowmap);

                device.SetMatrix(MatrixModeEnum.Projection, light.ProjectionMatrix);
                renderObjects(light.ViewMatrix);
            }

            device.SetFramebuffer(lighting_fb);
            device.SetMatrix(MatrixModeEnum.Projection, projectionMatrix);

            using (RenderStateBinder binder = device.ChangeRenderState())
            {
                device.SetShader(deferred_spotlight);
                deferred_spotlight.GetSampler("color_texture").Set(color_texture);
                deferred_spotlight.GetSampler("normal_texture").Set(normal_texture);
                deferred_spotlight.GetSampler("depth_texture").Set(depth_texture);
                deferred_spotlight.GetSampler("shadow_texture").Set(shadow_texture);

                deferred_spotlight.GetUniform("buffer_range").Set(1.0f / device.Width, 1.0f / device.Height);
                deferred_spotlight.GetUniform("ndc_to_view").Set(m);

                deferred_spotlight.GetUniform("shadow_matrix").Set(invViewMatrix * light.ShadowMatrix);
                deferred_spotlight.GetUniform("light_params").Set(light.Ambient, light.Diffuse, light.Specular, light.Shininess);
                deferred_spotlight.GetUniform("light_params2").Set(light.Length2, light.AngleParam1, light.AngleParam2, light.Exponent);
                deferred_spotlight.GetUniform("light_position").Set(Vector3.TransformPosition(light.Position, camera.ViewMatrix));
                deferred_spotlight.GetUniform("light_direction").Set(Vector3.TransformVector(light.Direction, camera.ViewMatrix));
                deferred_spotlight.GetUniform("light_color").Set(light.Color);

                device.SetVertexBuffer(spot_light_VB, 0);
                device.SetIndexBuffer(spot_light_IB);

                device.SetMatrix(MatrixModeEnum.ModelView, light.WorldMatrix * camera.ViewMatrix);

                bool inside = false;
                Vector3 p = (camera.Position + camera.ZAxis * zNear) - light.Position;
                float dot = Vector3.Dot(p, light.Direction);
                if (dot > 0.0f || dot < light.Length)
                {
                    float cosBeta = dot / p.Length;
                    if (cosBeta > Degrees.Cos(light.Angle + 5))
                    {
                        inside = true;
                    }
                }

                device.RenderState.DepthWrite = false;
                device.RenderState.Blend = true;
                device.RenderState.BlendingFuncDst = BlendingFactorDest.One;
                device.RenderState.BlendingFuncSrc = BlendingFactorSrc.One;

                if (inside)
                {
                    device.RenderState.FrontFace = FrontFaceDirection.CCW;
                    device.RenderState.DepthFunction = DepthFunction.GEqual;
                    device.RenderState.StencilTest = false;
                    device.ApplyRenderState();

                    device.DrawElements(BeginMode.TriangleStrip, 0, spot_light_primitives);
                }
                else
                {
                    if (has_stencil)
                    {
                        device.RenderState.FrontFace = FrontFaceDirection.CW;
                        device.RenderState.ColorWrite = false;
                        device.RenderState.DepthFunction = DepthFunction.Less;
                        device.RenderState.StencilTest = true;
                        device.RenderState.StencilFunc = StencilFunction.Always;
                        device.RenderState.StencilFail = StencilOpEnum.Keep;
                        device.RenderState.StencilZFail = StencilOpEnum.Keep;
                        device.RenderState.StencilZPass = StencilOpEnum.Replace;
                        device.RenderState.StencilFuncReference = idx;
                        device.ApplyRenderState();

                        device.DrawElements(BeginMode.TriangleStrip, 0, spot_light_primitives);

                        device.RenderState.FrontFace = FrontFaceDirection.CCW;
                        device.RenderState.ColorWrite = true;
                        device.RenderState.DepthFunction = DepthFunction.GEqual;
                        device.RenderState.StencilFunc = StencilFunction.Equal;
                        device.RenderState.StencilZPass = StencilOpEnum.Keep;
                        device.RenderState.StencilFuncReference = idx;
                        device.ApplyRenderState();

                        device.DrawElements(BeginMode.TriangleStrip, 0, spot_light_primitives);
                    }
                    else
                    {
                        device.RenderState.FrontFace = FrontFaceDirection.CW;
                        device.RenderState.DepthFunction = DepthFunction.Less;
                        device.RenderState.StencilTest = false;
                        device.ApplyRenderState();

                        device.DrawElements(BeginMode.TriangleStrip, 0, spot_light_primitives);
                    }
                }
            }
        }

        void DrawLight(int idx, RenderObjects renderObjects, PointLight light, Matrix4 m, Matrix4 projectionMatrix, Matrix4 invViewMatrix, Camera camera, float zNear)
        {
            using (RenderStateBinder binder = device.ChangeRenderState())
            {
                device.RenderState.DepthWrite = true;
                device.RenderState.DepthTest = true;
                device.RenderState.ColorWrite = false;
                device.RenderState.FrontFace = FrontFaceDirection.CCW;
                device.RenderState.PolygonOffset = true;
                device.RenderState.PolygonOffsetFactor = -1.0f;
                device.RenderState.PolygonOffsetUnits = -1.0f;
                device.ApplyRenderState();

                device.SetShader(generate_dp_shadowmap);

                Vector3[,] direction = new Vector3[6, 2]
                {
                    { -Vector3.UnitX, Vector3.UnitY },
                    { Vector3.UnitX, Vector3.UnitY },
                    { -Vector3.UnitY, -Vector3.UnitZ },
                    { Vector3.UnitY, Vector3.UnitZ },
                    { -Vector3.UnitZ, Vector3.UnitY },
                    { Vector3.UnitZ, Vector3.UnitY },
                };

                TextureTarget[] targets = new TextureTarget[6]
                {
                    TextureTarget.TextureCubeMapPositiveX,
                    TextureTarget.TextureCubeMapNegativeX,
                    TextureTarget.TextureCubeMapPositiveY,
                    TextureTarget.TextureCubeMapNegativeY,
                    TextureTarget.TextureCubeMapPositiveZ,
                    TextureTarget.TextureCubeMapNegativeZ,
                };

                device.SetMatrix(MatrixModeEnum.Projection, Matrix4.Perspective(90.0f, 1.0f, Light.zNear, light.Radius));
                device.SetFramebuffer(shadow_fb);

                for (int i = 0; i < 6; i++)
                {
                    shadow_fb.SetTextureCubeMap(FramebufferAttachment.DepthAttachment, targets[i], 0, shadow_texture_cube);
                    device.Clear(BufferMask.DepthBuffer);

                    Matrix4 lightViewMatrix = Matrix4.LookAt(light.Position, light.Position + direction[i, 0], direction[i, 1]);

                    renderObjects(lightViewMatrix);
                }


                //// front
                //shadow_fb.SetTexture2D(FramebufferAttachment.DepthAttachment, 0, shadow_texture[1]);
                //device.SetFramebuffer(shadow_fb);
                //device.Clear(BufferMask.DepthBuffer);

                //generate_dp_shadowmap.GetUniform("direction").Set(1.0f);

                //Vector3 pos = light.Position;// -new Vector3(0.0f, 0.0f, 0.02f);
                //Matrix4 lightViewMatrix = Matrix4.LookAt(pos, pos + Vector3.UnitZ, Vector3.UnitY);

                //renderObjects(lightViewMatrix);

                //// back
                //shadow_fb.SetTexture2D(FramebufferAttachment.DepthAttachment, 0, shadow_texture[2]);
                //device.Clear(BufferMask.DepthBuffer);

                //generate_dp_shadowmap.GetUniform("direction").Set(-1.0f);

                //pos = light.Position;// -new Vector3(0.0f, 0.0f, 0.02f);
                //lightViewMatrix = Matrix4.LookAt(pos, pos + Vector3.UnitZ, Vector3.UnitY);
                //renderObjects(lightViewMatrix);
            }

            device.SetFramebuffer(lighting_fb);
            device.SetMatrix(MatrixModeEnum.Projection, projectionMatrix); 
            
            using (RenderStateBinder binder = device.ChangeRenderState())
            {
                device.SetShader(deferred_pointlight);
                deferred_pointlight.GetSampler("color_texture").Set(color_texture);
                deferred_pointlight.GetSampler("normal_texture").Set(normal_texture);
                deferred_pointlight.GetSampler("depth_texture").Set(depth_texture);
                //deferred_pointlight.GetSampler("shadowFront_texture").Set(shadow_texture[1]);
                //deferred_pointlight.GetSampler("shadowBack_texture").Set(shadow_texture[2]);
                deferred_pointlight.GetSampler("shadow_texture").Set(shadow_texture_cube);

                deferred_pointlight.GetUniform("buffer_range").Set(1.0f / device.Width, 1.0f / device.Height);

                deferred_pointlight.GetUniform("ndc_to_view").Set(m);

                device.SetVertexBuffer(point_light_VB, 0);
                device.SetIndexBuffer(point_light_IB);

                deferred_pointlight.GetUniform("shadow_matrix").Set(invViewMatrix);
                deferred_pointlight.GetUniform("light_params").Set(light.Ambient, light.Diffuse, light.Specular, light.Shininess);
                deferred_pointlight.GetUniform("light_position").Set(Vector3.TransformPosition(light.Position, camera.ViewMatrix));
                deferred_pointlight.GetUniform("light_positionW").Set(light.Position);
                deferred_pointlight.GetUniform("light_radius2").Set(light.Radius2);
                deferred_pointlight.GetUniform("light_color").Set(light.Color);
                deferred_pointlight.GetUniform("depth_param1").Set(light.DepthParam1);
                deferred_pointlight.GetUniform("depth_param2").Set(light.DepthParam2);

                device.SetMatrix(MatrixModeEnum.ModelView, light.WorldMatrix * camera.ViewMatrix);

                Vector3 p = camera.Position - light.Position;

                device.RenderState.DepthWrite = false;
                device.RenderState.Blend = true;
                device.RenderState.BlendingFuncDst = BlendingFactorDest.One;
                device.RenderState.BlendingFuncSrc = BlendingFactorSrc.One;

                if (p.Length < light.Radius + 2.0f * zNear)
                {
                    device.RenderState.FrontFace = FrontFaceDirection.CCW;
                    device.RenderState.DepthFunction = DepthFunction.GEqual;
                    device.RenderState.StencilTest = false;
                    device.ApplyRenderState();

                    device.DrawElements(BeginMode.TriangleStrip, 0, point_light_primitives);
                }
                else
                {
                    if (has_stencil)
                    {
                        device.RenderState.FrontFace = FrontFaceDirection.CW;
                        device.RenderState.ColorWrite = false;
                        device.RenderState.DepthFunction = DepthFunction.Less;
                        device.RenderState.StencilTest = true;
                        device.RenderState.StencilFunc = StencilFunction.Always;
                        device.RenderState.StencilFail = StencilOpEnum.Keep;
                        device.RenderState.StencilZFail = StencilOpEnum.Keep;
                        device.RenderState.StencilZPass = StencilOpEnum.Replace;
                        device.RenderState.StencilFuncReference = idx;
                        device.ApplyRenderState();

                        device.DrawElements(BeginMode.TriangleStrip, 0, point_light_primitives);

                        device.RenderState.FrontFace = FrontFaceDirection.CCW;
                        device.RenderState.ColorWrite = true;
                        device.RenderState.DepthFunction = DepthFunction.GEqual;
                        device.RenderState.StencilFunc = StencilFunction.Equal;
                        device.RenderState.StencilZPass = StencilOpEnum.Keep;
                        device.RenderState.StencilFuncReference = idx;
                        device.ApplyRenderState();

                        device.DrawElements(BeginMode.TriangleStrip, 0, point_light_primitives);
                    }
                    else
                    {
                        device.RenderState.FrontFace = FrontFaceDirection.CW;
                        device.RenderState.DepthFunction = DepthFunction.Less;
                        device.RenderState.StencilTest = false;
                        device.ApplyRenderState();

                        device.DrawElements(BeginMode.TriangleStrip, 0, point_light_primitives);
                    }
                }
            }
        }

        public void DrawLights(RenderObjects renderObjects, List<Light> lights, Matrix4 projectionMatrix, float zNear, Camera camera)
        {
            if (lights.Count == 0)
            {
                return;
            }

            Matrix4 invProjectionMatrix = Matrix4.Invert(projectionMatrix);
            Matrix4 invViewMatrix = Matrix4.Invert(camera.ViewMatrix);

            Matrix4 m = 
                    new Matrix4(
                        new Vector4(2.0f, 0.0f, 0.0f, 0.0f),
                        new Vector4(0.0f, 2.0f, 0.0f, 0.0f),
                        new Vector4(0.0f, 0.0f, 2.0f, 0.0f),
                        new Vector4(-1.0f, -1.0f, -1.0f, 1.0f))
                  * invProjectionMatrix;

            int idx = 0;
            foreach (Light light in lights)
            {
                idx++;

                if (light is SpotLight)
                {
                    DrawLight(idx, renderObjects, (SpotLight)light, m, projectionMatrix, invViewMatrix, camera, zNear);
                }
                else // light is PointLight
                {
                    DrawLight(idx, renderObjects, (PointLight)light, m, projectionMatrix, invViewMatrix, camera, zNear);
                }
            }
        }

        public void Finish()
        {
            // TODO: Postprocessing effects
            //

            device.Blit(lighting_fb, null, BufferMask.ColorBuffer, BlitFramebufferFilter.Nearest);
        }

        void CreateShaders()
        {
            deferred_geometry = Loaders.LoadShader<VF.PositionTexcoordNT>("deferred_geometry");
            deferred_ambient = Loaders.LoadShader<VF.Position2>("deferred_ambient");
            deferred_pointlight = Loaders.LoadShader<VF.Position3>("deferred_pointlight");
            deferred_spotlight = Loaders.LoadShader<VF.Position3>("deferred_spotlight");
            generate_shadowmap = Loaders.LoadShader<VF.PositionTexcoordNT>("generate_shadowmap");
            generate_dp_shadowmap = Loaders.LoadShader<VF.PositionTexcoordNT>("generate_dp_shadowmap");
        }

        void CreateBuffers()
        {
            unit_quad_VB = device.CreateVertexBuffer<VF.Position2>(BufferUsage.StaticDraw,
                new VF.Position2[4]
                {
                    new VF.Position2() { Position = new Vector2(-1.0f, -1.0f) },
                    new VF.Position2() { Position = new Vector2(-1.0f,  1.0f) },
                    new VF.Position2() { Position = new Vector2( 1.0f, -1.0f) },
                    new VF.Position2() { Position = new Vector2( 1.0f,  1.0f) },
                }
            );

            point_light_primitives = Utils.generateLightSphere(8, 8, out point_light_VB, out point_light_IB);
            spot_light_primitives = Utils.generateLightCone(1, 8, out spot_light_VB, out spot_light_IB);
        }

        void CreateFramebuffers()
        {
            deferred_fb = device.CreateFrameBuffer(device.Width, device.Height);
            lighting_fb = device.CreateFrameBuffer(device.Width, device.Height);
            shadow_fb = device.CreateFrameBuffer(SHADOW_MAP_SIZE, SHADOW_MAP_SIZE);

            if (device.Extensions.NV_depth_buffer_float)
            {
                has_stencil = true;
                depth_texture = device.CreateTexture2D(InternalFormat.DepthComponent32F_Stencil8_NV, device.Width, device.Height);

                shadow_texture = device.CreateTexture2D(InternalFormat.DepthComponent32F_NV, SHADOW_MAP_SIZE, SHADOW_MAP_SIZE);
                shadow_texture_cube = device.CreateTextureCubeMap(InternalFormat.DepthComponent32F_NV, SHADOW_MAP_SIZE);
            }
            else if (device.Extensions.ARB_depth_buffer_float)
            {
                // TODO: does not work on ATI
                //has_stencil = true;
                //depth_texture = device.CreateTexture2D(InternalFormat.Depth24Stencil8, device.Width, device.Height);

                has_stencil = false;
                depth_texture = device.CreateTexture2D(InternalFormat.DepthComponent32F, device.Width, device.Height);

                shadow_texture = device.CreateTexture2D(InternalFormat.DepthComponent32F, SHADOW_MAP_SIZE, SHADOW_MAP_SIZE);
                shadow_texture_cube = device.CreateTextureCubeMap(InternalFormat.DepthComponent32F, SHADOW_MAP_SIZE);
            }
            else
            {
                throw new System.ApplicationException("no hardware floating shadow maps available!");
            }

            depth_texture.SetFilter(TextureMagFilter.Nearest, TextureMinFilter.Nearest);
            depth_texture.SetWrap(TextureWrap.ClampToEdge, TextureWrap.ClampToEdge);
            depth_texture.SetCompareMode(TextureCompareMode.None);

            shadow_texture.SetFilter(TextureMagFilter.Linear, TextureMinFilter.Linear);
            shadow_texture.SetWrap(TextureWrap.ClampToEdge, TextureWrap.ClampToEdge);
            shadow_texture.SetCompareMode(TextureCompareMode.CompareRToTexture);
            shadow_texture.SetCompareFunc(TextureCompareFunc.LEqual);
            
            shadow_texture_cube.SetFilter(TextureMagFilter.Linear, TextureMinFilter.Linear);
            shadow_texture_cube.SetWrap(TextureWrap.ClampToEdge, TextureWrap.ClampToEdge);
            //shadow_texture_cube.SetCompareMode(TextureCompareMode.CompareRToTexture);
            //shadow_texture_cube.SetCompareFunc(TextureCompareFunc.LEqual);

            //color_texture = device.CreateTexture2D(InternalFormat.RGBA8, device.Width, device.Height); // fallback
            color_texture = device.CreateTexture2D(InternalFormat.RGBA16F, device.Width, device.Height);
            color_texture.SetFilter(TextureMagFilter.Nearest, TextureMinFilter.Nearest);
            color_texture.SetWrap(TextureWrap.ClampToEdge, TextureWrap.ClampToEdge);

            //normal_texture = device.CreateTexture2D(InternalFormat.RGBA8, device.Width, device.Height); // fallback
            normal_texture = device.CreateTexture2D(InternalFormat.RGBA16F, device.Width, device.Height);
            normal_texture.SetFilter(TextureMagFilter.Nearest, TextureMinFilter.Nearest);
            normal_texture.SetWrap(TextureWrap.ClampToEdge, TextureWrap.ClampToEdge);

            lighting_output = device.CreateTexture2D(InternalFormat.RGBA8, device.Width, device.Height);
            //lighting_output = device.CreateTexture2D(InternalFormat.RGB16F, device.Width, device.Height); // for HDR
            lighting_output.SetFilter(TextureMagFilter.Nearest, TextureMinFilter.Nearest);
            lighting_output.SetWrap(TextureWrap.ClampToEdge, TextureWrap.ClampToEdge);

            GL.ReadBuffer(ReadBufferMode.None); // TODO
            GL.DrawBuffer(DrawBufferMode.None); // TODO

            shadow_fb.SetTexture2D(FramebufferAttachment.DepthAttachment, 0, shadow_texture);
            System.Diagnostics.Debug.Assert(shadow_fb.Status == FramebufferStatus.Complete);


            deferred_fb.SetTexture2D(FramebufferAttachment.DepthAttachment, 0, depth_texture);
            if (has_stencil)
            {
                deferred_fb.SetTexture2D(FramebufferAttachment.StencilAttachment, 0, depth_texture);
            }
            deferred_fb.SetTexture2D(FramebufferAttachment.Color0, 0, color_texture);
            deferred_fb.SetTexture2D(FramebufferAttachment.Color1, 0, normal_texture);
            System.Diagnostics.Debug.Assert(deferred_fb.Status == FramebufferStatus.Complete);

            lighting_fb.SetTexture2D(FramebufferAttachment.DepthAttachment, 0, depth_texture);
            if (has_stencil)
            {
                lighting_fb.SetTexture2D(FramebufferAttachment.StencilAttachment, 0, depth_texture);
            }
            lighting_fb.SetTexture2D(FramebufferAttachment.Color0, 0, lighting_output);
            System.Diagnostics.Debug.Assert(lighting_fb.Status == FramebufferStatus.Complete);
        }

        Device device;

        FrameBuffer deferred_fb;
        FrameBuffer lighting_fb;
        FrameBuffer shadow_fb;

        Texture2D shadow_texture;
        TextureCubeMap shadow_texture_cube;

        Texture2D color_texture;
        Texture2D normal_texture;
        Texture2D depth_texture;
        Texture2D lighting_output;

        Shader deferred_geometry;
        Shader deferred_ambient;
        Shader deferred_pointlight;
        Shader deferred_spotlight;

        Shader generate_shadowmap;
        Shader generate_dp_shadowmap;

        Buffer unit_quad_VB;

        Buffer point_light_VB;
        Buffer point_light_IB;
        Buffer spot_light_VB;
        Buffer spot_light_IB;

        bool has_stencil;

        int point_light_primitives;
        int spot_light_primitives;
    }
}
