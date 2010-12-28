using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckstazyLive.game.levels;

namespace DuckstazyLive.game
{
    public class VersusLevel : Level
    {
        public VersusLevel(GameState gameState) : base(gameState)
        {
        }

        protected override LevelStage createStage(int stageIndex)
        {
            throw new NotImplementedException();
        }

        protected override LevelStage createNextStage()
        {
            throw new NotImplementedException();
        }

        protected override int getStagesCount()
        {
            throw new NotImplementedException();
        }
    }
}
