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
    public abstract class Level : BaseElementContainer
    {
        public static Level instance;        

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

            info = new GameInfo();            
            getEnv().reset();            

            stageMedia = new StageMedia();
            stage = null;            
        }
        
        protected abstract LevelStage createStage(int stageIndex);        

        public void reset()
        {
            getHeroes().clear();
        }

        public virtual void start()
        {
            getEnv().blanc = 1.0f;
            power = 0.0f;
            powerUp = 0.0f;            

            getParticles().clear();
            getPills().clear();
            info.reset();

            stage = createStage(state.level);

            getHeroes().init();
            save();
            
            syncScores();
            enterLevel();
        }

        public abstract void drawHud(Canvas canvas);       

        public void draw(Canvas canvas)
        {            
            getEnv().draw1(canvas);
 
            levelPreDraw();
            stage.draw1(canvas);

            info.drawFT(canvas);
            getPills().draw(canvas);
                                
            getHeroes().draw(canvas);

            getParticles().draw(canvas);
            levelPostDraw();

            getEnv().draw2(canvas);                
            
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
            getEnv().blanc = 1.0f;
            infoText = null;

            getEnv().playMusic();

            stage.start();
            Application.sharedSoundMgr.playSound(Res.SND_LEVEL_START);
        }

        public override void update(float dt)
        {
            base.update(dt);

            float power_drain = 0.0f;
            Heroes heroes = getHeroes();
                        
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
            getPills().update(dt, power);

            Env env = getEnv();
            env.x = heroes[0].x;
            env.y = heroes[0].y;
            env.update(dt, power);                

            getParticles().update(dt);                

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

        public override bool buttonPressed(ref ButtonEvent e)
        {
            Heroes heroes = getHeroes();
            if (heroes.hasAliveHero() && heroes.buttonPressed(ref e))
                return true;            

            if (e.button == Buttons.Back || e.key == Keys.Escape)
            {
                pause();
                return true;
            }            

            if (e.button == Buttons.LeftShoulder || e.key == Keys.PageUp)
            {
                int heroIndex = e.playerIndex;
                Debug.Assert(heroIndex >= 0 && heroIndex < heroes.getHeroesCount());
                heroes[heroIndex].doToxicDamage(heroes[heroIndex].x, heroes[heroIndex].y, 1, 0);
                return true;
            }           
            else if (e.key == Keys.P)
            {
                power = powerUp = 1.0f;
            }

            return false;
        }

        public override bool buttonReleased(ref ButtonEvent e)
        {           
            return getHeroes().buttonReleased(ref e);            
        }

        public void restart()
        {
            start();
        }        

        public virtual void pause()
        {

        }

        public virtual void save()
        {

        }

        public void onPause()
        {           
            getHeroes().buttonsReset();            
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
            Heroes heroes = getHeroes();
            foreach (Hero h in heroes)
            {
                h.gameState.syncScores();
            }
        }        

        public void onEnd()
        {
            getPills().finish();
            getEnv().blanc = 1.0f;
        }        

        public void resetPower(float newPower)
        {
            power = powerUp = newPower;
        }

        protected GameMgr getGameMgr()
        {
            return GameMgr.getInstance();
        }

        protected Pills getPills()
        {
            return getGameMgr().getPills();
        }

        protected Particles getParticles()
        {
            return getGameMgr().getParticles();
        }

        protected Heroes getHeroes()
        {
            return getGameMgr().getHeroes();
        }

        protected Env getEnv()
        {
            return Env.getIntance();
        }
    }
}
