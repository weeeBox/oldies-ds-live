using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckstazyLive.game.levels;
using DuckstazyLive.app;
using Framework.visual;
using Framework.core;
using System.Diagnostics;
using DuckstazyLive.game.stages.story;
using DuckstazyLive.game.stages;

namespace DuckstazyLive.game
{
    public class SingleLevelHud : StoryLevelHud
    {
        public SingleLevelHud(Level level) : base(level)
        {            
            infoText.setAlign(ALIGN_CENTER, ALIGN_MIN);
            infoText.parentAlignX = ALIGN_CENTER;
        }        

        protected override HealthBar[] createBars()
        {            
            HealthBar bar = new HealthBar(Res.IMG_UI_HEALTH_EMO_BASE);
            return new HealthBar[] { bar };
        }
    }

    public class SingleLevel : StoryLevel
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
            Snakes,
            Grinder,   
            LevelsCount
        }

        private int stageIndex;
        private int stagesCount;

        public SingleLevel(StoryController storyController) : base(storyController)
        {
            GameElements.initHeroes(1);
            GameElements.Heroes[0].gameState.leftOriented = true;
            stagesCount = (int)LevelStages.LevelsCount;
        }

        public override bool isSingleLevel()
        {
            return true;
        }

        protected override LevelStage createStage(int stageIndex)
        {
            Debug.Assert(stageIndex >= 0 && stageIndex < getStagesCount());

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
                case LevelStages.Snakes:
                    return new Snakes();
                case LevelStages.Grinder:
                    return new GrinderStage();
                case LevelStages.Fireworks:
                    return new Fireworks();

                default:
                    Debug.Assert(false, "Bad stage: " + stage);
                    break;
            }
            return null;
        }

        protected override Hud createHud()
        {
            return new SingleLevelHud(this);
        }

        protected override LevelStage createNextStage()
        {            
            stageIndex++;
            return createStage(stageIndex);
        }

        protected override int getStagesCount()
        {
            return stagesCount;
        }        
    }
}
