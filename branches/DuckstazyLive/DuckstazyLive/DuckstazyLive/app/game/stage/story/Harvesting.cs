using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckstazyLive.game.levels.generator;
using DuckstazyLive.app.game;
using asap.graphics;
using System.Diagnostics;

namespace DuckstazyLive.game.levels
{
    public class Harvesting : PumpLevelStage
    {
        public Generator gen;
        public PowerSetuper powers1;
        public PowerSetuper powers2;
        public PowerSetuper powers3;
        public float prog;

        public Harvesting()
        {
            pumpVel = 0.2f;
        }

        public override void onStart()
        {
            base.onStart();
            startX = 439.5f;            
            prog = 0.0f;

            gen = new Generator();
            powers1 = new PowerSetuper(0.0f, PowerSetuper.POWER1);
            powers2 = new PowerSetuper(0.3f, PowerSetuper.POWER2);
            powers3 = new PowerSetuper(1.0f, PowerSetuper.POWER3);
            powers1.userCallback = pillLogic;
            powers2.userCallback = pillLogic;
            powers3.userCallback = pillLogic;
            gen.regen = true;
            gen.addLine(powers1, 60, 510, 60, 0, 15);

            gen.start();            
        }

        public override void onWin()
        {
            gen.regen = false;
        }

        public override void Update(float dt)
        {
            int i = 0;

            base.Update(dt);

            gen.Update(dt);

            Heroes heroes = getHeroes();
            foreach (Placer o in gen.map)
            {
                if (i < 15)
                    o.y = 570 - heroes.getJumpHeight();
                else if (i < 30)
                    o.y = 570 - heroes.getJumpHeight() * 0.5f;
                else if (i < 45)
                    break;
                ++i;
            }

            if (gen.map.Count < 30 && level.power > 0.33)
            {
                i = (int)(570 - heroes.getJumpHeight() * 0.5f);
                gen.addLine(powers2, 60, i, 60, 0, 15);
            }
            else if (gen.map.Count < 45 && level.power > 0.66)
            {
                i = 570;
                gen.addLine(powers3, 60, i, 60, 0, 15);
            }

        }

        public override void draw1(Graphics g)
        {

        }

        public void pillLogic(Pill pill, String msg, float dt)
        {
            float t;

            if (msg == null)
            {
                t = pill.t1;
                t += dt * (0.5f + level.power * 0.5f);
                if (t > 1.0f) t -= (int)(t);
                pill.t1 = t;
                pill.y = (float)(570 - getHeroes().getJumpHeight() * pill.t2 + 10 * Math.Sin(pill.t1 * 6.28f));                
            }
            else if (msg == "born")
            {
                pill.t1 = 0.0f;
                pill.t2 = (570 - pill.y) / getHeroes().getJumpHeight();//pill.y;
            }
        }        
    }
}
