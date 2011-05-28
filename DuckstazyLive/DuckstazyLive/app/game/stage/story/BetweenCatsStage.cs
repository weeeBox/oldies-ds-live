using System;
using DuckstazyLive.game.levels.generator;
using Microsoft.Xna.Framework;
using DuckstazyLive.app.game;
using DuckstazyLive.game.levels.fx;
using asap.util;
using asap.graphics;
using DuckstazyLive.app;

namespace DuckstazyLive.game.levels
{
    public class BetweenCatsStage : PillCollectLevelStage
    {
        private const int TOXIC_RATE = 3;

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
        private int catSpawnedPillsCount;

        private HintArrow catArrow;

        public BetweenCatsStage() : base(100)
        {
            catArrow = new HintArrow(media);            
        }

        public override void onStart()
        {
            JumpStarter jumper = new JumpStarter();
            PartySetuper party = new PartySetuper();
            Placer placer1 = new Placer(party, 157.5f, 444.0f);
            Placer placer2 = new Placer(party, 802.5f, 444.0f);

            int i;

            party.userCallback = partyLogic;

            base.onStart();

            gen = new Generator();

            gen.map.Add(new Placer(jumper, 450.0f, 555.0f));
            gen.map.Add(new Placer(jumper, 510.0f, 510.0f));
            gen.map.Add(new Placer(jumper, 450.0f, 465.0f));
            gen.map.Add(new Placer(jumper, 510.0f, 420.0f));

            gen.map.Add(new Placer(jumper, 750.0f, 375.0f));
            gen.map.Add(new Placer(jumper, 810.0f, 330.0f));
            gen.map.Add(new Placer(jumper, 750.0f, 285.0f));
            gen.map.Add(new Placer(jumper, 810.0f, 240.0f));

            gen.map.Add(new Placer(jumper, 150.0f, 375.0f));
            gen.map.Add(new Placer(jumper, 210.0f, 330.0f));
            gen.map.Add(new Placer(jumper, 150.0f, 285.0f));
            gen.map.Add(new Placer(jumper, 210.0f, 240.0f));

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

            Pills pills = getPills();
            pills.findDead().startMatrix(480.0f, 150.0f);
            pills.findDead().startToxic(330.0f, 270.0f, 1);
            pills.findDead().startToxic(630.0f, 270.0f, 1);
            pills.actives += 3;

            catGen = 1.0f;
            catHum = 0.0f;
            catAttack = 0.0f;
            catFinalAttack = 0.0f;
            catToxic = null;

            catAliveL = true;
            catAliveR = true;

            catStage = 0;

            catArrow.place(165.0f, 225.0f, 0, 0xfff7a0e1, false);

            //killer.init();

            startX = 439.5f;
            catSpawnedPillsCount = 0;
        }

        public void catPillsCallback(Pill pill, String msg, float dt)
        {
            if (msg == null)
            {
                float t;
                pill.t1 += dt * 0.5f;
                t = (float)(Math.Cos(pill.t1 * 10.064f) * 0.2f + 0.8f) * 212.0f;
                pill.x = 480.0f - (float)(t * Math.Cos(pill.t1));
                pill.y = 336.0f - (float)(t * Math.Sin(pill.t1));
                if (pill.state < Pill.DYING)
                {
                    if (pill.t1 > 2.95f && pill.t1 < 3.12f)
                        catHum = 0.5f;
                    else if (pill.t1 >= 3.12f)
                    {
                        pill.kill();
                        getParticles().startAcid(pill.x, pill.y);
                        catHum = 0.0f;
                    }
                }
            }
        }

