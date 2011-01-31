using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.core;
using Framework.visual;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DuckstazyLive.app
{
    public class ControllerWarning : BaseElementContainer
    {
        private const int CHILD_BACKGROUND = 0;
        private const int CHILD_MESSAGE = 1;
        private const int CHILD_BUTTON = 2;

        public ControllerWarning() : base(Constants.SCREEN_WIDTH, Constants.SCREEN_HEIGHT)
        {
            Color backColor = Color.Multiply(Color.Black, 0.85f);
            CustomGeomerty background = GeometryFactory.createSolidRect(0, 0, width, height, backColor);
            addChild(background, CHILD_BACKGROUND);            

            Font font = Application.sharedResourceMgr.getFont(Res.FNT_INFO);
            Text message = new Text(font);
            message.setString("RECONNECT CONTROLLER");            
            addChild(message, CHILD_MESSAGE);
            attachCenter(message);

            UiControllerButtons buttons = new UiControllerButtons("CONTINUE", null);
            addChild(buttons, CHILD_BUTTON);
            attachHor(buttons, AttachStyle.CENTER);

            UiLayout.attachVert(buttons, message, this, AttachStyle.CENTER);

            stop();
        }     

        public void start()
        {
            setEnabled(true);
            BaseElement button = getChild(CHILD_BUTTON);
            button.setEnabled(false);
        }
        
        public void controllerConnected()
        {
            BaseElement button = getChild(CHILD_BUTTON);
            button.setEnabled(true);
        }

        public override bool buttonPressed(ref ButtonEvent e)
        {
            if (e.button == Buttons.A)
                stop();

            return true;
        }

        public void stop()
        {
            setEnabled(false);
        }

        public bool isShowed()
        {
            return isEnabled();
        }
    }
}
