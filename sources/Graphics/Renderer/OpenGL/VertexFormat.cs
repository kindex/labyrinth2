using System;
using System.Runtime.InteropServices;

namespace Game.Graphics.Renderer.OpenGL
{
    public struct VertexAttribute
    {
        public VertexAttribute(string name, int count, VertexAttributePointerType type)
        {
            this.name = name;
            this.count = count;
            this.type = type;
            this.offset = 0;
        }

        internal string name;
        internal int count;
        internal VertexAttributePointerType type;
        internal int offset;
    }

    public sealed class VertexFormat
    {
        public VertexFormat(params VertexAttribute[] attributes)
        {
            this.attributes = attributes;

            int offset = 0;
            for (int i = 0; i < attributes.Length; i++)
            {
                int elementSize = 0;
                switch (attributes[i].type)
                {
                    case VertexAttributePointerType.Byte:
                    case VertexAttributePointerType.UnsignedByte:
                        elementSize = 1;
                        break;

                    case VertexAttributePointerType.Short:
                    case VertexAttributePointerType.UnsignedShort:
                        elementSize = 2;
                        break;

                    case VertexAttributePointerType.Int:
                    case VertexAttributePointerType.UnsignedInt:
                    case VertexAttributePointerType.Float:
                        elementSize = 4;
                        break;

                    case VertexAttributePointerType.Double:
                        elementSize = 8;
                        break;
                }

                attributes[i].offset = offset;
                offset += attributes[i].count * elementSize;
            }
        }

        internal VertexAttribute[] attributes;
    }
}
