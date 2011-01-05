using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using DuckstazyLive.game.stages.fx;
using DuckstazyLive.game.levels.generator;

namespace DuckstazyLive.game.stages.story
{
    public class Fireworks : StoryLevelStage
    {
        private struct FireworkInfo
        {
            public float x1, y1, x2, y2;
            public float flyTime;
            public float expSpeed;
            public float lifeTime;
            public float delay;

            public FireworkInfo(float x1, float y1, float x2, float y2, float flyTime, float expSpeed, float lifeTime, float delay)
            {
                this.x1 = x1;
                this.y1 = y1;
                this.x2 = x2;
                this.y2 = y2;
                this.flyTime = flyTime;
                this.lifeTime = lifeTime;
                this.expSpeed = expSpeed;
                this.delay = delay;
            }
        }

        private FireworkInfo[] fireworksData =
        {
            new FireworkInfo(0, 400, 320, 80, 2.5f, 120.0f, 4.0f, 3.0f),
            new FireworkInfo(640, 400, 320, 80, 2.0f, 150.0f, 4.0f, 1.0f),
            new FireworkInfo(0, 0, 160, 80, 1.0f, 100.0f, 4.0f, 1.0f),
            new FireworkInfo(640, 0, 480, 80, 1.0f, 100.0f, 4.0f, 1.0f),
        };

        private Generator jumpGen;
        private JumpStarter jumper;

        private Firework firework;
        private int fireworkIndex;

        public Fireworks()
        {
            firework = new Firework();
            jumper = new JumpStarter();
        }

        public override void start()
        {
            day = false;

            base.start();

            jumpGen = new Generator();
            startJumps();

            fireworkIndex = 0;
            startFirework(fireworkIndex);
        }

        public override void stop()
        {            
            jumpGen.reset();
            base.stop();
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
            if (firework.isDone())
            {                
                if (fireworkIndex < fireworksData.Length - 1)
                {
                    fireworkIndex++;
                    startFirework(fireworkIndex);
                }
            }
        }
        
        protected override void startProgress()
        {
            progress.start(0, 0);
        }

        private void startFirework(int index)
        {
            Debug.Assert(index >= 0 && index < fireworksData.Length);

            FireworkInfo info = fireworksData[index];
            float x1 = info.x1;
            float y1 = info.y1;
            float x2 = info.x2;
            float y2 = info.y2;
            firework.start(x1, y1, x2, y2, info.delay);
            firework.flyTime = info.flyTime;
            firework.lifeTime = info.lifeTime;
            firework.explSpeed = info.expSpeed;            
        }
    }
}
