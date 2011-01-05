using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckstazyLive.game.levels;
using Microsoft.Xna.Framework;
using DuckstazyLive.game.levels.generator;
using DuckstazyLive.app;

namespace DuckstazyLive.game.stages.fx
{
    public class Firework
    {
        private const int STATE_LAUNCHING = 0;
        private const int STATE_FLYING = 1;
        private const int STATE_GENERATING = 2;
        private const int STATE_DONE = 3;

        private int state;

        private Setuper setuper;

        private float counter;
        private float x1, y1, x2, y2;
        private int pillsGenerated;
        private int pillsCollected;

        public int pillsCount;
        public float lifeTime;
        public float flyTime;
        public float flyOscAmplitude;
        public float flyOscOmega;        
        public float genTimeout;
        public float gravity;
        public float explSpeed;

        private float power;        

        public void start(Setuper setuper, float x1, float y1, float x2, float y2, float timeout)
        {
            this.setuper = setuper;
            this.x1 = x1;
            this.y1 = y1;
            this.x2 = x2;
            this.y2 = y2;

            state = STATE_LAUNCHING;            
            counter = timeout;
            
            explSpeed = 120.0f;
            lifeTime = 5.0f;
            flyTime = 5.0f;
            flyOscAmplitude = 15.0f;
            flyOscOmega = 30.0f;
            gravity = 350.0f;
            pillsCount = 10;
            genTimeout = 0.05f;
            power = 0.0f;

            pillsCollected = 0;
            pillsGenerated = 0;
        }

        public void update(float dt, float newPower)
        {
            power = newPower;

            switch (state)
            {
                case STATE_LAUNCHING:
                    updateLaunching(dt);
                    break;

                case STATE_FLYING:                    
                    break;

                case STATE_GENERATING:
                    updateGenerating(dt);
                    break;

                case STATE_DONE:
                    break;
            }
        }

        private void updateLaunching(float dt)
        {
 	        counter -= dt;
            if (counter <= 0)
            {
                launch();
            }
        }

        private void updateGenerating(float dt)
        {
            counter -= dt;
            if (counter <= 0.0f)
            {
                counter = genTimeout;
                double angle = pillsGenerated * MathHelper.TwoPi / pillsCount;

                Pills pills = getPills();             
                Pill pill = pills.findDead();                
                if (pill != null)
                {
                    float vx = (float)(explSpeed * Math.Cos(angle));
                    float vy = -Math.Abs((float)(explSpeed * Math.Sin(angle)));
                    
                    setuper.start(x2, y2, pill);
                    pill.user = fireworkCallback;
                    pill.vx = vx;
                    pill.vy = vy;
                    pill.t1 = lifeTime;
                    
                    pills.actives++;                    
                }

                pillsGenerated++;
                if (pillsGenerated == pillsCount)
                {
                    state = STATE_DONE;
                }
            }
        }

        private void launch()
        {
            state = STATE_FLYING;

            Pill pill = getPills().findDead();
            if (pill != null)
            {
                float dx = x2 - x1;
                float dy = y2 - y1;                

                pill.startToxic(x1, y1, Pill.TOXIC_SKULL);
                pill.hookedHero = Constants.UNDEFINED;
                pill.vx = dx / flyTime;
                pill.vy = dy / flyTime;
                pill.t1 = x1;
                pill.t2 = y1;
                pill.user = launchCallback;

                counter = flyTime;
                getPills().actives++;
            }
        }

        public void launchCallback(Pill pill, String msg, float dt)
        {
            if (msg == null && pill.isAlive())
            {
                float dx = pill.vx * dt;
                float dy = pill.vy * dt;
                pill.t1 += dx;
                pill.t2 += dy;

                Vector2 perpN = new Vector2(-dy * 10, dx * 10); // eliminate equation error
                perpN.Normalize();

                float progress = 1 - counter / flyTime;
                float amplitude = flyOscAmplitude * (1 - 0.5f * progress);
                float omega = flyOscOmega * (1 + 2.0f * progress);
                Vector2 amplitudePerp = Vector2.Multiply(perpN, (float)(amplitude * Math.Sin(counter * omega)));

                pill.x = pill.t1 + amplitudePerp.X;
                pill.y = pill.t2 + amplitudePerp.Y;

                counter -= dt;
                if (counter <= 0)
                {
                    explode(pill);
                }
            }
            else if (msg == "dead")
            {
                if (state == STATE_FLYING)
                {
                    pillsCollected = pillsCount;
                    state = STATE_DONE;
                }
            }
        }

        private void explode(Pill pill)
        {
            counter = 0.0f;
            state = STATE_GENERATING;

            pill.kill();

            float px = pill.x;
            float py = pill.y;
            if (pill.type == Pill.POWER)
                getParticles().explStarsPower(px, py, pill.id);
            else if (pill.type == Pill.TOXIC)
                getParticles().explStarsToxic(px, py, 0, true);            
        }        

        public void fireworkCallback(Pill pill, String msg, float dt)
        {
            if (msg == null && pill.isAlive())
            {
                float friction = 0.7f + power * 0.3f;

                pill.vy += gravity * dt;
                pill.x += pill.vx * dt;
                pill.y += pill.vy * dt;

                if (pill.x > 630)
                {
                    pill.vx = -pill.vx * friction;
                    pill.vy = pill.vy * friction;
                    pill.x = 630;
                }
                if (pill.x < 10)
                {
                    pill.vx = -pill.vx * friction;
                    pill.vy = pill.vy * friction;
                    pill.x = 10;
                }

                if (pill.y < 10)
                {
                    pill.vy = -pill.vy * friction;
                    pill.vx = pill.vx * friction;
                    pill.y = 10;
                }
                if (pill.y > 390)
                {
                    pill.vy = -pill.vy * friction;
                    pill.vx = pill.vx * friction;
                    pill.y = 390;
                }

                pill.t1 -= dt;
                if (pill.t1 <= 0.0f)
                {
                    pill.kill();
                    getParticles().explStarsPower(pill.x, pill.y, 0);
                }
            }
            else if (msg == "dead")
            {
                pillsCollected++;
            }
        }

        public bool isDone()
        {
            return state == STATE_DONE && pillsCollected == pillsCount;
        }

        private GameMgr getGameMgr()
        {
            return GameMgr.getInstance();
        }        

        private Pills getPills()
        {
            return getGameMgr().getPills();
        }

        private Particles getParticles()
        {
            return getGameMgr().getParticles();
        }
    }    
}
