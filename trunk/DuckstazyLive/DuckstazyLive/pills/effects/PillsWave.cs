using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace DuckstazyLive.pills.effects
{
    public class PillsWave : PillsManager
    {        
        private const float SPAWN_TIMEOUT = 0.3f;
        
        private float lambda;
        private float amplitude;
        private float omega;
        private float t;
        private float baseY;

        public PillsWave(float x, float y, float width, float height, int pillsCount) : base(pillsCount)
        {
            this.pillsCount = pillsCount;

            float dx = width / (pillsCount - 1f);
            lambda = 0.75f * width;
            amplitude = height;
            omega = MathHelper.PiOver2;           

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

        public override void UpdatePill(int pillIndex, float dt)
        {
            base.UpdatePill(pillIndex, dt);

            Pill pill = pills[pillIndex];
            pill.y = baseY + (float)(amplitude * Math.Sin(omega * (t - MathHelper.TwoPi / lambda * pill.x)));
        }

        public override void Draw(SpriteBatch batch)
        {
            for (int pillIndex = 0; pillIndex < pills.Length; pillIndex++)
            {
                Pill pill = pills[pillIndex];
                if (pill.delay <= 0.0f)
                    pill.Draw(batch);
            }
        }        
    }
}