        private void launchMissle(Pill pill, float xo, float yo)
        {
            pill.startMissle(xo, yo, 0);

            Hero hero = getHeroes()[0];

            if (catStage == 0)
            {
                pill.t1 = 5.0f;
                pill.vx = (40.5f + hero.x - xo) / 1.2f;
                pill.vy = (30.0f + hero.y - yo) / 1.2f - 675.0f * 1.2f;
                if (pill.vy > 150.0f) pill.vy = 150.0f;
                else if (pill.vy < -150.0f) pill.vy = -150.0f;
            }
            else if (catStage == 1)
            {
                pill.t1 = 5.0f;
                pill.vx = (40.5f + hero.x - xo) / 1.2f;
                pill.vy = (30.0f + hero.y - yo) / 1.2f - 675.0f * 1.2f;
                if (pill.vy > 150.0f) pill.vy = 150.0f;
                else if (pill.vy < -150.0f) pill.vy = -150.0f;
            }
            else if (catStage == 2)
            {
                pill.t1 = 5.0f;
                pill.vx = (40.5f + hero.x - xo) / 1.2f;
                pill.vy = (30.0f + hero.y - yo) / 1.2f - 675.0f * 1.2f;
                if (pill.vy > 150.0f) pill.vy = 150.0f;
                else if (pill.vy < -150.0f) pill.vy = -150.0f;
            }
            pill.t2 = 0.1f;
            catToxic = pill;
        }

        public override void Update(float dt)
        {
            Pill pill;
            int newPills = 0;

            base.Update(dt);

            if (!catAliveL)
                gen1.Update(dt);
            if (!catAliveR)
                gen2.Update(dt);
            gen.Update(dt);

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
                            getParticles().startStarToxic(catToxic.x, catToxic.y, -catToxic.vx * 0.2f, -catToxic.vy * 0.2f, 0);
                        }

                        catToxic.vy += 1350.0f * dt;
                        catToxic.x += catToxic.vx * dt;
                        catToxic.y += catToxic.vy * dt;
                        catToxic.t1 -= dt;

                        if (catToxic.x < 15.0f || catToxic.x > 945.0f)
                        {
                            catToxic.vx = -catToxic.vx * 0.7f;
                            if (catToxic.x < 15.0f) catToxic.x = 15.0f;
                            else catToxic.x = 945.0f;
                        }
                        if (catToxic.y > 585.0f || catToxic.y < 15.0f)
                        {
                            catToxic.vy = -catToxic.vy * 0.7f;
                            if (catToxic.y < 15.0f) catToxic.y = 15.0f;
                            else catToxic.y = 585.0f;
                        }
                        if (catToxic.t1 < 0.0f)
                        {
                            getParticles().explStarsToxic(catToxic.x, catToxic.y, 0, true);
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
                    pill = getPills().findDead();
                    if (pill != null)
                    {
                        if (catStage == 0)
                        {                            
                            float psx = 160.5f;
                            float psy = 336.0f;
                            if (catSpawnedPillsCount % TOXIC_RATE == 0)
                            {
                                pill.startMissle(psx, psy, Pill.TOXIC_SKULL);
                            }
                            else
                            {
                                pill.startPower(psx, psy, 1, false);
                            }
                            pill.t1 = 0.0f;
                            pill.user = catPillsCallback;
                            catGen -= 1.0f;
                            newPills++;
                            catSpawnedPillsCount++;
                        }
                    }

                }

                if (catStage != 0)
                {
                    catAttack -= dt;
                    if (catAliveR && catHum <= 0.0f && catAttack < 0.0f && catToxic == null)
                    {
                        pill = getPills().findDead();
                        if (pill != null)
                        {
                            launchMissle(pill, 798.0f, 331.5f);
                            if (catStage == 0) catAttack = 5.0f;
                            else if (catStage == 1) catAttack = 2.0f;
                            else if (catStage == 2) catAttack = 1.5f;
                            catHum = 0.5f;
                            newPills++;
                        }
                    }
                }

                catArrow.Update(dt);
            }

            Hero hero = getHeroes()[0];

            switch (catStage)
            {
                case 0:
                    if (getCollectedPills() >= 25)
                    {
                        catStage = 1;
                        catArrow.visible = true;
                    }
                    break;
                case 1:
                    if (hero.x >= 114.0f - 81.0f && hero.x <= 210.0f &&
                        hero.lastPos.Y < 267.0f - 60.0f && hero.y >= 267.0f - 60.0f)
                    {
                        hero.jump(180.0f);
                        getEnv().startBlanc();
                        catAliveL = false;
                        catStage = 2;
                        catFinalAttack = 15.0f;

                        catArrow.x = 798.0f;
                        catArrow.visible = false;
                        catArrow.visibleCounter = 0.0f;
                        ColorUtils.ctSetRGB(ref catArrow.ctForm, 0xffb300);
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
                        if (hero.x >= 750.0f - 81.0f && hero.x <= 849.0f &&
                            hero.lastPos.Y < 267.0f - 60.0f && hero.y >= 267.0f - 60.0f)
                        {
                            hero.jump(180.0f);
                            getEnv().startBlanc();
                            catAliveR = false;
                            catStage = 3;
                            catArrow.visible = false;
                        }
                    }
                    break;
            }


