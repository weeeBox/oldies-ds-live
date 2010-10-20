using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DuckstazyLive.game.utils
{
    public class CollisionHelper
    {
        public static bool collidesRectVsCircle(float x, float y, float w, float h, float cx, float cy, float r)
        {
            // Find the closest point to the circle within the rectangle
            float closestX = MathHelper.Clamp(cx, x, x + w);
            float closestY = MathHelper.Clamp(cy, y, y + h);

            // Calculate the distance between the circle's center and this closest point
            float distanceX = cx - closestX;
            float distanceY = cy - closestY;

            // If the distance is less than the circle's radius, an intersection occurs
            float distanceSquared = (distanceX * distanceX) + (distanceY * distanceY);
            return distanceSquared < (r * r);
        }
    }
}
