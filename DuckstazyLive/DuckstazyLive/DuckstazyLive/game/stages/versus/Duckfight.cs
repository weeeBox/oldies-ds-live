using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuckstazyLive.game.stages.versus
{
    public class Duckfight : VersusLevelStage
    {
        public Duckfight(VersusLevel level) : base(level, 60)
        {

        }

        public override void start()
        {
            base.start();

            getHero(0).queuePillsToAdd(50);
            getHero(1).queuePillsToAdd(50);
        }
    }
}
