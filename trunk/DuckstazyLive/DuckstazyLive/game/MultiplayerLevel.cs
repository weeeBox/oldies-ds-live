using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckstazyLive.game.levels;

namespace DuckstazyLive.game
{
    public class MultiplayerLevel : Level
    {
        public MultiplayerLevel(GameState gameState) : base(gameState)
        {
            stages.Add(LevelStages.Harvesting);
            stages.Add(LevelStages.PartyTime);
            stages.Add(LevelStages.Bubbles);
            stages.Add(LevelStages.DoubleFrog);
            stages.Add(LevelStages.PartyTime2);
            stages.Add(LevelStages.BetweenCatsStage);
            stages.Add(LevelStages.Bubbles2);
            stages.Add(LevelStages.AirAttack);
            stages.Add(LevelStages.PartyTime3);
            stages.Add(LevelStages.Trains);
            stages.Add(LevelStages.Bubbles3);
            stagesCount = stages.Count;
        }

        protected override void initHero()
        {
            heroes = new Heroes();
            Hero heroInstance = new Hero(heroes, 0);
            heroInstance.state = state;
            heroes.addHero(heroInstance);

            heroInstance = new Hero(heroes, 1);
            heroInstance.state = state;
            heroes.addHero(heroInstance);

            pills = new Pills(heroes, ps, this);
            heroes.particles = ps;
            heroes.env = env;
            heroes.init();
        }
    }
}
