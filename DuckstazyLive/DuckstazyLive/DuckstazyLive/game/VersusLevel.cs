using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckstazyLive.game.levels;
using Microsoft.Xna.Framework;

namespace DuckstazyLive.game
{
    public class VersusLevel : Level
    {
        public VersusLevel() : base(new GameState())
        {
        }

        public override void drawHud(Canvas canvas)
        {
            
        }

        protected override void initHero()
        {
            heroes = new Heroes();
            Hero hero = new Hero(heroes, 0);
            hero.gameState.leftOriented = true;
            hero.gameState.color = Color.Yellow;
            heroes.addHero(hero);

            hero = new Hero(heroes, 1);
            hero.gameState.leftOriented = false;
            hero.gameState.color = Color.Pink;
            heroes.addHero(hero);

            pills = new Pills(heroes, ps, this);
            heroes.particles = ps;
            heroes.env = env;
            heroes.clear();
        }

        protected override LevelStage createStage(int stageIndex)
        {
            throw new NotImplementedException();
        }        
    }
}
