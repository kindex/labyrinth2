using System;
using System.Reflection;

namespace Game.Graphics.Renderer.OpenGL
{
    public sealed class Extensions
    {
        public Extensions()
        {
            HashSet<string> ext = new HashSet<string>();
            
            Array.ForEach<string>(
                GL.GetString(StringName.Extensions).Split(' '),
                (string extension) => { if (extension.Length > 0) ext.AddIfNotExists(extension); }
            );

            PropertyInfo[] properties = typeof(Extensions).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                property.SetValue(this, ext.Contains("GL_" + property.Name), null);
            }
        }

        public bool ATI_separate_stencil { get; private set; }
        public bool ATI_texture_compression_3dc { get; private set; }

        public bool ARB_depth_buffer_float { get; private set; }
        public bool NV_depth_buffer_float { get; private set; }
        public bool ARB_depth_texture { get; private set; }
        public bool ARB_draw_buffers { get; private set; }
        public bool ARB_fragment_shader { get; private set; }
        public bool ARB_multisample { get; private set; }
        public bool ARB_multitexture { get; private set; }
        public bool ARB_pixel_buffer_object { get; private set; }
        public bool ARB_shading_language_100 { get; private set; }
        public bool ARB_shadow { get; private set; }
        public bool ARB_texture_compression { get; private set; }
        public bool ARB_texture_cube_map { get; private set; }
        public bool ARB_texture_non_power_of_two { get; private set; }
        public bool ARB_texture_float { get; private set; }
        public bool ARB_vertex_buffer_object { get; private set; }
        public bool ARB_vertex_shader { get; private set; }

        public bool EXT_bgra { get; private set; }
        public bool EXT_draw_range_elements { get; private set; }
        public bool EXT_framebuffer_blit { get; private set; }
        public bool EXT_framebuffer_multisample { get; private set; }
        public bool EXT_framebuffer_object { get; private set; }
        public bool EXT_packed_depth_stencil { get; private set; }
        public bool EXT_packed_pixels { get; private set; }
        public bool EXT_stencil_two_side { get; private set; }
        public bool EXT_stencil_wrap { get; private set; }
        public bool EXT_texture3D { get; private set; }
        public bool EXT_texture_compression_latc { get; private set; }
        public bool EXT_texture_compression_s3tc { get; private set; }
        public bool EXT_texture_filter_anisotropic { get; private set; }

        public bool SGIS_texture_edge_clamp { get; private set; }
    }
}

/* Extensions to add
 * 
 * ARB_depth_buffer_float
 * ARB_draw_instanced / EXT_draw_instanced
 * ARB_framebuffer = EXT_framebuffer_blit + EXT_framebuffer_multisample + EXT_framebuffer_object
 * ARB_geometry_shader4
 * ARB_half_float_vertex
 * ARB_instanced_arrays
 * ARB_texture_rg
 * ARB_vertex_array_object / EXT_vertex_array_object
 * WGL_ARB_create_context / GLX_ARB_create_context = OpenGL 3.0
 * EXT_gpu_shader4
 * EXT_bindable_uniform
 * EXT_texture_integer
 * EXT_direct_state_access !!
 * 
 * ARB_texture_buffer_object / EXT_texture_buffer_object ?
 */
