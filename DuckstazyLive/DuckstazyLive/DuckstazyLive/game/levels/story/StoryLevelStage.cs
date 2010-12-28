using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        // уровень
        protected StoryLevel level;
        protected LevelProgress progress;

        public StoryLevelStage()
        {
            progress = createLevelProgress();

            level = (StoryLevel) StoryLevel.instance;
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

        public override void start()
        {
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
        }

        protected void win()
        {
            setState(State.WIN);
            level.onWin();
            onWin();                    
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
    }
}
