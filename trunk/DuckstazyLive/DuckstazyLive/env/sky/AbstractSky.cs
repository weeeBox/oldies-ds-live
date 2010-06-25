using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace DuckstazyLive.env.sky
{
    public abstract class AbstractSky
    {        
        private GradientRect backfill;
        private Color upperColor;
        private Color lowerColor;

        public AbstractSky(Color upperColor, Color lowerColor)
        {
            this.upperColor = upperColor;
            this.lowerColor = lowerColor;
        }

        public abstract void Draw();
        public virtual void Update(float dt) {}

        protected GradientRect Backfill
        {
            get { return backfill; }
        }

        protected Color UpperColor
        {
            get { return upperColor; }
        }

        protected Color LowerColor
        {
            get { return lowerColor; }
        }
    }
}
