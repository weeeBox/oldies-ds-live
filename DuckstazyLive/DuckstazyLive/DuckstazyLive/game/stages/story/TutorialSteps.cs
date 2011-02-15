using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.core;
using DuckstazyLive.app;
using Framework.visual;
using Microsoft.Xna.Framework.Input;

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
}
