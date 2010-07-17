using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using DuckstazyLive.core.graphics;
using System.Diagnostics;
using DuckstazyLive.graphics;

namespace DuckstazyLive.env.sky
{
    public class NightSky : AbstractSky
    {
        private float[] starX;
        private float[] starY;
        private float[] starVx;
        private float[] starVy;
        private float[] starScale;
        private Color[] starColor;
        private int starsCount;
        private int maxStarsCount;

        private float width;
        private float height;

        public NightSky(float width, float height, Color upperColor, Color lowerColor, int starsCount, int maxStarsCount) : base(width, height, upperColor, lowerColor)
        {
            Debug.Assert(starsCount <= maxStarsCount, starsCount + "<=" + maxStarsCount);

            this.maxStarsCount = maxStarsCount;

            starX = new float[maxStarsCount];
            starY = new float[maxStarsCount];
            starVx = new float[maxStarsCount];
            starVy = new float[maxStarsCount];
            starScale = new float[maxStarsCount];
            starColor = new Color[maxStarsCount];

            this.width = width;
            this.height = height;

            Application app = Application.Instance;

            Color color = new Color(120, 120, 139);
            for (int starIndex = 0; starIndex < starsCount; starIndex++)
            {
                float x = app.GetRandomNonNegativeFloat() * width;
                float y = app.GetRandomNonNegativeFloat() * height;
                float vx = app.GetRandomFloat() * 100;
                float vy = app.GetRandomFloat() * 100;
                AddStar(x, y, vx, vy, color);
            }
        }

        public void AddStar(float x, float y, float vx, float vy, Color color)
        {
            if (starsCount == maxStarsCount)
            {
                return;
            }

            float scale = 1.0f - 0.5f * Application.Instance.GetRandomNonNegativeFloat();
            color.A = (byte)(255 - 128 * (1 - scale));

            starX[starsCount] = x;
            starY[starsCount] = y;
            starVx[starsCount] = vx;
            starVy[starsCount] = vy;
            starColor[starsCount] = color;
            starScale[starsCount] = scale;
            starsCount++;
        }

        public override void Update(float dt)
        {
            for (int starIndex = 0; starIndex < starsCount; starIndex++)
            {
                starX[starIndex] += starVx[starIndex] * dt;
                starY[starIndex] += starVy[starIndex] * dt;

                if (starX[starIndex] < 0)
                {
                    starX[starIndex] = 0;
                    starVx[starIndex] = -starVx[starIndex];
                }
                else if (starX[starIndex] > width)
                {
                    starX[starIndex] = width;
                    starVx[starIndex] = -starVx[starIndex];
                }

                if (starY[starIndex] < 0)
                {
                    starY[starIndex] = 0;
                    starVy[starIndex] = -starVy[starIndex];
                }
                else if (starY[starIndex] > height)
                {
                    starY[starIndex] = height;
                    starVy[starIndex] = -starVy[starIndex];
                }
            }
        }

        public override void Draw(RenderContext context)
        {
            base.Draw(context);

            SpriteBatch batch = context.SpriteBatch;


            batch.Begin();
            Image starImage = Resources.GetImage(Res.IMG_STAR);
            starImage.SetOriginToCenter();

            for (int starIndex = 0; starIndex < starsCount; starIndex++)
            {
                float drawX = starX[starIndex];
                float drawY = starY[starIndex];                

                starImage.SetScale(starScale[starIndex]);
                starImage.SetColor(starColor[starIndex]);
                starImage.Draw(batch, drawX, drawY);
            }
            batch.End();
        }
    }
}
