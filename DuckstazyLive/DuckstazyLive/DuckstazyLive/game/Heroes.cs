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
    public class Heroes : IEnumerable<Hero>
    {
        private bool started;

        public HeroMedia media;        

        private const int MAX_HEROES = 2;
        private List<Hero> heroes;

        private float jumpStartVelocity;

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

        public void clearHeroes()
        {
            heroes.Clear();
        }

        public void init()
        {
            foreach (Hero hero in heroes)
            {
                hero.init();
            }
        }

        public void clear()
        {
            foreach (Hero hero in heroes)
            {
                hero.clear();
            }
        }

        public void update(float dt, float newPower)
        {
            if (!started) return;

            jumpStartVelocity = Hero.get_jump_start_vel(newPower);

            foreach (Hero hero in heroes)
            {                
                hero.update(dt, newPower);
                hero.gameState.update(dt, newPower);                
            }

            if (heroes.Count > 1)
            {
                Hero hero1 = heroes[0];
                Hero hero2 = heroes[1];

                // check attack                
                if (doHeroAttack(hero1, hero2))
                {
                    hero1.jumpOn(hero2);
                }
                else if (doHeroAttack(hero2, hero1))
                {
                    hero2.jumpOn(hero1);
                }                
            }
        }        

        private bool doHeroAttack(Hero attacker, Hero victim)
        {
            if (!canMakeAttack(attacker, victim))
                return false;

            Vector2 attackeOffset = attacker.pos - attacker.lastPos;
            Vector2 victimOffset = victim.pos - victim.lastPos;

            // use coordinate system where victim is still
            Vector2 p = attacker.lastPos - victim.lastPos;
            Vector2 r = attackeOffset - victimOffset;

            // check against different parts
            Rect[] attackerRects = attacker.getAttackerRects();
            Rect[] victimRects = victim.getVictimRect();

            float t = 1.0f;
            bool hasCollision = false;

            foreach (Rect ar in attackerRects)
            {
                float ax = p.X + ar.X;
                float ay = p.Y + ar.Y;
                float aw = ar.Width;
                float ah = ar.Height;

                LineSegment s1 = new LineSegment(ax, ay, r);
                LineSegment s2 = new LineSegment(ax + aw, ay, r);
                LineSegment s3 = new LineSegment(ax + aw, ay + ah, r);
                LineSegment s4 = new LineSegment(ax, ay + ah, r);

                foreach (Rect vr in victimRects)
                {
                    float vicX = vr.X;
                    float vicY = vr.Y;
                    float vw = vr.Width;
                    float vh = vr.Height;

                    float ct;
                    if (s1.collidesRect(vicX, vicY, vw, vh, out ct))
                    {
                        hasCollision = true;
                        if (ct < t)
                            t = ct;
                    }
                    if (s2.collidesRect(vicX, vicY, vw, vh, out ct))
                    {
                        hasCollision = true;
                        if (ct < t)
                            t = ct;
                    }
                    if (s3.collidesRect(vicX, vicY, vw, vh, out ct))
                    {
                        hasCollision = true;
                        if (ct < t)
                            t = ct;
                    }
                    if (s4.collidesRect(vicX, vicY, vw, vh, out ct))
                    {
                        hasCollision = true;
                        if (ct < t)
                            t = ct;
                    }
                }
            }

            if (hasCollision)
            {
                // move them out of collision
                attacker.pos = attacker.lastPos + t * attackeOffset;
                victim.pos = victim.lastPos + t * victimOffset;
            }

            return hasCollision;
        }

        private bool canMakeAttack(Hero attacker, Hero victim)
        {
            return victim.canBeJumped() // victim can be jumped
                && attacker.lastPos.Y < victim.lastPos.Y// attacker was higher than victim
                && attacker.lastPos.Y <= attacker.pos.Y; // attacker is flying horizontaly or drops down
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

        public bool buttonPressed(ref ButtonEvent e)
        {
            return buttonPressed(ref e, e.playerIndex);
        }

        public bool buttonReleased(ref ButtonEvent e)
        {
            return buttonReleased(ref e, e.playerIndex);
        }

        private bool buttonPressed(ref ButtonEvent e, int playerIndex)
        {
            Debug.Assert(playerIndex >= 0 && playerIndex < getHeroesCount());
            Hero hero = heroes[playerIndex];
            if (!hero.isDead())            
                return hero.buttonPressed(ref e);
            return false;
        }

        private bool buttonReleased(ref ButtonEvent e, int playerIndex)
        {
            Debug.Assert(playerIndex >= 0 && playerIndex < getHeroesCount());
            Hero hero = heroes[playerIndex];
            if (!hero.isDead())
                return hero.buttonReleased(ref e);
            return false;
        }

        public void buttonsReset()
        {
            foreach (Hero hero in heroes)
            {
                hero.buttonsReset();
            }
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
            if (!heroes[0].isDead())
            {
                heroes[0].start(x1);
                heroes[0].flip = true;
            }

            if (!heroes[1].isDead())
            {
                float x2 = 640 - (x1 + Hero.duck_w2);
                heroes[1].start(x2);
                heroes[1].flip = false;
            }
        }

        public bool hasAsleepHero()
        {
            foreach (Hero h in heroes)
            {
                if (h.isSleep())
                    return true;
            }
            return false;
        }

        public bool hasAliveHero()
        {
            foreach (Hero h in heroes)
            {
                if (!h.isDead())
                    return true;
            }
            return false;
        }

        public float getJumpHeight()
        {
            return jumpStartVelocity * jumpStartVelocity * 0.5f / Hero.duck_jump_gravity;
        }

        public int getHeroesCount()
        {
            return heroes.Count;
        }

        public IEnumerator<Hero> GetEnumerator()
        {
            foreach (Hero h in heroes)
            {
                yield return h;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            foreach (Hero h in heroes)
            {
                yield return h;
            }
        }
    }
}
