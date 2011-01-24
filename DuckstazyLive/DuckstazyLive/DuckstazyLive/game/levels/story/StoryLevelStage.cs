using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.core;
using System.Diagnostics;

namespace DuckstazyLive.game
{
    public abstract class StoryLevelStage : LevelStage
    {
        protected enum State
        {
            PLAYING, // играем
            WIN, // выполнили goal
            LOOSE // не выполнили goal
        }

        private State state;        
        private int totalCollected;

        // уровень
        protected StoryLevel level;
        protected LevelProgress progress;        

        public StoryLevelStage()
        {
            progress = createLevelProgress();

            level = (StoryLevel) StoryLevel.instance;
            media = level.stageMedia;            
            day = true;            
        }

        protected virtual LevelProgress createLevelProgress()
        {
            return new LevelProgress();
        }

        protected abstract void startProgress();        

        public override void start()
        {            
            totalCollected = 0;

            base.start();
            startProgress();
        }

        public virtual void onWin()
        {
        }

        public virtual void onLoose()
        {
        }        

        public override void update(float dt)
        {
            base.update(dt);

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

        protected override void updateProgress(float dt)
        {
            progress.update(dt);
        }

        protected void setState(State state)
        {
            this.state = state;
        }

        public override bool isPlaying()
        {
            return state == State.PLAYING;
        }

        public bool isWin()
        {
            return state == State.WIN;
        }

        public bool isLoose()
        {
            return state == State.LOOSE;
        }
        
        public virtual string getLooseMessage()
        {
            return "YOU'VE LOST, SUCKER";
        }

        protected void loose()
        {
            setState(State.LOOSE);
            level.onLoose();
            onLoose();
            stop();
        }

        protected void win()
        {
            setState(State.WIN);
            level.onWin();
            onWin();
            stop();
        }        

        protected bool isSingleLevel()
        {
            return level.isSingleLevel();
        }

        protected void setInfoText(String text)
        {
            level.infoText = text;
        }        

        public bool hasTimeLimit()
        {
            return progress.hasTimeLimit();
        }

        public bool hasGoal()
        {
            return progress.hasGoalProgress();
        }

        public float getRemainingTime()
        {
            return progress.getGoalTime() - progress.getElapsedTime();
        }

        public override void collectPill(Hero hero, Pill pill)
        {
            int heroIndex = hero.getPlayerIndex();
            hero.queuePillsToAdd(pill.scores);
            totalCollected += pill.scores;
        }

        public int getCollectedPills()
        {
            return totalCollected;
        }
    }
}
