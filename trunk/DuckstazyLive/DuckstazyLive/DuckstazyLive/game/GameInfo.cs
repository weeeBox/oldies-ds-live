using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using Framework.visual;
using Framework.core;
using DuckstazyLive.app;

namespace DuckstazyLive.game
{
    public class GameInfo
    {
        private const int ftSize = 50;
        private FloatText[] ftPool;
        private int ftCount;

        public string one;
        public string[] powers;
        public string[] toxics;
        public string[] sleeps;
        public string[] damages;

        public float r;
        public float g;
        public float b;

        //public var state:GameState;

        public GameInfo()
        {
            int i = 0;
            one = "+1";

            ftPool = new FloatText[ftSize];
            for (; i < ftSize; ++i)
                ftPool[i] = new FloatText();

            ftCount = 0;

            powers = new String[] { "+1", "+5", "+10", "+25", "+50", "+100", "+150" };
            toxics = new String[] 
            { 
                "FIRST BLOOD! +100", 
                "MANIACALISTIC! +150",
                "SUPER RESISTANCE! +200",
            };
            sleeps = new String[] 
            { 
                "WAKE UP!",
                "LULLABY...",
                "FALLING ASLEEP..",

            };
            damages = new String[] 
            { 
                "OOPS!", 
                "REALLY HARD...", 
                "BE CAREFUL!",
            };
        }

        public void reset()
        {
            foreach (FloatText it in ftPool)
            {
                it.t = 0.0f;
                it.text = null;
            }
            ftCount = 0;
        }

        public void drawFT(Canvas canvas)
        {
            int i = 0;
            Font font = Application.sharedResourceMgr.getFont(Res.FNT_FLOAT);

            foreach (FloatText ft in ftPool)
            {
                if (i == ftCount)
                    break;

                if (ft.t > 0.0f)
                {
                    ft.draw();
                    ++i;
                }
            }
        }

        public void add(float x, float y, String text)
        {
            foreach (FloatText ft in ftPool)
            {
                if (ft.t <= 0.0f)
                {
                    ft.t = 1.0f;
                    ft.x = x;
                    ft.y = y;
                    ft.text = text;

                    ++ftCount;

                    break;
                }
            }
        }

        public void setRGB(uint color)
        {
            r = (uint)(((color >> 16) & 0xFF) * 0.003921569f);
            g = (uint)(((color >> 8) & 0xFF) * 0.003921569f);
            b = (uint)((color & 0xFF) * 0.003921569f);
        }

        private void ctCalc(ref ColorTransform color, float t)
        {
            float x = (int)(0.5f * (1.0f + Math.Sin(t * 6.28f * 4.0f)));
            color.redMultiplier = r * x;
            color.greenMultiplier = g * x;
            color.blueMultiplier = b * x;
        }

        public void update(float power, float dt)
        {
            int i = 0;
            int ft_proc = ftCount;
            float a;

            foreach (FloatText ft in ftPool)
            {
                if (i == ft_proc)
                    break;

                if (ft.t > 0.0f)
                {
                    ft.t -= dt;

                    if (ft.t <= 0.0f)
                        --ftCount;
                    else
                    {
                        ft.y -= 50.0f * dt;
                        a = 0.25f;
                        if (ft.t > 0.75) a = 1.0f - ft.t;
                        else if (ft.t < 0.25f) a = ft.t;
                        a *= 4.0f;
                        ctCalc(ref ft.ct, ft.t);
                        ft.ct.alphaMultiplier = a;
                        utils.colorTransformToColor(ref ft.color, ref ft.ct);
                    }

                    ++i;
                }
            }
        }
    }
}
