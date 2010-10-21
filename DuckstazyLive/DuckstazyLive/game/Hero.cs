using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.core;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using DuckstazyLive.app;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using DuckstazyLive.game.utils;
using Framework.utils;

namespace DuckstazyLive.game
{
    public interface HeroListener
    {
        void heroPowerChanged(float oldPower, float newPower);
    }

    public class Hero : BaseElement, InputListener
    {
        private static readonly int STEP_BUBBLE_OFFSET_X = 20;
        private static readonly int STEP_BUBBLE_OFFSET_Y = -5;       
        

        private readonly float JUMP_START_VY_MIN;
        private readonly float JUMP_START_VY_MAX;
        public readonly float JUMP_HEIGHT_MIN;
        public readonly float JUMP_HEIGHT_MAX;

        private static readonly float ACC_X = 10; // ускорение по оси oX
        private static readonly float ACC_Y = 400; // ускорение по оси oY       

        private static readonly float Y_MIN = 0;

        // TODO: add y_max

        private static readonly float VX_MIN = 80;
        private static readonly float VX_MAX = 500;

        private static readonly float SLOWDOWN_SKY = 2;
        private static readonly float SLOWDOWN_GROUND = 20;

        private static readonly int STEP_DISTANCE_MAX = 4;

        private const float duck_wings_limit = -20;
        private const float duck_wings_bonus = 60;

        private bool key_left;
        private bool key_right;
        private bool key_up;
        private bool key_down;

        // Vars        
        private Vector2 velocity;

        private bool flying;        

        private bool controlledByDPad;

        public float power;
        private bool flyingOnWings;

        private float gravityBoostCoeff;
        private bool steping;
        private bool flipped;
        private float steppingDistance;

        private Rect bounds;
        private Rect[] collisionRectsNorm; // collision rects for normal state
        private Rect[] collisionRectsFlip; // collision rects for flipped state
        private Rect[] collisionRects; // current collision rects

        private List<HeroListener> listeners;

        public Hero()
        {
            Texture2D duck = Application.sharedResourceMgr.getTexture(Res.IMG_DUCK_FAKE);
            width = duck.Width;
            height = duck.Height;

            initCollisionRects();
            velocity = Vector2.Zero;                        
            bounds = new Rect(0, 0, Constants.WORLD_VIEW_WIDTH, Constants.WORLD_VIEW_HEIGHT);
            listeners = new List<HeroListener>();

            JUMP_HEIGHT_MIN = 0.25f * bounds.Height - height;
            JUMP_HEIGHT_MAX = bounds.Height - height;

            JUMP_START_VY_MIN = -(float)Math.Sqrt(2 * ACC_Y * JUMP_HEIGHT_MIN);
            JUMP_START_VY_MAX = -(float)Math.Sqrt(2 * ACC_Y * JUMP_HEIGHT_MAX);

            Application.sharedInputMgr.addInputListener(this);            
        }

        public override void draw()
        {
            float dx = x;
            float dy = y;           

            if (steppingDistance > 2 && !flying)
            {
                dy -= 2.0f;
            }

            Draw(dx, dy);
            if (dx < 0)
            {
                dx += bounds.Width;
                Draw(dx, dy);
            }
            else if (dx > bounds.Width - width)
            {
                dx -= bounds.Width;
                Draw(dx, dy);
            }

            //foreach (Rect r in collisionRects)
            //{
            //    AppGraphics.DrawRect(r.X + x, r.Y + y, r.Width, r.Height, Color.White);
            //} 

            AppGraphics.DrawString(0, 0, "Power: " + power);
        }

