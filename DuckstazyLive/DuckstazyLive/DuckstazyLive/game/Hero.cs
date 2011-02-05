using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using DuckstazyLive.app;
using Framework.core;
using Framework.utils;
using System.Diagnostics;
using DuckstazyLive.game.levels;

namespace DuckstazyLive.game
{
    public enum HeroMessage
    {
        JUMP,
        ATTACKER,
        VICTIM,
    };

    public delegate void HeroCallback(Hero hero, HeroMessage message);

    public class Hero
    {
        // duck logic consts
        private const float duck_jump_start_vel_min = 127;
        private const float duck_jump_start_vel_max = 379;
        private const float duck_rapid_jump_delay = 0.1f;
        private const float duck_dive_vel_max = 800.0f;

        public const float duck_jump_gravity = 200;
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
        private bool stickDive;
        private float stickMoveCoeff;

        public const int duck_w = 27;
        public const int duck_h = 20;
        public const int duck_w2 = 54;
        public const int duck_h2 = 40;
        
        private const float COMPRESS_TIMEOUT = 0.25f;
        private float compressCounter;        
        private float scaleX, scaleY;
        private float rotation;

        private const float JUMP_ON_TIMEOUT = 0.5f;
        private float jumpedElasped;

        private const float DROP_VELOCITY_MIN = 800.0f;
        private const float DROP_VELOCITY_MAX = 1200.0f;
        private const float DROP_TIME = 0.2f;
        private const float DROP_DA = MathHelper.TwoPi / DROP_TIME;
        private float dropCounter;
        private float dropDAngle;
        private bool dropping;
        private bool droppingCanceled;
        private Vector4 dropRect;

        public HeroCallback user;

        private static Rect[] COLLISION_RECTS_SLEEP = 
        {
            new Rect(0, 0, duck_w2, duck_h2)
        };

        private static Rect[] COLLISION_RECTS = 
        {
            new Rect(8.0f, 0.0f, 16.0f, 17.0f),
            new Rect(0.0f, 10.0f, 15.0f, 6.0f),
            new Rect(12.0f, 11.0f, 36.0f, 26.0f),
            new Rect(29.0f, 35.0f, 12.0f, 6.0f),
        };

        private static Rect[] ATTACKER_RECTS =
        {
            new Rect(14.0f, 12.0f, 35.0f, 25.0f)            
        };

        private static Rect[] VICTIM_RECTS =
        {            
            new Rect(8.0f, 12.0f, 44.0f, 25.0f),
        };

        private static Rect[] COLLISION_RECTS_FLIP;
        private static Rect[] ATTACKER_RECTS_FLIP;
        private static Rect[] VICTIM_RECTS_FLIP;

        private const float STICK_HOR_THRESHOLD = 0.1f;
        private const float STICK_VER_THRESHOLD = 0.7f;
        private const float STICK_VER_THRESHOLD_DIVE = 0.9f;

        private bool rapidJump;
        private float jumpButtonPressedStartTime;

        private const int MIN_DROP_KICKED_PILLS = 10;
        private const int MAX_DROP_KICKED_PILLS = 25;
        private const int MIN_KICKED_PILLS = 1;
        private const int MAX_KICKED_PILLS = 10;
        
        public Vector2 pos;
        public Vector2 lastPos;
        //public float x;
        //public float y;
        //public float xLast;
        //public float yLast;
        private float dx;
        private float dy;
        public bool flip;

        public const int HERO_NORMAL = 0;
        public const int HERO_SLEEP = 1;
        public const int HERO_DEAD = 2;

        public int state;

        private bool started;
        private float power;

        public float jumpVel;
        private float dropVelocity;
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

        public GameState gameState;
        public GameInfo info;
        private Heroes heroes;
        private Canvas canvas;

        private int playerIndex;
        public int combo;

        public Hero(Heroes heroes, int playerIndex)
        {
            this.heroes = heroes;
            this.playerIndex = playerIndex;

            flip = true;
            state = HERO_NORMAL;
            started = false;
            canvas = new Canvas(0, 0);

            gameState = new GameState();
            info = new GameInfo(this);

            initCollisionRects();
        }

