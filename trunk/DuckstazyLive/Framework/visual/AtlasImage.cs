using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Framework.visual
{
    public class AtlasImage : SpriteTexture
    {
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

        public void draw(float x, float y) { }
        public void draw(float x, float y, float opacity) { }
        public void draw(float x, float y, ref Color color) { }
        public void draw(ref Vector2 position, ref Color color, float rotation, ref Vector2 origin, ref Vector2 scale, ref Vector2 flip) { }

        public void drawPart(ref Rectangle rectangle, float x, float y) { }
        public void drawTiled(ref Rectangle src, ref Rectangle dst) { }
    }
}
