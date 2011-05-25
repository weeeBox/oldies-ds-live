using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.core;
using DuckstazyLive.app;
using Framework.visual;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using asap.visual;
using app;
using asap.core;

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
            //BaseFont font = Application.sharedResourceMgr.GetFont(Res.FNT_BIG);
            //Text message = new Text(font);
            //addChild(message);

            //message.setString("HI! WANT SOME INSTUCTIONS?");
            //// message.setAlign(TextAlign.HCENTER | TextAlign.VCENTER);
            //attachCenter(message);

            //UiControllerButtons buttons = new UiControllerButtons("YEP", "TUTORIALS FOR SUCKERS");
            //addChild(buttons);
            //attachHor(buttons, AttachStyle.CENTER);
            //UiLayout.attachVert(buttons, message, this, AttachStyle.CENTER);
            throw new NotImplementedException();
        }

        //public override bool KeyPressed(ref ButtonEvent evt)
        //{
        //    if (evt.button == Buttons.A)
        //    {
        //        nextStep();
        //        return true;
        //    }

        //    if (evt.button == Buttons.B)
        //    {
        //        finish();
        //        return true;
        //    }

        //    return base.KeyPressed(ref evt);
        //}

        //public override bool KeyReleased(ref ButtonEvent evt)
        //{
        //    if (evt.button == Buttons.A || evt.button == Buttons.B)
        //        return true;

        //    return base.KeyReleased(ref evt);
        //}
    }

    public class MovementStep : SingleTutorialStep
    {
        private bool leftPressed, rightPressed;        
        private float innactivityCounter;

        private const int CHILD_MESSAGE = 0;
        private const int CHILD_TRY = 1;

        public MovementStep(SingleTutorialStage stage) : base(stage)
        {
            //BaseFont font = Application.sharedResourceMgr.getFont(Res.FNT_BIG);
            //Text message = new Text(font);
            //addChild(message, CHILD_MESSAGE);
            //message.setString("I KNOW, YOUR GONNA NOT BELIEVE IT...\n\nBUT LEFT STICK TO MOVE.");
            //// message.setAlign(TextAlign.HCENTER | TextAlign.VCENTER);
            //attachCenter(message);

            //Text innactivityMessage = new Text(font);
            //addChild(innactivityMessage, CHILD_TRY);
            //innactivityMessage.setString("GIVE IT A TRY!");
            //attachHor(innactivityMessage, AttachStyle.CENTER);
            //UiLayout.attachVert(innactivityMessage, message, this, AttachStyle.CENTER);
            //innactivityMessage.visible = false;
            throw new NotImplementedException();
        }

        public override void start()
        {
            innactivityCounter = 2.0f;
        }

        public override void Update(float delta)
        {
            //base.Update(delta);
            //if (innactivityCounter > 0)
            //{
            //    innactivityCounter -= delta;
            //    if (innactivityCounter <= 0)
            //    {
            //        BaseElement e = getChild(CHILD_TRY);
            //        e.visible = true;
            //        e.turnTimelineSupportWithMaxKeyFrames(2);
            //        e.scaleX = e.scaleY = 0.1f;
            //        e.addKeyFrame(new KeyFrame(e.x, e.y, Color.White, 1.0f, 1.0f, 0.0f, 0.2f));
            //        e.addKeyFrame(new KeyFrame(e.x, e.y, Color.White, 0.8f, 0.8f, 0.0f, 0.2f));                    
            //        e.playTimeline();
            //    }
            //}            
            throw new NotImplementedException();
        }

        //public override bool KeyPressed(KeyEvent evt)
        //{
        //    if (!rightPressed || !leftPressed)
        //    {
        //        switch (evt.code)
        //        {
        //            case KeyCode.DPadLeft:
        //            case KeyCode.LeftThumbstickLeft:
        //                leftPressed = true;
        //                innactivityCounter = 0;
        //                break;
        //            case KeyCode.DPadRight:
        //            case KeyCode.LeftThumbstickRight:
        //                rightPressed = true;
        //                innactivityCounter = 0;
        //                break;
        //        }

        //        if (rightPressed && leftPressed)
        //        {                    
        //            //BaseElement e = getChild(CHILD_MESSAGE);
        //            //e.turnTimelineSupportWithMaxKeyFrames(2);                    
        //            //e.addKeyFrame(new KeyFrame(e.x, e.y, Color.White, 1.0f, 1.0f, 0.0f, 0.2f));
        //            //e.addKeyFrame(new KeyFrame(e.x, e.y, Color.White, 0.1f, 0.1f, 0.0f, 0.2f));
        //            //e.playTimeline();

        //            //e = getChild(CHILD_TRY);
        //            //e.turnTimelineSupportWithMaxKeyFrames(2);
        //            //e.addKeyFrame(new KeyFrame(e.x, e.y, Color.White, 1.0f, 1.0f, 0.0f, 0.3f));
        //            //e.addKeyFrame(new KeyFrame(e.x, e.y, Color.White, 0.1f, 0.1f, 0.0f, 0.2f));
        //            //e.playTimeline();

        //            //e.timelineDelegate = this;
        //            throw new NotImplementedException();
        //        }
        //    }

        //    return base.KeyPressed(evt);
        //}

        public void elementTimelineFinished(BaseElement e)
        {
            nextStep();
        }
    }

    public class JumpStep : SingleTutorialStep
    {
        private const int CHILD_MESSAGE = 0;

        public JumpStep(SingleTutorialStage stage) : base(stage)
        {
            //BaseFont font = Application.sharedResourceMgr.getFont(Res.FNT_BIG);
            //Text message = new Text(font);
            //addChild(message, CHILD_MESSAGE);
            //message.setString("A TO JUMP");
            //// message.setAlign(TextAlign.HCENTER | TextAlign.VCENTER);
            //attachCenter(message);
            throw new NotImplementedException();
        }        

        //public override bool KeyPressed(KeyEvent evt)
        //{
        //    if (evt.code == KeyCode.A)
        //    {
        //        //BaseElement e = getChild(CHILD_MESSAGE);
        //        //e.turnTimelineSupportWithMaxKeyFrames(2);
        //        //e.scaleX = e.scaleY = 1.0f;
        //        //e.addKeyFrame(new KeyFrame(e.x, e.y, Color.White, 0.1f, 0.1f, 0.0f, 0.2f));
        //        //e.playTimeline();

        //        //e.timelineDelegate = this;
        //        //return false; // allow player to jump

        //        throw new NotImplementedException();
        //    }

        //    return base.KeyPressed(evt);
        //}

        public void elementTimelineFinished(BaseElement e)
        {
            nextStep();
        }
    }

    public class DropStep : SingleTutorialStep
    {
        private const int CHILD_MESSAGE = 0;

        public DropStep(SingleTutorialStage stage) : base(stage)
        {
            //BaseFont font = Application.sharedResourceMgr.getFont(Res.FNT_BIG);
            //Text message = new Text(font);
            //addChild(message, CHILD_MESSAGE);
            //message.setString("B TO DROP WHILE FLYING");
            //// message.setAlign(TextAlign.HCENTER | TextAlign.VCENTER);
            //attachCenter(message);
            throw new NotImplementedException();
        }

        //public override bool KeyPressed(ref ButtonEvent evt)
        //{
        //    if (evt.button == Buttons.B)
        //    {
        //        //Heroes heroes = GameElements.Heroes;
        //        //Hero hero1 = heroes[0];

        //        //if (hero1.isFlying())
        //        //{
        //        //    BaseElement e = getChild(CHILD_MESSAGE);
        //        //    e.turnTimelineSupportWithMaxKeyFrames(2);
        //        //    e.scaleX = e.scaleY = 1.0f;
        //        //    e.addKeyFrame(new KeyFrame(e.x, e.y, Color.White, 0.1f, 0.1f, 0.0f, 0.2f));
        //        //    e.playTimeline();

        //        //    e.timelineDelegate = this;
        //        //}
        //        //return false; // allow player to jump
        //        throw new NotImplementedException();
        //    }

        //    return base.KeyPressed(ref evt);
        //}

        //public void elementTimelineFinished(BaseElement e)
        //{
        //    nextStep();
        //}
    }
}
