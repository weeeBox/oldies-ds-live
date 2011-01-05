using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using DuckstazyLive.game.stages.fx;
using DuckstazyLive.game.levels.generator;
using DuckstazyLive.game.stages.generator;
using Framework.core;

namespace DuckstazyLive.game.stages.story
{
    public class Fireworks : PumpLevelStage
    {
        private struct FireworkInfo
        {
            public int[] ids;
            public float x1, y1, x2, y2;
            public float flyTime;
            public float expSpeed;
            public float lifeTime;
            public float delay;
            public int pillsCount;
            public int sleepCount;

            public FireworkInfo(int[] ids, float x1, float y1, float x2, float y2, float flyTime, float expSpeed, float lifeTime, float delay, int pillsCount, int sleepCount)
            {
                this.ids = ids;
                this.x1 = x1;
                this.y1 = y1;
                this.x2 = x2;
                this.y2 = y2;
                this.flyTime = flyTime;
                this.lifeTime = lifeTime;
                this.expSpeed = expSpeed;
                this.delay = delay;
                this.pillsCount = pillsCount;
                this.sleepCount = sleepCount;
            }
        }

        private const int STATE_PUMP = 0;
        private const int STATE_PENALTY = 1;
        private const int STATE_POWER = 2;
        private int state;

        private FireworkInfo[] pumpFireworkData;
        private FireworkInfo[] powerFireworkData;
        private FireworkInfo[] penaltyFireworkData;
        private FireworkInfo[] powerEndFireworkData;

        private Queue<FireworkInfo> fireworkQueue;

        private FireworkSetuper setuper;        

        private Generator jumpGen;
        private JumpStarter jumper;

        private Firework firework;        

        public Fireworks()
        {
            pumpVel = 0.025f;

            firework = new Firework();
            jumper = new JumpStarter();

            setuper = new FireworkSetuper();
            fireworkQueue = new Queue<FireworkInfo>();
        }

        public override void start()
        {
            day = false;

            base.start();

            if (isSingleLevel())
            {
                startX = 160.0f - Hero.duck_w;
                pumpFireworkData = new FireworkInfo[]
                {
                    new FireworkInfo(FireworkSetuper.POWER1, 0, 380, 320, 80, 2.5f, 120.0f, 8.0f, 3.0f, 12, 0),
                    new FireworkInfo(FireworkSetuper.POWER2, 640, 380, 320, 80, 2.0f, 150.0f, 8.0f, 1.0f, 10, 0),
                    new FireworkInfo(FireworkSetuper.POWER3, 320, 380, 320, 80, 1.0f, 160.0f, 4.0f, 1.0f, 8, 0),                    
                };
                powerFireworkData = new FireworkInfo[]
                {
                    new FireworkInfo(FireworkSetuper.POWER1, 0, 380, 480, 200, 2.0f, 200.0f, 4.0f, 0.0f, 8, 1),
                    new FireworkInfo(FireworkSetuper.POWER1, 640, 380, 160, 200, 2.0f, 200.0f, 4.0f, 0.0f, 7, 1),
                    new FireworkInfo(FireworkSetuper.POWERS, 640, 80, 160, 250, 1.5f, 200.0f, 3.5f, 0.0f, 6, 1),
                    new FireworkInfo(FireworkSetuper.POWERS, 0, 80, 480, 250, 1.5f, 200.0f, 3.5f, 0.0f, 6, 1),                    
                };
                penaltyFireworkData = new FireworkInfo[]
                {
                    new FireworkInfo(FireworkSetuper.TOXIC, 320, 80, 320, 100, 0.5f, 150.0f, 3.5f, 0.0f, 8, 3)
                };
                powerEndFireworkData = new FireworkInfo[]
                {
                    new FireworkInfo(FireworkSetuper.TOXIC, 600, 380, 80, 80, 2.5f, 350.0f, 3.0f, 0.5f, 5, 0),
                    new FireworkInfo(FireworkSetuper.POWERS, 600, 380, 80, 80, 2.5f, 350.0f, 3.0f, 0.5f, 5, 1),
                    new FireworkInfo(FireworkSetuper.TOXIC, 80, 380, 600, 80, 2.5f, 350.0f, 3.0f, 0.5f, 5, 0),
                    new FireworkInfo(FireworkSetuper.POWERS, 80, 380, 600, 80, 2.5f, 350.0f, 3.0f, 0.5f, 5, 1),
                };
            }
            else
            {
                //pumpFireworkData = new FireworkInfo[]
                //{
                //    new FireworkInfo(FireworkSetuper.POWER1, 0, 400, 320, 80, 2.5f, 120.0f, 4.0f, 3.0f, 0),
                //    new FireworkInfo(FireworkSetuper.POWER2, 640, 400, 320, 80, 2.0f, 150.0f, 4.0f, 1.0f, 0),
                //    new FireworkInfo(FireworkSetuper.POWER3, 0, 0, 160, 80, 1.0f, 100.0f, 4.0f, 1.0f, 0),
                //    new FireworkInfo(FireworkSetuper.POWER3, 640, 0, 480, 80, 1.0f, 100.0f, 4.0f, 1.0f, 0),
                //};
            }            

            startState(STATE_PUMP);

            jumpGen = new Generator();
            startJumps();
            
            startFirework();
        }        

