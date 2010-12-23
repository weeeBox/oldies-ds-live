using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuckstazyLive.game
{
    public class DucktripLevelStage : LevelStage
    {
        private int numPills;

        public DucktripLevelStage(int numPills)
        {
            this.numPills = numPills;
        }

        public override void updateProgress(float dt)
        {
            level.progress.updateProgress(collected);
            string str = collected.ToString() + "/" + ((int)getGoalProgress()).ToString();
            if (level.infoText != str) level.infoText = str;
        }

        public override float getGoalProgress()
        {
            return numPills;
        }
    }
}