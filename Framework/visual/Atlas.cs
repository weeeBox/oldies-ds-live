using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Framework.visual
{
    public class Atlas
    {
        private Texture2D texture;
        private AtlasImage[] images;

        public Atlas(Texture2D texture, int imagesCount)
        {
            this.texture = texture;
            images = new AtlasImage[imagesCount];
        }

        public void setQuad(int imageIndex, int x, int y, int w, int h, int ox, int oy)
        {
            AtlasImage image = new AtlasImage(this, x, y, w, h, ox, oy);
            images[imageIndex] = image;
        }

        public AtlasImage[] Images
        {
            get { return images; }
        }

        public Texture2D getTexture()
        {
            return texture;
        }
    }
}
