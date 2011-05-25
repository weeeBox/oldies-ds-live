using System;
using DuckstazyLive.game.levels.fx;
using DuckstazyLive.game.levels.generator;
using asap.graphics;
using DuckstazyLive.app.game;
using asap.util;

namespace DuckstazyLive.game.levels
{
    public class Bubbles : PumpLevelStage
    {
        public Generator gen;
        public JumpStarter jumper;
        public PartySetuper setuper;

        private float x;
        private int count;
        private float counter;

        private int danger;

        private HintArrow arrow1;
        private HintArrow arrow2;
        private HintArrow arrow3;
        private float arrowHider;

        private int lostPillsCount;

        public Bubbles(float pumVel, int danger)            
        {
            this.pumpVel = pumpVel;
            this.danger = danger;

            arrow1 = new HintArrow(media);
            arrow2 = new HintArrow(media);
            arrow3 = new HintArrow(media);
        }

        public override void onStart()
        {
            //float y = 350.0f;
            //float x0 = 40.0f;
            //float dx = 110.0f;
            //float dy = 30.0f;			

            base.onStart();

            setuper = new PartySetuper();
            setuper.userCallback = partyLogic;

            jumper = new JumpStarter();
            jumper.userCallback = jumpLogic;

            if (danger == 0)
            {
                //setuper.sleeps = 1.0;
                //setuper.toxics = 1.0;
                //setuper.sleeps = 1.0;
                setuper.dangerH = 0.0f;
                setuper.jump = 0.1f;
            }
            else if (danger == 1)
            {
                setuper.powers = 0.8f;
                setuper.sleeps = 0.9f;
                setuper.toxics = 1.0f;

                setuper.dangerH = 400.0f;

                setuper.jump = 0.1f;
            }
            else if (danger == 2)
            {
                setuper.powers = 0.6f;
                setuper.sleeps = 0.8f;
                setuper.toxics = 1.0f;

                setuper.dangerH = 400.0f;

                setuper.jump = 0.1f;
            }

            gen = new Generator();
            gen.regen = false;
            gen.speed = 4.0f;

            gen.map.Add(new Placer(jumper, 320, 380));
            gen.map.Add(new Placer(jumper, 170, 380));
            gen.map.Add(new Placer(jumper, 470, 380));
            gen.start();

            counter = 0.1f;
            count = 0;
            lostPillsCount = 0;
            x = 320.0f;

            if (RandomHelper.rnd() > 0.5f) startX = 218;
            else startX = 368;

            arrow1.place(320, 350, 0, 0xffffb300, true);
            arrow2.place(170, 350, 0, 0xffffb300, true);
            arrow3.place(470, 350, 0, 0xffffb300, true);
            arrow1.visibleCounter = 0.0f;
            arrow2.visibleCounter = 0.0f;
            arrow3.visibleCounter = 0.0f;
            arrow1.visible = true;
            arrow2.visible = true;
            arrow3.visible = true;
            arrowHider = 3.0f;            
        }

        public override void onWin()
        {
            gen.finish();
            //gen.regen = false;
        }

        public override void Update(float dt)
        {            
            Pill p;

            base.Update(dt);

            gen.Update(dt);

            if (count > 0)
            {
                counter -= dt;
                if (counter <= 0.0f)
                {
                    Pills pills = getPills();
                    p = pills.findDead();
                    if (p != null)
                    {
                        setuper.start(x - 150.0f + RandomHelper.rnd() * 300, 380, p);
                        pills.actives++;
                    }
                    --count;
                    counter = 0.1f;
                }
            }

            //if(hero.y<0)
            //{
            //    hero.y = 0;
            //    if(hero.jumpVel>0) hero.jumpVel = 0;//-hero.jumpVel;
            //}


            if (arrowHider > 0.0f)
            {
                arrowHider -= dt;
                if (arrowHider <= 0.0f)
                {
                    arrow1.visible = arrow2.visible = arrow3.visible = false;
                }
            }

            arrow1.Update(dt);
            arrow2.Update(dt);
            arrow3.Update(dt);
        }

        public override void draw1(Graphics g)
        {
            arrow1.draw(g);
            arrow2.draw(g);
            arrow3.draw(g);
        }

        public void jumpLogic(Pill pill, String msg, float dt)
        {
            Heroes heroes = getHeroes();
            if (msg == null)
            {
                pill.y = 420 - heroes.getJumpHeight();
                if (pill.y < 90) pill.y = 90;
            }
            else if (msg == "born")
            {
                pill.y = 420 - heroes.getJumpHeight();
            }
            else if (msg == "jump")
            {
                x = pill.x;
                count += 2 + (int)(level.power * 5);
            }
        }

        public void partyLogic(Pill pill, String msg, float dt)
        {
            if (msg == null && pill.enabled)
            {
                pill.vy -= (level.power + 0.1f) * 30.0f * dt;
                pill.vx += (float)(200.0 * Math.Sin((pill.t1 + pill.t2) * 6.2831) * dt);
                pill.x += pill.vx * dt;
                pill.y += pill.vy * dt;

                if (pill.x >= 630)
                {
                    pill.vx = -pill.vx;
                    pill.t1 = -pill.t1;
                    pill.x = 630;
                }
                if (pill.x <= 10)
                {
                    pill.vx = -pill.vx;
                    pill.t1 = -pill.t1;
                    pill.x = 10;
                }

                pill.t2 += dt;

                if (pill.y <= -10.0f && pill.isAlive())
                {
                    pill.kill();
                    getParticles().startAcid(pill.x, pill.y);

                    lostPillsCount++;                    
                }
            }
            else if (msg == "born")
            {
                pill.t1 = RandomHelper.rnd();
                pill.t2 = 0.5f + RandomHelper.rnd();
                pill.vx = -10.0f + RandomHelper.rnd() * 20.0f;
                pill.vy = -5.0f - 50 * level.power * RandomHelper.rnd();
                //pill.enabled = true;
                //pill.warning = 0.0;
            }            
        }
    }
}
