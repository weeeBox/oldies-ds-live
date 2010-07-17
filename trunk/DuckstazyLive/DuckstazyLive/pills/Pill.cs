using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using DuckstazyLive.core.graphics;
using System.Diagnostics;

namespace DuckstazyLive.pills
{
    public class Pill : IDisposable
    {
        private PillType type;
        private float x;
        private float y;
        private float vx;
        private float vy;
        private float lifeTime;
        private float delay;

        private PillsManager manager;

        public Pill(PillsManager manager)
        {
            this.manager = manager;
        }        

        public void Init(PillType type, float x, float y)
        {
            Init(type, x, y, 0.0f, 0.0f, 0.0f);
        }

        public void Init(PillType type, float x, float y, float vx, float vy, float delay)
        {
            this.type = type;
            this.x = x;
            this.y = y;
            this.vx = vx;
            this.vy = vy;
            this.delay = delay;

            lifeTime = 0;
        }

        public void Update(float dt)
        {
            if (delay > 0.0f)
            {
                delay -= dt;
                if (delay <= 0.0f)
                {
                    manager.OnPillAdded(this);
                }
            }
            else
            {
                lifeTime += dt;
                x += vx * dt;
                y += vy * dt;            
            }            
        }

        public void Draw(SpriteBatch batch)
        {
            Image baseImage = Resources.GetImage(Res.IMG_PILL_BASE);
            baseImage.SetOriginToCenter();

            Image pillImage = GetPillImage(type);
            pillImage.SetOriginToCenter();

            baseImage.Draw(batch, x, y);
            pillImage.Draw(batch, x, y);
        }

        private Image GetPillImage(PillType type)
        {
            switch (type)
            {
                case PillType.QUESTION:
                    return Resources.GetImage(Res.IMG_PILL_QUESTION);
                case PillType.STAR:
                    return Resources.GetImage(Res.IMG_PILL_STAR);
                default:
                    Debug.Assert(false, "Type not supported: " + type);
                    break;
            }

            return null;
        }

        public void Dispose()
        {            
        }
    }
}
