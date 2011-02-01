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
using DuckstazyLive.game.stages;

namespace DuckstazyLive.game
{
    public abstract class Level : View
    {
        public static Level instance;        

        public float power;
        protected float powerUp;
                
        public LevelStage stage; // текущий уровень        
        public StageMedia stageMedia;
        public Hud hud;        

        // инфа 100%        
        protected int stageIndex;

        protected int levelState;
        protected float levelStateElapsed;

        private Canvas canvas;
        protected GameController controller;

        public Level(GameController controller)
        {
            this.controller = controller;

            setDrawInnactive(true);

            instance = this;            
            getEnv().reset();            

            stageMedia = new StageMedia();
            stage = null;

            hud = createHud();
            canvas = new Canvas(0, 0);            
        }
        
        protected abstract LevelStage createStage(int stageIndex);
        protected abstract Hud createHud();        

        public virtual void start()
        {            
            power = 0.0f;
            powerUp = 0.0f;            

            getParticles().clear();
            getPills().clear();            
            getHeroes().init();            

            stage = createStage(stageIndex);
            stage.onStart();

            hud.onStart();

            Env env = getEnv();
            env.startBlanc();
            env.playMusic();            
            Application.sharedSoundMgr.playSound(Res.SND_LEVEL_START);
        }

        public virtual void restart()
        {
            start();
        }

        public virtual void pause()
        {
            getHeroes().buttonsReset();
            controller.showPause();
        }

        public virtual void onEnd()
        {
            getEnv().startBlanc();
        }

        protected virtual void startLevelState(int levelState)
        {
            levelStateElapsed = 0;
            this.levelState = levelState;
        }        

        public override void draw()
        {
            preDraw();            

            getEnv().draw1(canvas);
 
            levelPreDraw();

            stage.draw1(canvas);            
            getPills().draw(canvas);                                
            getHeroes().draw(canvas);
            getParticles().draw(canvas);
            getHeroes().drawInfo(canvas);
            levelPostDraw();

            getEnv().draw2(canvas);

            hud.draw();
            stage.draw2(canvas);
            stage.drawUI(canvas);

            postDraw();

            getEnv().drawBlanc(canvas);
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
            env.updateBlanc(dt);

            getParticles().update(dt);
                        
            hud.update(power, dt);
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
            if (e.isKeyboardEvent())
            {
                InputManager im = Application.sharedInputMgr;
                for (int playerIndex = 0; playerIndex < im.getPlayersCount(); ++playerIndex)
                {
                    if (im.hasMappedButton(e.key, playerIndex))
                    {
                        ButtonEvent newEvent = im.makeButtonEvent(playerIndex, im.getMappedButton(e.key, playerIndex));
                        return buttonPressed(ref newEvent);
                    }
                }
            }

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
            else if (e.key == Keys.B)
            {
                getEnv().startHitFade(false);
            }
            else if (e.key == Keys.N)
            {
                getEnv().startHitFade(true);
            }

            return false;
        }

        public override bool buttonReleased(ref ButtonEvent e)
        {
            if (e.isKeyboardEvent())
            {
                InputManager im = Application.sharedInputMgr;
                for (int playerIndex = 0; playerIndex < im.getPlayersCount(); ++playerIndex)
                {
                    if (im.hasMappedButton(e.key, playerIndex))
                    {
                        ButtonEvent newEvent = im.makeButtonEvent(playerIndex, im.getMappedButton(e.key, playerIndex));
                        return buttonReleased(ref newEvent);
                    }
                }
            }

            return getHeroes().buttonReleased(ref e);            
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

        public void resetPower(float newPower)
        {
            power = powerUp = newPower;
        }        

        protected Pills getPills()
        {
            return GameElements.Pills;
        }

        protected Particles getParticles()
        {
            return GameElements.Particles;
        }

        protected Heroes getHeroes()
        {
            return GameElements.Heroes;
        }

        protected Env getEnv()
        {
            return GameElements.Env;
        }
    }
}
