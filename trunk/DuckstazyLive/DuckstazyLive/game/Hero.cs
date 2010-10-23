﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.core;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using DuckstazyLive.app;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using Framework.utils;

namespace DuckstazyLive.game
{
    public class Hero
    {
        // consts

        private const int duck_w = 27;
        private const int duck_h = 20;
        private const int duck_w2 = 54;
        private const int duck_h2 = 40;

        // duck logic consts
        private const float duck_jump_start_vel_min = 127;
        private const float duck_jump_start_vel_max = 379;
        //private const duck_jump_start_vel_min:float = 480;
        //private const duck_jump_start_vel_max:float = 480;
        private const float duck_jump_gravity = 200;
        //private const duck_jump_gravity:float = 320;
        private const float duck_jump_toxic = 100;

        private const float duck_move_speed_min = 40;
        private const float duck_move_speed_max = 250;
        private const float duck_move_acc = 5;
        private const float duck_move_slowing = 10;
        private const float duck_move_slowing_in_the_sky = 1;

        private const float duck_wings_limit = -20;
        private const float duck_wings_bonus = 60;

        // Vars
        private bool key_up;
        private bool key_right;
        private bool key_left;
        private bool key_down;

        public float x;
        public float y;
        public float xLast;
        public float yLast;
        private float dx;
        private float dy;
        public bool flip;
        public bool sleep;
        private bool started;

        private float power;

        public float jumpVel;
        private float jumpWingVel;
        private float jumpStartVel;
        //private var gravityK:float;
        public float diveK;

        private float wingAngle;
        private float wingMod;
        private float wingCounter;
        private bool wingLock;
        private float wingY;
        private bool wingYLocked;

        private bool fly;
        private float move;
        private float slow;
        private float step;
        private bool steping;

        private float blinkTime;

        public int sleep_collected;
        public int toxic_collected;
        public int frags;

        public HeroMedia media;
        public GameState state;
        public Particles particles;
        public Env env;

        private float get_jump_start_vel(N x)
        {
            return utils.lerp(x, duck_jump_start_vel_min, duck_jump_start_vel_max);
        }

        public function Hero()
        {
            media = new HeroMedia();

            flip = true;
            sleep = false;
            started = false;
        }

        public void init()
        {
            move = 0.0;
            step = 0.0;

            keysReset();

            fly = false;
            sleep = false;
            started = false;

            jumpVel = 0.0;
            jumpStartVel = 0.0;
            jumpWingVel = 0.0;
            //gravityK = 1.0;
            diveK = 0.0;

            wingMod = 0.0;
            wingCounter = 0.0;
            wingAngle = 0.0;
            wingYLocked = false;

            blinkTime = 0.0;

            y = 400 - duck_h2;
            x = 0;

            power = 0.0;

            sleep_collected = 0;
            toxic_collected = 0;
            frags = 0;
        }

        private void doStepBubble()
        {
            float px = x + 4.0;
            if (!flip) px = x + 50.0;
            particles.startStepBubble(px, y + 39.0);
        }

        private void doLandBubbles()
        {
            float px;
            int i = Math.abs(jumpVel * 0.05);

            while (i > 0)
            {
                px = x + 17.0 + Math.random() * 20.0;
                particles.startBubble(px, y + duck_h2, 0xff999999);
                --i;
            }
        }

