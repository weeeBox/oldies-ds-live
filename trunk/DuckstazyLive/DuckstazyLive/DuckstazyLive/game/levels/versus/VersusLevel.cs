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
using DuckstazyLive.game.stages;

namespace DuckstazyLive.game
{
    public class CollectedText : BaseElement
    {        
        private int heroIndex;
        private Font font;

        public CollectedText(int heroIndex)
        {
            this.heroIndex = heroIndex;

            font = Application.sharedResourceMgr.getFont(Res.FNT_HUD_DIGITS);
            width = font.stringWidth("000") - 4;
            height = font.fontHeight();
        }

        public override void draw()
        {
            preDraw();

            Hero hero = GameElements.Heroes[heroIndex];
            int collected = hero.pillsCollected;            

            float dx = width / 6.0f;            

            int units = collected % 10;
            int tens = (collected / 10) % 10;
            int hundreds = collected / 100;
            drawDigit(hundreds, drawX + dx, drawY, true);
            drawDigit(tens, drawX + 3.0f * dx, drawY, collected < 100);
            drawDigit(units, drawX + 5.0f * dx, drawY, collected < 10);
            
            postDraw();
        }

        private void drawDigit(int digit, float dx, float dy, bool useTransparency)
        {
            if (useTransparency && digit == 0)
            {
                AppGraphics.SetColor(Color.White * 0.5f);
                font.drawString("0", dx, dy, TextAlign.TOP | TextAlign.HCENTER);
                AppGraphics.SetColor(Color.White);                
            }
            else
            {
                font.drawString(digit.ToString(), dx, dy, TextAlign.TOP | TextAlign.HCENTER);
            }
        }
    }

    public class VersusLevelHud : Hud
    {
        protected CollectedText[] collectedTexts;        

        public VersusLevelHud(Level level) : base(level)
        {
            collectedTexts = new CollectedText[2];            
            CollectedText text1 = new CollectedText(0);            
            CollectedText text2 = new CollectedText(1);
            text1.x = 5;
            text2.x = -5;
            text1.parentAlignX = ALIGN_MIN;
            text2.parentAlignX = ALIGN_MAX;
            text1.parentAlignY = text2.parentAlignY = ALIGN_MAX;
            text2.setAlign(ALIGN_MAX, ALIGN_MIN);

            addChild(text1);
            addChild(text2);
        }

        protected override HealthBar[] createBars()
        {
            HealthBar bar1 = new HealthBar(Res.IMG_UI_HEALTH_EMO_BASE);
            bar1.setAlign(ALIGN_MIN, ALIGN_CENTER);
            bar1.parentAlignX = ALIGN_MIN;
            bar1.parentAlignY = ALIGN_CENTER;

            HealthBar bar2 = new HealthBar(Res.IMG_UI_HEALTH_EMO_BASE2);
            bar2.setAlign(ALIGN_MAX, ALIGN_CENTER);
            bar2.parentAlignX = ALIGN_MAX;
            bar2.parentAlignY = ALIGN_CENTER;

            return new HealthBar[] { bar1, bar2 };
        }
    }

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
        private int stageIndex;

        public VersusLevel(GameController controller, int stageIndex) : base(controller)
        {            
            this.stageIndex = stageIndex;
            
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

        protected override Hud createHud()
        {
            return new VersusLevelHud(this);
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
            getController().showWinner(playerIndex);
        }

        protected virtual void onDraw()
        {            
            startLevelState(STATE_END);
            getController().showDraw();
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

        protected VersusController getController()
        {
            return (VersusController)controller;
        }

        protected VersusLevelStage getStage()
        {
            return (VersusLevelStage)stage;
        }           
    }
}
