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
            
            firework.start(0, 400, 320, 80);
            firework.flyTime = 2.5f;
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
