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

        public int getWidth()
        {
            return quad.Width;
        }

        public int getHeight()
        {
            return quad.Height;
        }
    }
}
