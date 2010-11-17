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

        public Hero this[int index]
        {
            get
            {
                Debug.Assert(index >= 0 && index < getHeroesCount());
                return heroes[index];
            }

            set
            {
                Debug.Assert(index >= 0 && index < getHeroesCount());
                heroes[index] = value;
            }
        }

        public void addHero(Hero hero)
        {
            Debug.Assert(getHeroesCount() < MAX_HEROES);
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

            if (heroes.Count > 1)
            {
                Hero hero1 = heroes[0];
                Hero hero2 = heroes[1];

                if (heroesIntersects(hero1, hero2))
                {
                    if (hero1.y < hero2.y)
                    {
                        hero1.jumpOn(hero2);
                    }
                    else
                    {
                        hero2.jumpOn(hero1);
                    }
                }
            }
        }

        private bool heroesIntersects(Hero h1, Hero h2)
        {
            float w = Hero.duck_w2;
            float h = Hero.duck_h2;
            if (rectRect(h1.x, h1.y, w, h, h2.x, h2.y, w, h))
            {
                Rect[] r1 = h1.getCollisionRects();
                Rect[] r2 = h2.getCollisionRects();

                for (int i = 0; i < r1.Length; ++i)
                {
                    float rx = h1.x + r1[i].X;
                    float ry = h1.y + r1[i].Y;
                    float rw = r1[i].Width;
                    float rh = r1[i].Height;
                    for (int j = 0; j < r2.Length; ++j)
                    {
                        if (rectRect(rx, ry, rw, rh, h2.x + r2[j].X, h2.y + r2[j].Y, r2[j].Width, r2[j].Height))
                            return true;
                    }
                }
            }
            return false;
        }

        private bool rectRect(ref Rect r1, ref Rect r2)
        {
            return rectRect(r1.X, r1.Y, r1.Width, r1.Height, r2.X, r2.Y, r2.Width, r2.Height);
        }

        private bool rectRect(float x1, float y1, float w1, float h1, float x2, float y2, float w2, float h2)
        {
            return !(x1 + w1 < x2 || x2 + w2 < x1 || y1 + h1 < y2 || y2 + h2 < y1);
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
            Debug.Assert(playerIndex >= 0 && playerIndex < getHeroesCount());
            heroes[playerIndex].buttonPressed(ref e);
        }

        private void buttonReleased(ref ButtonEvent e, int playerIndex)
        {
            Debug.Assert(playerIndex >= 0 && playerIndex < getHeroesCount());
            heroes[playerIndex].buttonReleased(ref e);
        }

        public void start(float _x)
        {
            Debug.Assert(getHeroesCount() == 1);

            started = true;
            heroes[0].start(_x);
        }

        public void startHeroes()
        {
            Debug.Assert(getHeroesCount() == 2);
            started = true;

            float x1 = 0.25f * 640;
            float x2 = 640 - x1;
            heroes[0].start(x1);
            heroes[0].flip = true;

            heroes[1].start(x2);
            heroes[1].flip = false;
        }

        public int getHeroesCount()
        {
            return heroes.Count;
        }
    }
}
