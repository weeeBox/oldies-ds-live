using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using DuckstazyLive.app;
using DuckstazyLive.game.levels;
using System.Diagnostics;
using Framework.core;
using Framework.visual;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace DuckstazyLive.game
{
    public abstract class Level
    {
        public static Level instance;

        private const String HARVEST_TEXT = "HARVESTING";
        private const String NEXT_LEVEL_TEXT_BEGIN = "WARP IN ";
        private const String NEXT_LEVEL_TEXT_END = " SEC...";                       

        public string infoText;

        public int sndStart;

        public Game game;
        public Heroes heroes;
        public Pills pills;
        public Env env;
        protected Particles ps;

        public LevelProgress progress;

        public float power;
        protected float powerUp;

        // Состояние уровня
        public GameState state;

        // Конец Уровня
        public bool finish;
        private float finishCounter;

        protected List<LevelStages> stages; // Уровни
        public LevelStage stage; // текущий уровень
        public StageMedia stageMedia;
        public int stagesCount;

        // инфа
        public GameInfo info;

        public bool pause;
        public int imgPause;

        protected float nextLevelCounter;
        protected int harvestProcess;
        protected int nextLevelCountdown;

        public Level(GameState gameState)
        {
            instance = this;
            state = gameState;

            game = Game.instance;

            info = new GameInfo();
            ps = new Particles();
            env = new Env(game);

            initHero();

            progress = new LevelProgress();
            progress.env = env;

            stageMedia = new StageMedia();
            stages = new List<LevelStages>();

            stage = null;
            finish = false;
            
            sndStart = Res.SND_LEVEL_START;            
        }

        protected virtual void initHero()
        {
            heroes = new Heroes();
            Hero heroInstance = new Hero(heroes, 0);            
            heroes.addHero(heroInstance);

            pills = new Pills(heroes, ps, this);
            heroes.particles = ps;
            heroes.env = env;
            heroes.clear();
        }

        public void reset()
        {
            heroes.clear();
        }

        public void start()
        {
            env.blanc = 1.0f;
            power = 0.0f;
            powerUp = 0.0f;

            stage = LevelStageFactory.createStage(stages[state.level]);

            ps.clear();
            pills.clear();
            info.reset();

            progress.start(stage.goalTime);
            heroes.init();
            game.save();

            finish = false;
            pause = false;
            
            syncScores();
            enterLevel();
        }

        public abstract void drawUI(Canvas canvas);        

        public void draw(Canvas canvas)
        {
            if (pause)
            {
                throw new NotImplementedException();
            }
            else
            {
                env.draw1(canvas);

                //if(!room)
                levelPreDraw();
                stage.draw1(canvas);

                info.drawFT(canvas);
                pills.draw(canvas);
                                
                heroes.draw(canvas);

                ps.draw(canvas);
                levelPostDraw();

                env.draw2(canvas);
                
                // progress.draw(canvas);
                drawUI(canvas);
                stage.draw2(canvas);

                //levelPreDraw();
                //AppGraphics.DrawRect(0, 0, utils.scale(640), utils.scale(480), Color.White);
                //levelPostDraw();
            }
        }

        private void levelPreDraw()
        {
            float tx = Constants.SAFE_OFFSET_X;
            float ty = Constants.SAFE_OFFSET_Y;
            AppGraphics.PushMatrix();
            AppGraphics.Translate(tx, ty, 0);
        }

        private void levelPostDraw()
        {
            AppGraphics.PopMatrix();
        }

        public void enterLevel()
        {
            env.blanc = 1.0f;

            stage.start();
            Application.sharedSoundMgr.playSound(sndStart);
        }

        public void update(float dt)
        {
            float power_drain = 0.0f;            

            if (!pause)
            {
                if (stage != null)
                {
                    stage.update(dt);
                    if (stage.win && !finish)
                    {
                        winLevel();
                    }
                }

                if (!heroes.hasAliveHero())
                {
                    if (!finish)
                    {
                        finish = true;                        
                        env.blanc = 1.0f;
                        progress.play = false;

                        game.loose();
                    }
                }
                else
                {
                    if (finish)
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
                                    infoText = NEXT_LEVEL_TEXT_BEGIN +
                                                    nextLevelCountdown.ToString() +
                                                    NEXT_LEVEL_TEXT_END;
                                }
                            }
                            else
                                nextLevel();
                        }
                    }
                }

                if (heroes.hasAsleepHero()) 
                    power_drain = 0.3f;

                if (powerUp < power)
                {
                    power -= dt * power_drain;
                    if (power < 0.0f) power = 0.0f;
                }
                else
                {
                    power += dt * 0.05f;
                    if (power > powerUp) power = powerUp;
                }

                heroes.update(dt, power);

                pills.update(dt, power);

                env.x = heroes[0].x;
                env.y = heroes[0].y;
                env.update(dt, power);

                progress.update(dt, power);

                ps.update(dt);                

                if (power >= 0.5) info.setRGB(env.colors.bg);
                else
                {
                    if (env.day) info.setRGB(0x000000);
                    else info.setRGB(0xffffff);
                }
                info.update(power, dt);                
            }
        }

        public void gainPower(float gained)
        {
            powerUp += gained;
            if (powerUp > 1.0f)
                powerUp = 1.0f;
        }

        public void gainSleep()
        {
            powerUp = 0.0f;
        }

        public void buttonPressed(ref ButtonEvent e)
        {
            if (pause)
            {
                throw new NotImplementedException();
            }
            else
            {
                if (finish)
                {
                    if (heroes.hasAliveHero())
                    {
                        //if(code==0x0D || code==0x1B) // ENTER or ESC
                        //nextLevel();
                        //else
                        heroes.buttonPressed(ref e);
                    }
                    else
                    {
                        if (e.button == Buttons.A || e.button == Buttons.Start) // ENTER
                            game.startLevel();
                        else if (e.button == Buttons.Back)// ESC
                            game.goPause();
                    }
                }
                else
                {
                    heroes.buttonPressed(ref e);
                    if (e.button == Buttons.Back)// ESC
                        game.goPause();
                    else if (e.button == Buttons.RightShoulder)
                        nextLevel();
                    else if (e.button == Buttons.LeftShoulder)
                        heroes[0].doToxicDamage(heroes[0].x, heroes[0].y, 1, 0);
                    //else if (key == Keys.End)
                    //    powerUp = power = 1.0f;
                    //else if (key == Keys.Delete)
                    //    state.health = 0;
                    /*else if(code==0x44)
                        hero.doToxicDamage(320, 200, 20, 0);
                    else if(code==0x50)
                        powerUp = power = 1;*/
                }
            }
        }

        public void buttonReleased(ref ButtonEvent e)
        {
            if (!pause)
                heroes.buttonReleased(ref e);
        }

        public void nextLevel()
        {
            if (state.level >= stagesCount - 1)
            {
                game.goCredits();
            }
            else
            {
                state.level++;
                start();
            }
        }

        public void setPause(bool value)
        {
            if (value)
            {
                //draw(imgPause);
                //imgPause.applyFilter(imgPause, new Rect(0.0f, 0.0f, 640.0f, 480.0f), new Point(), new BlurFilter(16.0f, 16.0f, 2)); 
                //pause = true;
                //hero.keysReset();
                throw new NotImplementedException();
            }
            else
            {
                pause = false;
            }
            env.blanc = 1.0f;
        }

        public void switchEvnPower()
        {
            if (power >= 0.5f)
            {
                powerUp = power = 0.49f;
            }
            else
            {
                powerUp = power = 0.5f;
            }
        }

        // Синхронизировать очки, тоесть указать oldScore=state.scores, обновить надпись.
        public void syncScores()
        {
            foreach (Hero h in heroes)
            {
                h.gameState.syncScores();
            }
        }

        private void winLevel()
        {
            pills.finish();
            nextLevelCountdown = 3;
            harvestProcess = 2;
            infoText = HARVEST_TEXT + "...";
            nextLevelCounter = 0;
            finish = true;
            env.blanc = 1.0f;
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

        public void resetPower(float newPower)
        {
            power = powerUp = newPower;
        }
    }
}