        public void clear()
        {
            state = HERO_NORMAL;
            pos = lastPos = Vector2.Zero;
            gameState.health = gameState.maxHP;
            init();
        }

        public void init()
        {
            move = 0.0f;
            step = 0.0f;

            buttonsReset();

            fly = false;

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

            pos.Y = 400 - duck_h2;
            scaleX = scaleY = 1.0f;
            compressCounter = 0.0f;
            jumpedElasped = 0.0f;

            power = 0.0f;
            
            sleep_collected = 0;
            toxic_collected = 0;
            frags = 0;

            dropping = false;
            dropCounter = 0.0f;
            dropDAngle = 0.0f;
            rotation = 0.0f;
            combo = 0;

            info.reset();
            user = null;
        }

        private void doStepBubble()
        {
            float px = x + 4;
            if (!flip) px = x + 50;
            getParticles().startStepBubble(px, y + 39);
        }

        private void doDropBubbles()
        {

        }

        private void doLandBubbles()
        {
            float px;
            int i = (int)Math.Abs(jumpVel * 0.05f);

            while (i > 0)
            {
                px = x + 17 + utils.rnd() * 20;
                getParticles().startBubble(px, y + duck_h2, 0xff999999);
                --i;
            }
        }

        private void doCrapBubbles()
        {
            int i = 10;
            float px = x + 4;
            if (!flip) px = x + 50;
            while (i > 0)
            {
                float speed = utils.rnd_float(50, 150);
                float vx = flip ? -speed : speed;
                float vy = -utils.rnd_float(10, 100);
                getParticles().startCrapBubble(px, y + duck_h2, vx, vy);
                --i;
            }
        }

        public void update(float dt, float newPower)
        {
            if (!started) return;

            if (!isDead())
                updateGamepadInput();            

            lastPos.X = x;
            lastPos.Y = y;            

            heroes.media.updateSFX(x + duck_w);

            updateJumpCompress(dt);

            power = newPower;            

            if (isSleep() && power <= 0)
            {
                state = HERO_NORMAL;
                startSleepParticles();

                heroes.media.playAwake();
            }

            if (blinkTime > 0.0f)
                blinkTime -= dt * 8.0f;

            gameState.update(dt);
            info.update(newPower, dt);

            updateHor(dt);
            updateVer(dt);
            updateWings(dt);
        }        

        private void updateHor(float dt)
        {
            steping = false;

            if (dropping)
                return;

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

            pos.X += move * utils.lerp(power, duck_move_speed_min, duck_move_speed_max) * dt;

            if (x < -duck_w)
                pos.X += 640.0f;
            if (x > (640.0f - duck_w))
                pos.X -= 640.0f;
        }

        private void updateVer(float dt)
        {
            jumpStartVel = get_jump_start_vel(power);

            if ((wingLock || compressCounter > 0.0f) && isActive())
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
                if (dropping)
                    updateDropping(dt);
                else
                    updateFlying(dt);

                if (y >= 400 - duck_h2)
                {
                    wingLock = false;
                    fly = false;
                    pos.Y = 400 - duck_h2;

                    if (dropping)
                    {
                        doDropBubbles();
                        heroes.media.playLandHeavy();
                    }
                    else
                    {
                        doLandBubbles();
                        heroes.media.playLand();
                    }

                    dropping = false;
                    sleep_collected = 0;
                    toxic_collected = 0;
                    frags = 0;
                    diveK = 0.0f;
                    combo = 0;
                }
                else if (y < -50.0f)
                    pos.Y = -50.0f;
            }
        }

        private void updateDropping(float dt)
        {
            if (dropping)
            {
                if (dropCounter > 0)
                {
                    dropCounter -= dt;
                    if (dropCounter < 0)
                    {
                        dropCounter = 0.0f;
                        rotation = 0.0f;

                        if (droppingCanceled)
                        {
                            dropping = droppingCanceled = false;
                        }                        
                    }
                    else
                    {
                        rotation += dropDAngle;
                    }                    
                }
                else
                {
                    pos.Y += dropVelocity * dt;
                }

                // 14.0f, 12.0f, 35.0f, 25.0f
                dropRect.X = lastPos.X + 14.0f;
                dropRect.Y = lastPos.Y + 12.0f;
                dropRect.Z = dropRect.X + 35.0f;
                dropRect.W = pos.Y + 12.0f + 25.0f;
            }            
        }

