using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.core;
using Microsoft.Xna.Framework.Graphics;
using DuckstazyLive.app;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace DuckstazyLive.game
{
    public class PillsManager : BaseElement
    {
        private static int INITIAL_PILLS_COUNT = 50;
        private Pill[] pills;
        private int pillsCount;
        private Hero hero;
        private Rectangle bounds;

        public PillsManager(Hero hero)
        {
            pills = new Pill[INITIAL_PILLS_COUNT];
            this.hero = hero;
        }

        public override void update(float dt)
        {
            base.update(dt);
            
            for (int pillIndex = 0; pillIndex < pillsCount; ++pillIndex)
            {
                Pill pill = pills[pillIndex];

                if (pill.v.Equals(Vector2.Zero))
                    continue;

                float oldX = pill.r.X;
                float oldY = pill.r.Y;

                Vector2 dr = Vector2.Multiply(pill.v, dt);
                pill.r.X += dr.X;
                pill.r.Y += dr.Y;
                if (pill.r.X < bounds.X)
                {
                    pill.r.X = bounds.X;
                    pill.v.X = -pill.v.X;
                }
                else if (pill.r.X > bounds.X + bounds.Width)
                {
                    pill.r.X = bounds.X + bounds.Width;
                    pill.v.X = -pill.v.X;
                }
                if (pill.r.Y < bounds.Y)
                {
                    pill.r.Y = bounds.Y;
                    pill.v.Y = -pill.v.Y;
                }
                else if (pill.r.Y > bounds.Y + bounds.Height)
                {
                    pill.r.Y = bounds.Y + bounds.Height;
                    pill.v.Y = -pill.v.Y;
                }
            }
        }

        public override void draw()
        {
            Texture2D text = Application.sharedResourceMgr.getTexture(Res.IMG_PILL_FAKE);
            float halfWidth = 0.5f * text.Width;
            float halfHeigth = 0.5f * text.Height;
            for (int pillIndex = 0; pillIndex < pillsCount; ++pillIndex)
            {
                Pill pill = pills[pillIndex];
                AppGraphics.DrawImage(text, pill.r.X - halfWidth, pill.r.Y - halfHeigth);
            }
        }

        public void addPill(Pill pill)
        {
            Debug.Assert(pillsCount < pills.Length);
            pills[pillsCount] = pill;
            pillsCount++;
        }

        public Rectangle Bounds
        {
            get { return bounds; }
            set { bounds = value; }
        }
    }
}
