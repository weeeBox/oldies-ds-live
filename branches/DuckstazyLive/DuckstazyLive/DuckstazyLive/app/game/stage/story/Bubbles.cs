using System;
using DuckstazyLive.game.levels.fx;
using DuckstazyLive.game.levels.generator;
using asap.graphics;
using DuckstazyLive.app.game;
using asap.util;
using DuckstazyLive.app.game.stage.fx;
using DuckstazyLive.app.game.level;

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
            //float y = 525.0f;
            //float x0 = 60.0f;
            //float dx = 165.0f;
            //float dy = 45.0f;			

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

                setuper.dangerH = 600.0f;

                setuper.jump = 0.1f;
            }
            else if (danger == 2)
            {
                setuper.powers = 0.6f;
                setuper.sleeps = 0.8f;
                setuper.toxics = 1.0f;

                setuper.dangerH = 600.0f;

                setuper.jump = 0.1f;
            }

            gen = new Generator();
            gen.regen = false;
            gen.speed = 4.0f;

            gen.map.Add(new Placer(jumper, 480.0f, 570.0f));
            gen.map.Add(new Placer(jumper, 255.0f, 570.0f));
            gen.map.Add(new Placer(jumper, 705.0f, 570.0f));
            gen.start();

            counter = 0.1f;
            count = 0;
            lostPillsCount = 0;
            x = 480.0f;

            if (RandomHelper.rnd() > 0.5f) startX = 327.0f;
            else startX = 552.0f;

            arrow1.place(480.0f, 525.0f, 0.0f, 0xffffb300, true);
            arrow2.place(255.0f, 525.0f, 0.0f, 0xffffb300, true);
            arrow3.place(705.0f, 525.0f, 0.0f, 0xffffb300, true);
            arrow1.visibleCounter = 0.0f;
            arrow2.visibleCounter = 0.0f;
            arrow3.visibleCounter = 0.0f;            
            arrowHider = 3.0f;
            level.addPreDraw(arrow1);
            level.addPreDraw(arrow2);
            level.addPreDraw(arrow3);
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
                        setuper.start(x - 225.0f + RandomHelper.rnd() * 450.0f, 570.0f, p);
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
                    level.removePreDraw(arrow1);
                    level.removePreDraw(arrow2);
                    level.removePreDraw(arrow3);
                }
            }            
        }        

        public void jumpLogic(Pill pill, String msg, float dt)
        {
            Heroes heroes = getHeroes();
            if (msg == null)
            {
                pill.y = 630.0f - heroes.getJumpHeight();
                if (pill.y < 135.0f) pill.y = 135.0f;
            }
            else if (msg == "born")
            {
                pill.y = 630.0f - heroes.getJumpHeight();
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
                pill.vy -= (level.power + 0.1f) * 45.0f * dt;
                pill.vx += (float)(300.0f * Math.Sin((pill.t1 + pill.t2) * 6.2831) * dt);
                pill.x += pill.vx * dt;
                pill.y += pill.vy * dt;

                if (pill.x >= 945.0f)
                {
                    pill.vx = -pill.vx;
                    pill.t1 = -pill.t1;
                    pill.x = 945.0f;
                }
                if (pill.x <= 15.0f)
                {
                    pill.vx = -pill.vx;
                    pill.t1 = -pill.t1;
                    pill.x = 15.0f;
                }

                pill.t2 += dt;

                if (pill.y <= -15.0f && pill.isAlive())
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
                pill.vx = -15.0f + RandomHelper.rnd() * 30.0f;
                pill.vy = -7.5f - 75.0f * level.power * RandomHelper.rnd();
                //pill.enabled = true;
                //pill.warning = 0.0;
            }            
        }        
    }
}