        private void updateFlying(float dt)
        {
            if (stickDive)
            {
                diveK += dt * 10.0f;
                if (diveK > 10.0f) diveK = 10.0f;
            }
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

            if (wingLock && isActive() && wingYLocked)
            {
                jumpWingVel -= 392.0f * dt;//(gravityK+diveK)*dt;
                pos.Y -= jumpWingVel * dt;
            }
            else
            {
                if (wingLock && isActive())
                {
                    if (jumpVel >= 0.0f)
                    {
                        jumpVel -= duck_jump_gravity * (diveK + 1.0f) * dt;
                        pos.Y -= jumpVel * dt;
                        if (jumpVel <= 0.0f)
                        {
                            wingYLocked = true;
                            wingY = y;
                        }
                    }
                    else
                    {
                        jumpVel += 5.0f * duck_jump_gravity * dt;
                        pos.Y -= jumpVel * dt;
                    }
                }
                else
                {
                    jumpVel -= duck_jump_gravity * (diveK + 1.0f) * dt;
                    pos.Y -= jumpVel * dt;
                }
            }
        }

        private void updateWings(float dt)
        {
            if (wingCounter > 0)
            {
                wingCounter -= 10 * dt;
                if (wingCounter < 0)
                    wingCounter = 0;

                wingAngle = 0.5f * ((float)Math.Sin(wingCounter * 4.71f));
            }
        }        

        private void updateJumpCompress(float dt)
        {
            jumpedElasped += dt;
            if (compressCounter > 0.0f)
            {
                compressCounter -= dt;
                if (compressCounter > 0.0f)
                {
                    float compressProgress = compressCounter / COMPRESS_TIMEOUT;
                    float t = COMPRESS_TIMEOUT - compressCounter;
                    float omega = MathHelper.Pi / COMPRESS_TIMEOUT;
                    float compress = (float)(0.25 * (1 - 0.8 * compressProgress) * Math.Sin(omega * t));
                    scaleX = 1 + compress;
                    scaleY = 1 - 0.5f * compress;
                }
                else
                {
                    scaleX = scaleY = 1.0f;
                    compressCounter = 0.0f;
                    wingMod = 1.0f;
                    wingBeat();
                }
            }
        }

        private void updateGamepadInput()
        {
            Vector2 leftStick = Application.sharedInputMgr.ThumbSticks(playerIndex).Left;

            if (controlledByStick)
            {
                key_left = key_right = stickDive = false;
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
                stickDive = true;
            }
        }

