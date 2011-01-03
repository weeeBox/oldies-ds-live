using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.core;
using Microsoft.Xna.Framework;
using DuckstazyLive.app;
using Framework.visual;
using Microsoft.Xna.Framework.Input;

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

        public StoryLevel(GameState gameState) : base(gameState)
        {            
        }

        protected abstract LevelStage createNextStage();        
        protected abstract int getStagesCount();        

        public override void update(float dt)
        {
            base.update(dt);

            if (isPlaying())
            {
                if (!getHeroes().hasAliveHero())
                {               
                    getEnv().blanc = 1.0f;
                    game.death();               
                }
            }
            else if (isWin())
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
                        nextLevel();
                }
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
            onEnd();
            nextLevelCountdown = 3;
            harvestProcess = 2;
            infoText = HARVEST_TEXT + "...";
            nextLevelCounter = 0;            
        }

        public void onLoose()
        {
            onEnd();
            game.loose(getStage().getLooseMessage());
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
            return (StoryLevelStage) stage;
        }
    }
}
