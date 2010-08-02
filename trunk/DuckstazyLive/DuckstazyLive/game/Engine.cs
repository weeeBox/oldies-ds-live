using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckstazyLive.graphics;
using DuckstazyLive.pills;
using Microsoft.Xna.Framework.Graphics;
using DuckstazyLive.app;
using DuckstazyLiveXbox.pills;
using DuckstazyLive.pills.effects;
using DuckstazyLive.framework.core;
using DuckstazyLive.framework.graphics;

namespace DuckstazyLive.game
{
    public class Engine : Timer
    {
        private Background background;
        private Hero hero;
        private Wave wave;

        private PillsManager pillsManager;
        private float x;
        private float y;
        private float width;
        private float height;

        public Engine(float x, float y, float width, float height)
        {
            hero = new Hero();
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;

            float waveWidth = width;
            float waveHeight = 2 * 22.5f;
            float waveX = 0;
            float waveY = height - (Constants.GROUND_HEIGHT + waveHeight) / 2;
            wave = new Wave(waveX, waveY, waveWidth, waveHeight);

            App.InputManager.AddInputListener(hero);


            float pillsOffsetX = Application.Instance.Width / 16f;
            float pillsOffsetY = (Application.Instance.Height - Constants.GROUND_HEIGHT) / 16f;

            pillsManager = new PillsWave(hero, pillsOffsetX, 400, Application.Instance.Width - 2 * pillsOffsetX, 15, 15);
            pillsManager.AddPillListener(new PillParticles());

            StartTimer();
        }

        public void LoadContent()
        {
            background = new Background(Constants.GROUND_HEIGHT);
        }

        public void Draw(GameGraphics g)
        {
            background.DrawSky(g);
            
            pillsManager.Draw(g);
            hero.Draw(g);                        

            Application.Instance.Particles.Draw(g);            

            background.DrawGround(g);
            // wave.Draw(g);            
        }

        public override void Update(float dt)
        {
            hero.Update(dt);
            background.Update(dt);
            pillsManager.Update(dt);
        }

        private Application App
        {
            get { return Application.Instance; }
        }
    }
}
