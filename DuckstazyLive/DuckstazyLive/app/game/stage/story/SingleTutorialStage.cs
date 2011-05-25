using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.visual;
using Framework.core;
using DuckstazyLive.app;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using asap.graphics;
using DuckstazyLive.app.game.stage;
using asap.core;
using DuckstazyLive.app.game.level;

namespace DuckstazyLive.game.stages.story
{
    public class SingleTutorialStage : StoryLevelStage
    {
        private SingleTutorialStep[] steps;
        private int stepIndex;

        public SingleTutorialStage()
        {
            steps = new SingleTutorialStep[] 
            {
                new GreetingStep(this),
                new MovementStep(this),
                new JumpStep(this),
                new DropStep(this)
            };
        }
        
        protected override void startProgress()
        {
            progress.start(0, 0);
        }

        public override void onStart()
        {
            base.onStart();
            stepIndex = -1;
            nextStep();
        }

        public override void Update(float dt)
        {
            base.Update(dt);
            getCurrentStep().Update(dt);
        }

        public override void draw2(Graphics g)
        {
            getCurrentStep().Draw(g);
        }

        public override bool KeyPressed(KeyEvent e)
        {
            //if (getCurrentStep().KeyPressed(e))
            //    return true;            

            //return base.KeyPressed(e);
            throw new NotImplementedException();
        }

        public override bool KeyReleased(KeyEvent e)
        {
            //if (getCurrentStep().KeyReleased(e))
            //    return true;

            //return base.KeyReleased(e);
            throw new NotImplementedException();
        }

        public void nextStep()
        {
            stepIndex++;
            if (stepIndex < steps.Length)
            {
                getCurrentStep().start();
            }
            else
            {
                finish();
            }
        }

        public void finish()
        {
            win();

            StoryLevel level = (StoryLevel)Level.instance;
            level.nextLevel();
        }

        private SingleTutorialStep getCurrentStep()
        {
            Debug.Assert(stepIndex >= 0 && stepIndex < steps.Length);
            return steps[stepIndex];
        }
    }
}
