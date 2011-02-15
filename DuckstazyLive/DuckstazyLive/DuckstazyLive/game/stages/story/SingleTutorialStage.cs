using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.visual;
using Framework.core;
using DuckstazyLive.app;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

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
                new JumpStep(this)
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

        public override void update(float dt)
        {
            base.update(dt);
            getCurrentStep().update(dt);
        }

        public override void draw2(Canvas canvas)
        {
            getCurrentStep().draw();
        }

        public override bool buttonPressed(ref ButtonEvent e)
        {
            if (getCurrentStep().buttonPressed(ref e))
                return true;

            return base.buttonPressed(ref e);
        }

        public override bool buttonReleased(ref ButtonEvent e)
        {
            if (getCurrentStep().buttonReleased(ref e))
                return true;

            return base.buttonReleased(ref e);
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
