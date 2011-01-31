using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.core;
using DuckstazyLive.game;
using System.Diagnostics;

namespace DuckstazyLive.app
{
    public class GameController : ViewController
    {        
        public GameController(ViewController parent) : base(parent)
        {

        }

        public void restartLevel()
        {
            Level level = (Level)getActiveView();
            level.restart();
        }

        public void quitLevel()
        {
            deactivate();
        }

        public bool isPaused()
        {
            return getActiveView() is PauseView;
        }

        public void hidePause()
        {
            Debug.Assert(isPaused());
            hideView();
        }

        public void showPause()
        {
            PauseView pause = new PauseView(this);
            showNextView(pause);
        }
    }
}
