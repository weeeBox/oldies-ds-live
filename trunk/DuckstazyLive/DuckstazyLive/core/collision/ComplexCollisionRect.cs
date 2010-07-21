using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuckstazyLive.core.collision
{
    public class ComplexCollisionRect : ICollidable
    {
        private List<RectangleBounds> rects;        

        private float x;
        private float y;
        private float width;
        private float height;

        public ComplexCollisionRect(float x, float y)
        {
            rects = new List<RectangleBounds>();
            this.x = x;
            this.y = y;
        }

        public void AddCollisionRect(RectangleBounds rect)
        {
            //rects.Add(rect);

            //x = Math.Min(x, rect.X);
            //y = Math.Min(y, rect.Y);

            //width = Math.Max(x + width, rect.X + rect.Width) - x;
            //height = Math.Max(y + height, rect.X + rect.Height) - y;
        }

        public bool Collides(CollisionRect other)
        {
            //if (CollisionHelper.RectanglesCollide(x, y, width, height, other.X, other.Y, other.Width, other.Height))
            //{
            //    for (int rectIndex = 0; rectIndex < rects.Capacity; rectIndex++)
            //    {
            //        if (rects[rectIndex].Collides(other))
            //            return true;
            //    }
            //}

            return false;
        }

        public bool Collides(CollisionCircle other)
        {
            //if (CollisionHelper.CicleRectangleCollide(other.CenterX, other.CenterY, other.Radius, x, y, width, height))
            //{
            //    for (int rectIndex = 0; rectIndex < rects.Capacity; rectIndex++)
            //    {
            //        if (rects[rectIndex].Collides(other))
            //            return true;
            //    }
            //}

            return false;
        }

        public void Dispose()
        {
            //foreach (CollisionRect rect in rects)
            //{
            //    rect.Dispose();                
            //}
            //rects.Clear();
            //rects = null;
        }
     }
}
