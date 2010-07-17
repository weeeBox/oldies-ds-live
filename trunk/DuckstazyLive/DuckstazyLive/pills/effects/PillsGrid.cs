using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace DuckstazyLive.pills.effects
{
    public class PillsGrid
    {
        private const int PILLS_COUNT_LONG_ROW = 6;
        private const int PILLS_COUNT_SHORT_ROW = 5;

        private Pill[] pills;

        public PillsGrid(float x, float y, float width, float height, int rowsCount)
        {
            Debug.Assert(rowsCount > 1, rowsCount.ToString());

            int pillsCount = (PILLS_COUNT_LONG_ROW + PILLS_COUNT_SHORT_ROW) * rowsCount / 2;
            pills = new Pill[pillsCount];

            float linesDistance = height / (rowsCount - 1f);
            float pillsDistance = width / (PILLS_COUNT_LONG_ROW - 1f);            
            
            float pillY = y;
            for (int rowIndex = 0, pillIndex = 0; rowIndex < rowsCount / 2; rowIndex++)
            {
                float pillX = x;                
                for (int i = 0; i < PILLS_COUNT_LONG_ROW; i++)
                {
                    Pill pill = new Pill();
                    pill.x = pillX;
                    pill.y = pillY;
                    pills[pillIndex++] = pill;

                    pillX += pillsDistance;
                }
                pillY += linesDistance;

                pillX = x + pillsDistance / 2f;
                for (int i = 0; i < PILLS_COUNT_SHORT_ROW; i++)
                {
                    Pill pill = new Pill();
                    pill.x = pillX;
                    pill.y = pillY;
                    pills[pillIndex++] = pill;

                    pillX += pillsDistance;
                }
                pillY += linesDistance;
            }
        }

        public void Update(float dt)
        {

        }

        public void Draw(SpriteBatch batch)
        {
            for (int pillIndex = 0; pillIndex < pills.Length; pillIndex++)
            {
                pills[pillIndex].Draw(batch);
            }
        }
    }
}
