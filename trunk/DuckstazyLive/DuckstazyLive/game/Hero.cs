using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using DuckstazyLive.app;
using DuckstazyLive.core.input;

namespace DuckstazyLive
{
    class Hero : InputAdapter
    {
        // consts                
        private const int duck_w2 = 2 * 53;
        private const int duck_h2 = 2 * 40;

        // duck logic consts
        private const float duck_jump_start_vel_min = 2 * 127;
        private const float duck_jump_start_vel_max = 2 * 379;

        private const float duck_jump_gravity = 2 * 200;
        private const float duck_jump_toxic = 2 * 100;

        private const float duck_move_speed_min = 2 * 40;
        private const float duck_move_speed_max = 2 * 250;
        private const float duck_move_acc = 2 * 5;
        private const float duck_move_slowing = 2 * 10;
        private const float duck_move_slowing_in_the_sky = 2 * 1;

        private const float duck_wings_limit = -20;
        private const float duck_wings_bonus = 60;

        private bool key_left;
        private bool key_right;
        private bool key_up;
        private bool key_down;        
        
        // Vars

        private float x;
        private float y;

        private float xLast;
        private float yLast;
        private float power;
        private bool sleep;
        private bool wingLock;
        private float wingMod;
        private bool wingYLocked;
        private float wingY;
        private float jumpWingVel;
        private float diveK;
        private float jumpVel;
        private float jumpStartVel;
        private float wingCounter;
        private float wingAngle;
        
        private Vector2 position;
        private Vector2 origin;

        private bool steping;
        private bool flip;
        private float move;
        private float slow;
        private float step;
        private bool fly;

        public Hero()
        {            
            origin = new Vector2(duck_w2 / 2.0f, 0);
            position = new Vector2(0, 0);

            x = (App.Width - duck_w2) / 2;
            y = App.Height - Constants.GROUND_HEIGHT -duck_h2;            
        }

        public void Draw(SpriteBatch batch)
        {            
            float dx = x;
            float dy = y;                     

            if (step > 2 && !fly)
            {
                dy -= 2.0f;
            }

            Draw(batch, dx, dy);
            if (dx < 0)
            {
                dx += App.Width;
                Draw(batch, dx, dy);
            }
            else if (dx > App.Width - duck_w2)
            {
                dx -= App.Width;
                Draw(batch, dx, dy);
            }
        }

        private void Draw(SpriteBatch batch, float x, float y)
        {
            Texture2D duck = Resources.GetTexture(Res.IMG_DUCK);
            position.X = x + duck.Width / 2;
            position.Y = y;
            batch.Draw(duck, position, null, Color.White, 0.0f, origin, 1.0f, flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0.0f);
        }

        public void Update(float dt)
        {
            xLast = x;
            yLast = y;

            //media.updateSFX(x + duck_w);

            //power = newPower;

            jumpStartVel = get_jump_start_vel(power);                       

            if (sleep && power <= 0)
            {
                sleep = false;
                //startSleepParticles();

                //media.playAwake();
            }

            //if (blinkTime > 0.0)
            //    blinkTime -= dt * 8.0;

            steping = false;

            updateHorizontalSpeed(dt);
            

            if (wingLock && !sleep)
            {
                wingMod -= dt * 7.0f;
                if (wingMod <= 0.0)
                {
                    wingMod += 1.0f;
                    //wingBeat();
                }


                if (wingYLocked && y > wingY)
                {
                    jumpWingVel = 2 * 28.0f;
                }
            }

            if (fly)
            {
                if (key_down)
                {
                    diveK += dt * 6;
                    if (diveK > 3.0f) diveK = 3.0f;
                }
                else
                {
                    diveK -= dt * 6;
                    if (diveK < 0.0f) diveK = 0.0f;
                }

                if (jumpVel > 0.0f)
                    wingYLocked = false;

                if (wingLock && !sleep && wingYLocked)
                {
                    jumpWingVel -= 2 * 392.0f * dt;//(gravityK+diveK)*dt;
                    y -= jumpWingVel * dt;
                }
                else
                {
                    if (wingLock && !sleep)
                    {
                        if (jumpVel >= 0.0)
                        {
                            jumpVel -= duck_jump_gravity * (diveK + 1.0f) * dt;
                            y -= jumpVel * dt;
                            if (jumpVel <= 0.0)
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
                
                if (y >= App.Height - Constants.GROUND_HEIGHT - duck_h2)
                {
                    wingLock = false;
                    fly = false;
                    y = App.Height - Constants.GROUND_HEIGHT - duck_h2;

                    //media.playLand();

                    //doLandBubbles();

                    //sleep_collected = 0;
                    //toxic_collected = 0;
                    //frags = 0;
                    diveK = 0.0f;
                }
                else if (y < -2 * 50.0f)
                    y = -2 * 50.0f;
            }

            //if (wingCounter > 0)
            //{
            //    wingCounter -= 10 * dt;
            //    if (wingCounter < 0)
            //        wingCounter = 0;

            //    wingAngle = 0.5 * Math.sin(wingCounter * 4.71);
            //}                       
        }

        private void updateHorizontalSpeed(float dt)
        {
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
                if (move >= 0.0) step += move * dt * 2 * 15.0f;
                else step -= move * dt * 2 * 15.0f;

                if (step > 4)
                {
                    step -= 4;
                    if (!fly)
                    {
                        //media.playStep();
                        //doStepBubble();
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
                if (step > 2.0)
                {
                    //if (!fly)
                    //    media.playStep();

                    step = 0.0f;
                }
            }

            x += move * Utils.lerp(power, duck_move_speed_min, duck_move_speed_max) * dt;
            
            if (x < -duck_w2)
                x += App.Width;
            if (x > (App.Width - duck_w2))
                x -= App.Width;
        }

        private float get_jump_start_vel(float x)
		{
			return Utils.lerp(x, duck_jump_start_vel_min, duck_jump_start_vel_max);
		}

        public override void ButtonUp(Buttons button)
        {
            switch (button)
            {
                case Buttons.A:
                {
                    if (key_up)
                    {
                        if (fly)
                        {
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
                }
                break;

                case Buttons.DPadRight:
                    key_right = false;
                break;

                case Buttons.DPadLeft:                
                    key_left = false;                
                break;

                case Buttons.DPadDown:                
                    key_down = false;                
                break;
            }
            
        }

        public override void ButtonDown(Buttons button)
        {
            switch (button)
            {
                case Buttons.A:
                    {
                        if (!key_up)
                        {
                            if (!fly)
                            {
                                if (!sleep)
                                {
                                    fly = true;
                                    jumpVel = jumpStartVel;
                                    //gravityK = 1;

                                    //media.playJump();
                                    //doLandBubbles();
                                }
                            }
                            else if (!wingLock && !sleep)
                            {
                                wingLock = true;
                                wingMod = 1.0f;
                                wingYLocked = false;

                                //wingBeat();
                            }
                        }
                        key_up = true;
                    }
                    break;

                case Buttons.DPadRight:
                    key_right = true;         
                break;

                case Buttons.DPadLeft:                
                    key_left = true;                
                break;

                case Buttons.DPadDown:                
                    key_down = true;                    
                break;
            }
        }              

        private Application App
        {
            get { return Application.Instance; }
        }
    }    
}
