using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using DuckstazyLive.app;
using System.Diagnostics;
using Framework.visual;
using Framework.core;
using DuckstazyLive.game.levels;

namespace DuckstazyLive.game
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

        public virtual void start()
        {
            startX = utils.rnd() * (640 - 54);
            heroStarted = false;           

            setDay(day);            
        }

        public virtual void stop()
        {
            GC.Collect();
        }

        public virtual void draw1(Canvas canvas)
        {
        }

        public virtual void draw2(Canvas canvas)
        {            
        }

        public virtual void drawUI(Canvas canvas)
        {            
        }

        public virtual void update(float dt)
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
            Env.getIntance().day = day;        
        }

        protected abstract void updateProgress(float dt);

        public abstract void collectPill(Hero hero, Pill pill);
        public abstract bool isPlaying();

        protected GameElements getGameMgr()
        {
            return GameElements.getInstance();
        }

        protected Hero getHero(int heroIndex)
        {
            Debug.Assert(heroIndex >= 0 && heroIndex < getHeroes().getHeroesCount());
            return getHeroes()[heroIndex];
        }

        protected Heroes getHeroes()
        {
            return getGameMgr().getHeroes();
        }

        protected Pills getPills()
        {
            return getGameMgr().getPills();
        }

        protected Particles getParticles()
        {
            return getGameMgr().getParticles();
        }

        protected Env getEnv()
        {
            return getGameMgr().getEnv();
        }
    }
}
