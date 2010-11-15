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
    public class Hero
    {        
        private bool started;    

        public HeroMedia media;        
        public Particles particles;
        public Env env;

        private const int MAX_HEROES = 2;
        private List<HeroInstance> heroes;        

        public Hero()
        {
            media = new HeroMedia();
            heroes = new List<HeroInstance>(MAX_HEROES);
        }

        public HeroInstance this [int index]
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

        public void addHero(HeroInstance hero)
        {
            Debug.Assert(heroes.Count < MAX_HEROES);
            heroes.Add(hero);
        }

        public void removeHero(HeroInstance hero)
        {
            heroes.Remove(hero);
        }

        public void init()
        {
            foreach (HeroInstance hero in heroes)
            {
                hero.init();
            }
        }        

        public void update(float dt, float newPower)
        {
            if (!started) return;

            foreach (HeroInstance hero in heroes)
            {
                hero.update(dt, newPower);
            }
        }

        public void draw(Canvas canvas)
        {
            if (started)
            {
                foreach (HeroInstance hero in heroes)
                {
                    hero.draw(canvas);
                }
            }
        }        

        public void keyDown(Keys keyCode)
        {
            foreach (HeroInstance hero in heroes)
            {
                if (hero.keyDown(keyCode))
                    break;
            }
        }

        public void keyUp(Keys keyCode)
        {
            foreach (HeroInstance hero in heroes)
            {
                if (hero.keyUp(keyCode))
                    break;
            }
        }        

        public void start(float _x)
        {
            started = true;
            foreach (HeroInstance hero in heroes)
            {
                hero.start(_x);
            }
        }        
    }
}
