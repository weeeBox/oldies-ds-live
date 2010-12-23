using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using DuckstazyLive.app;

namespace DuckstazyLive.game
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

        public void update(float dt)
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
            if (hasTimeLimit() && isTimeUp())
                return true;

            if (hasGoalProgress() && getCurrentProgress() == getGoalProgress())
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
