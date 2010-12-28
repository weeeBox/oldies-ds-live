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
    }

    public class StoryController : ViewController
    {
        private const int VIEW_GAME = 0;
        private const int VIEW_PAUSE = 1;
        private const int VIEW_DEATH = 2;
        private const int VIEW_LOOSE = 3;
        private const int VIEW_WIN = 4;

        private Game game;
        private GameMode gameMode;

        public StoryController(ViewController p) : base(p)
        {
            game = new Game(this);
            addViewWithId(game, VIEW_GAME);

            PauseView pauseView = new PauseView(this);
            addViewWithId(pauseView, VIEW_PAUSE);

            DeathView deathView = new DeathView(this);
            addViewWithId(deathView, VIEW_DEATH);

            LostView lostView = new LostView(this);
            addViewWithId(lostView, VIEW_LOOSE);

            WinView winView = new WinView(this);
            addViewWithId(winView, VIEW_WIN);
        }

        public override void activate()
        {
            base.activate();
            newGame();
        }        

        public void newGame()
        {
            game.newGame(gameMode);
            showView(VIEW_GAME);
        }

        public void restartLevel()
        {
            game.restartLevel();
            showView(VIEW_GAME);
        }

        public void nextLevel()
        {
            game.nextLevel();
            showView(VIEW_GAME);
        }

        public void showPause()
        {
            showView(VIEW_PAUSE);
        }

        public void hidePause()
        {
            showView(VIEW_GAME);
        }

        public void showLooseScreen(string message)
        {
            LostView v = (LostView) getView(VIEW_LOOSE);
            v.setMessage(message);
            showView(VIEW_LOOSE);
        }

        public void showWinView()
        {
            showView(VIEW_WIN);
        }

        public void showDeathView()
        {
            showView(VIEW_DEATH);
        }

        public void setGameMode(GameMode mode)
        {
            gameMode = mode;
        }        
    }
}
