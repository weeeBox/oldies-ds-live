using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using asap.visual;
using Microsoft.Xna.Framework;

namespace DuckstazyLive.app.game.env
{
    public abstract class Sky : BaseElementContainer
    {
        protected RectShape shape;

        protected float power;

        public Sky(float width, float height, Color color) : this(width, height, color, color)
        {
        }

        public Sky(float width, float height, Color c1, Color c2) : base(width, height)
        {
            shape = new RectShape(width, height, c1, c2);
            AddChild(shape);
        }

        public override void Update(float delta)
        {
            Update(delta, power);
        }

        protected virtual void Update(float dt, float power)
        {
        }
    }
}
