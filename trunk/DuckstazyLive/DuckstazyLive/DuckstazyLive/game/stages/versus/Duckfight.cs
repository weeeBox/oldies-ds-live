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

        public override void onStart()
        {
            base.onStart();

            float x1 = 0.25f * 640;            
            float x2 = 640 - (x1 + Hero.duck_w2);
            Heroes heroes = getHeroes();

            heroes[0].gameState.addPills(50);
            heroes[1].gameState.addPills(50);
            level.info.add(x1, 360, 50, 0);            
            level.info.add(x2, 360, 50, 1);
        }

        
    }
}
