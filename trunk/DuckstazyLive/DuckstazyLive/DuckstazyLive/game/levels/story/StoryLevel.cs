using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.core;
using Microsoft.Xna.Framework;
using DuckstazyLive.app;
using Framework.visual;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace DuckstazyLive.game
{
    public abstract class StoryLevel : Level
    {
        private const String HARVEST_TEXT = "HARVESTING";
        private const String NEXT_LEVEL_TEXT_BEGIN = "WARP IN ";
        private const String NEXT_LEVEL_TEXT_END = " SEC...";

        protected float nextLevelCounter;
        protected int harvestProcess;
        protected int nextLevelCountdown;
        protected StoryGame game;

        protected const int LEVEL_STATE_START = 0;
        protected const int LEVEL_STATE_PLAYING = 1;
        protected const int LEVEL_STATE_LOOSE = 2;
        protected const int LEVEL_STATE_WIN = 3;
        protected const int LEVEL_STATE_DIE = 4;        

        private const float DEATH_TIMEOUT = 3.0f;

        public StoryLevel(GameState gameState)
            : base(gameState)
        {
            game = StoryGame.instance;
        }

        protected abstract LevelStage createNextStage();
        protected abstract int getStagesCount();
        public abstract bool isSingleLevel();

        protected override void startLevelState(int levelState)
        {
            this.levelState = levelState;
            levelStateElapsed = 0;

            switch (levelState)
            {
                case LEVEL_STATE_DIE:
                    getEnv().blanc = 1.0f;
                    break;
                case LEVEL_STATE_LOOSE:
                case LEVEL_STATE_PLAYING:
                case LEVEL_STATE_START:
                case LEVEL_STATE_WIN:
                    break;
                default:
                    Debug.Assert(false, "Bad level state: " + levelState);
                    break;
            }
        }

        public override void start()
        {
            base.start();
            startLevelState(LEVEL_STATE_START);
        }

        public override void update(float dt)
        {
            levelStateElapsed += dt;
            if (levelState == LEVEL_STATE_DIE)
            {                
                if (levelStateElapsed > DEATH_TIMEOUT)
                {
                    game.death();
                }
                else
                {
                    float progress = levelStateElapsed / DEATH_TIMEOUT;
                    base.update(dt * (1 - progress));
                    
                    getEnv().blanc = progress;
                }

                return;
            }            

            base.update(dt);
            
            switch (levelState)
            {
                case LEVEL_STATE_START:
                    {
                        startLevelState(LEVEL_STATE_PLAYING);
                    }
                    break;

                case LEVEL_STATE_PLAYING:
                    if (!getHeroes().hasAliveHero())
                    {
                        startLevelState(LEVEL_STATE_DIE);                        
                    }
                    break;

                case LEVEL_STATE_LOOSE:
                    {
                        game.loose(getStage().getLooseMessage());
                    }
                    break;                

                case LEVEL_STATE_WIN:
                    {
                        if (getPills().harvestCount > 0)
                            updateHarvesting(dt);
                        else
                        {
                            if (nextLevelCountdown > 0)
                            {
                                nextLevelCounter += dt;
                                if (nextLevelCounter > 1)
                                {
                                    nextLevelCounter--;
                                    nextLevelCountdown--;
                                    infoText = NEXT_LEVEL_TEXT_BEGIN + nextLevelCountdown.ToString() + NEXT_LEVEL_TEXT_END;
                                }
                            }
                            else
                            {
                                nextLevel();
                            }
                        }
                    }
                    break;

                default:
                    {
                        Debug.Assert(false, "Bad state: " + state);
                    }                    
                    break;
            }
        }

        public override bool buttonPressed(ref ButtonEvent e)
        {
            if (base.buttonPressed(ref e))
                return true;

            if (e.button == Buttons.RightShoulder || e.key == Keys.PageDown)
            {
                nextLevel();
                return true;
            }

            return false;
        }

        public override void pause()
        {
            game.pause();
        }

        public void nextLevel()
        {
            if (state.level >= getStagesCount() - 1)
            {
                game.win();
            }
            else
            {
                state.level++;
                start();
            }
        }

        public void onWin()
        {
            startLevelState(LEVEL_STATE_WIN);

            onEnd();
            nextLevelCountdown = 3;
            harvestProcess = 2;
            infoText = HARVEST_TEXT + "...";
            nextLevelCounter = 0;
        }

        public void onLoose()
        {
            onEnd();
            startLevelState(LEVEL_STATE_LOOSE);
        }

        public bool isPlaying()
        {
            return getStage().isPlaying();
        }

        public bool isWin()
        {
            return getStage().isWin();
        }

        public bool isLoose()
        {
            return getStage().isLoose();
        }

        private void updateHarvesting(float dt)
        {
            String str = "";
            int i;

            Pills pills = getPills();
            pills.harvest(dt);
            if (pills.harvestCount > 0)
            {
                nextLevelCounter += dt;
                if (nextLevelCounter >= 1)
                {
                    nextLevelCounter--;
                    harvestProcess++;
                    if (harvestProcess > 2)
                        harvestProcess = 0;
                    i = harvestProcess;
                    while (i > 0)
                    {
                        str += ".";
                        --i;
                    }
                    infoText = HARVEST_TEXT + str;
                }
            }
            else
            {
                nextLevelCounter = 0;
                infoText = NEXT_LEVEL_TEXT_BEGIN +
                                nextLevelCountdown.ToString() +
                                NEXT_LEVEL_TEXT_END;
            }
        }

        protected StoryLevelStage getStage()
        {
            return (StoryLevelStage)stage;
        }
    }
}
