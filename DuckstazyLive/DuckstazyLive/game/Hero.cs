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
        private const int width = 2 * 53;
        private const int height = 2 * 40;

        // duck logic consts
        private static readonly float jumpStarVelMin = 2 * 127;
        private static readonly float jumpStartVelMax = 2 * 379;

        private static readonly float gravity = 2 * 200;
        private static readonly float duck_jump_toxic = 2 * 100;

        private static readonly float vxMin = 2 * 40;
        private static readonly float vxMax = 2 * 250;
        private static readonly float slowdownSky = 2;

        private static readonly float ax = 10;
        private static readonly float slowdownGround = 2 * 10;
        private static readonly int STEP_DISTANCE_MAX = 4;

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
        private float vx;        
        private float steppingDistance;
        private bool fly;

        public Hero()
        {            
            origin = new Vector2(width / 2.0f, 0);
            position = new Vector2(0, 0);

            x = (App.Width - width) / 2;
            y = App.Height - Constants.GROUND_HEIGHT -height;            
        }

        public void Draw(SpriteBatch batch)
        {            
            float dx = x;
            float dy = y;                     

            if (steppingDistance > 2 && !fly)
            {
                dy -= 2.0f;
            }

            Draw(batch, dx, dy);
            if (dx < 0)
            {
                dx += App.Width;
                Draw(batch, dx, dy);
            }
            else if (dx > App.Width - width)
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

            jumpStartVel = GetJumpStartVy(power);                       

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
                            jumpVel -= gravity * (diveK + 1.0f) * dt;
                            y -= jumpVel * dt;
                            if (jumpVel <= 0.0)
                            {
                                wingYLocked = true;
                                wingY = y;
                            }
                        }
                        else
                        {
                            jumpVel += 5.0f * gravity * dt;
                            y -= jumpVel * dt;
                        }
                    }
                    else
                    {
                        jumpVel -= gravity * (diveK + 1.0f) * dt;
                        y -= jumpVel * dt;
                    }
                }
                
                if (y >= App.Height - Constants.GROUND_HEIGHT - height)
                {
                    wingLock = false;
                    fly = false;
                    y = App.Height - Constants.GROUND_HEIGHT - height;

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
                vx -= ax * dt;
                if (vx < -1)
                    vx = -1;
            }
            if (key_right)
            {
                steping = true;
                flip = true;
                vx += ax * dt;
                if (vx > 1)
                    vx = 1;
            }

            if (steping)
            {
                if (vx >= 0.0) 
                    steppingDistance += vx * dt * 2 * 15.0f;
                else 
                    steppingDistance -= vx * dt * 2 * 15.0f;

                if (steppingDistance > STEP_DISTANCE_MAX)
                {
                    steppingDistance -= STEP_DISTANCE_MAX;
                    if (!fly)
                    {
                        //media.playStep();
                        //doStepBubble();
                    }
                }
            }

            if (!key_left && !key_right)
            {
                float slow = fly ? slowdownSky : slowdownGround;
                vx -= vx * slow * dt;
                if (steppingDistance > STEP_DISTANCE_MAX)
                {
                    //if (!fly)
                    //    media.playStep();

                    steppingDistance = 0.0f;
                }
            }

            x += vx * Utils.lerp(power, vxMin, vxMax) * dt;
            
            if (x < -width)
                x += App.Width;
            if (x > (App.Width - width))
                x -= App.Width;
        }

        private float GetJumpStartVy(float x)
		{
			return Utils.lerp(x, jumpStarVelMin, jumpStartVelMax);
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
