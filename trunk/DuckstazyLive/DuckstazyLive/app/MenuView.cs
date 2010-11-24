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
    public class UiButton : BaseElement
    {
        public static UiButton focusedButton;

        private const int CHILD_STROKE = 0;
        private const int CHILD_ROTATION = 1;

        public UiButton buttonLeft;
        public UiButton buttonRight;
        public UiButton buttonUp;
        public UiButton buttonDown;

        private Color targetColor;
        private Vector2 targetScale;
        private float omega;

        public UiButton(float x, float y)
        {
            this.x = x;
            this.y = y;

            width = utils.imageWidth(Res.IMG_BUTTON_STROKE_DEFAULT);
            height = utils.imageHeight(Res.IMG_BUTTON_STROKE_DEFAULT);

            // button stroke part            
            Image strokeImage = new Image(Application.sharedResourceMgr.getTexture(Res.IMG_BUTTON_STROKE_FOCUSED));
            strokeImage.toParentCenter();
            addChildWithId(strokeImage, CHILD_STROKE);

            // button rotating part
            Image baseImage = new Image(Application.sharedResourceMgr.getTexture(Res.IMG_BUTTON_BASE));
            baseImage.toParentCenter();            
            addChildWithId(baseImage, CHILD_ROTATION);            

            focusLost();
        }

        public override void update(float delta)
        {
            BaseElement stroke = getChild(CHILD_STROKE);
            BaseElement rotation = getChild(CHILD_ROTATION);

            stroke.color.A = (byte)(0.5f * (stroke.color.A + targetColor.A));
            stroke.color.R = (byte)(0.5f * (stroke.color.R + targetColor.R));
            stroke.color.G = (byte)(0.5f * (stroke.color.G + targetColor.G));
            stroke.color.B = (byte)(0.5f * (stroke.color.B + targetColor.B));

            scaleX = 0.5f * (scaleX + targetScale.X);
            scaleY = 0.5f * (scaleY + targetScale.Y);

            rotation.rotation += omega * delta;
        }

        public bool isFocused()
        {
            return this == focusedButton;
        }

        public void setFocused()
        {
            if (focusedButton != null)
                focusedButton.focusLost();

            focusedButton = this;
            focusGained();
        }

        private void focusLost()
        {
            targetColor = Color.Black;
            targetScale = new Vector2(1.0f, 1.0f);
            omega = 350.0f / 5.0f;
        }

        private void focusGained()
        {
            targetColor = Color.White;
            targetScale = new Vector2(1.2f, 1.2f);
            omega = 360.0f / 2.5f;
        }
    }

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
            UiButton newGameButton = new UiButton(buttonX, buttonY);
            newGameButton.setAlign(0.5f, 0.0f);            
            addChild(newGameButton);

            // last save
            buttonX = Constants.TITLE_SAFE_LEFT_X + 0.66f * Constants.TITLE_SAFE_AREA_WIDTH;
            buttonY = newGameButton.y;
            UiButton resumeGame = new UiButton(buttonX, buttonY);
            resumeGame.setAlign(0.5f, 0.0f);
            addChild(resumeGame);

            // about
            buttonX = Constants.TITLE_SAFE_LEFT_X + 50;
            buttonY = Constants.TITLE_SAFE_TOP_Y + 0.5f * Constants.TITLE_SAFE_AREA_HEIGHT;
            UiButton aboutButton = new UiButton(buttonX, buttonY);
            aboutButton.setAlign(0.0f, 0.5f);
            addChild(aboutButton);

            // exit
            buttonX = Constants.TITLE_SAFE_RIGHT_X - 50;
            buttonY = aboutButton.y;
            UiButton exitButton = new UiButton(buttonX, buttonY);
            exitButton.setAlign(1.0f, 0.5f);
            addChild(exitButton);

            // coop
            buttonX = newGameButton.x;
            buttonY = Constants.TITLE_SAFE_BOTTOM_Y;
            UiButton coopButton = new UiButton(buttonX, buttonY);
            coopButton.setAlign(0.5f, 1.0f);
            addChild(coopButton);

            // versus
            buttonX = resumeGame.x;
            buttonY = coopButton.y;
            UiButton vsButton = new UiButton(buttonX, buttonY);
            vsButton.setAlign(0.5f, 1.0f);
            addChild(vsButton);

            // focus
            newGameButton.setFocused();
            newGameButton.buttonLeft = aboutButton;
            newGameButton.buttonRight = resumeGame;
            newGameButton.buttonDown = coopButton;

            resumeGame.buttonLeft = newGameButton;
            resumeGame.buttonRight = exitButton;
            resumeGame.buttonDown = vsButton;

            aboutButton.buttonUp = newGameButton;
            aboutButton.buttonDown = coopButton;
            aboutButton.buttonRight = exitButton;

            exitButton.buttonUp = resumeGame;
            exitButton.buttonDown = vsButton;
            exitButton.buttonLeft = aboutButton;

            coopButton.buttonUp = newGameButton;
            coopButton.buttonLeft = aboutButton;
            coopButton.buttonRight = vsButton;

            vsButton.buttonUp = resumeGame;
            vsButton.buttonLeft = coopButton;
            vsButton.buttonRight = exitButton;
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
            Debug.Assert(UiButton.focusedButton != null);

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

        private void focusButtonRight()
        {
            UiButton nextButton = UiButton.focusedButton.buttonRight;
            if (nextButton != null)
                nextButton.setFocused();
        }

        private void focusButtonLeft()
        {
            UiButton nextButton = UiButton.focusedButton.buttonLeft;
            if (nextButton != null)
                nextButton.setFocused();
        }

        private void focusButtonUp()
        {
            UiButton nextButton = UiButton.focusedButton.buttonUp;
            if (nextButton != null)
                nextButton.setFocused();
        }

        private void focusButtonDown()
        {
            UiButton nextButton = UiButton.focusedButton.buttonDown;
            if (nextButton != null)
                nextButton.setFocused();
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
