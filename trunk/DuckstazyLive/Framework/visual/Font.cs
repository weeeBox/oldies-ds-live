using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.visual
{
    public interface Font
    {
        string[] wrapString(string text, int wrapWidth);
        int stringWidth(string str);
        int fontHeight();
        int LineOffset { get; }

        void drawString(string text, float x, float y);
        void drawString(string text, float x, float y, TextAlign textAlign);
    }
}
