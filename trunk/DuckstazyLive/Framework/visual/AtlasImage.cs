using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Framework.core;

namespace Framework.visual
{
    public class AtlasImage : SpriteTexture
    {
        private static Vector2 VECTOR_ZERO = Vector2.Zero;
        private static Vector2 VECTOR_ONE = new Vector2(1.0f, 1.0f);
        private static Color COLOR_WHITE = Color.White;

        private Atlas atlas;
        private Rectangle quad;
        private int ox;
        private int oy;

        public AtlasImage(Atlas atlas, int x, int y, int w, int h, int offX, int offY)
        {            
            quad = new Rectangle(x, y, w, h);
            this.atlas = atlas;
            this.ox = offX;
            this.oy = offY;
        }        

        public int Width
        {
            get { return quad.Width; }
        }

        public int Height
        {
            get { return quad.Height; }
        }

        public void draw(float x, float y) 
        {
            Texture2D texture = atlas.getTexture();
            draw(x, y, ref COLOR_WHITE);
        }

        public void draw(float x, float y, float opacity) 
        {
            Color color = Color.White * opacity;
            draw(x, y, ref color);
        }

        public void draw(float x, float y, ref Color color) 
        {
            Texture2D texture = atlas.getTexture();
            Vector2 position = new Vector2(x, y);
            AppGraphics.DrawImage(texture, ref quad, ref position, ref color, 0.0f, ref VECTOR_ZERO, ref VECTOR_ONE, ref VECTOR_ZERO);
        }

        public void draw(ref Vector2 position, ref Color color, float rotation, ref Vector2 origin, ref Vector2 scale, ref Vector2 flip) 
        {
            Texture2D texture = atlas.getTexture();
            AppGraphics.DrawImage(texture, ref quad, ref position, ref color, rotation, ref origin, ref scale, ref flip);
        }

        public void drawPart(ref Rectangle rectangle, float x, float y) 
        {
            Texture2D texture = atlas.getTexture();
            Vector2 position = new Vector2(x, y);
            Rectangle partQuad = rectangle;
            partQuad.X += quad.X;
            partQuad.Y += quad.Y;
            AppGraphics.DrawImage(texture, ref partQuad, ref position, ref COLOR_WHITE, 0.0f, ref VECTOR_ZERO, ref VECTOR_ONE, ref VECTOR_ZERO);
        }

        public void drawTiled(ref Rectangle src, ref Rectangle dst) 
        {
            Texture2D texture = atlas.getTexture();
            Rectangle partSrc = src;
            partSrc.X += quad.X;
            partSrc.Y += quad.Y;
            AppGraphics.DrawImageTiled(texture, ref partSrc, ref dst);
        }
    }
}
