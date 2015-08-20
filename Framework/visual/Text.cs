using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.core;
using System.Diagnostics;

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

        public Text(Font font)
        {
            Debug.Assert(font != null);

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
            height = (font.fontHeight() + font.LineOffset) * formattedStrings.Length - font.LineOffset;
        }

        public String getString()
        {
            return text;
        }

        public void setAlign(TextAlign textAlign)
        {
            float alignX;            
            if ((textAlign & TextAlign.RIGHT) != 0)
            {
                alignX = ALIGN_MAX;
            }
            else if ((textAlign & TextAlign.HCENTER) != 0)
            {
                alignX = ALIGN_CENTER;
            }                
            else
            {
                alignX = ALIGN_MIN;
            }

            float alignY;
            if ((textAlign & TextAlign.BOTTOM) != 0)
            {
                alignY = ALIGN_MAX;
            }
            else if ((textAlign & TextAlign.VCENTER) != 0)
            {
                alignY = ALIGN_CENTER;
            }
            else
            {
                alignY = ALIGN_MIN;
            }

            setAlign(alignX, alignY);
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
                dx = drawX + alignX * (width - str.width);
                font.drawString(str.text, dx, dy);                
                dy += itemHeight + font.LineOffset;
            }

            postDraw();
        }
    }
}
