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
        private Canvas canvas;
     
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

        private void addButton(String text, int buttonID, float x, float y, float ax, float ay)
        {
            MenuButton button = new MenuButton(text, buttonID, x, y);
            button.setAlign(ax, ay);
            button.buttonDelegate = this;
            addChildWithId(button, buttonID);
        }

        private void addButtons()
        {
            // new game
            float newGameButtonX = Constants.TITLE_SAFE_LEFT_X + 0.33f * Constants.TITLE_SAFE_AREA_WIDTH;
            float newGameButtonY = Constants.TITLE_SAFE_TOP_Y;            
            addButton("NEW\nGAME", BUTTON_NEW_GAME, newGameButtonX, newGameButtonY, 0.5f, 0.0f);            

            // last save
            float resumeGameButtonX = Constants.TITLE_SAFE_LEFT_X + 0.66f * Constants.TITLE_SAFE_AREA_WIDTH;
            float resumeGameButtonY = newGameButtonY;
            addButton("LAST\nSAVE", BUTTON_RESUME_GAME, resumeGameButtonX, resumeGameButtonY, 0.5f, 0.0f);            

            // about
            float aboutButtonX = Constants.TITLE_SAFE_LEFT_X + 50;
            float aboutButtonY = Constants.TITLE_SAFE_TOP_Y + 0.5f * Constants.TITLE_SAFE_AREA_HEIGHT;
            addButton("ABOUT", BUTTON_ABOUT, aboutButtonX, aboutButtonY, 0.0f, 0.5f);            

            // exit
            float exitButtonX = Constants.TITLE_SAFE_RIGHT_X - 50;
            float exitButtonY = aboutButtonY;
            addButton("EXIT", BUTTON_EXIT, exitButtonX, exitButtonY, 1.0f, 0.5f);            

            // coop
            float coopButtonX = newGameButtonX;
            float coopButtonY = Constants.TITLE_SAFE_BOTTOM_Y;
            addButton("COOP\nMODE", BUTTON_COOP, coopButtonX, coopButtonY, 0.5f, 1.0f);            

            // versus
            float versusButtonX = resumeGameButtonX;
            float versusButtonY = coopButtonY;
            addButton("VERSUS\nMODE", BUTTON_VERSUS, versusButtonX, versusButtonY, 0.5f, 1.0f);            

            // focus
            focusedButton = oldFocusedButton = Constants.UNDEFINED;
            focusButton(BUTTON_NEW_GAME);
        }

        public override void update(float delta)
        {
            base.update(delta);
            Env.getIntance().update(delta, 0.0f);
        }

        public override void draw()
        {
            preDraw();
            drawEnv();
            postDraw();
        }

        private void drawEnv()
        {
            Env.getIntance().draw1(canvas);
            Env.getIntance().draw2(canvas);
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
