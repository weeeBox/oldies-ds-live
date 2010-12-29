using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.core;
using DuckstazyLive.game;

namespace DuckstazyLive.app
{
    public class VersusController : ViewController
    {
        private const int CHILD_STAGE_SELECT = 0;
        private const int CHILD_GAME = 1;
        private const int CHILD_RESULT = 2;
        private const int CHILD_PAUSE = 3;

        private VersusGame game;

        public VersusController(ViewController parent) : base(parent)
        {
            VersusStageSelect stageSelect = new VersusStageSelect(this);
            addViewWithId(stageSelect, CHILD_STAGE_SELECT);

            game = new VersusGame(this);
            addViewWithId(game, CHILD_GAME);
        }
        
        public void selectStage()
        {
            showView(CHILD_STAGE_SELECT);
        }

        public void newGame(int levelIndex)
        {
            game.newGame(levelIndex);
            showView(CHILD_GAME);
        }

        public void restart()
        {
            game.restartLevel();
        }

        public void showPause()
        {
            throw new NotImplementedException();
        }

        public void hidePause()
        {
            throw new NotImplementedException();
        }
    }
}
