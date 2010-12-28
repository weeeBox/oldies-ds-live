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

        protected override void initHero()
        {
            heroes = new Heroes();
            Hero hero = new Hero(heroes, 0);
            hero.gameState.leftOriented = true;
            hero.gameState.color = Color.Yellow;
            heroes.addHero(hero);            

            pills = new Pills(heroes, ps, this);
            heroes.particles = ps;
            heroes.env = env;
            heroes.clear();
        }        

        public override void drawHud(Canvas canvas)
        {
            heroes[0].gameState.draw(canvas, Constants.TITLE_SAFE_LEFT_X, Constants.TITLE_SAFE_TOP_Y);

            Font font = Application.sharedResourceMgr.getFont(Res.FNT_BIG);

            float infoX = 0.5f * (Constants.TITLE_SAFE_RIGHT_X + Constants.TITLE_SAFE_LEFT_X);
            float infoY = Constants.TITLE_SAFE_TOP_Y;            

            if (infoText != null)
            {                
                font.drawString(infoText, infoX, infoY, TextAlign.HCENTER | TextAlign.VCENTER);
            }            

            if (getStage().hasTimeLimit())
            {
                float t = getStage().getRemainingTime();
                int i = (int)(t / 60);
                string timeStr;
                if (i < 10) timeStr = "0" + i.ToString() + ":";
                else timeStr = i.ToString() + ":";
                i = ((int)t) % 60;
                if (i < 10) timeStr += "0" + i.ToString();
                else timeStr += i.ToString();

                if (infoText != null)
                {
                    float timeX = Constants.TITLE_SAFE_RIGHT_X;
                    float timeY = infoY;
                    font.drawString(timeStr, timeX, timeY, TextAlign.RIGHT | TextAlign.VCENTER);
                }
                else
                {
                    font.drawString(timeStr, infoX, infoY, TextAlign.HCENTER | TextAlign.VCENTER);
                }
            }
        }

        public override void update(float dt)
        {
            base.update(dt);

            if (isPlaying())
            {
                if (!heroes.hasAliveHero())
                {               
                    env.blanc = 1.0f;
                    game.death();               
                }
            }
            else if (isWin())
            {
                if (pills.harvestCount > 0)
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
