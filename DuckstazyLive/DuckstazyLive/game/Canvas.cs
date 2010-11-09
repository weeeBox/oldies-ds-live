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
            Vector2 scaledPosition;
            Vector2.Multiply(ref mat.POSITION, Constants.SCALE, out scaledPosition);
            Vector2 scaledOrigin;
            Vector2.Multiply(ref mat.ORIGIN, Constants.SCALE, out scaledOrigin);
            AppGraphics.DrawImage(getTexture(imageId), ref scaledPosition, ref color, mat.ROTATION, ref scaledOrigin, ref mat.SCALE, ref mat.FLIP);
        }        

        public void copyPixels(int imageId, Rect dest, Vector2 pos)
        {
            AppGraphics.DrawImage(getTexture(imageId), utils.scale(pos.X), utils.scale(pos.Y));
        }

        public void draw(int fontId, String text, DrawMatrix mat)
        {
            Font fnt = Application.sharedResourceMgr.getFont(fontId);
            float x = utils.scale(mat.POSITION.X + mat.ORIGIN.X);
            float y = utils.scale(mat.POSITION.Y + mat.ORIGIN.Y);

            bool changeScale = mat.SCALE.X != 1.0f || mat.SCALE.Y != 1.0f;
            bool changeRotation = mat.ROTATION != 0.0f;
            if (changeScale || changeRotation)
            {
                AppGraphics.PushMatrix();
                AppGraphics.Translate(x, y, 0.0f);

                if (changeRotation)
                    AppGraphics.Rotate(mat.ROTATION, 0.0f, 0.0f, 1.0f);
                if (changeScale)
                    AppGraphics.Scale(mat.SCALE.X, mat.SCALE.Y, 0.0f);

                fnt.draw(text, utils.scale(mat.ORIGIN.X / mat.SCALE.X), utils.scale(mat.ORIGIN.Y / mat.SCALE.Y));
                AppGraphics.PopMatrix();
            }
            else
            {
                fnt.draw(text, x, y);
            }
        }

        private Texture2D getTexture(int imageId)
        {
            return Application.sharedResourceMgr.getTexture(imageId);
        }
    }
}
