using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuckstazyLive.game.stages.versus
{
    public class Duckfight : VersusLevelStage
    {
        private bool fightStarted;

        public Duckfight(VersusLevel level) : base(level, 60)
        {

        }

        public override void onStart()
        {
            base.onStart();

            fightStarted = false;

            getPills().findDead().startMatrix(320, 320);
            getPills().actives++;
        }

        public override void update(float dt)
        {
            base.update(dt);

            if (level.power >= 0.5f && !fightStarted)
            {
                getEnv().startBlanc();

                fightStarted = true;
                Hero h1 = getHero(0);
                Hero h2 = getHero(1);
                addStartScores(h1);
                addStartScores(h2);
            }
        }

        private void addStartScores(Hero hero)
        {            
            hero.info.add(50);
            hero.gameState.addPills(50);
        }
    }
}
