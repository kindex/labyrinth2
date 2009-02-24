using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Game.Graphics.Renderer.OpenGL
{
    public struct ShaderUniform
    {
        internal ShaderUniform(int location, UniformType type)
        {
            this.location = location;
            this.type = type;
        }
        
        public void Set(bool x)
        {
            if (location != -1)
            {
                Debug.Assert(type == UniformType.Bool);
                GL.Uniform(location, x ? 1 : 0);
            }
        }

        public void Set(int x)
        {
            if (location != -1)
            {
                Debug.Assert(type == UniformType.Int);
                GL.Uniform(location, x);
            }
        }
        
        public void Set(float x)
        {
            if (location != -1)
            {
                Debug.Assert(type == UniformType.Float);
                GL.Uniform(location, x);
            }
        }

        public void Set(int x, int y)
        {
            if (location != -1)
            {
                Debug.Assert(type == UniformType.Int2);
                GL.Uniform(location, x, y);
            }
        }

        public void Set(float x, float y)
        {
            if (location != -1)
            {
                Debug.Assert(type == UniformType.Float2);
                GL.Uniform(location, new Vector2(x, y));
            }
        }

        public void Set(Vector2 v)
        {
            if (location != -1)
            {
                Debug.Assert(type == UniformType.Float2);
                GL.Uniform(location, v);
            }
        }

        public void Set(int x, int y, int z)
        {
            if (location != -1)
            {
                Debug.Assert(type == UniformType.Int3);
                GL.Uniform(location, x, y, z);
            }
        }

        public void Set(float x, float y, float z)
        {
            if (location != -1)
            {
                Debug.Assert(type == UniformType.Float3);
                GL.Uniform(location, new Vector3(x, y, z));
            }
        }

        public void Set(Vector3 v)
        {
            if (location != -1)
            {
                Debug.Assert(type == UniformType.Float3);
                GL.Uniform(location, v);
            }
        }

        public void Set(int x, int y, int z, int w)
        {
            if (location != -1)
            {
                Debug.Assert(type == UniformType.Int4);
                GL.Uniform(location, x, y, z);
            }
        }

        public void Set(float x, float y, float z, float w)
        {
            if (location != -1)
            {
                Debug.Assert(type == UniformType.Float4);
                GL.Uniform(location, new Vector4(x, y, z, w));
            }
        }

        public void Set(Vector4 v)
        {
            if (location != -1)
            {
                Debug.Assert(type == UniformType.Float4);
                GL.Uniform(location, v);
            }
        }

        public void Set(Matrix2 m)
        {
            if (location != -1)
            {
                Debug.Assert(type == UniformType.Matrix2);
                GL.Uniform(location, false, m);
            }
        }

        public void Set(Matrix3 m)
        {
            if (location != -1)
            {
                Debug.Assert(type == UniformType.Matrix3);
                GL.Uniform(location, false, m);
            }
        }

        public void Set(Matrix4 m)
        {
            if (location != -1)
            {
                Debug.Assert(type == UniformType.Matrix4);
                GL.Uniform(location, false, m);
            }
        }

        int location;
        UniformType type;
    }

    public sealed class ShaderSampler
    {
        internal ShaderSampler(int location, TextureUnit unit, UniformType type)
        {
            this.location = location;
            this.unit = unit;
            this.type = type;
        }

        public void Set(Texture1D texture)
        {
            if (location != -1)
            {
                Debug.Assert(type == UniformType.Sampler1D);
                GL.ActiveTexture(unit);
                GL.BindTexture(TextureTarget.Texture1D, texture.Handle);
                GL.Uniform(location, unit - TextureUnit.Texture0);
            }
        }

        public void Set(Texture2D texture)
        {
            if (location != -1)
            {
                Debug.Assert(type == UniformType.Sampler2D || type == UniformType.Sampler2DShadow);
                GL.ActiveTexture(unit);
                GL.BindTexture(TextureTarget.Texture2D, texture.Handle);
                GL.Uniform(location, unit - TextureUnit.Texture0);
            }
        }

        public void Set(Texture3D texture)
        {
            if (location != -1)
            {
                Debug.Assert(type == UniformType.Sampler3D);
                GL.ActiveTexture(unit);
                GL.BindTexture(TextureTarget.Texture3D, texture.Handle);
                GL.Uniform(location, unit - TextureUnit.Texture0);
            }
        }

        public void Set(TextureCubeMap texture)
        {
            if (location != -1)
            {
                Debug.Assert(type == UniformType.SamplerCube);
                GL.ActiveTexture(unit);
                GL.BindTexture(TextureTarget.TextureCubeMap, texture.Handle);
                GL.Uniform(location, unit - TextureUnit.Texture0);
            }
        }

        int location;
        TextureUnit unit;
        UniformType type;
    }

    public sealed class Shader : IDisposable
    {
        internal Shader(VertexFormat vertexFormat, params string[] shaders)
        {
            this.VertexFormat = vertexFormat;

            string[] vertex_sources = new string[shaders.Length];
            string[] fragment_sources = new string[shaders.Length];
            for (int i = 0; i < shaders.Length; i++)
            {
                SplitSource(shaders[i], out vertex_sources[i], out fragment_sources[i]);
            }
            vertex_sources[0] = "#version 120\n" + vertex_sources[0];
            fragment_sources[0] = "#version 120\n" + fragment_sources[0];

            uint vshader = GL.CreateShaderObject(ShaderObjectType.VertexShader);
            uint fshader = GL.CreateShaderObject(ShaderObjectType.FragmentShader);

            try
            {
                GL.ShaderSource(vshader, shaders.Length, vertex_sources, IntPtr.Zero);
                GL.ShaderSource(fshader, shaders.Length, fragment_sources, IntPtr.Zero);
                GL.CompileShader(vshader);
                GL.CompileShader(fshader);

                int vparam;
                int fparam;
                GL.GetObjectParameter(vshader, ObjectParameter.CompileStatus, out vparam);
                GL.GetObjectParameter(fshader, ObjectParameter.CompileStatus, out fparam);

                if (vparam != 1)
                {
                    ErrorLog = GL.GetInfoLog(vshader);
                    return;
                }

                if (fparam != 1)
                {
                    ErrorLog = GL.GetInfoLog(fshader);
                    return;
                }

                this.shader = GL.CreateProgramObject();
                GL.AttachObject(shader, vshader);
                GL.AttachObject(shader, fshader);
            }
            finally
            {
                GL.DeleteObject(vshader);
                GL.DeleteObject(fshader);
            }

            VertexAttribute[] attributes = vertexFormat.attributes;

            for (int i = 0; i < attributes.Length; i++)
            {
                GL.BindAttribLocation(shader, i, attributes[i].name);
            }
        }

        const string VertexShaderHead = "[Vertex Shader]";
        const string FragmentShaderHead = "[Fragment Shader]";

        void SplitSource(string source, out string vertex_source, out string fragment_source)
        {
            int vpos = source.IndexOf(VertexShaderHead);
            int fpos = source.IndexOf(FragmentShaderHead);
            Debug.Assert(vpos != -1, "Shader has invalid format - no vertex shader source found!");
            Debug.Assert(fpos != -1, "Shader has invalid format - no fragment shader source found!");

            int vpos_start = vpos;
            int vpos_end = fpos;

            vpos += VertexShaderHead.Length;
            fpos += FragmentShaderHead.Length;
            while (source[vpos] != '\n')
            {
                vpos++;
            }

            while (source[fpos] != '\n')
            {
                fpos++;
            }

            int vline = CountLines(source, 0, vpos);
            int fline = vline + CountLines(source, vpos, fpos);

            string common = source.Substring(0, vpos_start);
            vertex_source = String.Format("{0}\n#line {1}\n{2}", common, vline, source.Substring(vpos, vpos_end - vpos));
            fragment_source = String.Format("{0}\n#line {1}\n{2}", common, fline, source.Substring(fpos));
        }

        int CountLines(string source, int start, int end)
        {
            int lines = 0;
            for (int i = start; i < end; i++)
            {
                if (source[i] == '\n')
                {
                    lines++;
                }
            }
            return lines;
        }

        public void Dispose()
        {
            if (shader != 0)
            {
                GL.DeleteObject(shader);
            }
            Device.Current.Dispose(this);
        }

        public bool Link()
        {
            if (shader == 0)
            {
                return false;
            }

            GL.LinkProgram(shader);
            int param;
            GL.GetObjectParameter(shader, ObjectParameter.LinkStatus, out param);
            ErrorLog = GL.GetInfoLog(shader);
            if (param == 1)
            {
                int uniform_count;
                int uniform_maxlen;
                GL.GetObjectParameter(shader, ObjectParameter.ActiveUniforms, out uniform_count);
                GL.GetObjectParameter(shader, ObjectParameter.ActiveUniformMaxLength, out uniform_maxlen);

                uniform_locations = new Dictionary<string, int>();
                uniform_types = new UniformType[uniform_count];
                sampler_locations = new Dictionary<string, int>();
                sampler_types = new UniformType[uniform_count];
                sampler_units = new TextureUnit[uniform_count];

                TextureUnit unit = TextureUnit.Texture0;
                for (int i = 0; i < uniform_count; i++)
                {
                    int len;
                    int size;
                    UniformType type;
                    StringBuilder sb = new StringBuilder(uniform_maxlen);
                    GL.GetActiveUniform(shader, i, uniform_maxlen, out len, out size, out type, sb);
                    if (type >= UniformType.Sampler1D && type <= UniformType.Sampler2DRectShadow)
                    {
                        sampler_locations.Add(sb.ToString().Substring(0, len), i);
                        sampler_types[i] = type;
                        sampler_units[i] = unit;
                        unit++;
                    }
                    else
                    {
                        if (sb.ToString().StartsWith("gl_") == false)
                        {
                            uniform_locations.Add(sb.ToString().Substring(0, len), i);
                            uniform_types[i] = type;
                        }
                    }
                }

                GL.UseProgramObject(shader);
            }

            return param == 1;
        }

        public ShaderUniform GetUniform(string name)
        {
            Debug.Assert(Device.Current.CurrentShader == this);

            int location = uniform_locations.ContainsKey(name) ? uniform_locations[name] : -1;
            UniformType type = location != -1 ? uniform_types[location] : UniformType.Bool;
            return new ShaderUniform(location, type);
        }

        public ShaderSampler GetSampler(string name)
        {
            Debug.Assert(Device.Current.CurrentShader == this);

            int location = sampler_locations.ContainsKey(name) ? sampler_locations[name] : -1;
            TextureUnit unit = location != -1 ? sampler_units[location] : TextureUnit.Texture0;
            UniformType type = location != -1 ? sampler_types[location] : UniformType.Sampler2D;
            return new ShaderSampler(location, unit, type);
        }

        internal void Bind()
        {
            if (Device.Current.CurrentShader == this)
            {
                return;
            }
            
            Device.Current.CurrentShader = this;

            GL.UseProgramObject(shader);
        }

        public string ErrorLog { get; private set; }

        uint shader;
        internal VertexFormat VertexFormat;

        Dictionary<string, int> uniform_locations;
        Dictionary<string, int> sampler_locations;
        UniformType[] uniform_types;
        UniformType[] sampler_types;
        TextureUnit[] sampler_units;
    }

}