        public void update(N dt, N newPower)
        {
            if (!started) return;

            xLast = x;
            yLast = y;

            media.updateSFX(x + duck_w);

            power = newPower;

            jumpStartVel = get_jump_start_vel(power);

            if (sleep && power <= 0)
            {
                sleep = false;
                startSleepParticles();

                media.playAwake();
            }

            if (blinkTime > 0.0)
                blinkTime -= dt * 8.0;

            steping = false;

            if (key_left)
            {
                steping = true;
                flip = false;
                move -= duck_move_acc * dt;
                if (move < -1)
                    move = -1;
            }
            if (key_right)
            {
                steping = true;
                flip = true;
                move += duck_move_acc * dt;
                if (move > 1)
                    move = 1;
            }


            if (steping)
            {
                if (move >= 0.0) step += move * dt * 15.0;
                else step -= move * dt * 15.0;

                if (step > 2)
                {
                    step -= 2;
                    if (!fly)
                    {
                        media.playStep();
                        doStepBubble();
                    }
                }
            }

            if (!key_left && !key_right)
            {
                if (fly)
                    slow = duck_move_slowing_in_the_sky;
                else
                    slow = duck_move_slowing;

                move -= move * slow * dt;
                if (step > 1.0)
                {
                    if (!fly)
                        media.playStep();

                    step = 0.0;
                }
            }

            x += move * utils.lerp(power, duck_move_speed_min, duck_move_speed_max) * dt;


            if (x < -duck_w)
                x += 640.0;
            if (x > (640.0 - duck_w))
                x -= 640.0;

            if (wingLock && !sleep)
            {
                wingMod -= dt * 7.0;
                if (wingMod <= 0.0)
                {
                    wingMod += 1.0;
                    wingBeat();
                }


                if (wingYLocked && y > wingY)
                {
                    jumpWingVel = 28.0;
                }
            }

            if (fly)
            {
                if (key_down)
                {
                    diveK += dt * 6;
                    if (diveK > 3.0) diveK = 3.0;
                }
                else
                {
                    diveK -= dt * 6;
                    if (diveK < 0.0) diveK = 0.0;
                }

                if (jumpVel > 0.0)
                    wingYLocked = false;

                if (wingLock && !sleep && wingYLocked)
                {
                    jumpWingVel -= 392.0 * dt;//(gravityK+diveK)*dt;
                    y -= jumpWingVel * dt;
                }
                else
                {
                    if (wingLock && !sleep)
                    {
                        if (jumpVel >= 0.0)
                        {
                            jumpVel -= duck_jump_gravity * (diveK + 1.0) * dt;
                            y -= jumpVel * dt;
                            if (jumpVel <= 0.0)
                            {
                                wingYLocked = true;
                                wingY = y;
                            }
                        }
                        else
                        {
                            jumpVel += 5.0 * duck_jump_gravity * dt;
                            y -= jumpVel * dt;
                        }
                    }
                    else
                    {
                        jumpVel -= duck_jump_gravity * (diveK + 1.0) * dt;
                        y -= jumpVel * dt;
                    }
                }


                if (y >= 400 - duck_h2)
                {
                    wingLock = false;
                    fly = false;
                    y = 400 - duck_h2;

                    media.playLand();
                    //utils.playSound(land_snd, (power+0.3)*Math.abs(jumpVel)/200.0, pos.x+27);

                    doLandBubbles();

                    sleep_collected = 0;
                    toxic_collected = 0;
                    frags = 0;
                    diveK = 0.0;
                }
                else if (y < -50.0)
                    y = -50.0;
            }

            if (wingCounter > 0)
            {
                wingCounter -= 10 * dt;
                if (wingCounter < 0)
                    wingCounter = 0;

                wingAngle = 0.5 * Math.sin(wingCounter * 4.71);
            }
        }

        private void wingBeat()
        {
            /*if(jumpVel<duck_wings_limit)
            {
                jumpVel+=duck_wings_bonus;
                gravityK = (jumpVel + jumpStartVel)/(jumpStartVel*2 - jumpVel);
            }*/
            wingCounter = 1.0;

            media.playWing();
        }

        public void draw(B canvas)
        {
            if (started)
            {
                //dx = int(x);
                //dy = int(y);
                dx = x;
                dy = y;


                if (step > 1 && !fly)
                    dy -= 1.0;

                drawHero(canvas, dx, dy);
                if (dx < 0)
                {
                    dx += 640;
                    drawHero(canvas, dx, dy);
                }
                else if (dx > 640.0 - duck_w2)
                {
                    dx -= 640;
                    drawHero(canvas, dx, dy);
                }
            }
        }

        private void drawHero(B dest, N _x, N _y)
		{
			bool vis = (blinkTime<=0.0 || int(blinkTime)&0x1!=0);
			
			if(vis)
			{
				if(sleep)
					media.drawSleep(dest, _x, _y, flip);
				else
					media.drawDuck(dest, _x, _y, power, flip, wingAngle);
			}
		}

        public void keyDown(u keyCode)
        {
            switch (keyCode)
            {
                case 40: key_down = true; break;
                case 37: key_left = true; break;
                case 38:
                    if (!key_up && started)
                    {
                        if (!fly)
                        {
                            if (!sleep)
                            {
                                fly = true;
                                jumpVel = jumpStartVel;
                                //gravityK = 1;

                                media.playJump();
                                doLandBubbles();
                            }
                        }
                        else if (!wingLock && !sleep)
                        {
                            wingLock = true;
                            wingMod = 1.0;
                            wingYLocked = false;

                            wingBeat();
                        }
                    }
                    key_up = true; break;
                case 39: key_right = true; break;
            }
        }

        public void keyUp(u keyCode)
        {
            switch (keyCode)
            {
                case 40: key_down = false; break;
                case 37: key_left = false; break;
                case 38:
                    if (key_up && started)
                    {
                        if (fly)
                        {
                            if (wingLock)
                            {
                                wingLock = false;
                                if (jumpVel < 0.0)
                                    jumpVel = 0.0;
                            }

                            //if(jumpVel>0 && gravityK==1)
                            //gravityK = (jumpVel + jumpStartVel)/(jumpStartVel*2 - jumpVel);
                        }
                    }
                    key_up = false; break;
                case 39: key_right = false; break;
            }
        }

