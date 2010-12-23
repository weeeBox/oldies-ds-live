using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuckstazyLive.game
{
    public class BonusLevelStage : LevelStage
    {
        protected float goalTime;

        public BonusLevelStage(float goalTime)
        {
            this.goalTime = goalTime;
        }

        public override void updateProgress(float dt)
        {
            float t = level.progress.getGoalTime() - level.progress.getElapsedTime();
            int i = (int)(t / 60);
            string str;
            if (i < 10) str = "0" + i.ToString() + ":";
            else str = i.ToString() + ":";
            i = ((int)t) % 60;
            if (i < 10) str += "0" + i.ToString();
            else str += i.ToString();

            if (level.infoText != str) level.infoText = str;
        }

        public override float getGoalTime()
        {
            return goalTime;
        }
    }
}
