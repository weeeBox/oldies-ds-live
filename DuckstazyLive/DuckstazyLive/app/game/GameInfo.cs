using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using asap.graphics;

namespace DuckstazyLive.app.game
{
    public class GameInfo
    {
        private const int ftSize = 50;
        private FloatText[] ftPool;
        private int ftCount;
        private Color color;
        private Hero hero;
        private float addCounter;
        private int addBuffer;
        private float bufferX, bufferY;

        private static Color[] PLAYERS_COLORS = 
        {
            ColorUtils.MakeColor(0xfff799),
            ColorUtils.MakeColor(0xf49ac1),
        };
        private static Color BAD_COLOR = ColorUtils.MakeColor(0xed1c24);

        public GameInfo(Hero hero)
        {
            this.hero = hero;
            this.color = PLAYERS_COLORS[hero.getPlayerIndex()];

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
            addCounter = 0.0f;
            addBuffer = 0;
            bufferX = bufferY = 0.0f;
        }

        public void draw(Graphics g)
        {
            int i = 0;
            foreach (FloatText ft in ftPool)
            {
                if (i == ftCount)
                    break;

                if (ft.isAlive())
                {
                    ft.draw(g);
                    ++i;
                }
            }
        }

        public void add(int score)
        {
            if (score > 0)
            {
                if (addCounter == 0.0f)
                {
                    addCounter = 0.5f;
                    bufferX = hero.x;
                    bufferY = hero.y;
                }
                addBuffer += score;
            }
            else if (score < 0)
            {
                commitScore(score);
            }

        }

        private void commitScore(int score)
        {
            Debug.Assert(score != 0);
            string str;
            Color drawColor;
            if (score > 0)
            {
                str = "+" + score.ToString();
                drawColor = color;
            }
            else
            {
                str = score.ToString();
                drawColor = BAD_COLOR;
            }
            float addX = hero.flip ? (hero.x + Hero.duck_w2) : hero.x;
            float addY = hero.y;
            add(addX, addY, str, ref drawColor);
        }

        private void add(float x, float y, String text, ref Color color)
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

        public void Update(float power, float dt)
        {
            if (addCounter > 0)
            {
                addCounter -= dt;
                if (addCounter <= 0)
                {
                    addCounter = 0.0f;
                    if (addBuffer != 0)
                    {
                        commitScore(addBuffer);
                        addBuffer = 0;
                    }
                }
            }

            int i = 0;
            int ft_proc = ftCount;
            foreach (FloatText ft in ftPool)
            {
                if (i == ft_proc)
                    break;

                if (ft.isAlive())
                {
                    ft.Update(dt);

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
