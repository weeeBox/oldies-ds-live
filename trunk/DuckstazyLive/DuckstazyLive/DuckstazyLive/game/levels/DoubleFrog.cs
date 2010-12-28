using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckstazyLive.game.levels.fx;
using DuckstazyLive.game.levels.generator;
using DuckstazyLive.app;

namespace DuckstazyLive.game.levels
{
    public class DoubleFrog : PillCollectLevelStage
    {
        private FrogActor frog1;
        private FrogActor frog2;
        private float frog1c;
        private float frog2c;
        private Generator frogGen1;
        private Generator frogGen2;

        private Generator gen;

        private JumpStarter jumper;
        private PartySetuper setuper;

        private HintArrow arrow1;
        private HintArrow arrow2;
        private float arrowHider;

        public DoubleFrog()
            : base(100)
        {
            frog1 = new FrogActor(media);
            frog2 = new FrogActor(media);

            frog1.speedHands = 5.0f;
            frog2.speedHands = 5.0f;

            frog1.x = 0.0f;
            frog2.x = 640.0f - 144.0f;
            frog2.y = frog1.y = 400.0f - 64;

            arrow1 = new HintArrow(media);
            arrow2 = new HintArrow(media);

            day = false;
        }

        public override void start()
        {
            Placer placer;

            base.start();

            setuper = new PartySetuper();
            setuper.userCallback = partyLogic;

            jumper = new JumpStarter();
            jumper.userCallback = jumpLogic;

            setuper.dangerH = 0.0f;
            setuper.jump = 0.1f;

            frogGen1 = new Generator();
            frogGen1.regen = true;
            frogGen1.speed = 4.0f;
            placer = new Placer(setuper, 72, 346);
            frogGen1.map.Add(placer);
            frogGen1.map.Add(placer);
            frogGen1.map.Add(placer);
            frogGen1.map.Add(placer);
            frogGen1.map.Add(placer);
            frogGen1.map.Add(placer);
            frogGen1.map.Add(placer);
            frogGen1.map.Add(placer);
            frogGen1.map.Add(placer);
            frogGen1.map.Add(placer);

            frogGen2 = new Generator();
            frogGen2.regen = true;
            frogGen2.speed = 4.0f;
            placer = new Placer(setuper, 640 - 72, 346);
            frogGen2.map.Add(placer);
            frogGen2.map.Add(placer);
            frogGen2.map.Add(placer);
            frogGen2.map.Add(placer);
            frogGen2.map.Add(placer);
            frogGen2.map.Add(placer);
            frogGen2.map.Add(placer);
            frogGen2.map.Add(placer);
            frogGen2.map.Add(placer);
            frogGen2.map.Add(placer);

            frog1c = 0.0f;
            frog2c = 0.0f;
            //frogGen1.finish();
            //frogGen2.finish();

            gen = new Generator();
            gen.regen = false;
            gen.speed = 4.0f;

            gen.map.Add(new Placer(jumper, 170, 380));
            gen.map.Add(new Placer(jumper, 470, 380));

            arrow1.place(190, 360, 3.14f * 0.25f, 0xfff7a0e1, true);
            arrow2.place(450, 360, -3.14f * 0.25f, 0xfff7a0e1, true);
            arrow1.visibleCounter = 0.0f;
            arrow2.visibleCounter = 0.0f;
            arrow1.visible = true;
            arrow2.visible = true;
            arrowHider = 3.0f;

            gen.start();

            startX = 293;
        }

        protected override void startProgress()
        {
            progress.start(numPills, 70);
        }

        public override void onWin()
        {
            gen.finish();
        }

        public override void update(float dt)
        {
            base.update(dt);

            gen.update(dt);

            if (frog1.open && frog1.openCounter >= 1.0f)
            {
                frogGen1.update(dt);
                frog1c -= dt;
                if (frog1c <= 0.0f)
                {
                    frog1c = 0.0f;
                    frog1.open = false;
                }
            }

            if (frog2.open && frog2.openCounter >= 1.0f)
            {
                frogGen2.update(dt);
                frog2c -= dt;
                if (frog2c <= 0.0f)
                {
                    frog2c = 0.0f;
                    frog2.open = false;
                }
            }

            frog1.update(dt);
            frog2.update(dt);

            if (arrowHider > 0.0f)
            {
                arrowHider -= dt;
                if (arrowHider <= 0.0f)
                {
                    arrow1.visible = arrow2.visible = false;
                }
            }

            arrow1.update(dt);
            arrow2.update(dt);

        }

        public override void draw1(Canvas canvas)
        {
            frog1.draw(canvas);
            frog2.draw(canvas);
            arrow1.draw(canvas);
            arrow2.draw(canvas);
        }        

        public void jumpLogic(Pill pill, String msg, float dt)
        {
            FrogActor frog;
            if (msg == "jump")
            {
                if (pill.x > 320)
                {
                    frog = frog1;
                    frog1c = 3.0f;
                }
                else
                {
                    frog = frog2;
                    frog2c = 3.0f;
                }

                if (frog.openCounter <= 0.0f)
                    frog.open = true;
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
            }
            else if (msg == "born")
            {
                pill.vx = (150.0f + 150.0f * level.power) * (utils.rnd() * 2.0f - 1.0f);
                pill.vy = -100.0f - utils.rnd() * 200.0f - 200.0f * level.power;
            }
        }
    }
}
