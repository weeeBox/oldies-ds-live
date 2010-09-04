using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using DuckstazyLive.graphics;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using DuckstazyLive.app;
using DuckstazyLive.core.graphics;
using DuckstazyLive.framework.core;
using DuckstazyLive.framework.graphics;

namespace DuckstazyLive.env.particles
{
    public enum ParticleType
    {
        BUBBLE, DROP, STAR
    }

    public class ParticlesManager
    {        
        private const float LIFETIME_BUBBLE = 2.0f;        
        
        public ParticleType[] types;
        public float[] xs;
        public float[] ys;
        public float[] vxs;
        public float[] vys;
        public float[] alphas;
        public float[] alphaSpeeds;
        public int[] imageIds;
        public float[] lifeTimes;
        public int numParticles;
        public Color[] colors;        

        public ParticlesManager(int maxParticlesCount)
        {            
            types = new ParticleType[maxParticlesCount];
            xs = new float[maxParticlesCount];
            ys = new float[maxParticlesCount];
            vxs = new float[maxParticlesCount];
            vys = new float[maxParticlesCount];
            alphas = new float[maxParticlesCount];
            alphaSpeeds = new float[maxParticlesCount];
            imageIds = new int[maxParticlesCount];
            lifeTimes = new float[maxParticlesCount];
            colors = new Color[maxParticlesCount];            
        }
        
        public void Draw(GameGraphics g)
        {            
            int processedParticles = 0;
            int totalParticles = numParticles;
            
            for (int particleIndex = 0; processedParticles < totalParticles; particleIndex++)
            {
                if (!IsDead(particleIndex))                
                {
                    DrawParticle(g, particleIndex);
                    processedParticles++;
                }                
            }         
        }

        private void DrawParticle(GameGraphics g, int index)
        {
            int imageId = imageIds[index];
            Image image = Resources.GetImage(imageId);
            float x = xs[index];
            float y = ys[index];

            switch (types[index])
            {
                case ParticleType.BUBBLE:
                case ParticleType.DROP:
                {
                    Color color = colors[index];

                    float scale = lifeTimes[index] / LIFETIME_BUBBLE;
                    color.A = (byte)(255 - 64 * (1 - scale));

                    image.SetScale(scale);
                    image.SetColor(color);
                    image.SetOriginToCenter();                        
                    image.Draw(g, x, y);
                }
                    break;
            }
            
        }       

        public void Update(float dt)
        {
            int processedParticles = 0;
            int totalParticles = numParticles;            

            for (int particleIndex = 0; processedParticles < totalParticles; particleIndex++)
            {
                if (!IsDead(particleIndex))                
                {
                    UpdateParticle(particleIndex, dt);
                    processedParticles++;
                }                
            }            
        }

        private void UpdateParticle(int index, float dt)
        {
            Debug.Assert(index >= 0 && index < lifeTimes.Length, index + "<" + lifeTimes.Length);

            lifeTimes[index] -= dt;
            if (IsDead(index))
            {
                RemoveParticle(index);
                return;
            }

            switch (types[index])
            {
                case ParticleType.BUBBLE:                    
                    UpdateBubbleParticle(index, dt);                    
                    break;

                case ParticleType.DROP:
                    UpdateDropParticle(index, dt);
                    break;

                default:
                    Debug.Assert(false, "Particle type not supported: " + types[index]);
                    break;
            }
        }        

        private void UpdateBubbleParticle(int index, float dt)
        {
            vys[index] -= 100.0f * dt;
            vxs[index] += (float)(200 * Math.Sin(MathHelper.TwoPi * lifeTimes[index]) * dt);

            xs[index] += vxs[index] * dt;
            ys[index] += vys[index] * dt;
        }

        private void UpdateDropParticle(int index, float dt)
        {
            vys[index] += 500f * dt;

            xs[index] += vxs[index] * dt;
            ys[index] += vys[index] * dt;
        }

        private bool IsDead(int index)
        {
            Debug.Assert(index >= 0 && index < lifeTimes.Length, index + "<" + lifeTimes.Length);
            return lifeTimes[index] <= 0;
        }

        private int AddParticle(ParticleType type, int imageId, float x, float y, float vx, float vy, float lifeTime)
        {
            int index = FindDead();
            if (index != -1)
            {
                if (numParticles == 0)
                {
                    
                }

                types[index] = type;
                imageIds[index] = imageId;
                xs[index] = x;
                ys[index] = y;
                vxs[index] = vx;
                vys[index] = vy;
                lifeTimes[index] = lifeTime;

                numParticles++;
            }
            return index;
        }

        private void RemoveParticle(int index)
        {
            lifeTimes[index] = 0;
            numParticles--;

            if (numParticles == 0)
            {
                
            }
        }

        private int FindDead()
        {
            for (int particleIndex = 0; particleIndex < lifeTimes.Length; particleIndex++)
            {
                if (lifeTimes[particleIndex] <= 0)
                    return particleIndex;
            }

            return -1;
        }

        public void StartBubble(float x, float y, Color color)
        {
            float vx = -40.0f + App.GetRandomNonNegativeFloat() * 80.0f;
            float vy = -200 * App.GetRandomNonNegativeFloat();
            int index = AddParticle(ParticleType.BUBBLE, Res.IMG_BUBBLE, x, y, vx, vy, Math.Max(0.2f, App.GetRandomNonNegativeFloat()) * LIFETIME_BUBBLE);
            if (index != -1)
            {
                colors[index] = color;
            }
        }

        public void StartDrop(float x, float y, Color color)
        {
            float vx = -200.0f + App.GetRandomNonNegativeFloat() * 400.0f;
            float vy = -200.0f + App.GetRandomNonNegativeFloat() * 100.0f;
            int index = AddParticle(ParticleType.DROP, Res.IMG_BUBBLE, x, y, vx, vy, Math.Max(0.2f, App.GetRandomNonNegativeFloat()) * LIFETIME_BUBBLE);
            if (index != -1)
            {
                colors[index] = color;
            }
        }

        private Application App
        {
            get { return Application.Instance; }
        }
    }
}
