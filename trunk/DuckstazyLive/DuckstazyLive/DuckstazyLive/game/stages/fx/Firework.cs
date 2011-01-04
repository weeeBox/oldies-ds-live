﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckstazyLive.game.levels;
using Microsoft.Xna.Framework;

namespace DuckstazyLive.game.stages.fx
{
    public class Firework
    {
        private const int STATE_LAUNCHING = 0;
        private const int STATE_FLYING = 1;
        private const int STATE_GENERATING = 2;
        private const int STATE_DONE = 3;

        private int state;

        private float counter;
        private float x1, y1, x2, y2;
        private int pillsGenerated;

        public int pillsCount;
        public float lifeTime;
        public float lauchTimeout;
        public float genTimeout;

        public Firework()
        {            
        }        

        public void start(float x1, float y1, float x2, float y2)
        {
            this.x1 = x1;
            this.y1 = y1;
            this.x2 = x2;
            this.y2 = y2;

            state = STATE_LAUNCHING;
            counter = lauchTimeout;
        }

        public void update(float dt)
        {
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

        private void updateFlying(float dt)
        {

        }

        private void updateGenerating(float dt)
        {
            counter -= dt;
            if (counter <= 0.0f)
            {
                counter = genTimeout;
                double angle = pillsGenerated * Math.PI / pillsCount;

                Pills pills = getPills();             
                Pill pill = pills.findDead();
                float speed = 400.0f;
                if (pill != null)
                {
                    float vx = (float)(speed * Math.Cos(angle));
                    float vy = -Math.Abs((float)(speed * Math.Sin(angle)));

                    pill.startPower(x2, y2, 0, false);
                    pill.vx = vx;
                    pill.vy = vy;                    
                    pill.t1 = lifeTime;
                    pill.user = fireworkCallback;
                    pills.actives++;
                }

                pillsGenerated++;
                if (pillsGenerated == pillsCount)
                    state = STATE_DONE;
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

                float time = 5.0f;

                pill.startPower(x1, y1, 0, false);
                pill.vx = dx / time;
                pill.vy = dy / time;
                pill.t1 = x1;
                pill.t2 = y1;
                pill.user = launchCallback;

                counter = time;
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

                Vector2 perpN = new Vector2(-dy * 10, dx * 10);
                perpN.Normalize();

                float amplitude = 5.0f;
                float omega = 24.0f;
                Vector2 amplitudePerp = Vector2.Multiply(perpN, (float)(amplitude * Math.Sin(counter * omega)));

                pill.x = pill.t1 + amplitudePerp.X;
                pill.y = pill.t2 + amplitudePerp.Y;

                counter -= dt;
                if (counter <= 0)
                {
                    explode(pill);
                }
            }
        }

        private void explode(Pill pill)
        {
            pill.kill();

            float px = pill.x;
            float py = pill.y;
            if (pill.type == Pill.POWER)
                getParticles().explStarsPower(px, py, pill.id);
            else if (pill.type == Pill.TOXIC)
                getParticles().explStarsToxic(px, py, 0, true);

            startFirework();
        }

        private void startFirework()
        {
            counter = 0.0f;
            state = STATE_GENERATING;            
        }

        public void fireworkCallback(Pill pill, String msg, float dt)
        {
            if (msg == null && pill.isAlive())
            {
                pill.vy += 350.0f * dt;

                pill.x += pill.vx * dt;
                pill.y += pill.vy * dt;

                if (pill.x < 0.0f)
                {
                    pill.x = -pill.x;
                    pill.vx -= pill.vx;
                }
                else if (pill.x > 640.0f)
                {
                    pill.x = 640.0f;
                    pill.vx -= pill.vx;
                }
                if (pill.y < 0)
                {
                    pill.y = 0;
                    pill.vy = 0;
                }
                else if (pill.y > 400.0f)
                {
                    pill.vy = -0.5f * pill.vy;
                    pill.y = 400;
                }

                pill.t1 -= dt;
                if (pill.t1 <= 0)
                {
                    pill.kill();
                }
            }
        }

        private Pills getPills()
        {
            return GameMgr.getInstance().getPills();
        }

        private Particles getParticles()
        {
            return GameMgr.getInstance().getParticles();
        }
    }    
}