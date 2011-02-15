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
    public abstract class SingleTutorialStep : BaseElementContainer
    {
        protected SingleTutorialStage stage;

        public SingleTutorialStep(SingleTutorialStage stage)
        {
            this.stage = stage;
            this.width = Constants.SCREEN_WIDTH;
            this.height = (int)Constants.ENV_HEIGHT;
        }

        public override bool buttonPressed(ref ButtonEvent evt)
        {
            return false;
        }

        public override bool buttonReleased(ref ButtonEvent evt)
        {
            return false;
        }
    }

    public class GreetingStep : SingleTutorialStep
    {
        public GreetingStep(SingleTutorialStage stage) : base(stage)
        {
            Font font = Application.sharedResourceMgr.getFont(Res.FNT_BIG);
            Text message = new Text(font);
            addChild(message);

            message.setString("HI! WANT SOME INSTUCTIONS?");
            // message.setAlign(TextAlign.HCENTER | TextAlign.VCENTER);
            attachCenter(message);            

            UiControllerButtons buttons = new UiControllerButtons("YEP", "TUTORIALS FOR SUCKERS");
            addChild(buttons);
            attachHor(buttons, AttachStyle.CENTER);
            UiLayout.attachVert(buttons, message, this, AttachStyle.CENTER);
        }

        public override bool buttonPressed(ref ButtonEvent evt)
        {
            if (evt.button == Buttons.A)
            {
                stage.nextStep();
                return true;
            }

            if (evt.button == Buttons.B)
            {
                stage.finish();
                return true;
            }

            return base.buttonPressed(ref evt);
        }

        public override bool buttonReleased(ref ButtonEvent evt)
        {
            if (evt.button == Buttons.A || evt.button == Buttons.B)
                return true;

            return base.buttonReleased(ref evt);
        }
    }

    public class SingleTutorialStage : StoryLevelStage
    {
        private SingleTutorialStep[] steps;
        private int stepIndex;

        public SingleTutorialStage()
        {
            steps = new SingleTutorialStep[] 
            {
                new GreetingStep(this)
            };
        }
        
        protected override void startProgress()
        {
            progress.start(0, 0);
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
            if (stepIndex == steps.Length)
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
