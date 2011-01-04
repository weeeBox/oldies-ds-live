using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckstazyLive.game.levels.generator;
using DuckstazyLive.app;
using Microsoft.Xna.Framework;

namespace DuckstazyLive.game.stages.story
{
    public class GrinderStage : StoryLevelStage
    {
        private static readonly byte[] PILLS_CIRCLES =
        {
            8, 12, 16, 20, 24
        };
        
        private const float GRINDER_PILLS_DISTANCE = 70.0f;

        private Generator[] grinderGens;        

        private Generator generator;
        private PowerSetuper setuper;

        private float gcx, gcy;

        public GrinderStage()
        {
            generator = new Generator();
            setuper = new PowerSetuper(0.0f, PowerSetuper.POWER1);
                        
            grinderGens = new Generator[4];
            for (int i = 0; i < grinderGens.Length; ++i)
            {
                grinderGens[i] = new Generator();
            }
        }

        protected override void startProgress()
        {
            progress.start(0, 0);
        }

        public override void start()
        {
            base.start();

            gcx = 320.0f;
            gcy = 390.0f;

            startSculls();
            startPills();

            startX = 320.0f - Hero.duck_w;
        }        

        private void startSculls()
        {
            for (int i = 0; i < grinderGens.Length; ++i)
            {
                cleanGenerator(grinderGens[i]);
            }

            MissleStarter missleStarter = new MissleStarter();            
            for (int i = 1; i <= PILLS_CIRCLES.Length; ++i)
            {
                grinderGens[0].map.Add(new Placer(missleStarter, gcx + i * GRINDER_PILLS_DISTANCE, gcy));
                grinderGens[1].map.Add(new Placer(missleStarter, gcx - i * GRINDER_PILLS_DISTANCE, gcy));
                grinderGens[2].map.Add(new Placer(missleStarter, gcx, gcy + i * GRINDER_PILLS_DISTANCE));
                grinderGens[3].map.Add(new Placer(missleStarter, gcx, gcy - i * GRINDER_PILLS_DISTANCE));
            }
            for (int i = 0; i < grinderGens.Length; ++i)
            {
                grinderGens[i].regen = true;
                grinderGens[i].start();
            }
        }

        private void startPills()
        {
            cleanGenerator(generator);
                        
            for (int i = 0; i < PILLS_CIRCLES.Length; ++i)
            {
                addPillCircle(PILLS_CIRCLES[i], (i + 1) * GRINDER_PILLS_DISTANCE, gcx, gcy);
            }

            generator.regen = true;
            generator.start();
        }

        private void addPillCircle(int pillsCount, float radius, float cx, float cy)
        {
            double da = MathHelper.TwoPi / pillsCount;
            double angle = 0;
            int skipIndex = pillsCount / 4;
            for (int i = 0; i < pillsCount; ++i)
            {
                if (i % skipIndex != 0)
                {
                    float px = (float)(cx + radius * Math.Cos(angle));
                    float py = (float)(cy + radius * Math.Sin(angle));
                    generator.map.Add(new Placer(setuper, px, py));
                }
                angle += da;
            }
        }

        private void cleanGenerator(Generator generator)
        {
            generator.finish();
            generator.clearMap();
        }

        public override void update(float dt)
        {
            base.update(dt);

            generator.update(dt);
            for (int i = 0; i < grinderGens.Length; ++i)
            {
                grinderGens[i].update(dt);
            }
        }

        public override void onWin()
        {
            base.onWin();

            generator.regen = false;
        }
    }
}
