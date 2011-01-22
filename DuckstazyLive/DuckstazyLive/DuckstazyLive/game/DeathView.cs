using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.core;
using Framework.visual;
using Microsoft.Xna.Framework;
using DuckstazyLive.app;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DuckstazyLive.game
{
    public class DeathView : GameView
    {
        private const int SCULLS_COUNT = 15;

        public struct Scull
        {
            public float x, y;
            public float vx, vy;
            public float lifeTime;
            public float deathTime;
            public float amplitude;
            public double omega;
        }        

        private CustomGeomerty backgroud;
        private Scull[] sculls;

        private const int CHILD_TEXT = 0;
        private const int CHILD_BUTTONS = 1;

        public DeathView(StoryController controller) : base(controller)
        {
            this.height = (int)Constants.ENV_HEIGHT;

            backgroud = utils.createSolidRect(0, 0, width, height, Color.White);
            sculls = new Scull[SCULLS_COUNT];            
            for(int i = 0; i < SCULLS_COUNT; ++i)
            {
                initScull(ref sculls[i], i);
            }

            Text missText = new Text(Application.sharedResourceMgr.getFont(Res.FNT_BIG));
            addChild(missText, CHILD_TEXT);

            attachCenter(missText);
            missText.setString("THAT IS HOW IT HAPPENS.\n\nBEWARE...");            
            missText.setAlign(TextAlign.HCENTER | TextAlign.VCENTER);            

            UiControllerButtons buttons = new UiControllerButtons("NEW TRIP", "GO HOME");
            addChild(buttons, CHILD_BUTTONS);
            attachHor(buttons, AttachStyle.CENTER);
            UiLayout.attachVert(buttons, missText, this, AttachStyle.CENTER);
        }

        public override void onShow()
        {
            base.onShow();

            GameElements.Env.blanc = 1.0f;
        }

        private void initScull(ref Scull scull, int scullIndex)
        {            
            if (scullIndex != Constants.UNDEFINED)
            {
                float dx = width / (sculls.Length - 1);
                scull.x = dx * (scullIndex + utils.rnd_float(-0.25f, 0.25f));
                scull.y = utils.rnd_int(0, height);
            }
            else
                scull.y -= scull.y + getScullTex().Height;
            
            scull.vy = 100.0f + 50.0f * utils.rnd_float(-1.0f, 1.0f);

            scull.lifeTime = 0;
            scull.deathTime = (height - scull.y) / scull.vy * (0.8f + 0.2f * utils.rnd());
            scull.amplitude = 30.0f * utils.rnd_float(-1.0f, 1.0f);
            scull.omega = Math.PI + Math.PI * utils.rnd_float(-1.0f, 1.0f);
        }

        public override void update(float delta)
        {
            base.update(delta);

            for (int i = 0; i < SCULLS_COUNT; ++i)
            {
                updateScull(ref sculls[i], delta);
            }
        }

        private void updateScull(ref Scull scull, float dt)
        {
            scull.lifeTime += dt;            

            scull.vx = (float) (scull.amplitude * Math.Sin(scull.omega * scull.lifeTime));

            scull.y += scull.vy * dt;
            scull.x += scull.vx * dt;            

            if (scull.y > height + 0.5f * getScullTex().Height || scull.deathTime - scull.lifeTime < 0)
            {
                initScull(ref scull, Constants.UNDEFINED);
            }            
        }

        public override void draw()
        {
            preDraw();

            Canvas canvas = getCanvas();
            canvas.drawGeometry(backgroud);

            Texture2D tex = getScullTex();
            for (int i = 0; i < SCULLS_COUNT; ++i)
            {
                float opacity = 1.0f -sculls[i].lifeTime / sculls[i].deathTime;
                AppGraphics.DrawImage(tex, sculls[i].x - 0.5f * tex.Width, sculls[i].y - 0.5f * tex.Height, opacity);
            }

            postDraw();

            Env env = GameElements.Env;
            if (env.blanc > 0.0f)
                env.drawBlanc(canvas);
        }

        private Texture2D getScullTex()
        {
            return Application.sharedResourceMgr.getTexture(Res.IMG_PILL_TOXIC_1);
        }        

        public override bool buttonPressed(ref ButtonEvent evt)
        {
            switch (evt.action)
            {
                case ButtonAction.OK:
                    getController().newGame();
                    return true;
                case ButtonAction.Back:
                    getController().deactivate();
                    return true;
            }          

            return false;
        }
    }
}
