using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using asap.util;
using asap.graphics;

namespace DuckstazyLive.app.game
{
    public class Particle
    {
        public float x;
        public float y;
        public float vx;
        public float vy;

        public float a;
        public float va;

        public float t;

        public int type;
        public float p1;
        public float p2;

        public GameTexture img;
        public float px;
        public float py;
        public float s;

        public float alpha;
        public ColorTransform col = ColorTransform.NONE;

        public Particle()
        {
            t = 0.0f;
        }

        public float Width
        {
            get { return img.GetWidth(); }
        }

        public float Height
        {
            get { return img.GetHeight(); }
        }
    }
}
