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
    public class MenuView : View, ButtonDelegate
    {
        // дублеж, пиздешь и провокация

        private CustomGeomerty geomSkyDay;        
        private CustomGeomerty geomGround;

        private EnvCloud[] clouds;
        private int[] imgClouds;

        private Canvas canvas;
        private DrawMatrix MAT;

        private int focusedButton, oldFocusedButton;

        private const int BUTTON_NEW_GAME = 0;
        private const int BUTTON_RESUME_GAME = 1;
        private const int BUTTON_ABOUT = 2;
        private const int BUTTON_EXIT = 3;
        private const int BUTTON_COOP = 4;
        private const int BUTTON_VERSUS = 5;

        private const int CHILD_TITLE = 6;

        private MenuController menuController;

        public MenuView(MenuController menuController)
        {
            this.menuController = menuController;

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
            addButtons();
        }

        private void addTitle()
        {
            Image titleBack = new Image(Application.sharedResourceMgr.getTexture(Res.IMG_MENU_TITLE_BACK));
            titleBack.toParentCenter();
            Image title = new Image(Application.sharedResourceMgr.getTexture(Res.IMG_MENU_TITLE));
            title.toParentCenter();

            BaseElementContainer titleContainer = new BaseElementContainer(titleBack.width, titleBack.height);
            titleContainer.addChild(titleBack);
            titleContainer.addChild(title);

            addChildWithId(title, CHILD_TITLE);
        }

        private void addButton(int buttonID, float x, float y, float ax, float ay)
        {
            MenuButton button = new MenuButton(buttonID, x, y);
            button.setAlign(ax, ay);
            button.buttonDelegate = this;
            addChildWithId(button, buttonID);
        }

        private void addButtons()
        {
            // new game
            float newGameButtonX = Constants.TITLE_SAFE_LEFT_X + 0.33f * Constants.TITLE_SAFE_AREA_WIDTH;
            float newGameButtonY = Constants.TITLE_SAFE_TOP_Y;            
            addButton(BUTTON_NEW_GAME, newGameButtonX, newGameButtonY, 0.5f, 0.0f);            

            // last save
            float resumeGameButtonX = Constants.TITLE_SAFE_LEFT_X + 0.66f * Constants.TITLE_SAFE_AREA_WIDTH;
            float resumeGameButtonY = newGameButtonY;
            addButton(BUTTON_RESUME_GAME, resumeGameButtonX, resumeGameButtonY, 0.5f, 0.0f);            

            // about
            float aboutButtonX = Constants.TITLE_SAFE_LEFT_X + 50;
            float aboutButtonY = Constants.TITLE_SAFE_TOP_Y + 0.5f * Constants.TITLE_SAFE_AREA_HEIGHT;
            addButton(BUTTON_ABOUT, aboutButtonX, aboutButtonY, 0.0f, 0.5f);            

            // exit
            float exitButtonX = Constants.TITLE_SAFE_RIGHT_X - 50;
            float exitButtonY = aboutButtonY;
            addButton(BUTTON_EXIT, exitButtonX, exitButtonY, 1.0f, 0.5f);            

            // coop
            float coopButtonX = newGameButtonX;
            float coopButtonY = Constants.TITLE_SAFE_BOTTOM_Y;
            addButton(BUTTON_COOP, coopButtonX, coopButtonY, 0.5f, 1.0f);            

            // versus
            float versusButtonX = resumeGameButtonX;
            float versusButtonY = coopButtonY;
            addButton(BUTTON_VERSUS, versusButtonX, versusButtonY, 0.5f, 1.0f);            

            // focus
            focusedButton = oldFocusedButton = Constants.UNDEFINED;
            focusButton(BUTTON_NEW_GAME);
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

        public override bool buttonPressed(ref ButtonEvent e)
        {
            if (base.buttonPressed(ref e))
                return true;

            Debug.Assert(focusedButton != Constants.UNDEFINED);

            switch (e.button)
            {
                case Buttons.DPadDown:
                case Buttons.LeftThumbstickDown:
                    {
                        focusButtonDown();
                    }
                    return true;

                case Buttons.DPadUp:
                case Buttons.LeftThumbstickUp:
                    {
                        focusButtonUp();
                    }
                    return true;

                case Buttons.DPadLeft:
                case Buttons.LeftThumbstickLeft:
                    {
                        focusButtonLeft();
                    }
                    return true;

                case Buttons.DPadRight:
                case Buttons.LeftThumbstickRight:
                    {
                        focusButtonRight();
                    }
                    return true;

                case Buttons.Start:
                    {
                        ButtonEvent newEvent = e;
                        newEvent.button = Buttons.A;
                        buttonPressed(ref newEvent);
                    }
                    return true;
            }

            return false;
        }

        private void focusButton(int buttonId)
        {
            if (focusedButton != buttonId)
            {
                oldFocusedButton = focusedButton;
                if (focusedButton != Constants.UNDEFINED)
                    getChild(focusedButton).setFocused(false);

                focusedButton = buttonId;
                if (focusedButton != Constants.UNDEFINED)
                    getChild(focusedButton).setFocused(true);
            }
        }

        private void focusButtonRight()
        {
            if (focusedButton == BUTTON_RESUME_GAME || focusedButton == BUTTON_VERSUS)
                focusButton(BUTTON_EXIT);
            else if (focusedButton == BUTTON_NEW_GAME)
                focusButton(BUTTON_RESUME_GAME);            
            else if (focusedButton == BUTTON_COOP)
                focusButton(BUTTON_VERSUS);
            else if (focusedButton == BUTTON_ABOUT)
            {
                if (oldFocusedButton == BUTTON_NEW_GAME)
                    focusButton(BUTTON_COOP);
                else
                    focusButton(BUTTON_NEW_GAME);
            }

        }

        private void focusButtonLeft()
        {
            if (focusedButton == BUTTON_NEW_GAME || focusedButton == BUTTON_COOP)
                focusButton(BUTTON_ABOUT);
            else if (focusedButton == BUTTON_RESUME_GAME)
                focusButton(BUTTON_NEW_GAME);
            else if (focusedButton == BUTTON_EXIT)
            {
                if (oldFocusedButton == BUTTON_RESUME_GAME)
                    focusButton(BUTTON_VERSUS);
                else
                    focusButton(BUTTON_RESUME_GAME);
            }
            else if (focusedButton == BUTTON_VERSUS)
                focusButton(BUTTON_COOP);
        }

        private void focusButtonUp()
        {
            if (focusedButton == BUTTON_COOP)
                focusButton(BUTTON_NEW_GAME);
            else if (focusedButton == BUTTON_VERSUS)
                focusButton(BUTTON_RESUME_GAME);
            else if (focusedButton == BUTTON_ABOUT)
                focusButton(BUTTON_NEW_GAME);
            else if (focusedButton == BUTTON_EXIT)
                focusButton(BUTTON_RESUME_GAME);
        }

        private void focusButtonDown()
        {
            if (focusedButton == BUTTON_NEW_GAME)
                focusButton(BUTTON_COOP);
            else if (focusedButton == BUTTON_RESUME_GAME)
                focusButton(BUTTON_VERSUS);
            else if (focusedButton == BUTTON_ABOUT)
                focusButton(BUTTON_COOP);
            else if (focusedButton == BUTTON_EXIT)
                focusButton(BUTTON_VERSUS);
        }        

        public override bool keyPressed(Keys key)
        {
            switch (key)
            {
                case Keys.Enter:
                    {
                        ButtonEvent evt = Application.sharedInputMgr.makeButtonEvent(0, Buttons.A);
                        buttonPressed(ref evt);
                    }
                    return true;
                case Keys.Left:
                    focusButtonLeft();
                    return true;
                case Keys.Right:
                    focusButtonRight();
                    return true;
                case Keys.Up:
                    focusButtonUp();
                    return true;
                case Keys.Down:
                    focusButtonDown();
                    return true;                
            }
            return false;
        }

        public void onButtonPressed(int id, int playerIndex)
        {
            Application.sharedSoundMgr.playSound(Res.SND_UI_CLICK);

            if (id == BUTTON_NEW_GAME)
            {
                menuController.newGame(GameMode.SINGLE);
            }
            else if (id == BUTTON_COOP)
            {
                menuController.newGame(GameMode.COOP);
            }
            else if (id == BUTTON_VERSUS)
            {
                menuController.newGame(GameMode.VERSUS);
            }
        }

        public void onButtonFocused(int id)
        {
            Application.sharedSoundMgr.playSound(Res.SND_UI_FOCUS);
        }

        public void onButtonDefocused(int id)
        {
        }
    }
}
