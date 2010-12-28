using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckstazyLive.game.levels;
using DuckstazyLive.app;
using Framework.visual;
using Framework.core;
using System.Diagnostics;

namespace DuckstazyLive.game
{
    public class SingleLevel : Level
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
            DuckStage,            
        }

        private int stageIndex;
        private int stagesCount;

        public SingleLevel(GameState gameState) : base(gameState)
        {
            stagesCount = Enum.GetNames(typeof(LevelStages)).Length;
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

                default:
                    Debug.Assert(false, "Bad stage: " + stage);
                    break;
            }
            return null;
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
