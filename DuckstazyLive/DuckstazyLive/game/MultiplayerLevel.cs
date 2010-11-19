using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckstazyLive.game.levels;
using DuckstazyLive.app;
using Framework.visual;
using Framework.core;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

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
            Hero hero = new Hero(heroes, 0);
            hero.gameState.leftOriented = true;
            hero.gameState.fontColor = Color.Yellow;
            heroes.addHero(hero);

            hero = new Hero(heroes, 1);
            hero.gameState.leftOriented = false;
            hero.gameState.fontColor = Color.Pink;
            heroes.addHero(hero);

            pills = new Pills(heroes, ps, this);
            heroes.particles = ps;
            heroes.env = env;
            heroes.clear();
        }

        public override void drawUI(Canvas canvas)
        {
            heroes[0].gameState.draw(canvas, Constants.TITLE_SAFE_LEFT_X, Constants.TITLE_SAFE_TOP_Y);
            heroes[1].gameState.draw(canvas, Constants.TITLE_SAFE_RIGHT_X, Constants.TITLE_SAFE_TOP_Y);

            //mat.identity();

            //mat.translate(40.0f, 410.0f);//445.0f;
            //String str = state.health.ToString() + "/" + state.maxHP.ToString();
            //canvas.draw(Res.FNT_BIG, str, mat);

            //Font font = Application.sharedResourceMgr.getFont(Res.FNT_BIG);
            //mat.translate(600.0f - font.stringWidth(scoreText), 410.0f);
            //canvas.draw(Res.FNT_BIG, scoreText, mat);
        }
    }
}