        private void Draw(float x, float y)
        {
            Texture2D duck = Application.sharedResourceMgr.getTexture(Res.IMG_DUCK_FAKE);
            AppGraphics.DrawImage(duck, x, y, flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
        }

        public override void update(float dt)
        {
            base.update(dt);

            UpdateGamepadInput();
            UpdateHorizontalPosition(dt);
            UpdateVerticalPosition(dt);

            Console.WriteLine("vx=" + velocity.X + " vy=" + velocity.Y);
        }

        private void UpdateGamepadInput()
        {
            float dx = 0; // InputManager.LeftThumbStickX;
            if (Math.Abs(dx) < 0.15f)
            {
                if (!controlledByDPad)
                {
                    key_right = false;
                    key_left = false;
                }
                controlledByDPad = true;
                return; // DEAD ZONE
            }

            controlledByDPad = false;
            if (dx > 0)
            {
                key_right = true;
                key_left = false;

                velocity.X = dx * dx;
            }
            else
            {
                key_right = false;
                key_left = true;

                velocity.X = -dx * dx;
            }
        }

        private void UpdateHorizontalPosition(float dt)
        {
            steping = false;
            if (key_left)
            {
                steping = true;
                setFlipped(false);
                if (controlledByDPad)
                {
                    velocity.X -= ACC_X * dt;
                    if (velocity.X < -1)
                        velocity.X = -1;
                }
            }
            if (key_right)
            {
                steping = true;
                setFlipped(true);
                if (controlledByDPad)
                {
                    velocity.X += ACC_X * dt;
                    if (velocity.X > 1)
                        velocity.X = 1;
                }
            }

            if (steping)
            {
                if (velocity.X >= 0.0)
                    steppingDistance += velocity.X * dt * 30.0f;
                else
                    steppingDistance -= velocity.X * dt * 30.0f;

                if (steppingDistance > STEP_DISTANCE_MAX)
                {
                    steppingDistance -= STEP_DISTANCE_MAX;
                    if (!flying)
                    {
                        //media.playStep();
                        // doStepBubbles();
                    }
                }
            }
            else
            {
                float slow = flying ? SLOWDOWN_SKY : SLOWDOWN_GROUND;
                velocity.X -= velocity.X * slow * dt;

                if (steppingDistance > STEP_DISTANCE_MAX)
                {
                    //if (!fly)
                    //    media.playStep();

                    steppingDistance = 0.0f;
                }
            }

            float dx = velocity.X * MathHelper.Lerp(VX_MIN, VX_MAX, power) * dt;
            x += dx;

            if (x < -width)
                x += bounds.Width;
            if (x > (bounds.Width - width))
                x -= bounds.Width;
        }

        private void UpdateVerticalPosition(float dt)
        {
            if (flying)
            {
                if (key_down)
                {
                    gravityBoostCoeff += dt * 6;
                    if (gravityBoostCoeff > 3.0f) gravityBoostCoeff = 3.0f;
                }
                else
                {
                    gravityBoostCoeff -= dt * 6;
                    if (gravityBoostCoeff < 0.0f) gravityBoostCoeff = 0.0f;
                }

                if (flyingOnWings && velocity.Y <= 0) // we can't go down, if we fly on wings
                {
                    velocity.Y = 0.0f;
                }
                else
                {
                    velocity.Y += ACC_Y * (gravityBoostCoeff + 1.0f) * dt;
                    y += velocity.Y * dt;
                }

                float minY = bounds.Height - height;
                if (y >= minY)
                {
                    flyingOnWings = false;
                    flying = false;
                    y = minY;
                    gravityBoostCoeff = 0.0f;
                    // doLandBubble(velocity.Y);
                }
            }
        }

        private float GetJumpStartVy(float x)
        {
            return MathHelper.Lerp(JUMP_START_VY_MIN, JUMP_START_VY_MAX, x);
        }

        public void keyPressed(Keys key)
        {
            switch (key)
            {
                case Keys.Up:
                    upPressed();
                    break;

                case Keys.Right:
                    rigthPressed();
                    break;

                case Keys.Left:
                    leftPressed();
                    break;

                case Keys.Down:
                    downPressed();
                    break;
            }
        }

        public void buttonPressed(ButtonEvent e)
        {
            switch (e.button)
            {
                case Buttons.A:
                    upPressed();                    
                    break;

                case Buttons.DPadRight:
                    rigthPressed();
                    break;

                case Buttons.DPadLeft:
                    leftPressed();
                    break;

                case Buttons.DPadDown:
                    downPressed();                    
                    break;
            }
        }

        public void keyReleased(Keys key)
        {
            switch (key)
            {
                case Keys.Up:
                    upReleased();
                    break;

                case Keys.Right:
                    rightReleased();
                    break;

                case Keys.Left:
                    leftReleased();
                    break;

                case Keys.Down:
                    downReleased();
                    break;
            }
        }

        public void buttonReleased(ButtonEvent e)
        {
            switch (e.button)
            {
                case Buttons.A:
                    upReleased();                 
                    break;

                case Buttons.DPadRight:
                    rightReleased();                    
                    break;

                case Buttons.DPadLeft:
                    leftReleased();                    
                    break;

                case Buttons.DPadDown:
                    downReleased();                    
                    break;
            }            
        }
        
        private void upPressed()
        {
            if (!key_up)
            {
                if (flying)
                {
                    flyingOnWings = true;
                }
                else
                {
                    flying = true;
                    velocity.Y = GetJumpStartVy(power); ;
                    //doLandBubble(velocity.Y);
                }
            }
            key_up = true;
        }

        private void upReleased()
        {
            if (key_up)
            {
                if (flying)
                {
                    if (flyingOnWings)
                    {
                        flyingOnWings = false;
                        if (velocity.Y < 0.0f)
                            velocity.Y = 0.0f;
                    }

                    //if(jumpVel>0 && gravityK==1)
                    //gravityK = (jumpVel + jumpStartVel)/(jumpStartVel*2 - jumpVel);
                }
            }
            key_up = false;
        }

        private void downPressed()
        {
            key_down = true;
        }

        private void downReleased()
        {
            key_down = false;
        }        

        private void leftPressed()
        {
            key_left = true;
            controlledByDPad = true;
        }

        private void leftReleased()
        {
            key_left = false;
        }        

        private void rigthPressed()
        {
            key_right = true;
            controlledByDPad = true;
        }

        private void rightReleased()
        {
            key_right = false;
        }

        //private void doStepBubbles()
        //{
        //    float particleX = world.ToScreenX(flipped ? (x - STEP_BUBBLE_OFFSET_X) : (x + STEP_BUBBLE_OFFSET_X));
        //    float particleY = world.ToScreenY(STEP_BUBBLE_OFFSET_Y);
        //    Application.Instance.Particles.StartBubble(particleX, particleY, COLOR_STEP_BUBBLE);
        //}

        //private void doLandBubble(float velocity.Y)
        //{
        //    int particlesCount = Math.Abs((int)(velocity.Y / 20));
        //    for (int i = 0; i < particlesCount; i++)
        //    {
        //        float particleX = world.ToScreenX(x + 0.5f * width * Application.Instance.GetRandomFloat());
        //        float particleY = world.ToScreenY(STEP_BUBBLE_OFFSET_Y);
        //        Application.Instance.Particles.StartBubble(particleX, particleY, COLOR_LAND_BUBBLE);
        //    }
        //}

        public void eatPill(Pill p)
        {
            addPower(0.01f);
        }

        public void addPower(float pd)
        {
            float oldPower = power;
            power += pd;
            if (power > 1.0f)
                power = 1.0f;

            if (oldPower != power)
                firePowerChanged(oldPower, power);
        }

        private void firePowerChanged(float oldPower, float newPower)
        {
            foreach (HeroListener l in listeners)
            {
                l.heroPowerChanged(oldPower, newPower);
            }
        }

        public void addListener(HeroListener listener)
        {
            listeners.Add(listener);
        }

        public bool collides(Pill pill)
        {
            float pillX = pill.x;
            float pillY = pill.y;
            float pillR = Constants.PILL_RADIUS;
            if (!CollisionHelper.collidesRectVsCircle(x, y, width, height, pillX, pillY, pillR))
                return false;

            for (int i = 0; i < collisionRects.Length; ++i)
            {
                float rx = collisionRects[i].X + x;
                float ry = collisionRects[i].Y + y;
                float rw = collisionRects[i].Width;
                float rh = collisionRects[i].Height;
                if (CollisionHelper.collidesRectVsCircle(rx, ry, rw, rh, pillX, pillY, pillR))
                    return true;
            }

            return false;
        }

        private void setFlipped(bool flipped)
        {
            this.flipped = flipped;
            collisionRects = flipped ? collisionRectsFlip : collisionRectsNorm;
        }

        private void initCollisionRects()
        {
            float[] cords = Constants.HERO_COLLISION_SUB_RECTS;
            int rectsCount = cords.Length / 4;
            collisionRectsNorm = new Rect[rectsCount];
            collisionRectsFlip = new Rect[rectsCount];
            for (int rectIndex = 0, cordIndex = 0; rectIndex < rectsCount; ++rectIndex, cordIndex += 4)
            {
                float rx = cords[cordIndex] * width;
                float ry = cords[cordIndex + 1] * height;
                float rw = cords[cordIndex + 2] * width;
                float rh = cords[cordIndex + 3] * height;                
                collisionRectsNorm[rectIndex] = new Rect(rx, ry, rw, rh);
                collisionRectsFlip[rectIndex] = new Rect(width - (rx + rw), ry, rw, rh);
            }
            setFlipped(false);
        }
    }    

}
