﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuckstazyLive.game
{
    public class PillCollectLevelStage : LevelStage
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

            progress.updateProgress(collected);
            string str = collected.ToString() + "/" + ((int)progress.getGoalProgress()).ToString();
            setInfoText(str);
        }        
    }
}