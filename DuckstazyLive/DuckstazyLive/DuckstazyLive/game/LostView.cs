using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckstazyLive.app;
using Framework.core;
using Framework.visual;

namespace DuckstazyLive.game
{
    public class LostView : GameView
    {
        private const int CHILD_MESSAGE = 0;

        public LostView(StoryController controller) : base(controller)
        {
            this.height = (int)Constants.ENV_HEIGHT;

            Text text = new Text(Application.sharedResourceMgr.getFont(Res.FNT_BIG));
            addChild(text, CHILD_MESSAGE);
            text.setAlign(TextAlign.HCENTER | TextAlign.VCENTER);
            attachCenter(text);

            UiControllerButtons buttons = new UiControllerButtons("RETRY", "GIVEUP");
            addChild(buttons);
            attachHor(buttons, AttachStyle.CENTER);
            UiLayout.attachVert(buttons, text, this, AttachStyle.CENTER);
        }

        public void setMessage(String message)
        {
            Text text = (Text) getChild(CHILD_MESSAGE);
            text.setString(message);
        }

        public override bool buttonPressed(ref ButtonEvent e)
        {
            if (e.action == ButtonAction.OK)
            {
                getController().restartLevel();
            }
            else if (e.action == ButtonAction.Back)
            {
                getController().nextLevel();
            }

            return false;
        }
    }
}