            getPills().actives += newPills;
        }

        public override void draw1(Graphics g)
        {
            //Rect rc;
            //Vector2 p;
            //int bm;
            //SpriteTexture bmTex;

            //if (level.power >= 0.5f)
            //{
            //    rc = new Rect();
            //    p = new Vector2();

            //    Env env = GameElements.Env;
            //    bool drawFade = env.isHitFaded();

            //    if (catAliveL)
            //    {                    
            //        drawElement(canvas, media.imgCatL, 81.0f, 205.5f, ref ColorTransform.NONE);

            //        if (catGen < 0.5f)
            //        {
            //            bm = media.imgCatHum;
            //            bmTex = utils.getTexture(bm);
            //            rc.Width = bmTex.Width;
            //            rc.Height = bmTex.Height;
            //            p.X = 138.0f;
            //            p.Y = 318.0f;
            //            canvas.copyPixels(bm, rc, p);
            //        }
            //        else
            //        {
            //            bm = media.imgCatSmile;
            //            bmTex = utils.getTexture(bm);
            //            rc.Width = bmTex.Width;
            //            rc.Height = bmTex.Height;
            //            p.X = 138.0f;
            //            p.Y = 328.5f;
            //            canvas.copyPixels(bm, rc, p);
            //        }
            //        if (drawFade)
            //        {
            //            drawElement(canvas, media.imgCatL, 81.0f, 205.5f, ref env.blackFade);
            //        }
            //    }

            //    if (catAliveR)
            //    {                    
            //        drawElement(canvas, media.imgCatR, 576.0f, 201.0f, ref ColorTransform.NONE);

            //        if (catHum > 0.0f)
            //        {
            //            bm = media.imgCatHum;
            //            bmTex = utils.getTexture(bm);
            //            rc.Width = bmTex.Width;
            //            rc.Height = bmTex.Height;
            //            p.X = 775.5f;
            //            p.Y = 313.5f;
            //            canvas.copyPixels(bm, rc, p);
            //        }
            //        else
            //        {
            //            bm = media.imgCatSmile;
            //            bmTex = utils.getTexture(bm);
            //            rc.Width = bmTex.Width;
            //            rc.Height = bmTex.Height;
            //            p.X = 774.0f;
            //            p.Y = 324.0f;
            //            canvas.copyPixels(bm, rc, p);
            //        }
            //        if (drawFade)
            //        {
            //            drawElement(canvas, media.imgCatR, 576.0f, 201.0f, ref env.blackFade);
            //        }
            //    }
                                
            //    drawElement(canvas, media.imgPedestalL, 0.0f, 429.0f, ref ColorTransform.NONE);
            //    if (drawFade) drawElement(canvas, media.imgPedestalL, 0.0f, 429.0f, ref env.blackFade);

            //    drawElement(canvas, media.imgPedestalR, 612.0f, 432.0f, ref ColorTransform.NONE);
            //    if (drawFade) drawElement(canvas, media.imgPedestalR, 612.0f, 432.0f, ref env.blackFade);

            //    catArrow.draw(canvas);
            //}
            throw new NotImplementedException();
        }

        private void drawElement(Graphics g, int tex, int x, int y, ref ColorTransform transform)
        {            
            //DrawMatrix m = DrawMatrix.ScaledInstance;
            //m.translate(x, y);
            //canvas.draw(tex, m, transform);
            throw new NotImplementedException();
        }        

        public void partyLogic(Pill pill, String msg, float dt)
        {
            float friction = 0.7f;
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
                pill.vx = 450.0f * (RandomHelper.rnd() * 2.0f - 1.0f);
                pill.vy = -450.0f - RandomHelper.rnd() * 300.0f;
            }
        }
    }
}
