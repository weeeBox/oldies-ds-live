using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Framework.utils
{
    public struct LineSegment
    {
        public Vector2 a, b;

        public LineSegment(float x1, float y1, float x2, float y2)
        {
            a = new Vector2(x1, y1);
            b = new Vector2(x2, y2);
        }

        public LineSegment(float x, float y, Vector2 r)
        {
            a = new Vector2(x, y);
            b = new Vector2(x + r.X, y + r.Y);
        }       

        public bool collidesRect(float x, float y, float w, float h, out float t)
        {
            t = 1.0f;

            Vector2 s1 = new Vector2(x, y);
            Vector2 s2 = new Vector2(x + w, y);
            Vector2 s3 = new Vector2(x + w, y + h);
            Vector2 s4 = new Vector2(x, y + h);

            bool hasIntersect = false;
            float ct;
            if (Intersects(a, b, s1, s2, out ct))
            {
                hasIntersect = true;
                if (ct < t)
                    t = ct;
            }
            if (Intersects(a, b, s2, s3, out ct))
            {
                hasIntersect = true;
                if (ct < t)
                    t = ct;
            }
            if (Intersects(a, b, s3, s4, out ct))
            {
                hasIntersect = true;
                if (ct < t)
                    t = ct;
            }
            if (Intersects(a, b, s4, s1, out ct))
            {
                hasIntersect = true;
                if (ct < t)
                    t = ct;
            }

            return hasIntersect;
        }

        static bool Intersects(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2, out float t)
        {
            Vector2 b = a2 - a1;
            Vector2 d = b2 - b1;

            float bDotDPerp = b.X * d.Y - b.Y * d.X;
            t = 0.0f;

            // if b dot d == 0, it means the lines are parallel so have infinite intersection points
            if (bDotDPerp == 0)
                return false;

            Vector2 c = b1 - a1;
            t = (c.X * d.Y - c.Y * d.X) / bDotDPerp;
            if (t < 0 || t > 1)
                return false;

            float u = (c.X * b.Y - c.Y * b.X) / bDotDPerp;
            if (u < 0 || u > 1)
                return false;

            return true;
        }
    }
}
