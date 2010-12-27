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
        private const int SCULLS_COUNT = 30;

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

        public DeathView(GameController controller) : base(controller)
        {
            backgroud = utils.createSolidRect(0, 0, width, height, Color.White);
            sculls = new Scull[SCULLS_COUNT];            
            for(int i = 0; i < SCULLS_COUNT; ++i)
            {
                initScull(ref sculls[i], true);
            }

            Text missText = new Text(Application.sharedResourceMgr.getFont(Res.FNT_BIG));
            missText.setString("THAT IS HOW IT HAPPENS.\n\nBEWARE...");
            missText.setParentAlign(ALIGN_CENTER, ALIGN_CENTER);
            missText.setAlign(TextAlign.HCENTER | TextAlign.VCENTER);
            addChild(missText);

            // add ui
            Image buttonImage = new Image(Application.sharedResourceMgr.getTexture(Res.IMG_UI_BUTTON_A));
            buttonImage.x = 0.5f * (Constants.TITLE_SAFE_LEFT_X + Constants.TITLE_SAFE_RIGHT_X);
            buttonImage.y = Constants.TITLE_SAFE_BOTTOM_Y;
            buttonImage.setAlign(ALIGN_CENTER, ALIGN_MAX);
            addChild(buttonImage);
        }        

        private void initScull(ref Scull scull, bool firstInit)
        {
            scull.x = utils.rnd_int(0, width);
            if (firstInit)            
                scull.y = utils.rnd_int(0, height);
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
                initScull(ref scull, false);
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
