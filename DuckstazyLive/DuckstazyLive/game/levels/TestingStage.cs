using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckstazyLive.game.levels.generator;

namespace DuckstazyLive.game.levels
{
    public class TestingStage : LevelStage
    {
        public Generator gen;        

        public TestingStage() : base(TYPE_BONUS)
        {
            goalTime = 100.0f;            
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

                pill.y = (float)(380 - heroes.getJumpHeight() * pill.t2 + 10 * Math.Sin(pill.t1 * 6.28f));
                pill.x += pill.vx * dt;

                if (pill.x > 640.0f || pill.x < 0.0f)
                    pill.die();
            }
            else if (msg == "born")
            {
                pill.t1 = 0.0f;
                pill.t2 = (380 - pill.y) / heroes.getJumpHeight();//pill.y;
            }
        }

        public override void start()
        {
            base.start();            

            gen = new Generator();
            PowerSetuper powers1 = new PowerSetuper(0.0f, PowerSetuper.POWER1);
            PowerSetuper powers2 = new PowerSetuper(0.3f, PowerSetuper.POWER2);
            PowerSetuper powers3 = new PowerSetuper(1.0f, PowerSetuper.POWER3);
            MissleStarter missle = new MissleStarter();
            
            powers1.userCallback = pillLogic;
            powers2.userCallback = pillLogic;
            powers3.userCallback = pillLogic;
            missle.userCallback = pillLogic;
            gen.regen = true;
            gen.speed = 2.0f;

            for (int i = 0; i < 4; ++i)
            {
                gen.map.Add(new Placer(powers1, 0, 340));
                gen.map.Add(new Placer(powers2, 0, 340));
                gen.map.Add(new Placer(missle, 0, 340));
                gen.map.Add(new Placer(powers3, 0, 340));
                gen.map.Add(new Placer(powers1, 0, 340));                
            }
            gen.start();
        }

        public override void onWin()
        {
            gen.regen = false;
        }

        public override void update(float dt)
        {
            base.update(dt);
            gen.update(dt);            
        }
    }
}
