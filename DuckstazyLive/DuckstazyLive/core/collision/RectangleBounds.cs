using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuckstazyLive.core.collision
{
    public struct RectangleBounds
    {
        private float x;
        private float y;
        private float width;
        private float height;

        public RectangleBounds(float x, float y, float width, float height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }
    }
}
