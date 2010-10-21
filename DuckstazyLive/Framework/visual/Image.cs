using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Framework.core;
using Microsoft.Xna.Framework;

namespace Framework.visual
{
    public class Image : BaseElement
    {
        public Texture2D texture;
        public Rectangle[] quads;
        public int quadToDraw = FrameworkConstants.UNDEFINED;

        public void setDrawQuad(int n)
        {
            quadToDraw = n;
        }

        public void setDrawFullImage()
        {
            quadToDraw = FrameworkConstants.UNDEFINED;
        }

        public Image(Texture2D texture)
        {
            this.texture = texture;
            this.width = texture.Width;
            this.height = texture.Height;
        }

        public Image(Texture2D texture, int capacity)
        {
            this.texture = texture;
            quads = new Rectangle[capacity];

            this.width = texture.Width;
            this.height = texture.Height;
        }

        public Image(Texture2D texture, int rows, int columns)
        {
            this.texture = texture;
            quads = new Rectangle[rows * columns];
            int pos = 0;
            this.width = texture.Width / columns;
            this.height = texture.Height / rows;
            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    quads[pos++] = new Rectangle(x * texture.Width / columns, y * texture.Height / rows, texture.Width / columns, texture.Height / rows);
                }
            }

            setDrawQuad(0);
        }

        public Image(Texture2D texture, int qw, int qh, bool unused)
            : this(texture, qw, qh, texture.Width / qw, (texture.Width / qw) * (texture.Height / qh))
        {
        }

        public Image(Texture2D texture, int qw, int qh, int c, int qc)
        {
            this.texture = texture;
            this.width = qw;
            this.height = qh;
            quads = new Rectangle[qc];

            int columns = c;
            int rows = 1 + (qc - 1) / columns;

            int quadNo = 0;
            for (int y = 0; y < rows && quadNo < qc; y++)
            {
                for (int x = 0; x < columns && quadNo < qc; x++)
                {
                    quads[quadNo++] = new Rectangle(x * texture.Width / columns, y * texture.Height / rows, texture.Width / columns, texture.Height / rows);
                }
            }

            setDrawQuad(0);
        }

        public void setQuad(Rectangle rect, int pos)
        {
            quads[pos] = rect;
        }

        public void drawQuad(int n, int x, int y)
        {
            AppGraphics.DrawImagePart(texture, quads[n], x, y);
        }

        public void drawQuad(int n, float x, float y)
        {
            AppGraphics.DrawImagePart(texture, quads[n], x, y);
        }        

        public void drawQuad(int n)
        {
            drawQuad(n, drawX, drawY);
        }

        public override void draw()
        {
            preDraw();
            if (quadToDraw == FrameworkConstants.UNDEFINED)
            {
                AppGraphics.DrawImage(texture, drawX, drawY);
            }
            else
            {
                drawQuad(quadToDraw, drawX, drawY);
            }
            postDraw();
        }

    }
}
