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
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace DuckstazyLive.game
{
    public class VersusLevel : Level
    {
        private enum VersusStages
        {
            DoubleFrog,            
            TripleFrog,
            AirAttack,
        }

        private struct StageInfo
        {
            public VersusStages stage;
            public String name;

            public StageInfo(VersusStages stage, String name)
            {
                this.stage = stage;
                this.name = name;
            }
        }

        private static StageInfo[] stagesInfo =
        {
            new StageInfo(VersusStages.DoubleFrog, "Double Frog"),
            new StageInfo(VersusStages.TripleFrog, "Triple Frog"),
            new StageInfo(VersusStages.AirAttack, "Air Attack"),
        };

        private const int STATE_START = 0;
        private const int STATE_PLAYING = 1;
        private const int STATE_END = 2;
                
        private VersusController controller;
        private int stageIndex;

        public VersusLevel(VersusController controller, int stageIndex)
        {
            setUpdateInnactive(true);
            this.stageIndex = stageIndex;

            this.controller = controller;
            GameElements.initHeroes(2);
            GameElements.reset();
        }        

        public override void start()
        {
            state.level = stageIndex;

            base.start();
            startLevelState(STATE_START);
        }

        protected override void startLevelState(int levelState)
        {
            base.startLevelState(levelState);

            switch (levelState)
            {
                case STATE_START:
                {
                    getHeroes().clear();
                    break;
                }                    
                case STATE_PLAYING:                 
                    break;
                case STATE_END:
                {
                    getPills().finish();
                    getHeroes().buttonsReset();
                    break;
                }                    
                default:
                    Debug.Assert(false, "Bad level state: " + levelState);
                    break;
            }
        }

        protected override LevelStage createStage(int stageIndex)
        {
            Debug.Assert(stageIndex >= 0 && stageIndex < stagesInfo.Length);
            VersusStages stage = stagesInfo[stageIndex].stage;            

            switch (stage)
            {
                case VersusStages.DoubleFrog:
                    return new DoubleFrogVs(this);

                case VersusStages.AirAttack:
                    return new AirAttackVs(this);

                case VersusStages.TripleFrog:
                    return new TripleFrog(this);

                default:
                    Debug.Assert(false, "Bad stage: " + stage);
                    break;
            }

            return null;
        }

        public override void update(float dt)
        {
            base.update(dt);

            levelStateElapsed += dt;
            switch (levelState)
            {
                case STATE_START:
                {
                    startLevelState(STATE_PLAYING);
                    break;
                }
                
                case STATE_PLAYING:
                {
                    Heroes heroes = getHeroes();
                    if (heroes[1].isDead())
                    {
                        onWin(0);
                    }
                    else if (heroes[0].isDead())
                    {
                        onWin(1);
                    }
                    else if (getStage().isEnded())
                    {
                        int collected0 = getStage().getPillCollected(0);
                        int collected1 = getStage().getPillCollected(1);

                        if (collected0 > collected1)
                        {
                            onWin(0);
                        }
                        else if (collected1 > collected0)
                        {
                            onWin(1);
                        }
                        else
                        {
                            onDraw();
                        }
                    }
                    break;
                }                
            }            
        }       
 
        protected virtual void onWin(int playerIndex)
        {            
            startLevelState(STATE_END);
            controller.showWinner(playerIndex);
        }

        protected virtual void onDraw()
        {            
            startLevelState(STATE_END);
            controller.showDraw();
        }

        public static int getStagesCount()
        {
            return stagesInfo.Length;
        }        

        public static String getStageName(int stageIndex)
        {
            Debug.Assert(stageIndex >= 0 && stageIndex < getStagesCount());
            return stagesInfo[stageIndex].name;
        }

        protected VersusLevelStage getStage()
        {
            return (VersusLevelStage)stage;
        }

        public override void drawHud(Canvas canvas)
        {
            Heroes heroes = getHeroes();
            heroes[0].gameState.draw(canvas, Constants.TITLE_SAFE_LEFT_X, Constants.TITLE_SAFE_TOP_Y);
            heroes[1].gameState.draw(canvas, Constants.TITLE_SAFE_RIGHT_X, Constants.TITLE_SAFE_TOP_Y);

            Font font = Application.sharedResourceMgr.getFont(Res.FNT_HUD_DIGITS);

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
            
            Texture2D clock = Application.sharedResourceMgr.getTexture(Res.IMG_CLOCK);
            AppGraphics.DrawImage(clock, timeX - 0.5f * clock.Width - 3, timeY - 0.5f * clock.Height + 3);
            font.drawString(timeStr, timeX, timeY, TextAlign.HCENTER | TextAlign.VCENTER);

            if (hasInfoText)
            {
                font = Application.sharedResourceMgr.getFont(Res.FNT_BIG);
                font.drawString(infoText, infoX, infoY, TextAlign.HCENTER | TextAlign.VCENTER);
            }
        }        
    }
}
