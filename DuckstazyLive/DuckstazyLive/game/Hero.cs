using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using DuckstazyLive.app;
using Framework.core;

namespace DuckstazyLive.game
{
    public class Hero
    {
        // duck logic consts
        private const float duck_jump_start_vel_min = 127;
        private const float duck_jump_start_vel_max = 379;
        private const float duck_rapid_jump_delay = 0.1f;

        private const float duck_jump_gravity = 200;
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
        private bool controlledByStick;
        private float stickMoveCoeff;

        public const int duck_w = 27;
        public const int duck_h = 20;
        public const int duck_w2 = 54;
        public const int duck_h2 = 40;

        private const float STICK_HOR_THRESHOLD = 0.1f;
        private const float STICK_VER_THRESHOLD = 0.7f;
        private const float STICK_VER_THRESHOLD_DIVE = 0.9f;

        private bool rapidJump;
        private float jumpButtonPressedStartTime;        

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
        
        public GameState state;
        private Heroes heroes;

        private int playerIndex;
       
        public Hero(Heroes heroes, int playerIndex)
        {
            this.heroes = heroes;
            this.playerIndex = playerIndex;

            flip = true;
            sleep = false;
            started = false;
        }

        public void init()
        {
            move = 0.0f;
            step = 0.0f;

            keysReset();

            fly = false;
            sleep = false;
            started = false;

            jumpVel = 0.0f;
            jumpStartVel = 0.0f;
            jumpWingVel = 0.0f;            
            diveK = 0.0f;

            wingMod = 0.0f;
            wingCounter = 0.0f;
            wingAngle = 0.0f;
            wingYLocked = false;

            blinkTime = 0.0f;

            y = 400 - duck_h2;
            x = 0;

            power = 0.0f;

            sleep_collected = 0;
            toxic_collected = 0;
            frags = 0;
        }

        private void doStepBubble()
        {
            float px = x + 4;
            if (!flip) px = x + 50;
            heroes.particles.startStepBubble(px, y + 39);
        }

        private void doLandBubbles()
        {
            float px;
            int i = (int)Math.Abs(jumpVel * 0.05f);

            while (i > 0)
            {
                px = x + 17 + utils.rnd() * 20;
                heroes.particles.startBubble(px, y + duck_h2, 0xff999999);
                --i;
            }
        }        

