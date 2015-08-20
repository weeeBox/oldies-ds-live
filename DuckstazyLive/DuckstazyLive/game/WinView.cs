using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckstazyLive.app;
using Framework.visual;
using Framework.core;

namespace DuckstazyLive.game
{
    public class WinView : GameView
    {
        private const int CHILD_MESSAGE = 0;

        public WinView(StoryController controller) : base(controller)
        {
            Text text = new Text(Application.sharedResourceMgr.getFont(Res.FNT_BIG));
            text.setString("THE WINER IS YOU");
            text.setParentAlign(ALIGN_CENTER, ALIGN_CENTER);
            text.setAlign(TextAlign.HCENTER | TextAlign.VCENTER);

            addChild(text, CHILD_MESSAGE);
        }       

        public override bool buttonPressed(ref ButtonEvent e)
        {
            if (e.action == ButtonAction.OK || e.action == ButtonAction.Back)
            {
                getController().deactivate();
                return true;
            }

            return false;
        }
    }
}
