using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.core;
using DuckstazyLive.game;

namespace DuckstazyLive.app
{
    public enum GameMode
    {
        SINGLE,
        COOP,
        VERSUS,
    }

    public class GameController : ViewController
    {
        enum Views
        {
            VIEW_GAME
        }

        private GameView gameView;
        private GameMode gameMode;

        public GameController(ViewController p) : base(p)
        {
            gameView = new GameView();
            addViewWithId(gameView, (int)Views.VIEW_GAME);
        }

        public override void activate()
        {
            base.activate();
            showView((int)Views.VIEW_GAME);

            gameView.newGame(gameMode);
        }

        public void setGameMode(GameMode mode)
        {
            gameMode = mode;
        }
    }
}
