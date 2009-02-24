using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Game.Graphics.Renderer.OpenGL
{
    public static class VF
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct Position2
        {
            public Vector2 Position;
        }
        
        [StructLayout(LayoutKind.Sequential)]
        public struct Position3
        {
            public Vector3 Position;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct PositionColor
        {
            public Vector3 Position;
            public Vector3 Color;
        }
        
        [StructLayout(LayoutKind.Sequential)]
        public struct PositionNormal
        {
            public Vector3 Position;
            public Vector3 Normal;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Position2Texcoord
        {
            public Vector2 Position;
            public Vector2 Texcoord;
        }
        
        [StructLayout(LayoutKind.Sequential)]
        public struct PositionTexcoord3
        {
            public Vector3 Position;
            public Vector3 Texcoord;
        }
        
        [StructLayout(LayoutKind.Sequential)]
        public struct PositionNormalTexcoord
        {
            public Vector3 Position;
            public Vector3 Normal;
            public Vector2 Texcoord;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct PositionTexcoordNT
        {
            public Vector3 Position;
            public Vector2 Texcoord;
            public Vector3 Normal;
            public Vector4 Tangent;
        }

        public static VertexFormat GetVertexFormat<T>() where T : struct
        {
            Type type = typeof(T);

            if (Cache.ContainsKey(type))
            {
                return Cache[type];
            }

            Debug.Assert(type.IsLayoutSequential);

            List<VertexAttribute> attributes = new List<VertexAttribute>();

            foreach (MemberInfo member in type.GetMembers())
            {
                if (member.MemberType == MemberTypes.Method || member.MemberType == MemberTypes.Constructor)
                {
                    continue;
                }

                Debug.Assert(member.MemberType == MemberTypes.Field);

                FieldInfo field = member as FieldInfo;
                
                Debug.Assert(Names.ContainsKey(field.Name) && FloatTypes.ContainsKey(field.FieldType));

                attributes.Add(new VertexAttribute(Names[field.Name], FloatTypes[field.FieldType], VertexAttributePointerType.Float));
            }

            var result = new VertexFormat(attributes.ToArray());
            Cache.Add(type, result);
            return result;
        }

        static Dictionary<Type, VertexFormat> Cache = new Dictionary<Type, VertexFormat>();

        static Dictionary<string, string> Names = new Dictionary<string, string>()
        {
            {"Position", "vertex_position"},
            {"Normal", "vertex_normal"},
            {"Color", "vertex_color"},
            {"Texcoord", "vertex_texcoord"},
            {"Tangent", "vertex_tangent"},
            {"Binormal", "vertex_binormal"},
        };

        static Dictionary<Type, int> FloatTypes = new Dictionary<Type, int>()
        {
            {typeof(float), 1},
            {typeof(Vector2), 2},
            {typeof(Vector3), 3},
            {typeof(Vector4), 4},
        };
    }
}
