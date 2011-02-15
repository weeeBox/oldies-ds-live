using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.core;
using DuckstazyLive.app;
using Framework.visual;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

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

        public virtual void start()
        {

        }

        public override bool buttonPressed(ref ButtonEvent evt)
        {
            return false;
        }

        public override bool buttonReleased(ref ButtonEvent evt)
        {
            return false;
        }

        protected void nextStep()
        {
            stage.nextStep();
        }

        protected void finish()
        {
            stage.finish();
        }
    }

    public class GreetingStep : SingleTutorialStep
    {
        public GreetingStep(SingleTutorialStage stage)
            : base(stage)
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
                nextStep();
                return true;
            }

            if (evt.button == Buttons.B)
            {
                finish();
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

    public class MovementStep : SingleTutorialStep, BaseElement.TimelineDelegate
    {
        private bool leftPressed, rightPressed;        
        private float innactivityCounter;

        private const int CHILD_MESSAGE = 0;
        private const int CHILD_TRY = 1;

        public MovementStep(SingleTutorialStage stage) : base(stage)
        {
            Font font = Application.sharedResourceMgr.getFont(Res.FNT_BIG);
            Text message = new Text(font);
            addChild(message, CHILD_MESSAGE);
            message.setString("I KNOW, YOUR GONNA NOT BELIEVE IT...\n\nBUT LEFT STICK TO MOVE.");
            // message.setAlign(TextAlign.HCENTER | TextAlign.VCENTER);
            attachCenter(message);

            Text innactivityMessage = new Text(font);
            addChild(innactivityMessage, CHILD_TRY);
            innactivityMessage.setString("GIVE IT A TRY!");
            attachHor(innactivityMessage, AttachStyle.CENTER);
            UiLayout.attachVert(innactivityMessage, message, this, AttachStyle.CENTER);
            innactivityMessage.visible = false;
        }

        public override void start()
        {
            innactivityCounter = 2.0f;
        }

        public override void update(float delta)
        {
            base.update(delta);
            if (innactivityCounter > 0)
            {
                innactivityCounter -= delta;
                if (innactivityCounter <= 0)
                {
                    BaseElement e = getChild(CHILD_TRY);
                    e.visible = true;
                    e.turnTimelineSupportWithMaxKeyFrames(2);
                    e.scaleX = e.scaleY = 0.1f;
                    e.addKeyFrame(new KeyFrame(e.x, e.y, Color.White, 1.0f, 1.0f, 0.0f, 0.2f));
                    e.addKeyFrame(new KeyFrame(e.x, e.y, Color.White, 0.8f, 0.8f, 0.0f, 0.2f));                    
                    e.playTimeline();
                }
            }            
        }

        public override bool buttonPressed(ref ButtonEvent evt)
        {
            if (!rightPressed || !leftPressed)
            {
                switch (evt.button)
                {
                    case Buttons.DPadLeft:
                    case Buttons.LeftThumbstickLeft:
                        leftPressed = true;
                        innactivityCounter = 0;
                        break;
                    case Buttons.DPadRight:
                    case Buttons.LeftThumbstickRight:
                        rightPressed = true;
                        innactivityCounter = 0;
                        break;
                }

                if (rightPressed && leftPressed)
                {                    
                    BaseElement e = getChild(CHILD_MESSAGE);
                    e.turnTimelineSupportWithMaxKeyFrames(2);                    
                    e.addKeyFrame(new KeyFrame(e.x, e.y, Color.White, 1.0f, 1.0f, 0.0f, 0.2f));
                    e.addKeyFrame(new KeyFrame(e.x, e.y, Color.White, 0.1f, 0.1f, 0.0f, 0.2f));
                    e.playTimeline();

                    e = getChild(CHILD_TRY);
                    e.turnTimelineSupportWithMaxKeyFrames(2);
                    e.addKeyFrame(new KeyFrame(e.x, e.y, Color.White, 1.0f, 1.0f, 0.0f, 0.3f));
                    e.addKeyFrame(new KeyFrame(e.x, e.y, Color.White, 0.1f, 0.1f, 0.0f, 0.2f));
                    e.playTimeline();

                    e.timelineDelegate = this;
                }
            }

            return base.buttonPressed(ref evt);
        }

        public void elementTimelineFinished(BaseElement e)
        {
            nextStep();
        }
    }

    public class JumpStep : SingleTutorialStep
    {
        public JumpStep(SingleTutorialStage stage) : base(stage)
        {
            Font font = Application.sharedResourceMgr.getFont(Res.FNT_BIG);
            Text message = new Text(font);
            addChild(message);
            message.setString("A TO JUMP");
            // message.setAlign(TextAlign.HCENTER | TextAlign.VCENTER);
            attachCenter(message);
        }

        public override bool buttonPressed(ref ButtonEvent evt)
        {
            if (evt.button == Buttons.A)
            {
                nextStep();
                return true;
            }

            return base.buttonPressed(ref evt);
        }
    }
}
