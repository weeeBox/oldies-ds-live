using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using asap.core;
using app;

namespace DuckstazyLive.app
{
    public class MenuController : Controller
    {
        public override void Start(int param)
        {
            base.Start(param);

            MainMenu menu = new MainMenu(this);
            Application.sharedScreensView.StartScreen(menu);
        }
    }
}
