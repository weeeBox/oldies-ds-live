using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using DuckstazyLive.app;
using System.Diagnostics;
using Framework.visual;
using Framework.core;

namespace DuckstazyLive.game
{
    public abstract class LevelStage
    {
        // обратить флаг в true, если прошли уровень.
        public bool win;

        // уровень
        protected Level level;
        protected LevelProgress progress;
        protected Pills pills;
        protected Particles particles;
        
        protected Heroes heroes;        
        
        public int collected;

        protected float startX;
        protected bool heroStarted;

        public StageMedia media;

        public LevelStage()
        {
            progress = createLevelProgress();

            level = Level.instance;
            media = level.stageMedia;
            pills = level.pills;
            particles = level.pills.ps;
            heroes = level.heroes;            
        }

        protected virtual LevelProgress createLevelProgress()
        {
            return new LevelProgress();
        }

        protected abstract void startProgress();        

        public virtual void start()
        {
            win = false;            
            collected = 0;

            startX = utils.rnd() * (640 - 54);
            heroStarted = false;           

            startProgress();
        }

        public virtual void onWin()
        {
        }

        public virtual void draw1(Canvas canvas)
        {

        }

        public virtual void draw2(Canvas canvas)
        {
            
        }

        public virtual void update(float dt)
        {
            if (!heroStarted)
            {
                heroStarted = true;
                if (heroes.getHeroesCount() > 1)
                {
                    heroes.startHeroes();
                }
                else
                {
                    heroes.start(startX);
                }
            }

            if (!win)
            {
                updateProgress(dt);                

                if (progress.isProgressComplete())
                {
                    win = true;
                    level.infoText = "";
                    onWin();                    
                }                
            }            
        }

        public virtual void updateProgress(float dt)
        {
            progress.update(dt);
        }        
    }
}