        public void update(float dt, float newPower)
        {
            if (!started) return;

            updateGamepadInput();

            xLast = x;
            yLast = y;

            heroes.media.updateSFX(x + duck_w);

            power = newPower;

            jumpStartVel = get_jump_start_vel(power);

            if (sleep && power <= 0)
            {
                sleep = false;
                startSleepParticles();

                heroes.media.playAwake();
            }

            if (blinkTime > 0.0f)
                blinkTime -= dt * 8.0f;

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
                if (controlledByStick)
                    move *= stickMoveCoeff;

                if (move >= 0.0f) step += move * dt * 15.0f;
                else step -= move * dt * 15.0f;

                if (step > 2)
                {
                    step -= 2;
                    if (!fly)
                    {
                        heroes.media.playStep();
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
                if (step > 1.0f)
                {
                    if (!fly)
                        heroes.media.playStep();

                    step = 0.0f;
                }
            }

            x += move * utils.lerp(power, duck_move_speed_min, duck_move_speed_max) * dt;


            if (x < -duck_w)
                x += 640.0f;
            if (x > (640.0f - duck_w))
                x -= 640.0f;

            if (wingLock && !sleep)
            {
                wingMod -= dt * 7.0f;
                if (wingMod <= 0.0f)
                {
                    wingMod += 1.0f;
                    wingBeat();
                }


                if (wingYLocked && y > wingY)
                {
                    jumpWingVel = 28.0f;
                }
            }

            if (fly)
            {
                if (key_down)
                {
                    diveK += dt * 6;
                    if (diveK > 4.0f) diveK = 4.0f;
                }
                else
                {
                    diveK -= dt * 6;
                    if (diveK < 0.0f) diveK = 0.0f;
                }

                if (rapidJump)
                {
                    if (jumpVel > 0.0f)
                        jumpVel = 0.7f * jumpVel;
                    rapidJump = false;
                }

                if (jumpVel > 0.0f)
                    wingYLocked = false;

                if (wingLock && !sleep && wingYLocked)
                {
                    jumpWingVel -= 392.0f * dt;//(gravityK+diveK)*dt;
                    y -= jumpWingVel * dt;
                }
                else
                {
                    if (wingLock && !sleep)
                    {
                        if (jumpVel >= 0.0f)
                        {
                            jumpVel -= duck_jump_gravity * (diveK + 1.0f) * dt;
                            y -= jumpVel * dt;
                            if (jumpVel <= 0.0f)
                            {
                                wingYLocked = true;
                                wingY = y;
                            }
                        }
                        else
                        {
                            jumpVel += 5.0f * duck_jump_gravity * dt;
                            y -= jumpVel * dt;
                        }
                    }
                    else
                    {
                        jumpVel -= duck_jump_gravity * (diveK + 1.0f) * dt;
                        y -= jumpVel * dt;
                    }
                }


                if (y >= 400 - duck_h2)
                {
                    wingLock = false;
                    fly = false;
                    y = 400 - duck_h2;

                    heroes.media.playLand();
                    //utils.playSound(land_snd, (power+0.3)*Math.abs(jumpVel)/200.0, pos.x+27);

                    doLandBubbles();

                    sleep_collected = 0;
                    toxic_collected = 0;
                    frags = 0;
                    diveK = 0.0f;
                }
                else if (y < -50.0f)
                    y = -50.0f;
            }

            if (wingCounter > 0)
            {
                wingCounter -= 10 * dt;
                if (wingCounter < 0)
                    wingCounter = 0;

                wingAngle = 0.5f * ((float)Math.Sin(wingCounter * 4.71f));
            }
        }

        private void updateGamepadInput()
        {
            Vector2 leftStick = Application.sharedInputMgr.ThumbSticks(playerIndex).Left;            

            if (controlledByStick)
            {
                key_left = key_right = key_down = false;
                controlledByStick = false;
            }
            if (leftStick.X > STICK_HOR_THRESHOLD)
            {
                controlledByStick = true;
                key_right = true;
                stickMoveCoeff = getStickMoveCoeff(ref leftStick);
            }
            else if (leftStick.X < -STICK_HOR_THRESHOLD)
            {
                controlledByStick = true;
                key_left = true;
                stickMoveCoeff = getStickMoveCoeff(ref leftStick);
            }            
            if (leftStick.Y < -STICK_VER_THRESHOLD)
            {
                controlledByStick = true;
                key_down = true;
            }
        }

        private float getStickMoveCoeff(ref Vector2 stickPos)
        {
            if (stickPos.Y < -STICK_VER_THRESHOLD)
            {
                return (float) Math.Sqrt(Math.Abs(stickPos.X));
            }
            else
            {            
                float len = stickPos.Length();
                return len;
            }
        }

        private void wingBeat()
        {
            /*if(jumpVel<duck_wings_limit)
            {
                jumpVel+=duck_wings_bonus;
                gravityK = (jumpVel + jumpStartVel)/(jumpStartVel*2 - jumpVel);
            }*/
            wingCounter = 1.0f;

            heroes.media.playWing();
        }

        public void draw(Canvas canvas)
        {
            if (started)
            {                
                dx = x;
                dy = y;


                if (step > 1 && !fly)
                    dy -= 1.0f;
                
                float alpha = 1.0f;
                if (dx < 0)
                {
                    alpha = 1.0f - Math.Abs(dx) / duck_w2;                    
                    drawHero(canvas, dx + 640, dy, 1.0f - alpha);
                }
                else if (dx > 640 - duck_w2)
                {
                    alpha = 1.0f - (dx - 640 + duck_w2) / duck_w2;                    
                    drawHero(canvas, dx - 640, dy, 1.0f - alpha);
                }
                drawHero(canvas, dx, dy, alpha);                
            }
        }

        private void drawHero(Canvas dest, float x, float y, float trans)
		{
			bool vis = (blinkTime<=0.0f || (((int)blinkTime)&0x1)!=0);
			
			if(vis)
			{
				if(sleep)
					heroes.media.drawSleep(dest, x, y, flip, trans);
				else
					heroes.media.drawDuck(playerIndex, dest, x, y, power, flip, wingAngle, trans);
			}
		}

        public void buttonPressed(ref ButtonEvent e)
        {
            switch (e.button)
            {
                case Buttons.DPadDown:
                    key_down = true;
                    break;

                case Buttons.DPadLeft:
                    key_left = true;
                    break;;

                case Buttons.A:
                    if (!key_up && started)
                    {
                        if (!fly)
                        {
                            if (!sleep)
                            {
                                fly = true;
                                jumpVel = jumpStartVel;
                                jumpButtonPressedStartTime = GameClock.ElapsedTime;

                                heroes.media.playJump();
                                doLandBubbles();
                            }
                        }
                        else if (!wingLock && !sleep)
                        {
                            wingLock = true;
                            wingMod = 1.0f;
                            wingYLocked = false;

                            wingBeat();
                        }
                    }
                    key_up = true;
                    break;

                case Buttons.DPadRight:
                    key_right = true;
                    break;;
            }
        }

        public void buttonReleased(ref ButtonEvent e)
        {       
            switch (e.button)
            {
                case Buttons.DPadDown: 
                    key_down = false;
                    break;

                case Buttons.DPadLeft: 
                    key_left = false;
                    break;

                case Buttons.A:
                    if (key_up && started)
                    {
                        if (fly)
                        {
                            rapidJump = GameClock.ElapsedTime - jumpButtonPressedStartTime < duck_rapid_jump_delay;

                            if (wingLock)
                            {
                                wingLock = false;
                                if (jumpVel < 0.0f)
                                    jumpVel = 0.0f;
                            }                            

                            //if(jumpVel>0 && gravityK==1)
                            //gravityK = (jumpVel + jumpStartVel)/(jumpStartVel*2 - jumpVel);
                        }
                    }
                    key_up = false;
                    break;;

                case Buttons.DPadRight: 
                    key_right = false;
                    break;;
            }            
        }

        public void doSleep()
        {
            if (!sleep)
            {
                startSleepParticles();
                heroes.media.playSleep();
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
                px = x + utils.rnd() * duck_w2;
                py = y + utils.rnd() * duck_h2;
                heroes.particles.startBubble(px, py, 0xff690c7a);
                --i;
            }
        }

        public bool checkDive(float cx, float cy)
        {
            bool check;
            float px = cx;
            float py = cy;

            if (x < 0.0f && px > (630 - duck_w2))
                px -= 640;

            if (flip)
                px = 2 * (x + duck_w) - px;

            check = fly && (yLast + duck_h2) <= py && (y + duck_h2) >= (py - 10);

            if (sleep)
            {
                check = check &&
                    px >= x + 1 - 9 &&
                    px <= x + 43 + 9;
            }
            else
            {
                check = check &&
                    px >= x + 14 - 9 &&
                    px <= x + 50 + 9;
            }

            return check;
        }

        public int doToxicDamage(float cx, float cy, int dmg, int id)
		{
			int dam = dmg - (int)(dmg*state.def/100);
			int ret = -1;
				
			if(checkDive(cx, cy))
			{
				//jumpVel = duck_jump_toxic;
				jump(40);
				//mBoard->KillToxic(world_draw_pos(ToxicPosition), mKills);
				// media.sndAttack.play(49);
                Application.sharedSoundMgr.playSound(heroes.media.sndAttack);
				heroes.particles.explStarsToxic(cx, cy-10, id, false);
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
				if(blinkTime<=0.0f)
				{
					if(state.health>1)
					{
						state.health-=dam;
						if(state.health<1)
							state.health = 1;
					}
					else
						state.health = 0;

					blinkTime = 12;
					heroes.particles.explStarsToxic(cx, cy, id, true);
					heroes.env.blanc = 1.0f;
					// media.sndToxic.play();
                    Application.sharedSoundMgr.playSound(heroes.media.sndToxic);
				}
				else
				{
					heroes.particles.explStarsToxic(cx, cy, id, false);
				}
				
				//mBoard->HitToxic(world_draw_pos(ToxicPosition), mToxicCollected);
				++toxic_collected;
			}
			
			return ret;
		}

        public bool doHigh(float cx, float cy)
        {
            bool succ = false;
            if (checkDive(cx, cy))
            {
                //jumpVel = duck_jump_toxic;
                jump(40);
                //media.playJump();
                succ = true;
            }
            return succ;
        }

        public void doHeal(int health)
        {
            state.health += health;

            if (state.health > state.maxHP)
                state.health = state.maxHP;

            heroes.particles.explHeal(x, y);
        }

        public void jump(float h)
        {
            float new_vy = (float)Math.Sqrt(2 * duck_jump_gravity * h);
            if (jumpVel < new_vy)
                jumpVel = new_vy;
        }

        public float getJumpHeight()
        {
            return jumpStartVel * jumpStartVel * 0.5f / duck_jump_gravity;
        }

        public bool overlapsCircle(float cx, float cy, float r)
        {
            bool over = false;

            if (x < 0.0f && cx > (630 - duck_w2))
                cx -= 640;

            if (flip)
                cx = 2 * (x + duck_w) - cx;

            if (sleep)
            {
                over = rectCircle(x + 1, y + 11, x + 41, y + 39, cx, cy, r);
            }
            else
            {
                over = rectCircle(x + 14, y + 13, x + 49, y + 38, cx, cy, r) ||
                    rectCircle(x + 9, y + 1.0f, x + 24, y + 17, cx, cy, r) ||
                    rectCircle(x + 1.0f, y + 13, x + 8, y + 17, cx, cy, r);
            }

            return over;
        }

        // Arvo's algorithm.
        private bool rectCircle(float x1, float y1, float x2, float y2, float cx, float cy, float r)
        {
            float s = 0.0f;
            float d = 0.0f;

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

        public void start(float _x)
        {
            started = true;
            x = _x;
            flip = utils.rnd() < 0.5;
            startSleepParticles();
            heroes.media.playAwake();
        }

        public void keysReset()
        {
            key_up = false;
            key_right = false;
            key_left = false;
            key_down = false;
            wingLock = false;
            controlledByStick = false;
            jumpButtonPressedStartTime = 0.0f;
            rapidJump = false;
            stickMoveCoeff = 0.0f;
        }

        private float get_jump_start_vel(float x)
        {
            return utils.lerp(x, duck_jump_start_vel_min, duck_jump_start_vel_max);
        }        
    }
}
