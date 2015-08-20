using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.core;
using DuckstazyLive.app;

namespace DuckstazyLive.game
{
    public class VersusView : View
    {
        private VersusController controller;
        private Canvas canvas;

        public VersusView(VersusController controller)
        {
            this.controller = controller;
            canvas = new Canvas(Constants.SCREEN_WIDTH, Constants.SCREEN_HEIGHT);
        }

        protected VersusController getController()
        {
            return controller;
        }

        protected Canvas getCanvas()
        {
            return canvas;
        }
    }
}
