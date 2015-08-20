using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuckstazyLive.game
{
    public class TimeoutLevelStage : StoryLevelStage
    {
        protected float goalTime;

        public TimeoutLevelStage(float goalTime)
        {
            this.goalTime = goalTime;
        }

        protected override void startProgress()
        {
            progress.start(0, goalTime);
        }        
    }
}
