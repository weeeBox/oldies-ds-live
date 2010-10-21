using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.core;
using Microsoft.Xna.Framework.Graphics;
using DuckstazyLive.app;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using Framework.utils;

namespace DuckstazyLive.game
{
    public abstract class Level : BaseElement
    {        
        public static Hero sharedHero;
        public static Rect levelBounds;        
        private static GlobalPillsPool pool;

        public Level(Hero hero)
        {
            sharedHero = hero;

            x = levelBounds.X;
            y = levelBounds.Y;
            width = (int)levelBounds.Width;
            height = (int)levelBounds.Height;
        }

        protected abstract void updateLevel(float dt);
        protected abstract void drawLevel();
        public virtual void init() {}

        public override void update(float dt)
        {
            base.update(dt);
            sharedHero.update(dt);
            updateLevel(dt);
        }

        public override void draw()
        {
            drawLevel();
            sharedHero.draw();
        }

        public Rect Bounds
        {
            get { return levelBounds; }            
        }

        public static GlobalPillsPool Pool
        {
            get { return pool; }
            set { pool = value; }
        }

        internal static float toScreenX(float x)
        {
            return levelBounds.X + x;
        }

        internal static float toScreenY(float y)
        {
            return levelBounds.Y + y;
        }
    }
}
