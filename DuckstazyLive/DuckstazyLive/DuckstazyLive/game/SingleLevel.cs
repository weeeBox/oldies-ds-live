using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckstazyLive.game.levels;
using DuckstazyLive.app;
using Framework.visual;
using Framework.core;

namespace DuckstazyLive.game
{
    public class SingleLevel : Level
    {
        public SingleLevel(GameState gameState) : base(gameState)
        {            
        }

        protected override List<LevelStages> createStages()
        {
            List<LevelStages> stages = new List<LevelStages>();

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
            stages.Add(LevelStages.DuckStage);

            return stages;
        }
    }
}
