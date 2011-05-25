using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckstazyLive.app.game.level;
using asap.core;

namespace DuckstazyLive.app.game.stage
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

            level = (StoryLevel)StoryLevel.instance;
            media = level.stageMedia;
            day = true;
        }

        protected virtual LevelProgress createLevelProgress()
        {
            return new LevelProgress();
        }

        protected abstract void startProgress();

        public override void onStart()
        {
            totalCollected = 0;

            base.onStart();
            startProgress();
        }

        public virtual void onWin()
        {
        }

        public virtual void onLoose()
        {
        }

        public override void Update(float dt)
        {
            base.Update(dt);

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
            progress.Update(dt);
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
            level.setInfoText(text);
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

        public virtual bool KeyPressed(KeyEvent e)
        {
            return false;
        }

        public virtual bool KeyReleased(KeyEvent e)
        {
            return false;
        }

        public override void collectPill(Hero hero, Pill pill)
        {
            int heroIndex = hero.getPlayerIndex();
            hero.gameState.addPills(pill.scores);
            totalCollected += pill.scores;
        }

        public int getCollectedPills()
        {
            return totalCollected;
        }
    }
}
