using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using app.menu;
using app;
using asap.graphics;
using asap.visual;
using DuckstazyLive.app.menu;

namespace DuckstazyLive.app
{
    public class IntroScreen : EnvScreen
    {
        private StartupController controller;

        private float elapsedTime;

        public IntroScreen(StartupController controller) : base(ScreenId.INTRO)
        {
            this.controller = controller;
            BaseFont font = Application.sharedResourceMgr.GetFont(Res.FNT_BIG);
            Text text = new Text(font, "SUPER AWESOME AND COOL INTRO GOES HERE");
            AddChild(text);
            AttachCenter(text);

            UiGamePadButtons buttons = new UiGamePadButtons(null, "SKIP");
            AddChild(buttons);
            AttachRight(buttons, Constants.TITLE_SAFE_LEFT_X);
            AttachBottom(buttons, Constants.TITLE_SAFE_TOP_Y);            

            //Application.sharedSoundMgr.playSound(Res.SONG_ENV_MENU, true, SoundTransform.NONE);
        }        

        public override void Update(float delta)
        {
            base.Update(delta);

            elapsedTime += delta;

            if (elapsedTime > 0.0f)
                hide();
        }        

        private void hide()
        {
            controller.Stop(0);
        }
    }
}
