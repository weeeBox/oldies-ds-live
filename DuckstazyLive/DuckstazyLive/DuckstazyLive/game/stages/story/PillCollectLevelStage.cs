using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuckstazyLive.game
{
    public class PillCollectLevelStage : StoryLevelStage
    {
        protected int numPills;

        public PillCollectLevelStage(int numPills)
        {
            this.numPills = numPills;
        }

        protected override void startProgress()
        {
            progress.start(numPills, 0);
        }

        public override void updateProgress(float dt)
        {
            base.updateProgress(dt);

            progress.updateProgress(getCollectedPills());
            string str = getCollectedPills() + "/" + ((int)progress.getGoalProgress()).ToString();
            setInfoText(str);
        }        
    }
}