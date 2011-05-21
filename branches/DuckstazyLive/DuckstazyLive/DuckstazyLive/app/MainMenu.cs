using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using asap.visual;
using app;
using app.menu;
using DuckstazyLive.app.menu;
using asap.ui;
using asap.graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace DuckstazyLive.app
{
    public class MainMenu : EnvScreen, ButtonListener
    {
        private MenuController menuController;

        private const int BUTTON_NEW_GAME = 0;

        private const int BUTTON_RESUME_GAME = 1;

        private const int BUTTON_ABOUT = 2;

        private const int BUTTON_EXIT = 3;

        private const int BUTTON_COOP = 4;

        private const int BUTTON_VERSUS = 5;

        public MainMenu(MenuController menuController) : base(ScreenId.MAIN_MENU)
        {
            this.menuController = menuController;            

            // title
            AddTitle();

            // ui
            AddButtons();
        }

        //public override void onShow()
        //{
        //    base.onShow();
        //    GameElements.Env.startBlanc();

        //    BaseElement titleBack = getChild(CHILD_TITLE_BACK);
        //    titleBack.turnTimelineSupportWithMaxKeyFrames(1);
        //    titleBack.addKeyFrame(new KeyFrame(titleBack.x, titleBack.y, Color.White, 1.0f, 1.0f, 360.0f, 100.0f));
        //    titleBack.setTimelineLoopType(Timeline.REPLAY);
        //    titleBack.playTimeline();
        //}

        private void AddTitle()
        {
            Image titleBack = new Image(Application.sharedResourceMgr.GetTexture(Res.IMG_MENU_TITLE_BACK));
            Image title = new Image(Application.sharedResourceMgr.GetTexture(Res.IMG_MENU_TITLE));            

            AddChild(titleBack);
            AddChild(title);

            AttachCenter(titleBack);
            AttachCenter(title);
        }        

        private void AddButtons()
        {
            UiComponent container = CreateTitleSafeContainer();            

            // new game            
            MenuButton button = new MenuButton("NEW GAME", BUTTON_NEW_GAME, this);            
            container.AddChild(button);
            container.AttachHor(button, 0.33f);            

            // last save            
            button = new MenuButton("LAST SAVE", BUTTON_RESUME_GAME, this);
            container.AddChild(button);
            container.AttachHor(button, 0.66f);

            // about            
            button = new MenuButton("ABOUT", BUTTON_ABOUT, this);
            container.AddChild(button);
            container.AttachHor(button, 0.05f);
            container.AttachVert(button, ALIGN_CENTER);

            // exit                        
            button = new MenuButton("EXIT", BUTTON_EXIT, this);
            container.AddChild(button);
            container.AttachHor(button, 0.95f);
            container.AttachVert(button, ALIGN_CENTER);

            // coop                        
            button = new MenuButton("COOP MODE", BUTTON_COOP, this);
            container.AddChild(button);
            container.AttachHor(button, 0.33f);
            container.AttachVert(button, ALIGN_MAX);

            // versus                        
            button = new MenuButton("VERSUS MODE", BUTTON_VERSUS, this);
            container.AddChild(button);
            container.AttachHor(button, 0.66f);
            container.AttachVert(button, ALIGN_MAX);
        }

        //public override bool buttonPressed(ref ButtonEvent e)
        //{
        //    if (base.buttonPressed(ref e))
        //        return true;

        //    Debug.Assert(focusedButton != Constants.UNDEFINED);

        //    switch (e.action)
        //    {
        //        case ButtonAction.Down:
        //            focusButtonDown();
        //            return true;

        //        case ButtonAction.Up:
        //            focusButtonUp();
        //            return true;

        //        case ButtonAction.Left:
        //            focusButtonLeft();
        //            return true;

        //        case ButtonAction.Right:
        //            focusButtonRight();
        //            return true;

        //        case ButtonAction.OK:
        //            {
        //                ButtonEvent newEvent = e;
        //                newEvent.button = Buttons.A;
        //                buttonPressed(ref newEvent);
        //            }
        //            return true;
        //    }

        //    return false;
        //}

        //private void focusButton(int buttonId)
        //{
        //    if (focusedButton != buttonId)
        //    {
        //        oldFocusedButton = focusedButton;
        //        if (focusedButton != Constants.UNDEFINED)
        //            getChild(focusedButton).setFocused(false);

        //        focusedButton = buttonId;
        //        if (focusedButton != Constants.UNDEFINED)
        //            getChild(focusedButton).setFocused(true);
        //    }
        //}

        //private void focusButtonRight()
        //{
        //    if (focusedButton == BUTTON_RESUME_GAME || focusedButton == BUTTON_VERSUS)
        //        focusButton(BUTTON_EXIT);
        //    else if (focusedButton == BUTTON_NEW_GAME)
        //        focusButton(BUTTON_RESUME_GAME);
        //    else if (focusedButton == BUTTON_COOP)
        //        focusButton(BUTTON_VERSUS);
        //    else if (focusedButton == BUTTON_ABOUT)
        //    {
        //        if (oldFocusedButton == BUTTON_NEW_GAME)
        //            focusButton(BUTTON_COOP);
        //        else
        //            focusButton(BUTTON_NEW_GAME);
        //    }
        //}

        //private void focusButtonLeft()
        //{
        //    if (focusedButton == BUTTON_NEW_GAME || focusedButton == BUTTON_COOP)
        //        focusButton(BUTTON_ABOUT);
        //    else if (focusedButton == BUTTON_RESUME_GAME)
        //        focusButton(BUTTON_NEW_GAME);
        //    else if (focusedButton == BUTTON_EXIT)
        //    {
        //        if (oldFocusedButton == BUTTON_RESUME_GAME)
        //            focusButton(BUTTON_VERSUS);
        //        else
        //            focusButton(BUTTON_RESUME_GAME);
        //    }
        //    else if (focusedButton == BUTTON_VERSUS)
        //        focusButton(BUTTON_COOP);
        //}

        //private void focusButtonUp()
        //{
        //    if (focusedButton == BUTTON_COOP)
        //        focusButton(BUTTON_NEW_GAME);
        //    else if (focusedButton == BUTTON_VERSUS)
        //        focusButton(BUTTON_RESUME_GAME);
        //    else if (focusedButton == BUTTON_ABOUT)
        //        focusButton(BUTTON_NEW_GAME);
        //    else if (focusedButton == BUTTON_EXIT)
        //        focusButton(BUTTON_RESUME_GAME);
        //}

        //private void focusButtonDown()
        //{
        //    if (focusedButton == BUTTON_NEW_GAME)
        //        focusButton(BUTTON_COOP);
        //    else if (focusedButton == BUTTON_RESUME_GAME)
        //        focusButton(BUTTON_VERSUS);
        //    else if (focusedButton == BUTTON_ABOUT)
        //        focusButton(BUTTON_COOP);
        //    else if (focusedButton == BUTTON_EXIT)
        //        focusButton(BUTTON_VERSUS);
        //}

        //public void onButtonPressed(int id, int playerIndex)
        //{
        //    Application.sharedSoundMgr.playSound(Res.SND_UI_CLICK);

        //    if (id == BUTTON_NEW_GAME)
        //    {
        //        menuController.newGame(StoryGameMode.SINGLE);
        //    }
        //    else if (id == BUTTON_COOP)
        //    {
        //        menuController.newGame(StoryGameMode.MULTIPLAYER);
        //    }
        //    else if (id == BUTTON_VERSUS)
        //    {
        //        menuController.versusGame();
        //    }
        //    else if (id == BUTTON_EXIT)
        //    {
        //        Application.quit();
        //    }
        //}

        //public void onButtonFocused(int id)
        //{
        //    Application.sharedSoundMgr.playSound(Res.SND_UI_FOCUS);
        //}

        //public void onButtonDefocused(int id)
        //{
        //}

        //public void ButtonPressed(int code)
        //{
        //    throw new NotImplementedException();
        //}        

        public void ButtonPressed(int code)
        {
            
        }
    }
}
