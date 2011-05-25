using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuckstazyLive.game.stages.versus
{
    public class VersusProgress
    {
        private float levelTime;
        private float elapsedTime;

        public VersusProgress(float levelTime)
        {
            this.levelTime = levelTime;
        }

        public void start()
        {
            elapsedTime = 0;
        }

        public void Update(float dt)
        {
            if (elapsedTime < levelTime)
            {
                elapsedTime += dt;
                if (elapsedTime > levelTime)
                {
                    elapsedTime = levelTime;
                }
            }            
        }

        public bool isTimeUp()
        {
            return elapsedTime == levelTime;
        }

        public float getLevelTime()
        {
            return levelTime;
        }

        public float getElapsedTime()
        {
            return elapsedTime;
        }
    }
}
