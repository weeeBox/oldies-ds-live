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

        public static uint lerpColor(uint fromColor, uint toColor, float progress)
        {
            float q = 1 - progress;
            uint fromA = (fromColor >> 24) & 0xFF;
            uint fromR = (fromColor >> 16) & 0xFF;
            uint fromG = (fromColor >> 8) & 0xFF;
            uint fromB = fromColor & 0xFF;

            uint toA = (toColor >> 24) & 0xFF;
            uint toR = (toColor >> 16) & 0xFF;
            uint toG = (toColor >> 8) & 0xFF;
            uint toB = toColor & 0xFF;

            uint resultA = (uint)(fromA * q + toA * progress);
            uint resultR = (uint)(fromR * q + toR * progress);
            uint resultG = (uint)(fromG * q + toG * progress);
            uint resultB = (uint)(fromB * q + toB * progress);
            uint resultColor = resultA << 24 | resultR << 16 | resultG << 8 | resultB;

            return resultColor;
        }

        public static void ctSetRGB(ref ColorTransform ct, uint rgb)
        {
            ct.MulR = ((rgb >> 16) & 0xFF) / 255.0f;
            ct.MulG = ((rgb >> 8) & 0xFF) / 255.0f;
            ct.MulB = (rgb & 0xFF) / 255.0f;
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
