using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.core;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Framework.visual
{
    public class VectorFont : BaseElement, Font
    {
        private SpriteFont fnt;
        private int fntHeight;        

        public VectorFont(SpriteFont fnt)
        {
            this.fnt = fnt;
            fntHeight = (int)fnt.MeasureString("0").Y;            
        }

        public string[] wrapString(string text, int wrapWidth)
        {
            throw new NotImplementedException();
        }

        public int stringWidth(string str)
        {
            return (int)(fnt.MeasureString(str).X);
        }

        public int fontHeight()
        {
            return fntHeight;
        }

        public int LineOffset
        {
            get { return fnt.LineSpacing; }
        }

        public void drawString(string text, float x, float y)
        {
            drawString(text, x, y, TextAlign.LEFT | TextAlign.TOP);
        }

        public void drawString(string text, float x, float y, TextAlign textAlign)
        {
            Debug.Assert(text != null);

            float dx = x;
            float dy = y;

            if ((textAlign & TextAlign.RIGHT) != 0)
            {
                dx -= stringWidth(text);
            }
            else if ((textAlign & TextAlign.HCENTER) != 0)
            {
                dx -= 0.5f * stringWidth(text);
            }
            if ((textAlign & TextAlign.BOTTOM) != 0)
            {
                dy -= fontHeight();
            }
            else if ((textAlign & TextAlign.VCENTER) != 0)
            {
                dy -= 0.5f * fontHeight();
            }

            AppGraphics.DrawString(fnt, dx, dy, text);
        }
    }
}
