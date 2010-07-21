using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuckstazyLive.core.collision
{
    public class CollisionHelper
    {
        public static bool RectanglesCollide(float x1, float y1, float w1, float h1, float x2, float y2, float w2, float h2)
        {
            float cx1 = x1 + 0.5f * w1;
            float cx2 = x2 + 0.5f * w2;

            if (Math.Abs(cx1 - cx2) > 0.5f * (w1 + w2))
                return false;

            float cy1 = y1 + 0.5f * h1;
            float cy2 = y2 + 0.5f * h2;

            return Math.Abs(cy1 - cy2) <= 0.5f * (h1 + h2);                
        }

        public static bool CirclesCollide(float x1, float y1, float r1, float x2, float y2, float r2)
        {
            float dx = x1 - x2;
            float dy = y1 - y2;
            float r = r1 + r2;
            return dx * dx + dy * dy < r * r;
        }

        public static bool CicleRectangleCollide(float cx, float cy, float r, float x, float y, float w, float h)
        {
            return RectanglesCollide(x, y, w, h, cx - r, cy - r, 2 * r, 2 * r) && CirclesCollide(cx, cy, r, x + 0.5f * w, y + 0.5f * h, (float)(0.5f * Math.Sqrt(w * w + h * h)));
        }
    }
}
