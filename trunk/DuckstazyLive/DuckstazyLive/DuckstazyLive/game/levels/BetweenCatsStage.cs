using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckstazyLive.game.levels.generator;
using DuckstazyLive.game.levels.fx;
using DuckstazyLive.app;
using Framework.utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DuckstazyLive.game.levels
{
    public class BetweenCatsStage : PillCollectLevelStage
    {
        private Generator gen;
        private Generator gen1;
        private Generator gen2;

        private float catGen;
        private float catHum;

        private Pill catToxic;
        private float catAttack;

        private bool catAliveL;
        private bool catAliveR;
        private float catFinalAttack;

        private int catStage;

        private HintArrow catArrow;

        public BetweenCatsStage() : base(100)
        {
            catArrow = new HintArrow(media);            
        }

        public override void start()
        {
            JumpStarter jumper = new JumpStarter();
            PartySetuper party = new PartySetuper();
            Placer placer1 = new Placer(party, 105, 296);
            Placer placer2 = new Placer(party, 535, 296);

            int i;

            party.userCallback = partyLogic;

            base.start();

            gen = new Generator();

            gen.map.Add(new Placer(jumper, 300.0f, 370.0f));
            gen.map.Add(new Placer(jumper, 340.0f, 340.0f));
            gen.map.Add(new Placer(jumper, 300.0f, 310.0f));
            gen.map.Add(new Placer(jumper, 340.0f, 280.0f));

            gen.map.Add(new Placer(jumper, 500.0f, 250.0f));
            gen.map.Add(new Placer(jumper, 540.0f, 220.0f));
            gen.map.Add(new Placer(jumper, 500.0f, 190.0f));
            gen.map.Add(new Placer(jumper, 540.0f, 160.0f));

            gen.map.Add(new Placer(jumper, 100.0f, 250.0f));
            gen.map.Add(new Placer(jumper, 140.0f, 220.0f));
            gen.map.Add(new Placer(jumper, 100.0f, 190));
            gen.map.Add(new Placer(jumper, 140.0f, 160.0f));

            gen.regen = false;
            gen.heroSqrDist = 0;
            gen.start();

            gen1 = new Generator();
            gen2 = new Generator();
            gen1.regen = gen2.regen = false;

            i = 40;
            while (i > 0) { gen1.map.Add(placer1); --i; }
            i = 40;
            while (i > 0) { gen2.map.Add(placer2); --i; }

            pills.findDead().startMatrix(320.0f, 100.0f);
            pills.findDead().startToxic(220.0f, 180.0f, 1);
            pills.findDead().startToxic(420.0f, 180.0f, 1);
            pills.actives += 3;

            catGen = 1.0f;
            catHum = 0.0f;
            catAttack = 0.0f;
            catFinalAttack = 0.0f;
            catToxic = null;

            catAliveL = true;
            catAliveR = true;

            catStage = 0;

            catArrow.place(110, 150, 0, 0xfff7a0e1, false);

            //killer.init();

            startX = 293;
        }

        public void catPillsCallback(Pill pill, String msg, float dt)
        {
            if (msg == null)
            {
                float t;
                pill.t1 += dt * 0.5f;
                t = (float)(Math.Cos(pill.t1 * 10.064f) * 0.2f + 0.8f) * 212.0f;
                pill.x = 320.0f - (float)(t * Math.Cos(pill.t1));
                pill.y = 224.0f - (float)(t * Math.Sin(pill.t1));
                if (pill.state < Pill.DYING)
                {
                    if (pill.t1 > 2.95f && pill.t1 < 3.12f)
                        catHum = 0.5f;
                    else if (pill.t1 >= 3.12f)
                    {
                        pill.kill();
                        level.pills.ps.startAcid(pill.x, pill.y);
                        catHum = 0.0f;
                    }
                }
            }
        }

        private void launchMissle(Pill pill, float xo, float yo)
        {
            pill.startMissle(xo, yo, 0);

            Hero hero = level.heroes[0];

            if (catStage == 0)
            {
                pill.t1 = 5.0f;
                pill.vx = (27.0f + hero.x - xo) / 1.2f;
                pill.vy = (20.0f + hero.y - yo) / 1.2f - 450.0f * 1.2f;
                if (pill.vy > 100.0f) pill.vy = 100.0f;
                else if (pill.vy < -100.0f) pill.vy = -100.0f;
            }
            else if (catStage == 1)
            {
                pill.t1 = 5.0f;
                pill.vx = (27.0f + hero.x - xo) / 1.2f;
                pill.vy = (20.0f + hero.y - yo) / 1.2f - 450.0f * 1.2f;
                if (pill.vy > 100.0f) pill.vy = 100.0f;
                else if (pill.vy < -100.0f) pill.vy = -100.0f;
            }
            else if (catStage == 2)
            {
                pill.t1 = 5.0f;
                pill.vx = (27.0f + hero.x - xo) / 1.2f;
                pill.vy = (20.0f + hero.y - yo) / 1.2f - 450.0f * 1.2f;
                if (pill.vy > 100.0f) pill.vy = 100.0f;
                else if (pill.vy < -100.0f) pill.vy = -100.0f;
            }
            pill.t2 = 0.1f;
            catToxic = pill;
        }

        public override void update(float dt)
        {
            Pill pill;
            int newPills = 0;

            base.update(dt);

            if (!catAliveL)
                gen1.update(dt);
            if (!catAliveR)
                gen2.update(dt);
            gen.update(dt);

            if (level.power >= 0.5f)
            {
                gen.clearPills();
                gen.clearMap();
            }

            if (catToxic != null)
            {
                if (catToxic.state != Pill.DEAD)
                {
                    if (catToxic.state < Pill.DYING)
                    {
                        catToxic.t2 -= dt;
                        if (catToxic.t2 < 0.0f)
                        {
                            catToxic.t2 = 0.05f;
                            pills.ps.startStarToxic(catToxic.x, catToxic.y, -catToxic.vx * 0.2f, -catToxic.vy * 0.2f, 0);
                        }

                        catToxic.vy += 900.0f * dt;
                        catToxic.x += catToxic.vx * dt;
                        catToxic.y += catToxic.vy * dt;
                        catToxic.t1 -= dt;

                        if (catToxic.x < 10.0f || catToxic.x > 630.0f)
                        {
                            catToxic.vx = -catToxic.vx * 0.7f;
                            if (catToxic.x < 10.0f) catToxic.x = 10.0f;
                            else catToxic.x = 630.0f;
                        }
                        if (catToxic.y > 390.0f || catToxic.y < 10.0f)
                        {
                            catToxic.vy = -catToxic.vy * 0.7f;
                            if (catToxic.y < 10.0f) catToxic.y = 10.0f;
                            else catToxic.y = 390.0f;
                        }
                        if (catToxic.t1 < 0.0f)
                        {
                            pills.ps.explStarsToxic(catToxic.x, catToxic.y, 0, true);
                            catToxic.kill();
                        }
                    }

                }
                else catToxic = null;
            }

            if (level.power >= 0.5f)
            {

                if (catHum > 0.0f)
                    catHum -= dt;

                catGen += dt * 2.0f;
                if (catGen > 1.0f)
                {
                    pill = pills.findDead();
                    if (pill != null)
                    {
                        if (catStage == 0)
                        {
                            pill.startPower(107, 224, 1, false);
                            pill.t1 = 0.0f;
                            pill.user = catPillsCallback;
                            catGen -= 1.0f;
                            newPills++;
                        }
                    }

                }

                if (catStage != 0)
                {
                    catAttack -= dt;
                    if (catAliveR && catHum <= 0.0f && catAttack < 0.0f && catToxic == null)
                    {
                        pill = pills.findDead();
                        if (pill != null)
                        {
                            launchMissle(pill, 532, 221);
                            if (catStage == 0) catAttack = 5.0f;
                            else if (catStage == 1) catAttack = 2.0f;
                            else if (catStage == 2) catAttack = 1.5f;
                            catHum = 0.5f;
                            newPills++;
                        }
                    }
                }

                catArrow.update(dt);
            }

            Hero hero = level.heroes[0];

            switch (catStage)
            {
                case 0:
                    if (collected >= 25)
                    {
                        catStage = 1;
                        catArrow.visible = true;
                    }
                    break;
                case 1:
                    if (hero.x >= 76 - 54 && hero.x <= 140 &&
                        hero.yLast < 178 - 40 && hero.y >= 178 - 40)
                    {
                        hero.jump(120);
                        level.env.blanc = 1.0f;
                        catAliveL = false;
                        catStage = 2;
                        catFinalAttack = 15.0f;

                        catArrow.x = 532.0f;
                        catArrow.visible = false;
                        catArrow.visibleCounter = 0.0f;
                        utils.ctSetRGB(catArrow.color, 0xffb300);
                    }
                    break;
                case 2:
                    catFinalAttack -= dt;
                    catArrow.visible = (catFinalAttack <= 5.0f);
                    if (catFinalAttack < 0.0f)
                        catFinalAttack = 15.0f;
                    else if (catFinalAttack < 5.0f)
                    {
                        catAttack = 5.0f;
                        if (hero.x >= 500 - 54 && hero.x <= 566 &&
                            hero.yLast < 178 - 40 && hero.y >= 178 - 40)
                        {
                            hero.jump(120);
                            level.env.blanc = 1.0f;
                            catAliveR = false;
                            catStage = 3;
                            catArrow.visible = false;
                        }
                    }
                    break;
            }


            pills.actives += newPills;
        }

        public override void draw1(Canvas canvas)
        {
            Rect rc;
            Vector2 p;
            int bm;
            Texture2D bmTex;

            if (level.power >= 0.5f)
            {
                rc = new Rect();
                p = new Vector2();

                if (catAliveL)
                {
                    bm = media.imgCatL;
                    bmTex = utils.getTexture(bm);
                    rc.Width = bmTex.Width;
                    rc.Height = bmTex.Height;
                    p.X = 54;
                    p.Y = 137;
                    canvas.copyPixels(bm, rc, p);

                    if (catGen < 0.5f)
                    {
                        bm = media.imgCatHum;
                        bmTex = utils.getTexture(bm);
                        rc.Width = bmTex.Width;
                        rc.Height = bmTex.Height;
                        p.X = 92;
                        p.Y = 212;
                        canvas.copyPixels(bm, rc, p);
                    }
                    else
                    {
                        bm = media.imgCatSmile;
                        bmTex = utils.getTexture(bm);
                        rc.Width = bmTex.Width;
                        rc.Height = bmTex.Height;
                        p.X = 92;
                        p.Y = 219;
                        canvas.copyPixels(bm, rc, p);
                    }
                }

                if (catAliveR)
                {
                    bm = media.imgCatR;
                    bmTex = utils.getTexture(bm);
                    rc.Width = bmTex.Width;
                    rc.Height = bmTex.Height;
                    p.X = 384;
                    p.Y = 134;
                    canvas.copyPixels(bm, rc, p);

                    if (catHum > 0.0f)
                    {
                        bm = media.imgCatHum;
                        bmTex = utils.getTexture(bm);
                        rc.Width = bmTex.Width;
                        rc.Height = bmTex.Height;
                        p.X = 517;
                        p.Y = 209;
                        canvas.copyPixels(bm, rc, p);
                    }
                    else
                    {
                        bm = media.imgCatSmile;
                        bmTex = utils.getTexture(bm);
                        rc.Width = bmTex.Width;
                        rc.Height = bmTex.Height;
                        p.X = 516;
                        p.Y = 216;
                        canvas.copyPixels(bm, rc, p);
                    }
                }

                bm = media.imgPedestalL;
                bmTex = utils.getTexture(bm);
                rc.Width = bmTex.Width;
                rc.Height = bmTex.Height;
                p.X = 0;
                p.Y = 286;
                canvas.copyPixels(bm, rc, p);

                bm = media.imgPedestalR;
                rc.Width = bmTex.Width;
                rc.Height = bmTex.Height;
                p.X = 408;
                p.Y = 288;
                canvas.copyPixels(bm, rc, p);


                catArrow.draw(canvas);
            }
        }

        public void partyLogic(Pill pill, String msg, float dt)
        {
            float friction = 0.7f;
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
                pill.vx = 300.0f * (utils.rnd() * 2.0f - 1.0f);
                pill.vy = -300.0f - utils.rnd() * 200.0f;
            }
        }
    }
}
