using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckstazyLive.game.levels.fx;
using DuckstazyLive.game.levels.generator;
using DuckstazyLive.app;
using asap.graphics;
using DuckstazyLive.app.game;
using asap.util;

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
            : base(100, 70)
        {
            frog1 = new FrogActor(media);
            frog2 = new FrogActor(media);

            frog1.speedHands = 5.0f;
            frog2.speedHands = 5.0f;

            frog1.x = 0.0f;
            frog2.x = 960.0f - 216.0f;
            frog2.y = frog1.y = 600.0f - 96.0f;

            arrow1 = new HintArrow(media);
            arrow2 = new HintArrow(media);

            day = false;
        }

        public override void onStart()
        {
            Placer placer;

            base.onStart();

            setuper = new PartySetuper();
            setuper.userCallback = partyLogic;

            jumper = new JumpStarter();
            jumper.userCallback = jumpLogic;

            setuper.dangerH = 0.0f;
            setuper.jump = 0.1f;

            frogGen1 = new Generator();
            frogGen1.regen = true;
            frogGen1.speed = 4.0f;
            placer = new Placer(setuper, 108.0f, 519.0f);
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
            placer = new Placer(setuper, 960.0f - 108.0f, 519.0f);
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

            gen.map.Add(new Placer(jumper, 255.0f, 570.0f));
            gen.map.Add(new Placer(jumper, 705.0f, 570.0f));

            arrow1.place(285.0f, 540.0f, 3.14f * 0.25f, 0xfff7a0e1, true);
            arrow2.place(675.0f, 540.0f, -3.14f * 0.25f, 0xfff7a0e1, true);
            arrow1.visibleCounter = 0.0f;
            arrow2.visibleCounter = 0.0f;
            arrow1.visible = true;
            arrow2.visible = true;
            arrowHider = 3.0f;

            gen.start();

            startX = 439.5f;
        }        

        public override void onWin()
        {
            gen.finish();
        }

        public override void Update(float dt)
        {
            base.Update(dt);

            gen.Update(dt);

            if (frog1.open && frog1.openCounter >= 1.0f)
            {
                frogGen1.Update(dt);
                frog1c -= dt;
                if (frog1c <= 0.0f)
                {
                    frog1c = 0.0f;
                    frog1.open = false;
                }
            }

            if (frog2.open && frog2.openCounter >= 1.0f)
            {
                frogGen2.Update(dt);
                frog2c -= dt;
                if (frog2c <= 0.0f)
                {
                    frog2c = 0.0f;
                    frog2.open = false;
                }
            }

            frog1.Update(dt);
            frog2.Update(dt);

            if (arrowHider > 0.0f)
            {
                arrowHider -= dt;
                if (arrowHider <= 0.0f)
                {
                    arrow1.visible = arrow2.visible = false;
                }
            }

            arrow1.Update(dt);
            arrow2.Update(dt);

        }

        public override void draw1(Graphics g)
        {
            frog1.draw(g);
            frog2.draw(g);
            arrow1.Draw(g);
            arrow2.Draw(g);
        }        

        public void jumpLogic(Pill pill, String msg, float dt)
        {
            FrogActor frog;
            if (msg == "jump")
            {
                if (pill.x > 480.0f)
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
                pill.vy += 450.0f * dt;
                pill.x += pill.vx * dt;
                pill.y += pill.vy * dt;

                if (pill.x > 945.0f)
                {
                    pill.vx = -pill.vx * friction;
                    pill.vy = pill.vy * friction;
                    pill.x = 945.0f;
                }
                if (pill.x < 15.0f)
                {
                    pill.vx = -pill.vx * friction;
                    pill.vy = pill.vy * friction;
                    pill.x = 15.0f;
                }

                if (pill.y < 15.0f)
                {
                    pill.vy = -pill.vy * friction;
                    pill.vx = pill.vx * friction;
                    pill.y = 15.0f;
                }
                if (pill.y > 585.0f)
                {
                    pill.vy = -pill.vy * friction;
                    pill.vx = pill.vx * friction;
                    pill.y = 585.0f;
                }
            }
            else if (msg == "born")
            {
                pill.vx = (225.0f + 225.0f * level.power) * (RandomHelper.rnd() * 2.0f - 1.0f);
                pill.vy = -150.0f - RandomHelper.rnd() * 300.0f - 300.0f * level.power;
            }
        }
    }
}
