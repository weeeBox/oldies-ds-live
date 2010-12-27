using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.core;
using DuckstazyLive.game;

namespace DuckstazyLive.app
{
    public class GameView : View
    {
        private GameController controller;
        private Canvas canvas;

        public GameView(GameController controller)
        {
            this.controller = controller;
            canvas = new Canvas(Constants.SCREEN_WIDTH, Constants.SCREEN_HEIGHT);
        }

        protected Canvas getCanvas()
        {
            return canvas;
        }

        protected GameController getController()
        {
            return controller;
        }
    }
}
