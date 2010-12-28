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

        public Game game;
        public Heroes heroes;
        public Pills pills;
        public Env env;
        protected Particles ps;        

        public float power;
        protected float powerUp;

        // Состояние уровня
        public GameState state;
        
        public LevelStage stage; // текущий уровень        
        public StageMedia stageMedia;        

        public String infoText;        

        // инфа
        public GameInfo info;

        public Level(GameState gameState)
        {
            instance = this;
            state = gameState;

            game = Game.instance;

            info = new GameInfo();
            ps = new Particles();
            env = Env.getIntance();
            env.reset();
            env.playMusic();

            initHero();            

            stageMedia = new StageMedia();
            stage = null;            
        }
        
        protected abstract LevelStage createStage(int stageIndex);
        protected abstract void initHero();        

        public void reset()
        {
            heroes.clear();
        }

        public void start()
        {
            env.blanc = 1.0f;
            power = 0.0f;
            powerUp = 0.0f;            

            ps.clear();
            pills.clear();
            info.reset();

            stage = createStage(state.level);

            heroes.init();
            game.save();            
            
            syncScores();
            enterLevel();
        }

        public abstract void drawHud(Canvas canvas);       

        public void draw(Canvas canvas)
        {            
            env.draw1(canvas);
 
            levelPreDraw();
            stage.draw1(canvas);

            info.drawFT(canvas);
            pills.draw(canvas);
                                
            heroes.draw(canvas);

            ps.draw(canvas);
            levelPostDraw();

            env.draw2(canvas);                
            
            drawHud(canvas);
            stage.draw2(canvas);
            stage.drawUI(canvas);
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
            infoText = null;

            stage.start();
            Application.sharedSoundMgr.playSound(Res.SND_LEVEL_START);
        }

        public virtual void update(float dt)
        {
            float power_drain = 0.0f;            
                        
            if (stage != null)
            {
                stage.update(dt);                    
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

            ps.update(dt);                

            if (power >= 0.5) info.setRGB(env.colors.bg);
            else
            {
                if (env.day) info.setRGB(0x000000);
                else info.setRGB(0xffffff);
            }
            info.update(power, dt);            
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

        public virtual bool buttonPressed(ref ButtonEvent e)
        {
            if (heroes.hasAliveHero() && heroes.buttonPressed(ref e))
                return true;            

            if (e.button == Buttons.Back || e.key == Keys.Escape)
            {
                game.pause();
                return true;
            }            

            if (e.button == Buttons.LeftShoulder || e.key == Keys.PageUp)
            {
                int heroIndex = e.playerIndex;
                Debug.Assert(heroIndex >= 0 && heroIndex < heroes.getHeroesCount());
                heroes[heroIndex].doToxicDamage(heroes[heroIndex].x, heroes[heroIndex].y, 1, 0);
                return true;
            }           

            return false;
        }

        public virtual bool buttonReleased(ref ButtonEvent e)
        {           
            return heroes.buttonReleased(ref e);            
        }

        public void restart()
        {
            start();
        }        

        public void onPause()
        {           
            heroes.buttonsReset();            
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

        public void onEnd()
        {
            pills.finish();
            env.blanc = 1.0f;
        }        

        public bool isPlaying()
        {
            return stage.isPlaying();
        }        

        public void resetPower(float newPower)
        {
            power = powerUp = newPower;
        }
    }
}
