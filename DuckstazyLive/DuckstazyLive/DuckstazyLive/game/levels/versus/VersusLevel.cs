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
    public class VersusLevelHud : Hud
    {
        public VersusLevelHud(Level level) : base(level)
        {

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
    }
}
