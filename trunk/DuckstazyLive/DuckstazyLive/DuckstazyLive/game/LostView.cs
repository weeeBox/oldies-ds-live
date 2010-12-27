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

        public LostView(GameController controller) : base(controller)
        {
            Text text = new Text(Application.sharedResourceMgr.getFont(Res.FNT_BIG));
            text.setParentAlign(ALIGN_CENTER, ALIGN_CENTER);
            text.setAlign(TextAlign.HCENTER | TextAlign.VCENTER);

            addChildWithId(text, CHILD_MESSAGE);
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
