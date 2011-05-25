using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using asap.core;
using app;
using app.menu;

namespace DuckstazyLive.app
{
    public class MenuController : Controller, ButtonListener
    {
        public override void Start(int param)
        {
            base.Start(param);

            MainMenu menu = new MainMenu(this);
            Application.sharedScreensView.StartScreen(menu);
        }

        public void ButtonPressed(int code)
        {
            switch (code)
            {
                case MainMenu.BUTTON_NEW_GAME:
                    AppRootController.StartChildController(AppRootController.gameController, GameController.START_SINGLE_GAME);
                    break;
            }

        }
    }
}
