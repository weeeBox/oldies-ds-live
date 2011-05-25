using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using asap.util;
using asap.graphics;
using DuckstazyLive.app.game.env;
using System.Diagnostics;

namespace DuckstazyLive.app.game.stage
{
    public abstract class LevelStage
    {
        public bool day;

        protected float startX;
        protected bool heroStarted;

        public StageMedia media;

        public LevelStage()
        {
            day = true;
        }

        public virtual void onStart()
        {
            startX = RandomHelper.rnd() * (960 - 81);
            heroStarted = false;

            setDay(day);
        }

        public virtual void stop()
        {
            GC.Collect();
        }

        public virtual void draw1(Graphics g)
        {
        }

        public virtual void draw2(Graphics g)
        {
        }

        public virtual void drawUI(Graphics g)
        {
        }

        public virtual void Update(float dt)
        {
            if (!heroStarted)
            {
                heroStarted = true;
                Heroes heroes = getHeroes();
                if (heroes.getHeroesCount() > 1)
                {
                    heroes.startHeroes();
                }
                else
                {
                    heroes.start(startX);
                }
            }
        }

        public void setDay(bool day)
        {
            GameElements.Env.day = day;
        }

        protected abstract void updateProgress(float dt);

        public abstract void collectPill(Hero hero, Pill pill);
        public abstract bool isPlaying();

        protected Hero getHero(int heroIndex)
        {
            Debug.Assert(heroIndex >= 0 && heroIndex < getHeroes().getHeroesCount());
            return getHeroes()[heroIndex];
        }

        protected Heroes getHeroes()
        {
            return GameElements.Heroes;
        }

        protected Pills getPills()
        {
            return GameElements.Pills;
        }

        protected Particles getParticles()
        {
            return GameElements.Particles;
        }

        protected Env getEnv()
        {
            return GameElements.Env;
        }
    }
}
