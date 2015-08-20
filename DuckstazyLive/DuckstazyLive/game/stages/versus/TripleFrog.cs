using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckstazyLive.game.levels.fx;
using DuckstazyLive.game.levels.generator;
using DuckstazyLive.app;
using Microsoft.Xna.Framework;
using Framework.core;

namespace DuckstazyLive.game.stages.versus
{
    public class TripleFrog : VersusLevelStage
    {
        private const int FROGS_COUNT = 3;
        private const float GEN_TIMEOUT = 8.0f;
        private const float PILL_TIMEOUT = GEN_TIMEOUT - 1.0f;

        private float[] counters;
        private FrogActor[] actors;
        private Generator[] generators;
        
        private PartySetuper setuper;

        private float genCounter;

        public TripleFrog(VersusLevel level) : base(level, 60)
        {
            float frogOffset = 64.0f;
            float frogDx = (640.0f - 2 * frogOffset) / (FROGS_COUNT - 1);
            float frogY = 400.0f - 64;

            actors = new FrogActor[FROGS_COUNT];
            for (int frogIndex = 0; frogIndex < actors.Length; ++frogIndex)
            {
                FrogActor frog = new FrogActor(media);
                frog.speedHands = 5.0f;
                frog.x = frogOffset + frogIndex * frogDx - 72.0f;
                frog.y = frogY;

                actors[frogIndex] = frog;
            }

            counters = new float[FROGS_COUNT];
            generators = new Generator[FROGS_COUNT];
            
            day = false;
        }

        public override void onStart()
        {
            base.onStart();

            setuper = new PartySetuper();
            setuper.userCallback = partyLogic;            

            setuper.dangerH = 0.0f;
            setuper.jump = 0.1f;

            genCounter = GEN_TIMEOUT - 2.0f;

            for (int genIndex = 0; genIndex < generators.Length; ++genIndex)
            {
                Generator frogGen = new Generator();                
                frogGen = new Generator();
                frogGen.regen = true;
                frogGen.speed = 4.0f;

                Placer placer = new Placer(setuper, actors[genIndex].x + 72, 346);
                frogGen.map.Add(placer);
                frogGen.map.Add(placer);
                frogGen.map.Add(placer);
                frogGen.map.Add(placer);
                frogGen.map.Add(placer);
                frogGen.map.Add(placer);
                frogGen.map.Add(placer);
                frogGen.map.Add(placer);
                frogGen.map.Add(placer);
                frogGen.map.Add(placer);

                generators[genIndex] = frogGen;
                counters[genIndex] = 0.0f;
            }            
        }        

        protected override void onStop()
        {
            base.onStop();            
        }

        public override void update(float dt)
        {
            base.update(dt);

            genCounter += dt;
            if (genCounter > GEN_TIMEOUT)
            {
                genCounter = 0;

                int genIndex = utils.rnd_int(actors.Length);                
                counters[genIndex] = 3.0f;

                FrogActor frog = actors[genIndex];
                if (frog.openCounter <= 0.0f)
                    frog.open = true;
            }

            for (int frogIndex = 0; frogIndex < actors.Length; ++frogIndex)
            {
                FrogActor frog = actors[frogIndex];
                if (frog.open && frog.openCounter >= 1.0f)
                {
                    generators[frogIndex].update(dt);
                    counters[frogIndex] -= dt;
                    if (counters[frogIndex] <= 0.0f)
                    {
                        counters[frogIndex] = 0.0f;
                        frog.open = false;
                    }
                }
                frog.update(dt);
            }
        }

        public override void draw1(Canvas canvas)
        {
            foreach (FrogActor frog in actors)
            {
                frog.draw(canvas);
            }            
        }        

        public void partyLogic(Pill pill, String msg, float dt)
        {
            float friction = 0.7f + level.power * 0.3f;            
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

                if (pill.isAlive())
                {
                    pill.t1 += dt;
                    if (pill.t1 > PILL_TIMEOUT)
                    {                        
                        getParticles().explStarsToxic(pill.x, pill.y, 0, true);
                        pill.kill();
                    }
                }                
            }
            else if (msg == "born")
            {
                pill.t1 = 0.0f;
                pill.vx = (150.0f + 150.0f * level.power) * (utils.rnd() * 2.0f - 1.0f);
                pill.vy = -100.0f - utils.rnd() * 200.0f - 200.0f * level.power;
            }
        }
    }
}
