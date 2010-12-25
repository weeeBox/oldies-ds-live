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
        protected enum State
        {
            PLAYING, // играем
            WIN, // выполнили goal
            LOOSE // не выполнили goal
        }

        private State state;        

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
            state = State.PLAYING;
            collected = 0;

            startX = utils.rnd() * (640 - 54);
            heroStarted = false;           

            startProgress();
        }

        public virtual void onWin()
        {
        }

        public virtual void onLoose()
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

            if (isPlaying())
            {
                updateProgress(dt);                

                if (progress.isProgressComplete())
                {
                    setState(State.WIN);
                    level.winLevel();
                    onWin();                    
                }
                else if (progress.isTimeUp())
                {
                    setState(State.LOOSE);
                    level.looseLevel();
                    onLoose();
                }
            }            
        }

        public virtual void updateProgress(float dt)
        {
            progress.update(dt);
        }

        protected void setState(State state)
        {
            this.state = state;
        }

        public bool isPlaying()
        {
            return state == State.PLAYING;
        }
    }
}
