using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Framework.core;
using DuckstazyLive.app;

namespace DuckstazyLive.game
{
    public class Pill
    {
        public float x, y, vx, vy;
        public bool active;
        public float lifeTime;

        private int poolIndex;
        internal int PoolIndex
        {
            get { return poolIndex; }
            set { poolIndex = value; }
        }
        internal void clear()
        {            
            x = y = vx = vy = 0.0f;
            active = true;
            lifeTime = 0.0f;
        }

        public void draw()
        {
            draw(x, y);
        }

        public void draw(float x, float y)
        {
            Texture2D tex = Framework.core.Application.sharedResourceMgr.getTexture(Res.IMG_PILL_FAKE);
            float halfWidth = 0.5f * tex.Width;
            float halfHeight = 0.5f * tex.Height;            
            AppGraphics.DrawImage(tex, x - halfWidth, y - halfHeight);            
        }
    }
}
