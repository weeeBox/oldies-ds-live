using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.visual;
using Framework.core;
using DuckstazyLive.app;

namespace DuckstazyLive.game.stages.story
{
    public abstract class SingleTutorialStep : BaseElementContainer
    {
        public SingleTutorialStep()
        {
            this.width = Constants.SCREEN_WIDTH;
            this.height = (int)Constants.ENV_HEIGHT;
        }
    }

    public class GreetingStep : SingleTutorialStep
    {
        public GreetingStep()
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
    }

    public class SingleTutorialStage : StoryLevelStage
    {
        private int TUTORIAL_STEP_GREETINGS = 0;
        private int TUTORIAL_STEP_MOVE = 1;

        SingleTutorialStep step;

        public SingleTutorialStage()
        {
            step = new GreetingStep();            
        }

        protected override void startProgress()
        {
            progress.start(0, 0);
        }

        public override void draw2(Canvas canvas)
        {
            step.draw();
        }
    }
}
