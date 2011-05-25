using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using asap.graphics;
using app;
using asap.visual;

namespace DuckstazyLive.app.game
{
    public class FloatText
    {
        private float t;
        private string text;
        private Color startColor;

        private float x, y;
        private float vx;

        public void start(string text, float x, float y, ref Color c)
        {
            startColor = c;
            this.x = x;
            this.y = y;
            this.text = text;
            t = 1.0f;
            vx = 0.0f;
        }

        public void Update(float dt)
        {
            vx = (float)(10.0f * Math.Sin(18 * t));
            x += vx * dt;
            y -= 50.0f * dt;
            t -= dt;
        }

        public void draw(Graphics g)
        {
            BaseFont font = Application.sharedResourceMgr.GetFont(Res.FNT_PICKUP);
            Color drawColor = startColor * t;
            AppGraphics.SetColor(drawColor);
            font.DrawString(g, text, x, y, 0);
            AppGraphics.SetColor(Color.White);
        }

        public bool isAlive()
        {
            return t > 0.0f;
        }

        public void reset()
        {
            t = 0.0f;
            text = null;
        }
    }
}
