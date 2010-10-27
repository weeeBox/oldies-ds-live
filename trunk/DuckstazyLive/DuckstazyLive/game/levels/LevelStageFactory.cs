using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace DuckstazyLive.game.levels
{
    public enum LevelStages
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
        Bubbles3
    }

    public class LevelStageFactory
    {
        public static LevelStage createStage(LevelStages stage)
        {
            switch (stage)
            {
                case LevelStages.Harvesting:
                    return new Harvesting();
                case LevelStages.PartyTime:
                    return new PartyTime(30, 0);
                case LevelStages.Bubbles:
                    return new Bubbles(0.05f, 0);
                case LevelStages.DoubleFrog:
                case LevelStages.PartyTime2:
                case LevelStages.BetweenCatsStage:
                case LevelStages.Bubbles2:
                case LevelStages.AirAttack:
                case LevelStages.PartyTime3:
                case LevelStages.Trains:
                case LevelStages.Bubbles3:
                    throw new NotImplementedException();

                default:
                    Debug.Assert(false, "Bad stage: " + stage);
                    break;
            }
            return null;
        }
    }
}
