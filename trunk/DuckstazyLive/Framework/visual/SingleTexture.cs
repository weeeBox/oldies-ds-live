using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Framework.visual
{
    public class SingleTexture : SpriteTexture
    {
        private Texture2D texture;

        public int getWidth()
        {
            return texture.Width;
        }

        public int getHeight()
        {
            return texture.Height;
        }
    }
}
