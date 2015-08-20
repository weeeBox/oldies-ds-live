using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuckstazyLive.game
{
    public class PillCollectLevelStage : StoryLevelStage
    {
        protected int numPills;
        protected float timeout;

        public PillCollectLevelStage(int numPills) : this(numPills, 0.0f)
        {

        }

        public PillCollectLevelStage(int numPills, float timeout)
        {
            this.numPills = numPills;
            this.timeout = timeout;
        }

        protected override void startProgress()
        {
            progress.start(numPills, timeout);
        }

        protected override void updateProgress(float dt)
        {
            base.updateProgress(dt);

            progress.updateProgress(getCollectedPills());
            string str = getCollectedPills() + "/" + ((int)progress.getGoalProgress()).ToString();
            setInfoText(str);
        }        
    }
}