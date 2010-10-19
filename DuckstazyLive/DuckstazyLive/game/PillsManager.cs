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
    public abstract class PillsManager : BaseElement
    {        
        private Hero hero;
        private Rectangle bounds;
        private static GlobalPillsPool pool;

        public PillsManager(Hero hero)
        {            
            this.hero = hero;            
        }

        protected abstract void updatePills(float dt);
        protected abstract void drawPills();
        public virtual void init() {}

        public override void update(float dt)
        {
            base.update(dt);
            updatePills(dt);
        }

        public override void draw()
        {
            drawPills();
        }

        protected bool collides(float x, float y, float w, float h, float cx, float cy, float r)
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

        public Rectangle Bounds
        {
            get { return bounds; }
            set { bounds = value; }
        }

        public static GlobalPillsPool Pool
        {
            get { return pool; }
            set { pool = value; }
        }

        protected Hero Hero
        {
            get { return hero; }
        }
    }
}
