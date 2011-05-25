using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuckstazyLive.app.game.level
{
    public class LevelProgress
    {
        private float progress;
        private float goalProgress;
        private float goalTime;
        private float elapsedTime;

        public void start(float goalProgress, float goalTime)
        {
            this.goalProgress = goalProgress;
            this.goalTime = goalTime;

            elapsedTime = 0.0f;
            progress = 0.0f;
        }

        public void Update(float dt)
        {
            elapsedTime += dt;
            if (hasTimeLimit() && elapsedTime > getGoalTime())
                elapsedTime = goalTime;
        }

        public void updateProgress(float newProgress)
        {
            if (isPlaying())
            {
                progress = newProgress;
                if (progress >= goalProgress)
                {
                    progress = goalProgress;
                }
            }
        }

        public float getCompletePercent()
        {
            if (goalProgress == 0)
                return 0;

            return progress / goalProgress;
        }

        public float getCurrentProgress()
        {
            return progress;
        }

        public float getGoalProgress()
        {
            return goalProgress;
        }

        public bool hasGoalProgress()
        {
            return getGoalProgress() > 0;
        }

        public bool isProgressComplete()
        {
            if (hasGoalProgress())
            {
                if (hasTimeLimit() && isTimeUp())
                    return false;
                if (getCurrentProgress() == getGoalProgress())
                    return true;
                return false;
            }

            if (hasTimeLimit() && isTimeUp())
                return true;

            return false;
        }

        public float getElapsedTime()
        {
            return elapsedTime;
        }

        public float getGoalTime()
        {
            return goalTime;
        }

        public bool hasTimeLimit()
        {
            return getGoalTime() > 0;
        }

        public bool isTimeUp()
        {
            if (hasTimeLimit())
                return elapsedTime >= goalTime;

            return false;
        }

        public bool isPlaying()
        {
            return !isTimeUp() && !isProgressComplete();
        }
    }
}
