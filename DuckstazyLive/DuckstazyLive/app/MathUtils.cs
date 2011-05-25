using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DuckstazyLive.app
{
    public class MathUtils
    {
        public static float lerp(float x, float a, float b)
        {
            return a + x * (b - a);
        }

        public static float vec2angle(Vector2 v1, Vector2 v2)
        {
            return (float)(Math.Atan2(v1.Y, v1.X) - Math.Atan2(v2.Y, v2.X));
        }

        public static Vector2 vec2norm(Vector2 vec)
        {
            float inv_len = (float)(1.0 / Math.Sqrt(vec.X * vec.X + vec.Y * vec.Y));

            return new Vector2(vec.X * inv_len, vec.Y * inv_len);
        }

        public static float vec2lenSqr(Vector2 vec)
        {
            return vec.X * vec.X + vec.Y * vec.Y;
        }

        public static float vec2distSqr(float x1, float y1, float x2, float y2)
        {
            float dx = x1 - x2;
            float dy = y1 - y2;

            return dx * dx + dy * dy;
        }

        public static Vector2 vec2norm2(Vector2 vec1, Vector2 vec2)
        {
            float dx = vec1.X - vec2.X;
            float dy = vec1.Y - vec2.Y;
            float inv_len = (float)(1.0 / Math.Sqrt(dx * dx + dy * dy));

            return new Vector2(dx * inv_len, dy * inv_len);
        }

        public static Vector2 vec2multScalar(Vector2 vec, float a)
        {
            float dx = a * vec.X;
            float dy = a * vec.Y;

            return new Vector2(dx, dy);
        }


        public static float pos2pan(float x)
        {
            float p = (x - 320.0f) / 320.0f;

            if (p > 1) p = 1;
            else if (p < -1) p = -1;

            return p;
        }
    }
}
