using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckstazyLive.game.levels;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using DuckstazyLive.game.stages.versus;
using DuckstazyLive.app;
using Framework.visual;
using Framework.core;

namespace DuckstazyLive.game
{
    public class VersusLevel : Level
    {
        private enum VersusStages
        {
            DoubleFrog,
            AirAttack
        }

        private int stagesCount;

        public VersusLevel() : base(new GameState())
        {
            stagesCount = Enum.GetNames(typeof(VersusStages)).Length;
        }

        public override void drawHud(Canvas canvas)
        {
            heroes[0].gameState.draw(canvas, Constants.TITLE_SAFE_LEFT_X, Constants.TITLE_SAFE_TOP_Y);
            heroes[1].gameState.draw(canvas, Constants.TITLE_SAFE_RIGHT_X, Constants.TITLE_SAFE_TOP_Y);

            Font font = Application.sharedResourceMgr.getFont(Res.FNT_BIG);

            font.drawString(getStage().getPillCollected(0).ToString(), Constants.TITLE_SAFE_LEFT_X, Constants.TITLE_SAFE_TOP_Y + 20);
            font.drawString(getStage().getPillCollected(1).ToString(), Constants.TITLE_SAFE_RIGHT_X, Constants.TITLE_SAFE_TOP_Y + 20, TextAlign.TOP | TextAlign.RIGHT);

            float infoX = 0.5f * (Constants.TITLE_SAFE_RIGHT_X + Constants.TITLE_SAFE_LEFT_X);
            float infoY = Constants.TITLE_SAFE_TOP_Y;
            bool hasInfoText = infoText != null;
            
            float t = getStage().getRemainingTime();
            int i = (int)(t / 60);
            string timeStr;
            if (i < 10) timeStr = "0" + i.ToString() + ":";
            else timeStr = i.ToString() + ":";
            i = ((int)t) % 60;
            if (i < 10) timeStr += "0" + i.ToString();
            else timeStr += i.ToString();

            float timeX;
            float timeY = infoY;
            if (hasInfoText)
            {
                infoX = 0.4f * (Constants.TITLE_SAFE_RIGHT_X + Constants.TITLE_SAFE_LEFT_X);
                timeX = 0.6f * (Constants.TITLE_SAFE_RIGHT_X + Constants.TITLE_SAFE_LEFT_X);
            }
            else
            {
                timeX = 0.5f * (Constants.TITLE_SAFE_RIGHT_X + Constants.TITLE_SAFE_LEFT_X);
            }

            font.drawString(timeStr, timeX, timeY, TextAlign.HCENTER | TextAlign.VCENTER);
            
            if (hasInfoText)
            {
                font.drawString(infoText, infoX, infoY, TextAlign.HCENTER | TextAlign.VCENTER);
            }
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
            Debug.Assert(stageIndex >= 0 && stageIndex < stagesCount);
            VersusStages stage = (VersusStages)stageIndex;

            switch (stage)
            {
                case VersusStages.DoubleFrog:
                    return new DoubleFrogVs(this);

                case VersusStages.AirAttack:
                    return new AirAttackVs(this);

                default:
                    Debug.Assert(false, "Bad stage: " + stage);
                    break;
            }

            return null;
        }

        public int getStagesCount()
        {
            return stagesCount;
        }

        protected VersusLevelStage getStage()
        {
            return (VersusLevelStage)stage;
        }
    }
}
