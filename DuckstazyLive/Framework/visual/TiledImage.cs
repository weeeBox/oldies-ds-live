using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.core;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Framework.visual
{
    public class TiledImage : BaseElement
    {
        private Rectangle src;
        private Rectangle dest;
        private Texture2D texture;

        public TiledImage(Texture2D texture, int width, int height) : this(texture, new Rectangle(0, 0, texture.Width, texture.Height), width, height)
        {            
        }

        public TiledImage(Texture2D texture, Rectangle src, int width, int height)
        {
            this.src = src;            
            this.texture = texture;
            this.width = width;
            this.height = height;
            dest.Width = width;
            dest.Height = height;
        }

        public override void draw()
        {
            preDraw();
            dest.X = (int)drawX;
            dest.Y = (int)drawY;
            AppGraphics.DrawImageTiled(texture, ref src, ref dest);
            postDraw();
        }
    }
}
