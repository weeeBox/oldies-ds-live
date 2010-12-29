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
        protected Env env;
        protected Canvas canvas;

        public EnvView()
        {
            env = Env.getIntance();
            canvas = new Canvas(width, height);
        }

        public override void onShow()
        {            
            env.day = true;
        }

        public override void update(float delta)
        {
            base.update(delta);
            env.update(delta, 0.0f);
            env.updateBlanc(delta);
        }

        public override void draw()
        {
            base.preDraw();

            env.draw1(canvas);
            env.draw2(canvas);

            base.postDraw();
            
            if (env.blanc > 0.0f)
                env.drawBlanc(canvas);
        }
    }
}
