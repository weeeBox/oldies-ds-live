using Framework.visual;
using Framework.core;
using System;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
namespace DuckstazyLive.app
{
    public class PauseView : GameView, ButtonDelegate
    {
        private int focusedButton, oldFocusedButton;

        private const int BUTTON_RESUME = 0;
        private const int BUTTON_RESTART = 1;
        private const int BUTTON_MENU = 2;
        private const int BUTTON_EXIT = 3;

        private const int CHILD_TITLE = 4;        

        public PauseView(GameController controller) : base(controller)
        {
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
            titleContainer.toParentCenter();

            addChildWithId(titleContainer, CHILD_TITLE);
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
            // resume
            float resumeGameButtonX = Constants.TITLE_SAFE_LEFT_X + 0.33f * Constants.TITLE_SAFE_AREA_WIDTH;
            float resumeGameButtonY = Constants.TITLE_SAFE_TOP_Y;
            addButton("RESUME", BUTTON_RESUME, resumeGameButtonX, resumeGameButtonY, 0.5f, 0.0f);

            // restart
            float restartGameButtonX = Constants.TITLE_SAFE_LEFT_X + 0.66f * Constants.TITLE_SAFE_AREA_WIDTH;
            float restartGameButtonY = resumeGameButtonY;
            addButton("RESTART", BUTTON_RESTART, restartGameButtonX, restartGameButtonY, 0.5f, 0.0f);

            // menu
            float menuButtonX = resumeGameButtonX;
            float menuButtonY = Constants.TITLE_SAFE_BOTTOM_Y;
            addButton("MAIN\nMENU", BUTTON_MENU, menuButtonX, menuButtonY, 0.5f, 1.0f);

            // exit
            float exitButtonX = restartGameButtonX;
            float exitButtonY = menuButtonY;
            addButton("EXIT", BUTTON_EXIT, exitButtonX, exitButtonY, 0.5f, 1.0f);            

            // focus
            focusedButton = oldFocusedButton = Constants.UNDEFINED;
            focusButton(BUTTON_RESUME);
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

                case ButtonAction.Back:
                    getController().hidePause();
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
            if (focusedButton == BUTTON_RESUME)
                focusButton(BUTTON_RESTART);
            else if (focusedButton == BUTTON_MENU)
                focusButton(BUTTON_EXIT);
        }

        private void focusButtonLeft()
        {
            if (focusedButton == BUTTON_RESTART)
                focusButton(BUTTON_RESUME);
            else if (focusedButton == BUTTON_EXIT)
                focusButton(BUTTON_MENU);
        }

        private void focusButtonUp()
        {
            if (focusedButton == BUTTON_MENU)
                focusButton(BUTTON_RESUME);
            else if (focusedButton == BUTTON_EXIT)
                focusButton(BUTTON_RESTART);
        }

        private void focusButtonDown()
        {
            if (focusedButton == BUTTON_RESUME)
                focusButton(BUTTON_MENU);
            else if (focusedButton == BUTTON_RESTART)
                focusButton(BUTTON_EXIT);
        }        

        public void onButtonPressed(int id, int playerIndex)
        {
            Application.sharedSoundMgr.playSound(Res.SND_UI_CLICK);

            if (id == BUTTON_RESUME)
            {
                getController().hidePause();
            }
            else if (id == BUTTON_RESTART)
            {
                getController().restartLevel();
            }
            else if (id == BUTTON_MENU)
            {
                getController().deactivate();
            }
            else if (id == BUTTON_EXIT)
            {
                
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
