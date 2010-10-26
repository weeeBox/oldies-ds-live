using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.core;
using Microsoft.Xna.Framework.Graphics;
using Framework.utils;
using Microsoft.Xna.Framework;

namespace DuckstazyLive.game
{
    public class Canvas
    {
        public void draw(int imageId)
        {
            draw(imageId, DrawMatrix.IDENTITY);
        }

        public void draw(int imageId, DrawMatrix mat)
        {
            draw(imageId, mat, null);
        }

        public void draw(int imageId, DrawMatrix mat, ColorTransform transform)
        {            
            Color color = Color.White;
            if (transform != null)
            {
                color.R = (byte)(color.R * transform.redMultiplier);
                color.G = (byte)(color.G * transform.greenMultiplier);
                color.B = (byte)(color.B * transform.blueMultiplier);
                color.A = (byte)(color.A * transform.alphaMultiplier);
            }
            AppGraphics.DrawImage(getTexture(imageId), ref mat.POSITION, ref color, mat.ROTATION, ref mat.ORIGIN, ref mat.SCALE);
        }        

        public void copyPixels(int imageId, Rect dest, Vector2 pos)
        {
            AppGraphics.DrawImage(getTexture(imageId), pos.X, pos.Y);
        }

        private Texture2D getTexture(int imageId)
        {
            return Application.sharedResourceMgr.getTexture(imageId);
        }
    }
}
