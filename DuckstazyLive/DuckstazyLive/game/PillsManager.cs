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
        private static Hero hero;
        private Rectangle bounds;
        private static GlobalPillsPool pool;

        public PillsManager()
        {                        
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

        public static Hero Hero
        {
            get { return hero; }
            set { hero = value; }
        }
    }
}
