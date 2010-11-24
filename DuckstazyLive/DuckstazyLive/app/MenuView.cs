using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.core;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Framework.visual;
using DuckstazyLive.game;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

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

        private UiButton focusedButton;
        private UiButton oldFocusedButton;

        private UiButton buttonNewGame;
        private UiButton buttonResumeGame;
        private UiButton buttonAbout;
        private UiButton buttonExit;
        private UiButton buttonCoop;
        private UiButton buttonVersus;        

        public MenuView()
        {
            canvas = new Canvas(Constants.SCREEN_WIDTH, Constants.SCREEN_HEIGHT);
            MAT = new DrawMatrix(true);

            // sky
            geomSkyDay = utils.createGradient(0, 0, Constants.SCREEN_WIDTH, Constants.SCREEN_HEIGHT, utils.makeColor(0x3FB5F2), utils.makeColor(0xDDF2FF), false);

            imgClouds = new int[] { Res.IMG_CLOUD_1, Res.IMG_CLOUD_2, Res.IMG_CLOUD_3 };

            clouds = new EnvCloud[] { new EnvCloud(), new EnvCloud(), new EnvCloud(), new EnvCloud(), new EnvCloud() };
            float cloudX = 0;
            foreach (EnvCloud it in clouds)
            {
                it.init(cloudX);
                cloudX += utils.scale(128.0f + utils.rnd() * 22.0f);
            }

            // ground
            float groundX = 0;
            float groundY = Constants.ENV_HEIGHT;
            float groundWidth = Constants.GROUND_WIDTH;
            float groundHeight = Constants.GROUND_HEIGHT;

            geomGround = utils.createGradient(groundX, groundY, groundWidth, groundHeight, utils.makeColor(0x371d06), utils.makeColor(0x5d310c), false);

            // title
            addTitle();

            // ui
            addUI();
        }

        private void addTitle()
        {
            Image titleBack = new Image(Application.sharedResourceMgr.getTexture(Res.IMG_MENU_TITLE_BACK));
            titleBack.toParentCenter();
            Image title = new Image(Application.sharedResourceMgr.getTexture(Res.IMG_MENU_TITLE));
            title.toParentCenter();

            addChild(titleBack);
            addChild(title);
        }

        private void addUI()
        {
            float buttonX = Constants.TITLE_SAFE_LEFT_X + 0.33f * Constants.TITLE_SAFE_AREA_WIDTH;
            float buttonY = Constants.TITLE_SAFE_TOP_Y;
            // new game
            buttonNewGame = new UiButton(buttonX, buttonY);
            buttonNewGame.setAlign(0.5f, 0.0f);            
            addChild(buttonNewGame);

            // last save
            buttonX = Constants.TITLE_SAFE_LEFT_X + 0.66f * Constants.TITLE_SAFE_AREA_WIDTH;
            buttonY = buttonNewGame.y;
            buttonResumeGame = new UiButton(buttonX, buttonY);
            buttonResumeGame.setAlign(0.5f, 0.0f);
            addChild(buttonResumeGame);

            // about
            buttonX = Constants.TITLE_SAFE_LEFT_X + 50;
            buttonY = Constants.TITLE_SAFE_TOP_Y + 0.5f * Constants.TITLE_SAFE_AREA_HEIGHT;
            buttonAbout = new UiButton(buttonX, buttonY);
            buttonAbout.setAlign(0.0f, 0.5f);
            addChild(buttonAbout);

            // exit
            buttonX = Constants.TITLE_SAFE_RIGHT_X - 50;
            buttonY = buttonAbout.y;
            buttonExit = new UiButton(buttonX, buttonY);
            buttonExit.setAlign(1.0f, 0.5f);
            addChild(buttonExit);

            // coop
            buttonX = buttonNewGame.x;
            buttonY = Constants.TITLE_SAFE_BOTTOM_Y;
            buttonCoop = new UiButton(buttonX, buttonY);
            buttonCoop.setAlign(0.5f, 1.0f);
            addChild(buttonCoop);

            // versus
            buttonX = buttonResumeGame.x;
            buttonY = buttonCoop.y;
            buttonVersus = new UiButton(buttonX, buttonY);
            buttonVersus.setAlign(0.5f, 1.0f);
            addChild(buttonVersus);

            // focus
            focusButton(buttonNewGame);
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

        public override void buttonPressed(ref ButtonEvent e)
        {
            Debug.Assert(focusedButton != null);

            switch (e.button)
            {
                case Buttons.DPadDown:
                case Buttons.LeftThumbstickDown:
                    {
                        focusButtonDown();
                    }
                    break;

                case Buttons.DPadUp:
                case Buttons.LeftThumbstickUp:
                    {
                        focusButtonUp();
                    }
                    break;

                case Buttons.DPadLeft:
                case Buttons.LeftThumbstickLeft:
                    {
                        focusButtonLeft();
                    }
                    break;

                case Buttons.DPadRight:
                case Buttons.LeftThumbstickRight:
                    {
                        focusButtonRight();
                    }
                    break;

                case Buttons.A:
                case Buttons.Start:
                    break;
            }
        }

        private void focusButton(UiButton button)
        {
            if (focusedButton != button)
            {
                oldFocusedButton = focusedButton;
                if (focusedButton != null)
                    focusedButton.setFocused(false);

                focusedButton = button;
                focusedButton.setFocused(true);
            }
        }

        private void focusButtonRight()
        {
            if (focusedButton == buttonResumeGame || focusedButton == buttonVersus)
                focusButton(buttonExit);
            else if (focusedButton == buttonNewGame)
                focusButton(buttonResumeGame);            
            else if (focusedButton == buttonCoop)
                focusButton(buttonVersus);
            else if (focusedButton == buttonAbout)
            {
                if (oldFocusedButton == buttonNewGame)
                    focusButton(buttonCoop);
                else
                    focusButton(buttonNewGame);
            }

        }

        private void focusButtonLeft()
        {
            if (focusedButton == buttonNewGame || focusedButton == buttonCoop)
                focusButton(buttonAbout);
            else if (focusedButton == buttonResumeGame)
                focusButton(buttonNewGame);
            else if (focusedButton == buttonExit)
            {
                if (oldFocusedButton == buttonResumeGame)
                    focusButton(buttonVersus);
                else
                    focusButton(buttonResumeGame);
            }
            else if (focusedButton == buttonVersus)
                focusButton(buttonCoop);
        }

        private void focusButtonUp()
        {
            if (focusedButton == buttonCoop)
                focusButton(buttonNewGame);
            else if (focusedButton == buttonVersus)
                focusButton(buttonResumeGame);
            else if (focusedButton == buttonAbout)
                focusButton(buttonNewGame);
            else if (focusedButton == buttonExit)
                focusButton(buttonResumeGame);
        }

        private void focusButtonDown()
        {
            if (focusedButton == buttonNewGame)
                focusButton(buttonCoop);
            else if (focusedButton == buttonResumeGame)
                focusButton(buttonVersus);
            else if (focusedButton == buttonAbout)
                focusButton(buttonCoop);
            else if (focusedButton == buttonExit)
                focusButton(buttonVersus);
        }

        public override void keyPressed(Keys key)
        {
            switch (key)
            {
                case Keys.Left:
                    focusButtonLeft();
                    break;
                case Keys.Right:
                    focusButtonRight();
                    break;
                case Keys.Up:
                    focusButtonUp();
                    break;
                case Keys.Down:
                    focusButtonDown();
                    break;
            }
        }        
    }
}