        public void doSleep()
        {
            if (!sleep)
            {
                startSleepParticles();
                media.playSleep();
                sleep = true;
                //wingLock = false;
            }
            //mBoard->HitSleep(world_draw_pos(mPosition), mSleepCollected);
            ++sleep_collected;
        }

        private void startSleepParticles()
        {
            int i = 25;
            float px;
            float py;

            while (i > 0)
            {
                px = x + Math.random() * duck_w2;
                py = y + Math.random() * duck_h2;
                particles.startBubble(px, py, 0xff690c7a);
                --i;
            }
        }

        public bool checkDive(N cx, N cy)
        {
            bool check;
            float px = cx;
            float py = cy;

            if (x < 0.0 && px > (630.0 - duck_w2))
                px -= 640.0;

            if (flip)
                px = 2.0 * (x + duck_w) - px;

            check = fly && (yLast + duck_h2) <= py && (y + duck_h2) >= (py - 10.0);

            if (sleep)
            {
                check = check &&
                    px >= x + 1.0 - 9.0 &&
                    px <= x + 43.0 + 9.0;
            }
            else
            {
                check = check &&
                    px >= x + 14.0 - 9.0 &&
                    px <= x + 50.0 + 9.0;
            }

            return check;
        }

        public int doToxicDamage(N cx, N cy, i dmg, i id)
		{
			int dam = dmg - int(dmg*state.def/100);
			int ret = -1;
				
			if(checkDive(cx, cy))
			{
				//jumpVel = duck_jump_toxic;
				jump(40.0);
				//mBoard->KillToxic(world_draw_pos(ToxicPosition), mKills);
				media.sndAttack.play(49.0);
				particles.explStarsToxic(cx, cy-10.0, id, false);
				if(frags<3) ret = 0;
				else
				{
					if(frags<6) ret = 1;
					else ret = 2;
				}
				++frags;
			}
			else if(dam>0)
			{
				if(blinkTime<=0.0)
				{
					if(state.health>1)
					{
						state.health-=dam;
						if(state.health<1)
							state.health = 1;
					}
					else
						state.health = 0;

					blinkTime = 12.0;
					particles.explStarsToxic(cx, cy, id, true);
					env.blanc = 1.0;
					media.sndToxic.play();
				}
				else
				{
					particles.explStarsToxic(cx, cy, id, false);
				}
				
				//mBoard->HitToxic(world_draw_pos(ToxicPosition), mToxicCollected);
				++toxic_collected;
			}
			
			return ret;
		}

        public bool doHigh(N cx, N cy)
        {
            bool succ = false;
            if (checkDive(cx, cy))
            {
                //jumpVel = duck_jump_toxic;
                jump(40.0);
                //media.playJump();
                succ = true;
            }
            return succ;
        }

        public void doHeal(i health)
        {
            state.health += health;

            if (state.health > state.maxHP)
                state.health = state.maxHP;

            particles.explHeal(x, y);
        }

        public void jump(N h)
        {
            float new_vy = Math.sqrt(2.0 * duck_jump_gravity * h);
            if (jumpVel < new_vy)
                jumpVel = new_vy;
        }

        public float getJumpHeight()
        {
            return jumpStartVel * jumpStartVel * 0.5 / duck_jump_gravity;
        }

        public bool overlapsCircle(N cx, N cy, N r)
        {
            bool over = false;

            if (x < 0.0 && cx > (630.0 - duck_w2))
                cx -= 640.0;

            if (flip)
                cx = 2.0 * (x + duck_w) - cx;

            if (sleep)
            {
                over = rectCircle(x + 1.0, y + 11.0, x + 41.0, y + 39.0, cx, cy, r);
            }
            else
            {
                over = rectCircle(x + 14.0, y + 13.0, x + 49.0, y + 38.0, cx, cy, r) ||
                    rectCircle(x + 9.0, y + 1.0, x + 24.0, y + 17.0, cx, cy, r) ||
                    rectCircle(x + 1.0, y + 13.0, x + 8.0, y + 17.0, cx, cy, r);
            }

            return over;
        }

        // Arvo's algorithm.
        private bool rectCircle(N x1, N y1, N x2, N y2, N cx, N cy, N r)
        {
            float s = 0.0;
            float d = 0.0;

            //find the square of the distance
            //from the sphere to the box
            if (cx < x1)
            {
                s = cx - x1;
                d += s * s;
            }
            else if (cx > x2)
            {
                s = cx - x2;
                d += s * s;
            }

            if (cy < y1)
            {
                s = cy - y1;
                d += s * s;
            }
            else if (cy > y2)
            {
                s = cy - y2;
                d += s * s;
            }

            return d <= r * r;
        }

        public void start(N _x)
        {
            started = true;
            x = _x;
            flip = Math.random() < 0.5;
            startSleepParticles();
            media.playAwake();
        }

        public void keysReset()
        {
            key_up = false;
            key_right = false;
            key_left = false;
            key_down = false;
            wingLock = false;
        }
    }
}
