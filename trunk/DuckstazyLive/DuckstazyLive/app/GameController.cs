using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.core;
using DuckstazyLive.game;

namespace DuckstazyLive.app
{
    public class GameController : ViewController
    {
        enum Views
        {
            VIEW_GAME
        }

        public GameController(ViewController p) : base(p)
        {
            IngameView view = new IngameView();
            addViewWithId(view, (int)Views.VIEW_GAME);
        }

        public override void activate()
        {
            base.activate();
            showView((int)Views.VIEW_GAME);
        }
    }
}
