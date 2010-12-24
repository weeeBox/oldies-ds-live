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
        protected Env env;        
        
        public int collected;

        protected float startX;
        protected bool heroStarted;

        public StageMedia media;        

        protected bool end;
        protected int endImg;
        protected float endCounter;        

        public LevelStage()
        {
            progress = createLevelProgress();

            level = Level.instance;
            media = level.stageMedia;
            pills = level.pills;
            particles = level.pills.ps;
            heroes = level.heroes;            
            env = level.env;            
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

            end = false;

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
                    this.onWin();
                    end = true;
                    endImg = media.imgStageEnd;
                    endCounter = 0.0f;
                }
                else if (!end && !heroes.hasAliveHero())
                {
                    level.infoText = "";
                    end = true;
                    endImg = media.imgTheEnd;
                    endCounter = 0.0f;
                }
            }

            if (end)
                endCounter += dt;            
        }

        public virtual void updateProgress(float dt)
        {
            progress.update(dt);
        }        
    }
}
