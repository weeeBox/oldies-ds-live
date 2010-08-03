using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuckstazyLive.game
{
    public class World
    {
        private float x;
        private float y;
        private float width;
        private float height;

        public World(float x, float y, float width, float height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public float ToScreenX(float worldX)
        {
            return worldX;
        }

        public float ToScreenY(float worldY)
        {
            return -worldY;
        }

        public float X
        {
            get { return x; }
        }

        public float Y
        {
            get { return y; }
        }

        public float Width
        {
            get { return width; }
        }

        public float Height
        {
            get { return height; }
        }
    }
}
