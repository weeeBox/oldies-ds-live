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
using Microsoft.Xna.Framework;

namespace DuckstazyLive.game
{
    public class Engine
    {
        private Background background;
        private Hero hero;
        private Wave wave;

        private PillsManager pillsManager;
        private World world;        

        public Engine(float x, float y, float width, float height)
        {            
            world = new World(x, y, width, height);
            hero = new Hero(world);

            float waveWidth = width;
            float waveHeight = 2 * 22.5f;
            float waveX = 0;
            float waveY = height - (Constants.GROUND_HEIGHT + waveHeight) / 2;
            wave = new Wave(waveX, waveY, waveWidth, waveHeight);

            App.InputManager.AddInputListener(hero);

            float pillsOffsetX = width / 16f;
            float pillsOffsetY = (Application.Instance.Height - Constants.GROUND_HEIGHT) / 16f;

            pillsManager = new PillsWave(hero, pillsOffsetX, 400, width - 2 * pillsOffsetX, 15, 15);
            pillsManager.AddPillListener(new PillParticles());

            // StartTimer();
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

        public void Update(float dt)
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
