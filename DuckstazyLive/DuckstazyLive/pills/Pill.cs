﻿using System;
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
        public PillType type;
        public float x;
        public float y;
        public float vx;
        public float vy;
        public float lifeTime;
        public float delay;
        public Color color;        

        public Pill()
        {            
            color = Color.White;
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
            }
            else
            {
                lifeTime += dt;
            }            
        }

        public void Draw(SpriteBatch batch)
        {
            Image baseImage = Resources.GetImage(Res.IMG_PILL_BASE);
            baseImage.SetOriginToCenter();

            Image pillImage = GetPillImage(type);
            pillImage.SetOriginToCenter();

            pillImage.SetColor(color);
            baseImage.SetColor(color);

            GDebug.DrawCircle(x, y, baseImage.Width / 2);

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