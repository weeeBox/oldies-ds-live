using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.core;

namespace Framework.visual
{
    public enum TextAlign
    {
        LEFT,
        CENTER,
        RIGHT
    }

    public class Text : BaseElement
    {
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

        public String text;
        public Font font;        

        protected FormattedString[] formattedStrings;
        protected TextAlign textAlign = TextAlign.LEFT;

        public Text(Font font)
        {
            this.font = font;
            width = FrameworkConstants.UNDEFINED;
            height = FrameworkConstants.UNDEFINED;            
        }

        public void setString(String newString)
        {
            setString(newString, Int32.MaxValue);
        }

        public virtual void setString(String newString, int wrapWidth)
        {
            text = newString;            

            String[] strings = font.wrapString(text, wrapWidth);
            int stringsCount = strings.Length;
            formattedStrings = new FormattedString[stringsCount];            
            for (int i = 0; i < stringsCount; ++i)
            {
                String str = strings[i];
                int strWidth = font.stringWidth(str);
                if (strWidth > width)
                    width = strWidth;
                formattedStrings[i] = new FormattedString(str, strWidth);
            }            
            height = (font.fontHeight() + font.lineOffset) * formattedStrings.Length - font.lineOffset;
        }

        public String getString()
        {
            return text;
        }

        public void setAlign(TextAlign a)
        {            
            textAlign = a;
        }

        public override void draw()
        {
            preDraw();

            float dx;
            float dy = drawY;
            int itemHeight = font.fontHeight();
            for (int i = 0; i < formattedStrings.Length; i++)
            {
                FormattedString str = formattedStrings[i];
                int len = str.text.Length;
                String s = str.text;

                if (textAlign != TextAlign.LEFT)
                {
                    if (textAlign == TextAlign.CENTER)
                    {
                        dx = drawX + (width - str.width) / 2;
                    }
                    else
                    {
                        dx = drawX + width - str.width;
                    }
                }
                else
                {
                    dx = drawX;
                }                             

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
    }
}
