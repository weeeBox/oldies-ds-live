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
    public class IntroView : View
    {        
        private StartupController controller;     

        private Env env;
        private Canvas canvas;

        private float elapsedTime;

        public IntroView(StartupController controller)
        {
            this.controller = controller;
            env = Env.getIntance();
            canvas = new Canvas(width, height);            
        }

        public override void onShow()
        {
            env.blanc = 1.0f;
            elapsedTime = 0;
            env.day = true;

            Font font = Application.sharedResourceMgr.getFont(Res.FNT_BIG);
            Text text = new Text(font);
            text.setString("SUPER AWESOME\nAND COOL\nINTRO GOES HERE");
            text.toParentCenter();
            text.setAlign(TextAlign.CENTER);
            addChild(text);
        }        

        public override void update(float delta)
        {
            base.update(delta);
            env.update(delta, 0.0f);

            elapsedTime += delta;

            if (elapsedTime > 3.0f)
                hide();
        }
                
        public override void draw()
        {
            base.preDraw();

            env.draw1(canvas);
            env.draw2(canvas);

            base.postDraw();
        }

        public override bool buttonPressed(ref ButtonEvent evt)
        {
            switch (evt.button)
            {
                case Buttons.A:
                case Buttons.Start:
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
