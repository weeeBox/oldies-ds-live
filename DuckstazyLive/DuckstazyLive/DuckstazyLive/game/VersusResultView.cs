using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckstazyLive.app;
using Framework.visual;
using Framework.core;
using Microsoft.Xna.Framework;

namespace DuckstazyLive.game
{
    public class VersusResultView : VersusView
    {
        private int CHILD_MESSAGE = 0;

        public VersusResultView(VersusController controller) : base(controller)
        {
            Font font = Application.sharedResourceMgr.getFont(Res.FNT_INFO);
            Text text = new Text(font);
            text.setParentAlign(ALIGN_CENTER, ALIGN_CENTER);
            text.setAlign(TextAlign.HCENTER | TextAlign.VCENTER);

            addChild(text, CHILD_MESSAGE);
        }

        public void setDraw()
        {
            setMessage("DEAD HEAT");
        }

        public void setWinner(int playerIndex)
        {
            setMessage("Player " + playerIndex + " wins");
        }

        private void setMessage(String text)
        {
            Text message = (Text)getChild(CHILD_MESSAGE);
            message.setString(text);
            message.scaleX = message.scaleY = 0.2f;
            message.turnTimelineSupportWithMaxKeyFrames(2);
            message.addKeyFrame(new KeyFrame(message.x, message.y, Color.White, 1.5f, 1.5f, 0.0f, 0.5f));
            message.addKeyFrame(new KeyFrame(message.x, message.y, Color.White, 1.0f, 1.0f, 0.0f, 0.5f));
            message.playTimeline();
        }

        public override bool buttonPressed(ref ButtonEvent e)
        {
            if (e.action == ButtonAction.OK)
            {
                getController().selectStage();
            }
            else if (e.action == ButtonAction.Back)
            {
                getController().deactivate();
            }

            return false;
        }
    }
}
