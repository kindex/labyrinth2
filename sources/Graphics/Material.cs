using System;
using System.Collections.Generic;
using System.Text;
using Game.Graphics.Renderer.OpenGL;

namespace Game.Graphics
{
    public class Material : IDisposable
    {
        public Texture2D Texture { get; private set; }
        public Texture2D TextureNMap { get; private set; }

        public string Name { get; private set; }

        public Material(string TextureFile)
        {
            Name = TextureFile;
            Texture = Loaders.LoadTexture2D_RGBA(TextureFile, true);
            Texture.SetWrap(TextureWrap.Repeat, TextureWrap.Repeat);
            Texture.SetFilterAnisotropy(4.0f);

            TextureNMap = Loaders.LoadTexture2D_RGBA(TextureFile + ".normal", true);
            TextureNMap.SetWrap(TextureWrap.Repeat, TextureWrap.Repeat);
            TextureNMap.SetFilterAnisotropy(4.0f);
        }

        public void Dispose()
        {
            Texture.Dispose();
            TextureNMap.Dispose();
        }
    }
}
