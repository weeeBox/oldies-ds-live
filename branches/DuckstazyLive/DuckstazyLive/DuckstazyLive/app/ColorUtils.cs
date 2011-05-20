using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DuckstazyLive.app
{
    public class ColorUtils
    {
        public static Color MakeColor(int color)
        {
            return MakeColor(color, false);
        }

        public static Color MakeColor(int color, bool hasAlpha)
        {
            byte a = hasAlpha ? (byte)((color >> 24) & 0xff) : (byte)255;
            byte r = (byte)((color >> 16) & 0xff);
            byte g = (byte)((color >> 8) & 0xff);
            byte b = (byte)((color >> 0) & 0xff);
            return new Color(r, g, b, a);
        }
    }
}
