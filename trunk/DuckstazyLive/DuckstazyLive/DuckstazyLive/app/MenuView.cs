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
    public class MenuView : EnvView, ButtonDelegate
    {     
        private int focusedButton, oldFocusedButton;

        private const int BUTTON_NEW_GAME = 0;
        private const int BUTTON_RESUME_GAME = 1;
        private const int BUTTON_ABOUT = 2;
        private const int BUTTON_EXIT = 3;
        private const int BUTTON_COOP = 4;
        private const int BUTTON_VERSUS = 5;

        private const int CHILD_TITLE_BACK = 6;
        private const int CHILD_TITLE = 7;        

        private MenuController menuController;

        public MenuView(MenuController menuController)
        {
            this.menuController = menuController;
     
            // title
            addTitle();

            // ui
            addButtons();
        }

        public override void onShow()
        {
            base.onShow();
            GameElements.Env.startBlanc();

            BaseElement titleBack = getChild(CHILD_TITLE_BACK);
            titleBack.turnTimelineSupportWithMaxKeyFrames(1);
            titleBack.addKeyFrame(new KeyFrame(titleBack.x, titleBack.y, Color.White, 1.0f, 1.0f, 360.0f, 100.0f));
            titleBack.setTimelineLoopType(Timeline.REPLAY);
            titleBack.playTimeline();
        }

        private void addTitle()
        {
            Image titleBack = new Image(Application.sharedResourceMgr.getTexture(Res.IMG_MENU_TITLE_BACK));                        
            Image title = new Image(Application.sharedResourceMgr.getTexture(Res.IMG_MENU_TITLE));
            titleBack.toParentCenter();
            title.toParentCenter();
            
            addChild(titleBack, CHILD_TITLE_BACK);
            addChild(title, CHILD_TITLE);
        }

        private void addButton(String text, int buttonID, float x, float y, float ax, float ay)
        {
            MenuButton button = new MenuButton(text, buttonID, x, y);
            button.setAlign(ax, ay);
            button.buttonDelegate = this;
            addChild(button, buttonID);
        }

        private void addButton(String text, int buttonID, float x, float y, float ax, float ay, uint c1, uint c2)
        {
            MenuButton button = new MenuButton(text, buttonID, x, y, c1, c2);
            button.setAlign(ax, ay);
            button.buttonDelegate = this;
            addChild(button, buttonID);
        }

        private void addButtons()
        {
            // new game
            float newGameButtonX = Constants.TITLE_SAFE_LEFT_X + 0.33f * Constants.TITLE_SAFE_AREA_WIDTH;
            float newGameButtonY = Constants.TITLE_SAFE_TOP_Y;            
            addButton("NEW GAME", BUTTON_NEW_GAME, newGameButtonX, newGameButtonY, 0.5f, 0.0f);            

            // last save
            float resumeGameButtonX = Constants.TITLE_SAFE_LEFT_X + 0.66f * Constants.TITLE_SAFE_AREA_WIDTH;
            float resumeGameButtonY = newGameButtonY;
            addButton("LAST SAVE", BUTTON_RESUME_GAME, resumeGameButtonX, resumeGameButtonY, 0.5f, 0.0f);            

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
            addButton("COOP MODE", BUTTON_COOP, coopButtonX, coopButtonY, 0.5f, 1.0f);            

            // versus
            float versusButtonX = resumeGameButtonX;
            float versusButtonY = coopButtonY;
            addButton("VERSUS MODE", BUTTON_VERSUS, versusButtonX, versusButtonY, 0.5f, 1.0f);            

            // focus
            focusedButton = oldFocusedButton = Constants.UNDEFINED;
            focusButton(BUTTON_NEW_GAME);
        }

        public override bool buttonPressed(ref ButtonEvent e)
        {
            if (base.buttonPressed(ref e))
                return true;

            Debug.Assert(focusedButton != Constants.UNDEFINED);

            switch (e.action)
            {
                case ButtonAction.Down:                    
                    focusButtonDown();                    
                    return true;

                case ButtonAction.Up:                
                    focusButtonUp();                
                    return true;

                case ButtonAction.Left:                
                    focusButtonLeft();                
                    return true;

                case ButtonAction.Right:                
                    focusButtonRight();                
                    return true;

                case ButtonAction.OK:
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

        public void onButtonPressed(int id, int playerIndex)
        {
            Application.sharedSoundMgr.playSound(Res.SND_UI_CLICK);

            if (id == BUTTON_NEW_GAME)
            {
                menuController.newGame(StoryGameMode.SINGLE);
            }
            else if (id == BUTTON_COOP)
            {
                menuController.newGame(StoryGameMode.MULTIPLAYER);
            }
            else if (id == BUTTON_VERSUS)
            {
                menuController.versusGame();
            }
            else if (id == BUTTON_EXIT)
            {
                Application.quit();
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
