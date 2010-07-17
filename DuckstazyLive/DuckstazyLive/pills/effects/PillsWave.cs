using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace DuckstazyLive.pills.effects
{
    public class PillsWave : IDisposable
    {        
        private Pill[] pills;
        private float lambda;
        private float amplitude;
        private float omega;
        private float t;
        private float baseY;

        public PillsWave(float x, float y, float width, float height, int pillsCount)
        {            
            float dx = width / (pillsCount - 1f);
            lambda = 0.75f * width;
            amplitude = height;
            omega = MathHelper.PiOver2;

            float pillX = x;
            float pillY = y + height / 2f;
            pills = new Pill[pillsCount];            
            for (int pillIndex = 0; pillIndex < pills.Length; pillIndex++)
            {
                Pill pill = new Pill();
                pill.Init(PillType.QUESTION, pillX, pillY);
                pills[pillIndex] = pill;

                pillX += dx;
            }

            baseY = pillY;
        }

        public void Update(float dt)
        {
            t += dt;

            for (int pillIndex = 0; pillIndex < pills.Length; pillIndex++)
            {
                Pill pill = pills[pillIndex];
                pill.y = baseY + (float)(amplitude * Math.Sin(omega * (t - MathHelper.TwoPi / lambda * pill.x)));
            }
        }

        public void Draw(SpriteBatch batch)
        {
            for (int pillIndex = 0; pillIndex < pills.Length; pillIndex++)
            {                
                pills[pillIndex].Draw(batch);
            }
        }

        public void Dispose()
        {
            for (int pillIndex = 0; pillIndex < pills.Length; pillIndex++)
            {
                pills[pillIndex].Dispose();
                pills[pillIndex] = null;
            }
            pills = null;
        }
    }
}
