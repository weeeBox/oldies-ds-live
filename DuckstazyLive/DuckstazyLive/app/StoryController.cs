using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.core;
using DuckstazyLive.game;
using System.Diagnostics;

namespace DuckstazyLive.app
{
    public enum StoryGameMode
    {
        SINGLE,
        MULTIPLAYER,
    }

    public class StoryController : GameController
    {
        private StoryGameMode gameMode;

        public StoryController(ViewController parent) : base(parent)
        {

        }

        public override void activate()
        {
            base.activate();
            newGame();
        }

        public void newGame()
        {
            StoryLevel level = null;
            switch (gameMode)
            {
                case StoryGameMode.SINGLE:
                    level = new SingleLevel(this);
                    break;
                case StoryGameMode.MULTIPLAYER:
                    level = new CoopLevel(this);
                    break;
                default:
                    Debug.Assert(false, "Bad mode: " + gameMode);
                    break;
            }
            showView(level);
            level.start();
        }        

        public void setGameMode(StoryGameMode mode)
        {
            this.gameMode = mode;
        }        

        public void nextLevel()
        {
            Debug.Assert(!isPaused());
            StoryLevel level = (StoryLevel)getActiveView();
            level.nextLevel();
        }

        internal void showLoose(string message)
        {
            LostView looseView = new LostView(this);
            showNextView(looseView);
        }

        internal void showWin()
        {
            WinView winView = new WinView(this);
            showView(winView);
        }

        internal void showDeath()
        {            
            DeathView deathView = new DeathView(this);
            showView(deathView);
        }        
    }
}
