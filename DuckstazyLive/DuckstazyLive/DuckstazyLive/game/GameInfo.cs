using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using Framework.visual;
using Framework.core;
using DuckstazyLive.app;
using Microsoft.Xna.Framework;

namespace DuckstazyLive.game
{
    public class GameInfo
    {
        private const int ftSize = 50;
        private FloatText[] ftPool;
        private int ftCount;

        private static Color[] PLAYERS_COLORS = 
        {
            utils.makeColor(0xfff799),
            utils.makeColor(0xf49ac1),
        };
        private static Color BAD_COLOR = utils.makeColor(0xed1c24);

        public GameInfo()
        {
            int i = 0;
            ftPool = new FloatText[ftSize];
            for (; i < ftSize; ++i)
                ftPool[i] = new FloatText();

            ftCount = 0;            
        }

        public void reset()
        {
            foreach (FloatText it in ftPool)
            {
                it.reset();
            }
            ftCount = 0;
        }

        public void draw(Canvas canvas)
        {
            int i = 0;
            Font font = Application.sharedResourceMgr.getFont(Res.FNT_PICKUP);
            Env env = GameElements.Env;
            
            foreach (FloatText ft in ftPool)
            {
                if (i == ftCount)
                    break;

                if (ft.isAlive())
                {
                    ft.draw(canvas);
                    ++i;
                }
            }
        }

        public void add(float x, float y, int score, int playerIndex)
        {
            Debug.Assert(score != 0);
            string str;
            Color color;
            if (score > 0)
            {
                str = "+" + score;
                color = PLAYERS_COLORS[playerIndex];
            }
            else
            {
                str = "-" + score;
                color = BAD_COLOR;
            }
            add(x, y, str, ref color);
        }

        public void add(float x, float y, String text, ref Color color)
        {
            foreach (FloatText ft in ftPool)
            {
                if (!ft.isAlive())
                {                    
                    ft.start(text, x, y, ref color);
                    ++ftCount;
                    break;
                }
            }
        }

        public void update(float power, float dt)
        {
            int i = 0;
            int ft_proc = ftCount;
            foreach (FloatText ft in ftPool)
            {
                if (i == ft_proc)
                    break;

                if (ft.isAlive())
                {
                    ft.update(dt);

                    if (!ft.isAlive())
                    {
                        --ftCount;
                    }
                    ++i;
                }
            }
        }
    }
}
