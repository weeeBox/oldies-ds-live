using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using DuckstazyLive.game.stages.fx;

namespace DuckstazyLive.game.stages.story
{
    public class Fireworks : StoryLevelStage
    {
        private struct FireworkInfo
        {
            public float x1, y1, x2, y2;
            public float flyTime;
            public float expSpeed;

            public FireworkInfo(float x1, float y1, float x2, float y2, float flyTime, float expSpeed)
            {
                this.x1 = x1;
                this.y1 = y1;
                this.x2 = x2;
                this.y2 = y2;
                this.flyTime = flyTime;
                this.expSpeed = expSpeed;
            }
        }

        private FireworkInfo[] fireworksData =
        {
            new FireworkInfo(0, 400, 320, 80, 2.5f, 120.0f),
            new FireworkInfo(640, 400, 320, 80, 2.0f, 150.0f),
        };

        private Firework firework;
        private int fireworkIndex;

        public Fireworks()
        {
            firework = new Firework();
        }

        public override void start()
        {
            day = false;

            base.start();

            fireworkIndex = 0;
            startFirework(fireworkIndex);
        }

        public override void update(float dt)
        {
            base.update(dt);
            
            firework.update(dt, level.power);
            if (firework.isDone())
            {                
                if (fireworkIndex < fireworksData.Length - 1)
                {
                    fireworkIndex++;
                    startFirework(fireworkIndex);
                }
            }
        }
        
        protected override void startProgress()
        {
            progress.start(0, 0);
        }

        private void startFirework(int index)
        {
            Debug.Assert(index >= 0 && index < fireworksData.Length);

            float x1 = fireworksData[index].x1;
            float y1 = fireworksData[index].y1;
            float x2 = fireworksData[index].x2;
            float y2 = fireworksData[index].y2;
            firework.start(x1, y1, x2, y2);
            firework.flyTime = fireworksData[index].flyTime;
        }
    }
}
