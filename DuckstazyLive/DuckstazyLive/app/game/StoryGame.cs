using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckstazyLive.app.game.level;
using System.Diagnostics;

namespace DuckstazyLive.app.game
{
    public enum StoryGameMode
    {
        SINGLE,
        MULTIPLAYER,
    }

    public class StoryGame : BaseGame
    {
        private StoryGameMode gameMode;
        
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
            level.start();
        }

        public void setGameMode(StoryGameMode mode)
        {
            this.gameMode = mode;
        }

        public void nextLevel()
        {
            throw new NotImplementedException();
        }

        internal void showLoose(string message)
        {
            throw new NotImplementedException();
        }

        internal void showWin()
        {
            throw new NotImplementedException();
        }

        internal void showDeath()
        {
            throw new NotImplementedException();
        }
    }
}
