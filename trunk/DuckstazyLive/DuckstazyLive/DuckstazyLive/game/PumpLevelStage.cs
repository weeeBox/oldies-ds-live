using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuckstazyLive.game
{
    public class PumpLevelStage : LevelStage
    {
        protected float pumpProg; // прогресс накачки 0->1 после power==1
        protected float pumpVel; // скорость накачки

        public PumpLevelStage()
        {            
            pumpVel = 1.0f;            
        }

        public override float getGoalProgress()
        {
            return 2.0f;
        }

        public override void start()
        {
            base.start();
            pumpProg = 0.0f;
        }

        public override void updateProgress(float dt)
        {            
            level.progress.updateProgress(level.power + pumpProg);
            if (level.power >= 1.0f)
            {
                pumpProg += dt * pumpVel;
                if (pumpProg > 1.0f)
                    pumpProg = 1.0f;
            }

            string str = ((int)(level.progress.getCompletePercent() * 100)).ToString() + "%";
            if (level.infoText != str) level.infoText = str;            
        }
    }
}
