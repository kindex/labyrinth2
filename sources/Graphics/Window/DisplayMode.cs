using System;

namespace Game.Graphics.Window
{
    public struct DisplayMode : IEquatable<DisplayMode>
    {
        public int Width;
        public int Height;

        public bool Equals(DisplayMode other)
        {
            return Width == other.Width && Height == other.Height;
        }

        public override bool Equals(object obj)
        {
            if (obj is DisplayMode)
            {
                return Equals((DisplayMode)obj);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return (Width << 16) + Height;
        }

        public override string ToString()
        {
            return String.Format("{0}x{1}", Width, Height);
        }
    }
}