        public override void stop()
        {            
            jumpGen.reset();
            pumpFireworkData = null;
            powerFireworkData = null;            
            base.stop();
        }

        private void startState(int state)
        {
            clearFireworkQueue();

            switch (state)
            {
                case STATE_PUMP:
                    addFireworksQueue(pumpFireworkData);
                    break;

                case STATE_PENALTY:
                    addFireworksQueue(penaltyFireworkData);
                    break;

                case STATE_POWER:
                    addFireworksQueue(powerFireworkData);
                    break;

                default:
                    Debug.Assert(false, "Bad state: " + state);
                    break;
            }
            
            this.state = state;            
        }

        private void addFireworksQueue(FireworkInfo[] info)
        {
            foreach (FireworkInfo i in info)
            {
                fireworkQueue.Enqueue(i);
            }
        }

        private void clearFireworkQueue()
        {
            fireworkQueue.Clear();
        }

        private void startJumps()
        {
            jumpGen.reset();
            jumpGen.regen = false;
            jumpGen.speed = 4.0f;

            jumpGen.map.Add(new Placer(jumper, 160.0f, 370.0f));
            jumpGen.map.Add(new Placer(jumper, 480.0f, 370.0f));

            jumpGen.map.Add(new Placer(jumper, 200.0f, 340.0f));
            jumpGen.map.Add(new Placer(jumper, 440.0f, 340.0f));

            jumpGen.map.Add(new Placer(jumper, 240.0f, 310.0f));
            jumpGen.map.Add(new Placer(jumper, 400.0f, 310.0f));

            jumpGen.map.Add(new Placer(jumper, 280.0f, 280.0f));
            jumpGen.map.Add(new Placer(jumper, 360.0f, 280.0f));

            jumpGen.map.Add(new Placer(jumper, 320.0f, 250.0f));

            jumpGen.start();
        }

        public override void update(float dt)
        {
            base.update(dt);            

            jumpGen.update(dt);
            firework.update(dt, level.power);

            if (firework.isKilled())
            {
                Debug.WriteLine("Restart firework");
                firework.restart();
            }
            else if (firework.isDone())
            {
                if (state == STATE_POWER)
                {
                    if (level.power < 0.5f)
                    {
                        Debug.WriteLine("Back to pump state");
                        startState(STATE_PUMP);
                    }
                }                

                if (fireworkQueue.Count > 0)                
                {
                    Debug.WriteLine("Start firework");
                    startFirework();
                }
                else
                {
                    if (state == STATE_PUMP)
                    {
                        if (level.power < 0.5f)
                        {
                            Debug.WriteLine("Penalty");
                            startState(STATE_PENALTY);
                        }
                        else
                        {
                            Debug.WriteLine("Start power state");
                            startState(STATE_POWER);
                        }
                    }
                    else if (state == STATE_PENALTY)
                    {
                        if (level.power < 0.5f)
                        {
                            startState(STATE_PUMP);
                        }
                        else
                        {
                            startState(STATE_POWER);
                        }
                    }
                    else if (state == STATE_POWER)
                    {
                        if (level.power > 0.7f)
                        {
                            Debug.WriteLine("Start end power");
                            addFireworksQueue(powerEndFireworkData);
                        }                        
                    }
                }
            }
        }        

        public override void draw2(Canvas canvas)
        {
            base.draw2(canvas);

            AppGraphics.DrawString(0, 0, "Power: " + level.power);
        }

        private void startFirework()
        {
            Debug.Assert(fireworkQueue.Count > 0);

            FireworkInfo info = fireworkQueue.Dequeue();
            float x1 = info.x1;
            float y1 = info.y1;
            float x2 = info.x2;
            float y2 = info.y2;            
            float delay = info.delay;
            int[] ids = info.ids;
                        
            firework.start(setuper, x1, y1, x2, y2, delay);
            firework.pillsCount = info.pillsCount;
            setuper.init(ids, info.sleepCount, firework.pillsCount - info.sleepCount);
            firework.flyTime = info.flyTime;
            firework.lifeTime = info.lifeTime;
            firework.explSpeed = info.expSpeed;            
        }
    }
}
