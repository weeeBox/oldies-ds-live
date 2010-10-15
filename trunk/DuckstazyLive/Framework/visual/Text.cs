using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.core;

namespace Framework.visual
{
    public class Text : BaseElement
    {
        const int DEFAULT_SPACE_WIDTH = 10;

        protected struct FormattedString
        {
            public String text;
            public float width;

            public FormattedString(String text, float width)
            {
                this.text = text;
                this.width = width;
            }
        }

        public float align;
        public String text;
        public Font font;
        public int wrapWidth;

        protected DynamicArray<FormattedString> formattedStrings;

        public Text(Font font)
        {
            this.font = font;
            width = Constants.UNDEFINED;
            height = Constants.UNDEFINED;
            formattedStrings = new DynamicArray<FormattedString>();
        }

        public void setString(String newString)
        {
            setString(newString, Int32.MaxValue);
        }

        public virtual void setString(String newString, int w)
        {
            text = newString;
            wrapWidth = w;

            formatText();

            width = wrapWidth;
            height = font.fontHeight() + font.lineOffset * formattedStrings.count();
        }

        public String getString()
        {
            return text;
        }

        public void setAlignment(float a)
        {            
            align = a;
        }

        public override void draw()
        {
            preDraw();

            float dx = drawX;
            float dy = drawY;

            int itemHeight = font.fontHeight();

            for (int i = 0; i < formattedStrings.count(); i++)
            {
                FormattedString str = formattedStrings[i];
                int len = str.text.Length;
                String s = str.text;             

                //if (align != HAlign.ALIGN_LEFT)
                //{
                //    if (align == HAlign.ALIGN_CENTER)
                //    {
                //        dx = drawX + (wrapWidth - str.width) / 2;
                //    }
                //    else
                //    {                        
                //        dx = drawX + wrapWidth - str.width;
                //    }
                //}
                //else
                //{
                //    dx = drawX;
                //}
                if (true) throw new NotImplementedException();

                for (int c = 0; c < len; c++)
                {
                    int quadIndex = font.getCharQuad(s[c]);
                    int itemWidth = font.quads[quadIndex].Width;
                   
                    font.drawQuad(quadIndex, dx, dy);
                    dx += itemWidth + font.charOffset;
                }
                dy += itemHeight + font.lineOffset;
            }

            postDraw();
        }

        public void formatText()
        {
            const int MAX_STRING_INDEXES = 512;
            short[] strIdx = new short[MAX_STRING_INDEXES];
            String s = text;

            int len = s.Length;

            int idx = 0;
            int xc = 0;
            int wc = 0;
            int xp = 0;
            int xpe = 0;
            int wp = 0;
            int dx = 0;

            int spaceIndex = font.getCharQuad(' ');
            int spaceWidth = DEFAULT_SPACE_WIDTH;
            if (spaceIndex != Constants.UNDEFINED)
            {
                spaceWidth = font.quads[spaceIndex].Width;
            }

            while (dx < len)
            {
                char c = s[dx++];

                int quadIndex = font.getCharQuad(c);
                int charWidth = font.quads[quadIndex].Width;

                if (c == ' ' || c == '\n')
                {
                    wp += wc;
                    xpe = dx - 1;
                    wc = 0;
                    xc = dx;

                    if (c == ' ')
                    {
                        xc--;
                        wc = charWidth + font.charOffset;
                    }
                }
                else
                {
                    wc += charWidth + font.charOffset;
                }

                if ((wp + wc) > wrapWidth && xpe != xp || c == '\n')
                {
                    strIdx[idx++] = (short)xp;
                    strIdx[idx++] = (short)xpe;
                    while (xc < len && s[xc] == ' ')
                    {
                        xc++;
                        wc -= spaceWidth;
                    }

                    xp = xc;
                    xpe = xp;
                    wp = 0;
                }
            }

            if (wc != 0)
            {
                strIdx[idx++] = (short)xp;
                strIdx[idx++] = (short)dx;
            }

            int strCount = idx >> 1;

            formattedStrings.removeAllObjects();

            for (int i = 0; i < strCount; i++)
            {
                int start = strIdx[i << 1];
                int end = strIdx[(i << 1) + 1];
               
                String str = text.Substring(start, end - start);
                int wd = font.stringWidth(str);
                FormattedString fs = new FormattedString(str, wd);
                formattedStrings[i] = fs;
            }
        }

    }
}
