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
        public bool day;

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
            day = true;
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

            setDay(day);
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
                    win();
                }
                else if (progress.isTimeUp())
                {
                    loose();
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
        
        public virtual string getLooseMessage()
        {
            return "YOU'VE LOST, SUCKER";
        }

        protected void loose()
        {
            setState(State.LOOSE);
            level.looseLevel();
            onLoose();
        }

        protected void win()
        {
            setState(State.WIN);
            level.winLevel();
            onWin();                    
        }

        public void setDay(bool day)
        {
            Env.getIntance().day = day;
        }        
    }
}
