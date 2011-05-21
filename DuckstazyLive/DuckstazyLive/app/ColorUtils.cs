using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using asap.util;

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

        public static int lerpColor(int fromColor, int toColor, float progress)
        {
            float q = 1 - progress;
            int fromA = (fromColor >> 24) & 0xFF;
            int fromR = (fromColor >> 16) & 0xFF;
            int fromG = (fromColor >> 8) & 0xFF;
            int fromB = fromColor & 0xFF;

            int toA = (toColor >> 24) & 0xFF;
            int toR = (toColor >> 16) & 0xFF;
            int toG = (toColor >> 8) & 0xFF;
            int toB = toColor & 0xFF;

            int resultA = (int)(fromA * q + toA * progress);
            int resultR = (int)(fromR * q + toR * progress);
            int resultG = (int)(fromG * q + toG * progress);
            int resultB = (int)(fromB * q + toB * progress);
            int resultColor = resultA << 24 | resultR << 16 | resultG << 8 | resultB;

            return resultColor;
        }

        public static void ARGB2ColorTransform(uint argb, ref ColorTransform ct)
        {
            uint a = (argb >> 24) & 0xFF;
            uint r = (argb >> 16) & 0xFF;
            uint g = (argb >> 8) & 0xFF;
            uint b = argb & 0xFF;

            ct.MulR= r * 0.0039216f;
            ct.MulG = g * 0.0039216f;
            ct.MulB = b * 0.0039216f;
            ct.MulA = a * 0.0039216f;
        }
    }
}
