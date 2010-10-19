using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;
using Framework.core;
using DuckstazyLive.app;

namespace DuckstazyLive.game.levels
{
    public class Level1 : PillsManager
    {
        private const int LINES_COUNT = 3;
        private const int PILLS_IN_LINE_COUNT = 15;

        private Pill[,] pills = new Pill[LINES_COUNT, PILLS_IN_LINE_COUNT];
        private bool[,] pillsActive = new bool[LINES_COUNT, PILLS_IN_LINE_COUNT];
        private float elapsedTime;

        public Level1(Hero hero) : base(hero)
        {            
        }

        public override void init()
        {
            float stepX = Bounds.Width / (PILLS_IN_LINE_COUNT + 2.0f);
            for (int lineIndex = 0; lineIndex < LINES_COUNT; ++lineIndex)
            {
                float y = 170 + lineIndex * 150;
                for (int pillIndex = 0; pillIndex < PILLS_IN_LINE_COUNT; ++pillIndex)
                {
                    float x = stepX * (pillIndex + 1);
                    pills[lineIndex, pillIndex] = PillsManager.Pool.allocatePill(x, y);
                    pillsActive[lineIndex, pillIndex] = true;
                }
            }
        }

        protected override void updatePills(float dt)
        {
            elapsedTime += dt;
            for (int lineIndex = 0; lineIndex < LINES_COUNT; ++lineIndex)
            {
                for (int pillIndex = 0; pillIndex < PILLS_IN_LINE_COUNT; ++pillIndex)
                {
                    if (!pillsActive[lineIndex, pillIndex])
                        continue;

                    Pill pill = pills[lineIndex, pillIndex];
                    if (collides(Hero.x, Hero.y, Hero.width, Hero.height, pill.x, pill.y, Constants.PILL_RADIUS))
                        pillsActive[lineIndex, pillIndex] = false;
                }
            }            
        }

        protected override void drawPills()
        {
            Texture2D tex = Application.sharedResourceMgr.getTexture(Res.IMG_PILL_FAKE);
            float halfWidth = 0.5f * tex.Width;
            float halfHeight = 0.5f * tex.Height;
            for (int lineIndex = 0; lineIndex < LINES_COUNT; ++lineIndex)
            {                
                for (int pillIndex = 0; pillIndex < PILLS_IN_LINE_COUNT; ++pillIndex)
                {
                    if (!pillsActive[lineIndex, pillIndex])
                        continue;

                    Pill pill = pills[lineIndex, pillIndex];
                    AppGraphics.DrawImage(tex, pill.x - halfWidth, pill.y - halfHeight);
                }
            }
        }
    }
}
