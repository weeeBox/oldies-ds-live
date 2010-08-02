using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DuckstazyLive.framework.graphics;

namespace DuckstazyLive.env.sky
{
    public class CloudsManager
    {
        private static int[] CLOUDS_IMAGE_ID = { Res.IMG_SKY_CLOUD_1, Res.IMG_SKY_CLOUD_2, Res.IMG_SKY_CLOUD_3 };

        private float Wsx = 1;
        private float Wsy = 1;
        private float Wx = -1;

        private int[] imageIds;
        private float[] x;
        private float[] y;
        private float[] scaleX;
        private float[] scaleY;
        private float[] velocity;
        private float[] elapsedTimes;        

        private Rectangle bounds;               

        private Random random;

        public CloudsManager(int cloudsCount, Rectangle bounds)
        {            
            this.bounds = bounds;

            imageIds = new int[cloudsCount];
            x = new float[cloudsCount];
            y = new float[cloudsCount];
            scaleX = new float[cloudsCount];
            scaleY = new float[cloudsCount];
            velocity = new float[cloudsCount];
            elapsedTimes = new float[cloudsCount];

            random = new Random();
            float dx = bounds.Width / cloudsCount;
            for (int cloudIndex = 0; cloudIndex < x.Length; cloudIndex++)
            {
                int imageId = CLOUDS_IMAGE_ID[random.Next(CLOUDS_IMAGE_ID.Length)];
                imageIds[cloudIndex] = imageId;            
                x[cloudIndex] = (float)((cloudIndex + 0.25f * random.Next(-100, 100) / 100.0f) * dx);
                y[cloudIndex] = bounds.Y + random.Next(0, bounds.Height);
                velocity[cloudIndex] = -10.0f;
            }
        }       
     
        private void SpawnCloud(int cloudIndex)
        {
            int imageId = CLOUDS_IMAGE_ID[random.Next(CLOUDS_IMAGE_ID.Length)];
            Image img = Resources.GetImage(imageId);

            int width = img.Width;
            int height = img.Height;

            imageIds[cloudIndex] = imageId;            
            x[cloudIndex] = bounds.Width + 0.5f * img.Width;
            y[cloudIndex] = bounds.Y + random.Next(height / 2, bounds.Height);                        
            elapsedTimes[cloudIndex] = 0.0f;

            Console.WriteLine("Spawn cloud: imageId=" + imageIds[cloudIndex] + " x=" + x[cloudIndex] + " y=" + y[cloudIndex] + " velocity=" + velocity[cloudIndex]);
        }        

        public void Update(float dt)
        {            
            for (int cloudIndex = 0; cloudIndex < velocity.Length; cloudIndex++)
            {
                UpdateCloud(cloudIndex, dt);
            }            
        }

        private void UpdateCloud(int cloudIndex, float dt)
        {
            Image image = GetCloudImage(cloudIndex);

            float minCloudX = bounds.X - image.Width / 2;
            if (x[cloudIndex] < minCloudX)
            {                
                SpawnCloud(cloudIndex);
                return;
            }

            float elapsedTime = elapsedTimes[cloudIndex];

            scaleX[cloudIndex] = (float)(1 + 0.1 * Math.Cos(Wsx * elapsedTime));
            scaleY[cloudIndex] = (float)(0.95 + 0.05 * Math.Sin(Wsy * elapsedTime));

            x[cloudIndex] += (float)(10 * velocity[cloudIndex] * (1 + 0.25 * Math.Sin(Wx * elapsedTime)) * dt);
            elapsedTimes[cloudIndex] += dt;
        }

        public void Draw(SpriteBatch batch)
        {
            for (int cloudIndex = 0; cloudIndex < velocity.Length; cloudIndex++)
            {
                DrawCloud(cloudIndex, ref batch);
            }
        }

        private void DrawCloud(int cloudIndex, ref SpriteBatch batch)
        {
            Image img = Resources.GetImage(imageIds[cloudIndex]);            
            img.SetOriginToCenter();
            img.Scale(scaleX[cloudIndex], scaleY[cloudIndex]);
            img.Draw(batch, x[cloudIndex], y[cloudIndex]);
        }

        private Image GetCloudImage(int cloudIndex)
        {
            return Resources.GetImage(imageIds[cloudIndex]);
        }

        private int GetCloudWidth(int cloudIndex)
        {
            return GetCloudImage(cloudIndex).Width;
        }

        private int GetCloudHeight(int cloudIndex)
        {
            return GetCloudImage(cloudIndex).Height;
        }
    }
}
