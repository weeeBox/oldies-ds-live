using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckstazyLive.app.game.stage;
using System.Diagnostics;
using DuckstazyLive.game.stages.story;
using DuckstazyLive.game.levels;

namespace DuckstazyLive.app.game.level
{
    public class CoopLevelHud : StoryLevelHud
    {
        public CoopLevelHud(Level level)
            : base(level)
        {

        }

        protected override HealthBar[] createBars()
        {
            //HealthBar bar1 = new HealthBar(Res.IMG_UI_HEALTH_EMO_BASE);
            //bar1.setAlign(ALIGN_MIN, ALIGN_CENTER);
            //bar1.parentAlignX = ALIGN_MIN;
            //bar1.parentAlignY = ALIGN_CENTER;

            //HealthBar bar2 = new HealthBar(Res.IMG_UI_HEALTH_EMO_BASE2);
            //bar2.setAlign(ALIGN_MAX, ALIGN_CENTER);
            //bar2.parentAlignX = ALIGN_MAX;
            //bar2.parentAlignY = ALIGN_CENTER;

            //return new HealthBar[] { bar1, bar2 };
            return new HealthBar[] { };
        }
    }

    public class CoopLevel : StoryLevel
    {
        private enum LevelStages
        {
            Fireworks,
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
            DuckStage,
            LevelsCount
        }

        private int stagesCount;

        public CoopLevel(StoryGame storyController, float width, float height)
            : base(storyController, width, height)
        {
            GameElements.initHeroes(2);
            stagesCount = (int)LevelStages.LevelsCount;
        }

        public override bool isSingleLevel()
        {
            return false;
        }

        protected override LevelStage createStage(int stageIndex)
        {
            LevelStages stage = (LevelStages)stageIndex;
            switch (stage)
            {
                case LevelStages.Fireworks:
                    return new Fireworks();
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

        protected override Hud createHud()
        {
            return new CoopLevelHud(this);
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
    }
}
