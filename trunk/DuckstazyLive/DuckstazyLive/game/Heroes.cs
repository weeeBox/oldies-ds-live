﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.core;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using DuckstazyLive.app;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using Framework.utils;

namespace DuckstazyLive.game
{
    public class Heroes
    {        
        private bool started;    

        public HeroMedia media;        
        public Particles particles;
        public Env env;

        private const int MAX_HEROES = 2;
        private List<Hero> heroes;        

        public Heroes()
        {
            media = new HeroMedia();
            heroes = new List<Hero>(MAX_HEROES);
        }

        public Hero this [int index]
        {
            get
            {
                Debug.Assert(index >= 0 && index < heroes.Count);
                return heroes[index];
            }

            set
            {
                Debug.Assert(index >= 0 && index < heroes.Count);
                heroes[index] = value;
            }
        }

        public void addHero(Hero hero)
        {
            Debug.Assert(heroes.Count < MAX_HEROES);
            heroes.Add(hero);
        }

        public void removeHero(Hero hero)
        {
            heroes.Remove(hero);
        }

        public void init()
        {
            foreach (Hero hero in heroes)
            {
                hero.init();
            }
        }        

        public void update(float dt, float newPower)
        {
            if (!started) return;

            foreach (Hero hero in heroes)
            {
                hero.update(dt, newPower);
            }
        }

        public void draw(Canvas canvas)
        {
            if (started)
            {
                foreach (Hero hero in heroes)
                {
                    hero.draw(canvas);
                }
            }
        }

        public void buttonPressed(ref ButtonEvent e)
        {
            buttonPressed(ref e, e.playerIndex);
        }

        public void buttonReleased(ref ButtonEvent e)
        {
            buttonReleased(ref e, e.playerIndex);
        }

        private void buttonPressed(ref ButtonEvent e, int playerIndex)
        {
            Debug.Assert(playerIndex >= 0 && playerIndex < heroes.Count);
            heroes[playerIndex].buttonPressed(ref e);
        }

        private void buttonReleased(ref ButtonEvent e, int playerIndex)
        {
            Debug.Assert(playerIndex >= 0 && playerIndex < heroes.Count);
            heroes[playerIndex].buttonReleased(ref e);
        }                

        public void start(float _x)
        {
            started = true;
            foreach (Hero hero in heroes)
            {
                hero.start(_x);
            }
        }        
    }
}
