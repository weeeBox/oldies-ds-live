using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.core;
using Microsoft.Xna.Framework.Graphics;
using Framework.utils;
using Microsoft.Xna.Framework;
using DuckstazyLive.app;
using Framework.visual;

namespace DuckstazyLive.game
{
    public class Canvas
    {
        private int width;
        private int height;

        public Canvas(int width, int height)
        {
            this.width = width;
            this.height = height;
        }        

        public void draw(int imageId, DrawMatrix mat)
        {
            draw(getTexture(imageId), mat);
        }

        public void draw(SpriteTexture image, DrawMatrix mat)
        {
            draw(image, mat, ColorTransform.NONE);
        }

        public void draw(int imageId, DrawMatrix mat, ColorTransform transform)
        {
            draw(getTexture(imageId), mat, transform);
        }

        public void draw(SpriteTexture image, DrawMatrix mat, ColorTransform transform)
        {
            AppBlendMode blendMode = AppGraphics.GetBlendMode();

            Color color = Color.White;            
            if (!transform.Equals(ColorTransform.NONE))
            {
                utils.colorTransformToColor(ref color, ref transform);
                AppGraphics.SetBlendMode(transform.blendMode);
            }

            Vector2 origin = mat.ORIGIN;
            origin.X += mat.ALIGN.X * image.Width;
            origin.Y += mat.ALIGN.Y * image.Height;            

            if (mat.UseScale)
            {
                Vector2 scaledPosition;
                Vector2.Multiply(ref mat.POSITION, Constants.SCALE, out scaledPosition);
                Vector2 scaledOrigin;
                Vector2.Multiply(ref origin, Constants.SCALE, out scaledOrigin);
                image.draw(ref scaledPosition, ref color, mat.ROTATION, ref scaledOrigin, ref mat.SCALE, ref mat.FLIP);
            }
            else
            {
                image.draw(ref mat.POSITION, ref color, mat.ROTATION, ref origin, ref mat.SCALE, ref mat.FLIP);
            }

            AppGraphics.SetBlendMode(blendMode);
        }        

        public void copyPixels(int imageId, Rect dest, Vector2 pos)
        {
            getTexture(imageId).draw(utils.scale(pos.X), utils.scale(pos.Y));
        }

        public void copyPixels(int imageId, Rect dest, Vector2 pos, ColorTransform transform)
        {
            Color color = Color.White;
            AppBlendMode blendMode = AppGraphics.GetBlendMode();
            if (!transform.Equals(ColorTransform.NONE))
            {
                utils.colorTransformToColor(ref color, ref transform);
                AppGraphics.SetBlendMode(transform.blendMode);
            }
            getTexture(imageId).draw(utils.scale(pos.X), utils.scale(pos.Y), ref color);
            AppGraphics.SetBlendMode(blendMode);
        }       

        public void drawGeometry(CustomGeomerty geom)
        {
            drawGeometry(geom, null);
        }

        public void drawGeometry(CustomGeomerty geom, DrawMatrix m)
        {
            AppGraphics.DrawGeomerty(geom);
        }        

        private SpriteTexture getTexture(int imageId)
        {
            return Application.sharedResourceMgr.getTexture(imageId);
        }        
    }
}
