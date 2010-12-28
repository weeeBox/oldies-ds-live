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

        public override void drawHud(Canvas canvas)
        {
            throw new NotImplementedException();
        }

        protected override void initHero()
        {
            throw new NotImplementedException();
        }

        protected override LevelStage createStage(int stageIndex)
        {
            throw new NotImplementedException();
        }        
    }
}
