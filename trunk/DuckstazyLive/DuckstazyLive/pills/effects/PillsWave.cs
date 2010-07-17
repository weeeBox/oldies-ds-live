using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace DuckstazyLive.pills.effects
{
    public class PillsWave : IDisposable
    {        
        private Pill[] pills;        

        public PillsWave(float x, float y, float width, float height, int pillsCount)
        {            
            float dx = width / (pillsCount - 1f);

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
        }

        public void Update(float dt)
        {
            for (int pillIndex = 0; pillIndex < pills.Length; pillIndex++)
            {

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
