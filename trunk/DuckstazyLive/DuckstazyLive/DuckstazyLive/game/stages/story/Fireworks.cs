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
        private Firework firework;

        public Fireworks()
        {
            firework = new Firework();
        }

        public override void start()
        {
            day = false;

            base.start();
            firework.lifeTime = 5.0f;
            firework.pillsCount = 10;
            firework.genTimeout = 0.05f;
            firework.start(0, 400, 320, 80);
        }

        public override void update(float dt)
        {
            base.update(dt);
            firework.update(dt);
        }
        
        protected override void startProgress()
        {
            progress.start(0, 0);
        }
    }
}
