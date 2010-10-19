using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DuckstazyLive.game
{
    public class Pill
    {
        public Vector2 r;
        public Vector2 v;
        public Vector2 a;

        public Pill(float x, float y)
        {
            r = new Vector2(x, y);
            v = Vector2.Zero;
            a = Vector2.Zero;
        }
    }
}
