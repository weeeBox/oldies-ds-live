using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.core;
using DuckstazyLive.game;

namespace DuckstazyLive.app
{
    public class EnvView : View
    {        
        protected Canvas canvas;

        public EnvView()
        {            
            canvas = new Canvas(width, height);
        }

        public override void onShow()
        {
            base.onShow();
            GameElements.Env.day = true;
        }

        public override void update(float delta)
        {
            Env env = GameElements.Env;

            base.update(delta);
            env.update(delta, 0.0f);
            env.updateBlanc(delta);
        }

        public override void draw()
        {
            base.preDraw();

            Env env = GameElements.Env;
            env.draw1(canvas);
            env.draw2(canvas);           

            base.postDraw();

            env.drawBlanc(canvas);
        }
    }
}
