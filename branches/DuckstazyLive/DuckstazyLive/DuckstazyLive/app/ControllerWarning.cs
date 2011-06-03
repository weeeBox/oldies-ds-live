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
    //public class ControllerWarning : DisplayObjectContainer
    //{
    //    private const int CHILD_BACKGROUND = 0;
    //    private const int CHILD_MESSAGE = 1;
    //    private const int CHILD_BUTTON = 2;

    //    public ControllerWarning() : base(Constants.SCREEN_WIDTH, Constants.SCREEN_HEIGHT)
    //    {
    //        Color backColor = Color.Multiply(Color.Black, 0.85f);
    //        CustomGeomerty background = GeometryFactory.createSolidRect(0, 0, width, height, backColor);
    //        AddChild(background, CHILD_BACKGROUND);            

    //        BaseFont font = Application.sharedResourceMgr.GetFont(Res.FNT_BIG);
    //        Text message = new Text(font);
    //        message.setString("RECONNECT CONTROLLER");            
    //        AddChild(message, CHILD_MESSAGE);
    //        attachCenter(message);

    //        UiControllerButtons buttons = new UiControllerButtons("CONTINUE", null);
    //        AddChild(buttons, CHILD_BUTTON);
    //        attachHor(buttons, AttachStyle.CENTER);

    //        UiLayout.attachVert(buttons, message, this, AttachStyle.CENTER);

    //        stop();
    //    }     

    //    public void start()
    //    {
    //        setEnabled(true);
    //        DisplayObject button = getChild(CHILD_BUTTON);
    //        button.setEnabled(false);
    //    }
        
    //    public void controllerConnected()
    //    {
    //        DisplayObject button = getChild(CHILD_BUTTON);
    //        button.setEnabled(true);
    //    }

    //    public override bool KeyPressed(ref ButtonEvent e)
    //    {
    //        if (e.button == Buttons.A)
    //            stop();

    //        return true;
    //    }

    //    public void stop()
    //    {
    //        setEnabled(false);
    //    }

    //    public bool isShowed()
    //    {
    //        return isEnabled();
    //    }
    //}
}
