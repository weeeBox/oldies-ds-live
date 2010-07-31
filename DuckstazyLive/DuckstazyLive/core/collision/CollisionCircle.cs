using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuckstazyLive.core.collision
{
    public class CollisionCircle
    {
        private float cx;
        private float cy;
        private float r;

        public CollisionCircle(float centerX, float centerY, float radius)
        {
            cx = centerX;
            cy = centerY;
            r = radius;
        }        

        public float CenterX
        {
            get { return cx; }
            set { cy = value; }
        }

        public float CenterY
        {
            get { return cy; }
            set { cy = value; }
        }

        public float Radius
        {
            get { return r; }
            set { r = value; }
        }

        public void Dispose() {}
    }
}
