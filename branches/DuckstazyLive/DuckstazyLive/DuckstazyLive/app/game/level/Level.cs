using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using app.menu;
using DuckstazyLive.app.game.stage;
using DuckstazyLive.app.game.env;
using app;
using asap.graphics;
using asap.core;
using asap.visual;
using asap.sound;

namespace DuckstazyLive.app.game.level
{
    public abstract class Level : BaseElementContainer, KeyListener
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

        public BaseGame controller;

        public Level(BaseGame controller, float width, float height) : base(width, height)
        {
            this.controller = controller;

            //setDrawInnactive(true);

            instance = this;

            Env env = getEnv();
            env.reset();            

            stageMedia = new StageMedia();
            stage = null;
            hud = createHud();

            Pills pills = getPills();
            pills.width = width;
            pills.height = height;
            AddChild(pills);

            Heroes heroes = getHeroes();
            heroes.width = width;
            heroes.Height = height;
            AddChild(heroes);


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
            Application.sharedSoundMgr.PlaySound(Res.SND_LEVEL_START);
        }

        public virtual void restart()
        {
            start();
        }

        public virtual void pause()
        {
            getHeroes().buttonsReset();
            //controller.showPause();
            throw new NotImplementedException();
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

        //public override void Draw(Graphics g)
        //{
        //    PreDraw(g);

        //    //getEnv().draw1(g);
 
        //    ////levelPreDraw();

        //    //draw1();
        //    //stage.draw1(g);            
        //    //getPills().Draw(g);                                
        //    //getHeroes().draw(g);
        //    //getParticles().draw(g);
        //    //getHeroes().drawInfo(g);
        //    //draw2();
        //    ////levelPostDraw();

        //    //getEnv().draw2(g);            

        //    ////hud.draw(g);
        //    //stage.draw2(g);
        //    //stage.drawUI(g);

        //    PostDraw(g);

        //    // getEnv().drawBlanc(canvas);
        //}

        public virtual void draw1()
        {

        }

        public virtual void draw2()
        {

        }

        private void levelPreDraw()
        {
            float tx = Constants.SAFE_OFFSET_X;
            float ty = Constants.SAFE_OFFSET_Y;
            AppGraphics.PushMatrix();
            AppGraphics.Translate(tx, ty);
        }

        private void levelPostDraw()
        {
            AppGraphics.PopMatrix();
        }        

        public override void Update(float dt)
        {                        
            float power_drain = 0.0f;
            Heroes heroes = getHeroes();

            if (stage != null)
            {
                stage.Update(dt);
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

            base.Update(dt);

            //heroes.Update(dt, power);
            //getPills().Update(dt, power);

            //Env env = getEnv();
            //env.x = heroes[0].x;
            //env.y = heroes[0].y;
            //env.Update(dt, power);
            //env.updateBlanc(dt);

            getParticles().Update(dt);
                        
            hud.Update(power, dt);
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

        public virtual bool KeyPressed(KeyEvent e)
        {
            //if (e.isKeyboardEvent())
            //{
            //    InputManager im = Application.sharedInputMgr;
            //    for (int playerIndex = 0; playerIndex < im.getPlayersCount(); ++playerIndex)
            //    {
            //        if (im.hasMappedButton(e.key, playerIndex))
            //        {
            //            ButtonEvent newEvent = im.makeButtonEvent(playerIndex, im.getMappedButton(e.key, playerIndex));
            //            return KeyPressed(ref newEvent);
            //        }
            //    }
            //}

            Heroes heroes = getHeroes();
            if (heroes.hasAliveHero() && heroes.KeyPressed(e))
                return true;            

            if (e.code == KeyCode.Back || e.code == KeyCode.VK_Escape)
            {
                pause();
                return true;
            }            

            //if (e.button == Buttons.LeftShoulder || e.key == Keys.PageUp)
            //{
            //    int heroIndex = e.playerIndex;
            //    Debug.Assert(heroIndex >= 0 && heroIndex < heroes.getHeroesCount());
            //    heroes[heroIndex].doToxicDamage(heroes[heroIndex].x, heroes[heroIndex].y, 1, 0);
            //    return true;
            //}           
            //else if (e.key == Keys.P)
            //{
            //    power = powerUp = 1.0f;
            //}
            //else if (e.key == Keys.B)
            //{
            //    getEnv().startHitFade(false);
            //}
            //else if (e.key == Keys.N)
            //{
            //    getEnv().startHitFade(true);
            //}

            return false;
        }

        public virtual bool KeyReleased(KeyEvent e)
        {
            //if (e.isKeyboardEvent())
            //{
            //    InputManager im = Application.sharedInputMgr;
            //    for (int playerIndex = 0; playerIndex < im.getPlayersCount(); ++playerIndex)
            //    {
            //        if (im.hasMappedButton(e.key, playerIndex))
            //        {
            //            ButtonEvent newEvent = im.makeButtonEvent(playerIndex, im.getMappedButton(e.key, playerIndex));
            //            return KeyReleased(ref newEvent);
            //        }
            //    }
            //}

            return getHeroes().KeyReleased(e);            
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

        public static void playSound(int snd, float vol, float x)
        {
            float halfWidth = 0.5f * instance.width;
            float pan = (x - halfWidth) / halfWidth;

            if (pan > 1) pan = 1;
            else if (pan < -1) pan = -1;

            SoundTransform tr = new SoundTransform(vol, pan);            
            Application.sharedSoundMgr.PlaySound(snd, tr);
        }
    }
}
