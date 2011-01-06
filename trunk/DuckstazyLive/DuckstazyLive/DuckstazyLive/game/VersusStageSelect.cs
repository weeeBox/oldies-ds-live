using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckstazyLive.app;
using Framework.core;
using Microsoft.Xna.Framework.Graphics;
using Framework.visual;
using Microsoft.Xna.Framework.Input;

namespace DuckstazyLive.game
{
    public class VersusStageSelect : EnvView, ButtonDelegate
    {
        private const int NUM_BUTTON_ROWS = 2;
        private const int NUM_BUTTON_COLS = 5;

        private const int BUTTON_HOR_DIST = 35;
        private const int BUTTON_VER_DIST = 20;

        private VersusController controller;
        private int focusedButtonId;

        public VersusStageSelect(VersusController controller, VersusLevel level)
        {
            this.controller = controller;
            this.height = (int)Constants.ENV_HEIGHT;

            Texture2D buttonTex = Application.sharedResourceMgr.getTexture(Res.IMG_BUTTON_STROKE_FOCUSED);
            int buttonWidth = buttonTex.Width;
            int buttonHeight = buttonTex.Height;

            int containerWidth = NUM_BUTTON_COLS * (buttonWidth + BUTTON_HOR_DIST) - BUTTON_HOR_DIST;
            int containerHeight = NUM_BUTTON_ROWS * (buttonHeight + BUTTON_VER_DIST) - BUTTON_VER_DIST;

            float buttonStartX = 0.5f * (width - containerWidth);
            float buttonStartY = 0.5f * (height - containerHeight);

            int buttonId = 0;
            float buttonX, buttonY;

            for (int row = 0; row < NUM_BUTTON_ROWS; ++row)
            {
                buttonY = buttonStartY + row * (buttonHeight + BUTTON_VER_DIST);

                for (int col = 0; col < NUM_BUTTON_COLS; ++col)
                {
                    buttonX = buttonStartX + col * (buttonWidth + BUTTON_HOR_DIST);
                    String buttonName = buttonId < level.getStagesCount() ? level.getStageName(buttonId).ToUpper() : "NO STAGE";
                    addButton(buttonId, buttonName, buttonX, buttonY);
                    buttonId++;
                }
            }

            BaseElement lastButton = getChild(buttonId - 1);

            UiControllerButtons buttons = new UiControllerButtons("SELECT", "BACK");            
            addChild(buttons);
            attachHor(buttons, AttachStyle.CENTER);
            UiLayout.attachVert(buttons, lastButton, this, AttachStyle.CENTER);
        }

        public override void onShow()
        {
            base.onShow();
            focusedButtonId = Constants.UNDEFINED;
            focusButton(0);
        }

        private void addButton(int id, String buttonName, float x, float y)
        {
            MenuButton button = new MenuButton(buttonName, id, x, y);
            button.buttonDelegate = this;
            addChildWithId(button, id);
        }

        public override bool buttonPressed(ref ButtonEvent e)
        {
            if (base.buttonPressed(ref e))
                return true;

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
                    {
                        controller.deactivate();
                    }
                    return true;
            }

            return false;
        }

        private void focusButtonDown()
        {
            int newFocusedButtonId = focusedButtonId + NUM_BUTTON_COLS;
            if (newFocusedButtonId < getButtonsCount())
            {
                focusButton(newFocusedButtonId);
            }
        }

        private void focusButtonUp()
        {
            int newFocusedButtonId = focusedButtonId - NUM_BUTTON_COLS;
            if (newFocusedButtonId >= 0)
            {
                focusButton(newFocusedButtonId);
            }
        }

        private void focusButtonLeft()
        {
            if (focusedButtonId % NUM_BUTTON_COLS > 0)
            {
                int newFocusedButtonId = focusedButtonId - 1;
                focusButton(newFocusedButtonId);
            }
        }

        private void focusButtonRight()
        {
            if (focusedButtonId % NUM_BUTTON_COLS < NUM_BUTTON_COLS - 1)
            {
                int newFocusedButtonId = focusedButtonId + 1;
                focusButton(newFocusedButtonId);
            }            
        }

        private void focusButton(int buttonId)
        {
            if (focusedButtonId != Constants.UNDEFINED)
            {
                getChild(focusedButtonId).setFocused(false);
            }
            focusedButtonId = buttonId;
            getChild(buttonId).setFocused(true);
        }

        private int getButtonsCount()
        {
            return NUM_BUTTON_COLS * NUM_BUTTON_ROWS;
        }

        public void onButtonPressed(int id, int playerIndex)
        {
            Application.sharedSoundMgr.playSound(Res.SND_UI_CLICK);
            controller.newGame(id);
        }

        public void onButtonFocused(int id)
        {

        }

        public void onButtonDefocused(int id)
        {

        }
    }
}
