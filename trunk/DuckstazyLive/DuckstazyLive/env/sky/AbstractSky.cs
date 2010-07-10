using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using DuckstazyLive.graphics;

namespace DuckstazyLive.env.sky
{
    public abstract class AbstractSky
    {        
        private GradientRect backfill;
        private Color upperColor;
        private Color lowerColor;
        private float width;
        private float height;

        public AbstractSky(float width, float height, Color upperColor, Color lowerColor)
        {
            this.upperColor = upperColor;
            this.lowerColor = lowerColor;
            this.width = width;
            this.height = height;
            backfill = new GradientRect(0.0f, 0.0f, width, height, upperColor, lowerColor);            
        }

        public virtual void Draw(RenderContext context)
        {
            backfill.Draw(context);
        }

        public virtual void Update(GameTime gameTime) {}

        public float Width
        {
            get { return width; }
        }

        public float Height
        {
            get { return height; }
        }

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
