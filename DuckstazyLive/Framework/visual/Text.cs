using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.core;

namespace Framework.visual
{
    public enum TextAlign
    {
        LEFT = 1 << 0,
        HCENTER = 1 << 1,
        RIGHT = 1 << 2,
        TOP = 1 << 3,
        VCENTER = 1 << 4,
        BOTTOM = 1 << 5
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
        protected TextAlign textAlign = TextAlign.LEFT | TextAlign.TOP;

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

            if ((textAlign & TextAlign.BOTTOM) != 0)
            {
                dy -= height;
            }
            else if ((textAlign & TextAlign.VCENTER) != 0)
            {
                dy -= 0.5f * height;
            }

            for (int i = 0; i < formattedStrings.Length; i++)
            {
                FormattedString str = formattedStrings[i];
                int len = str.text.Length;
                String s = str.text;

                dx = drawX;
                if ((textAlign & TextAlign.RIGHT) != 0)
                {
                    dx -= str.width;
                }
                else if ((textAlign & TextAlign.HCENTER) != 0)
                {
                    dx -= 0.5f * str.width;
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
