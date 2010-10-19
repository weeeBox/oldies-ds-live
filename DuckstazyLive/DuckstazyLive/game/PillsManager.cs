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
                {
                }
                else
                {
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

                if (collides(hero.x, hero.y, hero.width, hero.height, pill.r.X, pill.r.Y, Constants.PILL_RADIUS))
                {
                    pillsCount--;
                    pills[pillIndex] = pills[pillsCount];
                    pills[pillsCount] = pill;
                    pillIndex--;
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

            for (int pillIndex = 0; pillIndex < pillsCount; ++pillIndex)
            {
                Pill pill = pills[pillIndex];
                AppGraphics.DrawCircle(pill.r.X, pill.r.Y, Constants.PILL_RADIUS, Color.White);
            }           
            AppGraphics.DrawRect(hero.x, hero.y, hero.width, hero.height, Color.White);
        }

        private bool collides(float x, float y, float w, float h, float cx, float cy, float r)
        {
            // Find the closest point to the circle within the rectangle
            float closestX = MathHelper.Clamp(cx, x, x + w);
            float closestY = MathHelper.Clamp(cy, y, y + h);

            // Calculate the distance between the circle's center and this closest point
            float distanceX = cx - closestX;
            float distanceY = cy - closestY;

            // If the distance is less than the circle's radius, an intersection occurs
            float distanceSquared = (distanceX * distanceX) + (distanceY * distanceY);
            return distanceSquared < (r * r);
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
