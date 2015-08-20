using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuckstazyLive.game
{
    public class PumpLevelStage : StoryLevelStage
    {
        protected float pumpProg; // прогресс накачки 0->1 после power==1
        protected float pumpVel; // скорость накачки

        public PumpLevelStage()
        {            
            pumpVel = 1.0f;            
        }

        protected override void startProgress()
        {
            progress.start(2.0f, 0);
        }

        public override void onStart()
        {
            base.onStart();
            pumpProg = 0.0f;
        }

        protected override void updateProgress(float dt)
        {
            base.updateProgress(dt);

            progress.updateProgress(level.power + pumpProg);
            if (level.power >= 1.0f)
            {
                pumpProg += dt * pumpVel;
                if (pumpProg > 1.0f)
                    pumpProg = 1.0f;
            }

            string str = ((int)(progress.getCompletePercent() * 100)).ToString() + "%";
            setInfoText(str);
        }
    }
}
