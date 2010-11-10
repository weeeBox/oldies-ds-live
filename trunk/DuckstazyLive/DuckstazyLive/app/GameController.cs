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

        GameView gameView;

        public GameController(ViewController p) : base(p)
        {
            gameView = new GameView();
            addViewWithId(gameView, (int)Views.VIEW_GAME);
        }

        public override void activate()
        {
            base.activate();
            showView((int)Views.VIEW_GAME);

            gameView.newGame();
        }
    }
}
