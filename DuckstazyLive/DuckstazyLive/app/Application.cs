using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using DuckstazyLive.graphics;
using DuckstazyLive.env.particles;
using DuckstazyLive.core.input;
using DuckstazyLive.framework.core;

namespace DuckstazyLive
{
    class Application
    {
        private static Application instance;

        private int width;
        private int height;
        private GraphicsDevice graphicsDevice;
        private SpriteBatch spriteBatch;
        
        private ParticlesManager particles;
        private Random random;
        private InputManager inputManager;
        private TimerManager timerManager;

        public Application(int width, int height)
        {
            instance = this;

            this.width = width;
            this.height = height;
        }

        public void Init()
        {
            timerManager = new TimerManager(20);
            particles = new ParticlesManager(100);
            random = new Random();
            // inputManager = new InputManager();
            // inputManager.StartTimer();
        }

        public void Update(float dt)
        {
            // timerManager.Update(dt);
        }

        public int Width
        {
            get { return width; }
        }

        public int Height
        {
            get { return height; }
        }

        public GraphicsDevice GraphicsDevice
        {
            get { return graphicsDevice; }
            set { graphicsDevice = value; }
        }

        public SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
            set { spriteBatch = value; }
        }        

        public ParticlesManager Particles
        {
            get { return particles; }
        }

        public int GetRandomInt(int maxValue)
        {
            return random.Next(maxValue);
        }

        public int GetRandomInt(int minValue, int maxValue)
        {
            return random.Next(minValue, maxValue);
        }

        public float GetRandomFloat()
        {
            return (float)(((random.Next() % 2) == 0 ? 1 : -1) * random.NextDouble());
        }

        public float GetRandomNonNegativeFloat()
        {
            return (float)(random.NextDouble());
        }

        public Random Random
        {
            get { return random; }
        }

        public TimerManager TimerManager
        {
            get { return timerManager; }
        }

        public InputManager InputManager
        {
            get { return inputManager; }
        }        

        public static Application Instance
        {
            get { return instance; }
        }
    }
}
