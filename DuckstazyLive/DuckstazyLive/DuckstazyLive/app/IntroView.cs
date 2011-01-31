using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.core;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input;
using DuckstazyLive.game;
using Microsoft.Xna.Framework;
using Framework.visual;

namespace DuckstazyLive.app
{
    public class IntroView : EnvView
    {        
        private StartupController controller;        

        private float elapsedTime;

        public IntroView(StartupController controller)
        {
            this.controller = controller;          
        }

        public override void onShow()
        {
            base.onShow();

            elapsedTime = 0;

            Font font = Application.sharedResourceMgr.getFont(Res.FNT_INFO);
            Text text = new Text(font);
            text.setString("SUPER AWESOME AND COOL INTRO GOES HERE");
            text.setParentAlign(ALIGN_CENTER, ALIGN_CENTER);
            text.setAlign(TextAlign.HCENTER | TextAlign.VCENTER);
            addChild(text);

            UiControllerButtons buttons = new UiControllerButtons("SKIP", null);
            addChild(buttons);
            attachHor(buttons, new AttachStyle(AttachStyle.MAX, 0, Constants.TITLE_SAFE_LEFT_X));
            attachVert(buttons, new AttachStyle(AttachStyle.MAX, 0, Constants.TITLE_SAFE_TOP_Y));

            Application.sharedSoundMgr.playSound(Res.SONG_ENV_MENU, true, SoundTransform.NONE);
        }        

        public override void update(float delta)
        {
            base.update(delta);        

            elapsedTime += delta;

            if (elapsedTime > 3.0f)
                hide();
        }        

        public override bool buttonPressed(ref ButtonEvent evt)
        {
            if (evt.action == ButtonAction.OK)
            {                
                hide();
                return true;
            }

            return false;
        }

        private void hide()
        {
            controller.deactivate();            
        }
    }
}
