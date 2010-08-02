using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using DuckstazyLive.core.graphics;
using DuckstazyLive.framework.graphics;

namespace DuckstazyLive.pills.effects
{
    struct Bounds
    {
        public float x;
        public float y;
        public float w;
        public float h;

        public Bounds(float x, float y, float width, float height)
        {
            this.x = x;
            this.y = y;
            w = width;
            h = height;
        }
    }

    public class PillsWave : PillsManager
    {        
        private const float SPAWN_TIMEOUT = 0.3f;
        
        private float lambda;
        private float amplitude;
        private float omega;
        private float t;
        private float baseY;

        private Bounds bounds;

        public PillsWave(Hero hero, float x, float y, float width, float height, int pillsCount) : base(hero, pillsCount)
        {
            this.pillsCount = pillsCount;

            float dx = width / (pillsCount - 1f);
            lambda = 0.75f * width;
            amplitude = height;
            omega = MathHelper.PiOver2;

            bounds = new Bounds(x, y - amplitude, width, height + 2 * amplitude);

            float spawnTime = SPAWN_TIMEOUT;

            float pillX = x;
            float pillY = y + height / 2f;
            for (int pillIndex = 0; pillIndex < pills.Length; pillIndex++)
            {
                Pill pill = pills[pillIndex];
                pill.Init(PillType.QUESTION, pillX, pillY, 0.0f, 0.0f, spawnTime);                

                pillX += dx;
                spawnTime += SPAWN_TIMEOUT;
            }

            baseY = pillY;
        }

        public override void Update(float dt)
        {
            t += dt;
            base.Update(dt);
        }

        public override void UpdatePill(Pill pill, float dt)
        {            
            base.UpdatePill(pill, dt);            
            pill.y = baseY + (float)(amplitude * Math.Sin(omega * (t - MathHelper.TwoPi / lambda * pill.x)));
        }

        public override void Draw(GameGraphics g)
        {
            for (int pillIndex = 0; pillIndex < pills.Length; pillIndex++)
            {
                Pill pill = pills[pillIndex];
                if (pill.delay <= 0.0f)
                    pill.Draw(g);
            }
        }        
    }
}
