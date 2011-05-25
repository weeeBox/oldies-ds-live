using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using asap.core;
using DuckstazyLive.app.game;
using app;

namespace DuckstazyLive.app
{
    public class GameController : Controller
    {
        public const int START_SINGLE_GAME = 0;

        public override void Start(int param)
        {
            base.Start(param);

            switch (param)
            {
                case START_SINGLE_GAME:
                    SingleGameScreen screen = new SingleGameScreen(this);
                    Application.sharedScreensView.StartNextScreen(screen);
                    break;
            }
        }
    }
}
