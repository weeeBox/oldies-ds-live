using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckstazyLive.core.graphics;
using DuckstazyLive.game;

namespace DuckstazyLive.core.collision
{
    public class CollisionRect
    {
        private float x;
        private float y;
        private float width;
        private float height;

        public CollisionRect(float x, float y, float width, float height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }       

        public float X
        {
            get { return x; }
            set { x = value; }
        }

        public float Y
        {
            get { return y; }
            set { y = value; }
        }

        public float CenterX
        {
            get { return x + 0.5f * width; }
            set { x = value - 0.5f * width; }
        }

        public float CenterY
        {
            get { return y + 0.5f * height; }
            set { y = value - 0.5f * height; }
        }

        public float Width
        {
            get { return width; }
            set { width = value; }
        }

        public float Height
        {
            get { return height; }
            set { height = value; }
        }        
    }
}
