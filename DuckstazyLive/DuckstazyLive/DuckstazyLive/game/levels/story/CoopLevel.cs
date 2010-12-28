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
using System.Diagnostics;

namespace DuckstazyLive.game
{
    public class CoopLevel : StoryLevel
    {        
        private enum LevelStages
        {
            Harvesting,
            PartyTime,
            Bubbles,
            DoubleFrog,
            PartyTime2,
            BetweenCatsStage,
            Bubbles2,
            AirAttack,
            PartyTime3,
            Trains,
            Bubbles3,
            DuckStage
        }

        private int stageIndex;
        private int stagesCount;

        public CoopLevel(GameState gameState) : base(gameState)
        {
            stagesCount = Enum.GetNames(typeof(LevelStages)).Length;
        }

        protected override LevelStage createStage(int stageIndex)
        {
            LevelStages stage = (LevelStages)stageIndex;
            switch (stage)
            {
                case LevelStages.Harvesting:
                    return new Harvesting();
                case LevelStages.PartyTime:
                    {
                        PartyTime partyTime = new PartyTime(30, 0);
                        partyTime.day = false;
                        return partyTime;
                    }
                case LevelStages.Bubbles:
                    return new Bubbles(0.05f, 0);
                case LevelStages.DoubleFrog:
                    return new DoubleFrog();
                case LevelStages.PartyTime2:
                    return new PartyTime(60, 1);
                case LevelStages.BetweenCatsStage:
                    return new BetweenCatsStage();
                case LevelStages.Bubbles2:
                    return new Bubbles(0.04f, 1);
                case LevelStages.AirAttack:
                    return new AirAttack();
                case LevelStages.PartyTime3:
                    return new PartyTime(120, 2);
                case LevelStages.Trains:
                    return new Trains();
                case LevelStages.Bubbles3:
                    return new Bubbles(0.03f, 2);
                case LevelStages.DuckStage:
                    return new FigureStage();

                default:
                    Debug.Assert(false, "Bad stage: " + stage);
                    break;
            }
            return null;
        }

        protected override int getStagesCount()
        {
            return stagesCount;
        }

        protected override LevelStage createNextStage()
        {            
            stageIndex++;
            return createStage(stageIndex);
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

        public override void drawHud(Canvas canvas)
        {
            base.drawHud(canvas);            
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
