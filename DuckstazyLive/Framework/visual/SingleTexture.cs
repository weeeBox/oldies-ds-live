using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Framework.core;

namespace Framework.visual
{
    public class SingleTexture : SpriteTexture
    {
        private Texture2D texture;

        public SingleTexture(Texture2D texture)
        {
            this.texture = texture;
        }

        public int Width
        {
            get { return texture.Width; }
        }

        public int Height
        {
            get { return texture.Height; }
        }

        public void draw(float x, float y) 
        {
            AppGraphics.DrawImage(texture, x, y);
        }

        public void draw(float x, float y, float opacity) 
        {
            AppGraphics.DrawImage(texture, x, y, opacity);
        }

        public void draw(float x, float y, ref Color color) 
        {
            AppGraphics.DrawImage(texture, x, y, color);
        }

        public void draw(ref Vector2 position, ref Color color, float rotation, ref Vector2 origin, ref Vector2 scale, ref Vector2 flip) 
        {
            AppGraphics.DrawImage(texture, ref position, ref color, rotation, ref origin, ref scale, ref flip);
        }

        public void drawPart(ref Rectangle rectangle, float x, float y) 
        {
            AppGraphics.DrawImagePart(texture, rectangle, x, y);
        }

        public void drawTiled(ref Rectangle src, ref Rectangle dst) 
        {
            AppGraphics.DrawImageTiled(texture, ref src, ref dst);
        }
    }
}
