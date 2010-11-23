using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.core;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Framework.visual;
using DuckstazyLive.game;

namespace DuckstazyLive.app
{
    public class MenuView : View
    {
        // дублеж, пиздешь и провокация

        private CustomGeomerty geomSkyDay;        
        private CustomGeomerty geomGround;

        private EnvCloud[] clouds;
        private int[] imgClouds;

        private Canvas canvas;
        private DrawMatrix MAT;

        public MenuView()
        {
            canvas = new Canvas(Constants.SCREEN_WIDTH, Constants.SCREEN_HEIGHT);
            MAT = new DrawMatrix();

            // sky
            geomSkyDay = utils.createGradient(0, 0, Constants.SCREEN_WIDTH, Constants.SCREEN_HEIGHT, utils.makeColor(0x3FB5F2), utils.makeColor(0xDDF2FF), false);

            imgClouds = new int[] { Res.IMG_CLOUD_1, Res.IMG_CLOUD_2, Res.IMG_CLOUD_3 };

            clouds = new EnvCloud[] { new EnvCloud(), new EnvCloud(), new EnvCloud(), new EnvCloud(), new EnvCloud() };
            foreach (EnvCloud it in clouds)
            {
                it.init(x);
                x += utils.scale(128.0f + utils.rnd() * 22.0f);
            }

            // ground
            float groundX = 0;
            float groundY = Constants.ENV_HEIGHT;
            float groundWidth = Constants.GROUND_WIDTH;
            float groundHeight = Constants.GROUND_HEIGHT;

            geomGround = utils.createGradient(groundX, groundY, groundWidth, groundHeight, utils.makeColor(0x371d06), utils.makeColor(0x5d310c), false);
        }

        public override void update(float delta)
        {
            base.update(delta);

            foreach (EnvCloud c in clouds)
                c.update(delta, 0.0f);
        }

        public override void draw()
        {
            preDraw();

            drawEnv();
            drawTitle();
            drawUI();

            postDraw();
        }

        private void drawEnv()
        {
            // the sky is high
            AppGraphics.DrawGeomerty(geomSkyDay);

            // clouds           
            foreach (EnvCloud c in clouds)
            {
                float x = c.counter;
                int imageId = imgClouds[c.id];
                Texture2D img = Application.sharedResourceMgr.getTexture(imageId);

                MAT.identity();
                MAT.useScale = true;
                MAT.tx = utils.unscale(-img.Width * 0.5f);
                MAT.ty = utils.unscale(-img.Height * 0.5f);
                MAT.scale(0.9f + 0.1f * (float)Math.Sin(x * 6.28), 0.95f + 0.05f * (float)Math.Sin(x * 6.28 + 3.14));
                MAT.translate(c.x, c.y);

                canvas.draw(imageId, MAT);
            }

            // ground
            AppGraphics.DrawGeomerty(geomGround);

            // grass
            Texture2D tex = Application.sharedResourceMgr.getTexture(Res.IMG_GRASS1);
            Rectangle src = new Rectangle(0, 0, tex.Width, tex.Height);
            Rectangle dst = new Rectangle(0, (int)(geomGround.y - tex.Height), Constants.SCREEN_WIDTH, tex.Height);

            AppGraphics.SetColor(utils.makeColor(0xff00ff00));
            AppGraphics.DrawImageTiled(tex, ref src, ref dst);
            AppGraphics.SetColor(Color.White);
        }

        private void drawTitle()
        {
            Texture2D title = Application.sharedResourceMgr.getTexture(Res.IMG_MENU_TITLE);
            Texture2D titleBack = Application.sharedResourceMgr.getTexture(Res.IMG_MENU_TITLE_BACK);

            MAT.identity();
            MAT.useScale = false;
            MAT.translate(0.5f * width, 0.5f * height);
            MAT.tx = -0.5f * titleBack.Width;
            MAT.ty = -0.5f * titleBack.Height;            
            canvas.draw(titleBack, MAT);
                        
            MAT.tx = -0.5f * title.Width;
            MAT.ty = -0.5f * title.Height;            
            canvas.draw(title, MAT);
        }

        private void drawUI()
        {

        }
    }
}