        private float getStickMoveCoeff(ref Vector2 stickPos)
        {
            if (stickPos.Y < -STICK_VER_THRESHOLD)
            {
                return (float)Math.Sqrt(Math.Abs(stickPos.X));
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

        public void draw()
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
                    drawHero(dx + 640, dy, 1.0f - alpha);
                }
                else if (dx > 640 - duck_w2)
                {
                    alpha = 1.0f - (dx - 640 + duck_w2) / duck_w2;
                    drawHero(dx - 640, dy, 1.0f - alpha);
                }
                drawHero(dx, dy, alpha);
            }            
        }

        private void drawHero(float x, float y, float trans)
        {
            bool vis = (blinkTime <= 0.0f || (((int)blinkTime) & 0x1) != 0) || isDead();

            bool hasScale = scaleX != 0 || scaleY != 0;
            bool hasRotation = rotation != 0;
            bool hasTrans = hasScale || hasRotation;

            float dx;
            float dy;
            if (hasTrans)
            {
                float offX = duck_w;
                float offY = hasRotation ? duck_h : duck_h2;

                AppGraphics.PushMatrix();
                AppGraphics.Translate(utils.scale(x + offX), utils.scale(y + offY), 0.0f);
                AppGraphics.Rotate(rotation, 0.0f, 0.0f, 1.0f);
                AppGraphics.Scale(scaleX, scaleY, 1.0f);

                dx = -offX;
                dy = -offY;
            }
            else
            {
                dx = x - duck_w;
                dy = y - duck_h2;
            }            

            if (vis)
            {
                switch (state)
                {
                    case HERO_SLEEP:
                        drawSleep(dx, dy, flip, trans);
                        break;
                    case HERO_DEAD:
                        drawDead(dx, dy, flip);
                        break;
                    case HERO_NORMAL:
                        drawDuck(playerIndex, dx, dy, power, trans);
                        break;
                    default:
                        Debug.Assert(false, state.ToString());
                        break;
                }
            }

            if (hasTrans)
            {
                AppGraphics.PopMatrix();
            }
        }

        public void drawDuck(int playerIndex, float x, float y, float power, float trans)
        {
            DrawMatrix mat = DrawMatrix.ScaledInstance;

            ColorTransform eyeColor = new ColorTransform(1.0f, 1.0f, 1.0f, trans * (1.0f - power));
            ColorTransform duckColor = new ColorTransform(1.0f, 1.0f, 1.0f, trans);

            if (flip)
            {
                mat.flip(true, false);

                mat.translate(x, y);
                mat.flip(true, false);
                canvas.draw(heroes.media.imgDuck + playerIndex, mat, duckColor);

                mat.translate(37.0f + x, 6.0f + y);
                canvas.draw(heroes.media.imgEye1, mat, eyeColor);
                eyeColor.alphaMultiplier = power * trans;
                canvas.draw(heroes.media.imgEye2, mat, eyeColor);

                mat.tx = -12.0f;
                mat.ty = -7.0f;
                mat.rotate(-wingAngle);
                mat.translate(21.0f + x, 26.0f + y);
                duckColor.alphaMultiplier *= trans;
                canvas.draw(heroes.media.imgWing + playerIndex, mat, duckColor);
            }
            else
            {
                //canvas.copyPixels(imgDuck, rcHero, new Point(x, y));
                mat.translate(x, y);
                canvas.draw(heroes.media.imgDuck + playerIndex, mat, duckColor);

                mat.translate(11.0f + x, 6.0f + y);
                canvas.draw(heroes.media.imgEye1, mat, eyeColor);
                eyeColor.alphaMultiplier = power * trans;
                canvas.draw(heroes.media.imgEye2, mat, eyeColor);

                mat.tx = -3.0f;
                mat.ty = -7.0f;
                mat.rotate(wingAngle);
                mat.translate(33.0f + x, 26.0f + y);
                duckColor.alphaMultiplier *= trans;
                canvas.draw(heroes.media.imgWing + playerIndex, mat, duckColor);
            }
        }

        public void drawSleep(float x, float y, bool flip, float trans)
        {
            DrawMatrix mat = DrawMatrix.ScaledInstance;
            ColorTransform color = new ColorTransform(1.0f, 1.0f, 1.0f, trans);

            mat.translate(x, y);

            if (flip)
            {
                mat.flip(true, false);
            }

            canvas.draw(heroes.media.imgSleep, mat, color);
        }

        public void drawDead(float x, float y, bool flip)
        {
            DrawMatrix mat = DrawMatrix.ScaledInstance;
            mat.translate(x, y);

            if (flip)
            {
                mat.flip(true, false);
            }

            canvas.draw(heroes.media.imgDead, mat);

            if (getEnv().isHitFaded())
            {
                canvas.draw(heroes.media.imgDead, mat, getEnv().blackFade);
            }
        }

        public bool buttonPressed(ref ButtonEvent e)
        {
            switch (e.button)
            {
                case Buttons.B:
                    if (canDrop())
                    {
                        drop();
                    }
                    return true;
                case Buttons.DPadDown:
                    key_down = true;
                    return true;

                case Buttons.DPadLeft:
                    key_left = true;
                    return true;

                case Buttons.A:
                    if (!key_up && started)
                    {
                        if (!fly)
                        {
                            if (isActive())
                            {
                                fly = true;
                                jumpVel = jumpStartVel;
                                jumpButtonPressedStartTime = GameClock.ElapsedTime;

                                heroes.media.playJump();
                                doLandBubbles();
                            }
                        }
                        else if (!wingLock && isActive())
                        {
                            wingLock = true;
                            wingMod = 1.0f;
                            wingYLocked = false;

                            wingBeat();
                        }
                    }
                    key_up = true;
                    return true;

                case Buttons.DPadRight:
                    key_right = true;
                    return true;
            }
            return false;
        }

        public bool buttonReleased(ref ButtonEvent e)
        {
            switch (e.button)
            {
                case Buttons.DPadDown:                
                    key_down = false;
                    return true;

                case Buttons.DPadLeft:
                    key_left = false;
                    return true;

                case Buttons.A:
                    if (key_up && started)
                    {
                        endFlying();
                    }
                    key_up = false;
                    return true;

                case Buttons.DPadRight:
                    key_right = false;
                    return true;
            }

            return false;
        }

        private void endFlying()
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

        public void doSleep()
        {
            if (isActive())
            {
                startSleepParticles();
                heroes.media.playSleep();
                state = HERO_SLEEP;
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
                getParticles().startBubble(px, py, 0xff690c7a);
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

            check = fly && (lastPos.Y + duck_h2) <= py && (y + duck_h2) >= (py - 10);

            if (isSleep())
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
            int dam = dmg;
            int ret = -1;

            if (isDroppingDown())
            {
                ret = killToxic(cx, cy, id, ret);
            }
            else if (checkDive(cx, cy))
            {
                jump(40);                
                ret = killToxic(cx, cy, id, ret);

            }
            else if (dam > 0)
            {
                if (blinkTime <= 0.0f)
                {
                    if (gameState.health > 1)
                    {
                        gameState.health -= dam;
                        if (gameState.health < 1)
                            gameState.health = 1;
                    }
                    else
                    {
                        doDie();
                    }

                    blinkTime = 12;
                    getParticles().explStarsToxic(cx, cy, id, true);
                    getEnv().startHitFade(gameState.health == 0);
                    Application.sharedSoundMgr.playSound(heroes.media.sndToxic);
                }
                else
                {
                    getParticles().explStarsToxic(cx, cy, id, false);
                }

                //mBoard->HitToxic(world_draw_pos(ToxicPosition), mToxicCollected);
                ++toxic_collected;
            }

            return ret;
        }

        private int killToxic(float cx, float cy, int id, int ret)
        {
            Application.sharedSoundMgr.playSound(heroes.media.sndAttack);
            getParticles().explStarsToxic(cx, cy - 10, id, false);
            if (frags < 3) ret = 0;
            else
            {
                if (frags < 6) ret = 1;
                else ret = 2;
            }
            ++frags; 
            return ret;
        }

        public bool doHigh(float cx, float cy)
        {
            if (isDroppingDown() || checkDive(cx, cy))
            {                
                jump(40);                
                return true;
            }
            return false;
        }

        public bool isDroppingDown()
        {
            return dropping && dropCounter == 0.0f;
        }

        public void doHeal(int health)
        {
            gameState.health += health;

            if (gameState.health > gameState.maxHP)
                gameState.health = gameState.maxHP;

            getParticles().explHeal(x, y);
        }

        public void doDie()
        {
            gameState.health = 0;
            state = HERO_DEAD;
            buttonsReset();
        }

        public void jumpOn(Hero other)
        {
            float extraHeight = 0.0f;
            if (other.fly)
            {
                float powerCoeff = other.key_up ? 1.0f : 0.5f;
                extraHeight = getJumpHeight(other.jumpStartVel * powerCoeff * (1 - other.jumpVel / other.jumpStartVel));                    
            }
            float jumpHeight = 0.5f * getJumpHeight() + extraHeight;
            float maxJumpHeight = getJumpHeight(duck_jump_start_vel_max);
            if (jumpHeight > maxJumpHeight)
                jumpHeight = maxJumpHeight;

            int oldScore = other.gameState.getScores();
            other.jumpedBy(this);
            jump(jumpHeight);

            if (other.gameState.getScores() < oldScore)
            {
                combo++;
                if (user != null) user(this, HeroMessage.ATTACKER);
            }
        }

        public bool canBeJumped()
        {
            return isActive() && jumpedElasped > JUMP_ON_TIMEOUT;
        }

        public void jumpedBy(Hero other)
        {
            compressCounter = COMPRESS_TIMEOUT;
            jumpVel = 0.0f;
            jumpedElasped = 0.0f;

            int kickedPills;            
            if (other.isDroppingDown())
            {
                kickedPills = (int)utils.lerp(power, MIN_DROP_KICKED_PILLS, MAX_DROP_KICKED_PILLS);
            }
            else
            {
                float jumpPower = Math.Abs(other.jumpVel) / duck_dive_vel_max;
                kickedPills = (int)utils.lerp(jumpPower, MIN_KICKED_PILLS, MAX_KICKED_PILLS);
            }

            int oldScores = gameState.getScores();

            for (int i = 0; i < kickedPills && gameState.getScores() > 0; ++i)
            {
                Pill pill = getPills().findDead();
                if (pill != null)
                {
                    float speed = utils.rnd_float(100, 250);
                    float vx = flip ? -speed : speed;
                    float vy = -utils.rnd_float(100, 250);

                    float px = flip ? x : x + duck_w2;
                    float py = y + duck_h2;

                    int powerId = utils.rnd_int(3);
                    pill.startPower(px, py, powerId, false);
                    pill.vx = vx;
                    pill.vy = vy;
                    pill.user = kickedPillCallback;
                    getPills().actives++;
                    
                    int pillScore = pill.getScore();                    
                    if (gameState.getScores() - pillScore < 0)
                    {
                        gameState.addScores(-gameState.getScores());
                        break;
                    }
                    else
                    {
                        gameState.addScores(-pillScore);
                    }
                }
            }

            int scoreDif = gameState.getScores() - oldScores;
            if (scoreDif < 0)
            {                
                info.add(scoreDif);
                if (user != null)
                    user(this, HeroMessage.VICTIM);
            }

            key_up = false;
            endFlying();

            wingBeat();
            doCrapBubbles();            

            Application.sharedSoundMgr.playSound(Res.SND_HERO_SQUEAK);
        }

        public void kickedPillCallback(Pill pill, String msg, float dt)
        {
            float friction = 0.7f + power * 0.3f;
            if (msg == null && pill.enabled)
            {
                pill.vy += 300.0f * dt;
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
            }
            else if (msg == "born")
            {
                pill.vx = (150.0f + 150.0f * power) * (utils.rnd() * 2.0f - 1.0f);
                pill.vy = -100.0f - utils.rnd() * 200.0f - 200.0f * power;
            }
        }        

        private bool canDrop()
        {
            return isActive() && !dropping && pos.Y < 400 - 1.5f * duck_h2;
        }

        public void drop()
        {
            move = 0.0f;
            dropVelocity = utils.lerp(power, DROP_VELOCITY_MIN, DROP_VELOCITY_MAX);
            dropping = true;
            dropCounter = DROP_TIME;
            dropDAngle = flip ? DROP_DA : -DROP_DA;
            rotation = 0.0f;

            heroes.media.playFlip();
        }

        public void jump(float h)
        {
            if (dropping)
            {
                if (dropCounter > 0)
                    droppingCanceled = true;
                else
                    dropping = false;
            }

            float new_vy = (float)Math.Sqrt(2 * duck_jump_gravity * h);
            if (jumpVel < new_vy)
                jumpVel = new_vy;
        }

        public float getJumpHeight()
        {
            return getJumpHeight(jumpStartVel);
        }

        private float getJumpHeight(float vy)
        {
            return vy * vy * 0.5f / duck_jump_gravity;
        }

        public Rect[] getCollisionRects()
        {
            if (isSleep())
                return COLLISION_RECTS_SLEEP;

            return flip ? COLLISION_RECTS_FLIP : COLLISION_RECTS;
        }

        public Rect[] getAttackerRects()
        {
            if (flip)
                return ATTACKER_RECTS_FLIP;

            return ATTACKER_RECTS;
        }

        public Rect[] getVictimRect()
        {
            if (flip)
                return VICTIM_RECTS_FLIP;

            return VICTIM_RECTS;
        }

        public void doPillAttack(Pill pill)
        {
            if (dropping && dropCounter == 0.0f)
            {
                float x1 = dropRect.X;
                float y1 = dropRect.Y;
                float x2 = dropRect.Z;
                float y2 = dropRect.W;

                if (rectCircle(x1, y1, x2, y2, pill.x, pill.y, pill.r) || 
                    rectCircle(x1, y1, x2, y2, pill.xLast, pill.yLast, pill.r))
                {
                    if (pill.isJumper() && pill.highCounter <= 0.0f && lastPos.Y + duck_h2 < pill.y - pill.r)
                    {
                        pos.Y = pill.y - pill.r - duck_h2;                        
                    }
                    pill.heroTouch(this);
                }                
            }
            else
            {
                if ((pill.y + pill.r > y || pill.y - pill.r < y + 40 ) && (pill.x + pill.r > x || pill.x - pill.r < x + 54))
                {
                    if (overlapsCircle(pill.x, pill.y, pill.r))
                    {
                        pill.heroTouch(this);
                    }
                }
            }
        }

        public bool overlapsCircle(float cx, float cy, float r)
        {
            bool over = false;

            if (x < 0.0f && cx > (630 - duck_w2))
                cx -= 640;

            if (flip)
                cx = 2 * (x + duck_w) - cx;

            if (isSleep())
            {
                over = rectCircle(x + 1, y + 11, x + 41, y + 39, cx, cy, r);
            }
            else
            {
                over = rectCircle(x + 14.0f, y + 13.0f, x + 49.0f, y + 38.0f, cx, cy, r) ||
                    rectCircle(x + 9.0f, y + 1.0f, x + 24.0f, y + 17.0f, cx, cy, r) ||
                    rectCircle(x + 1.0f, y + 13.0f, x + 8.0f, y + 17.0f, cx, cy, r);
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
            pos.X = _x;
            flip = utils.rnd() < 0.5;
            startSleepParticles();
            heroes.media.playAwake();
        }

        public void buttonsReset()
        {
            key_up = false;
            key_right = false;
            key_left = false;
            key_down = false;
            stickDive = false;
            wingLock = false;
            controlledByStick = false;
            jumpButtonPressedStartTime = 0.0f;
            rapidJump = false;
            stickMoveCoeff = 0.0f;
        }

        public static float get_jump_start_vel(float x)
        {
            return utils.lerp(x, duck_jump_start_vel_min, duck_jump_start_vel_max);
        }

        private void initCollisionRects()
        {
            initCollisionRects(COLLISION_RECTS, ref COLLISION_RECTS_FLIP);
            initCollisionRects(ATTACKER_RECTS, ref ATTACKER_RECTS_FLIP);
            initCollisionRects(VICTIM_RECTS, ref VICTIM_RECTS_FLIP);            
        }

        private void initCollisionRects(Rect[] normal, ref Rect[] fliped)
        {
            if (fliped == null)
            {
                fliped = new Rect[normal.Length];
                for (int i = 0; i < normal.Length; ++i)
                {
                    Rect r = normal[i];
                    fliped[i] = new Rect(duck_w2 - (r.X + r.Width), r.Y, r.Width, r.Height);
                }
            }
        }

        public bool isDead()
        {
            return state == HERO_DEAD;
        }

        public bool isSleep()
        {
            return state == HERO_SLEEP;
        }

        public bool isActive()
        {
            return state == HERO_NORMAL;
        }

        public bool isFlying()
        {
            return y < 400 - duck_h2;
        }

        public int getPlayerIndex()
        {
            return playerIndex;
        }        

        private Pills getPills()
        {
            return GameElements.Pills;
        }

        private Particles getParticles()
        {
            return GameElements.Particles;
        }

        private Env getEnv()
        {
            return GameElements.Env;
        }

        public float x
        {
            get { return pos.X; }
        }

        public float y
        {
            get { return pos.Y; }
        }
    }
}
