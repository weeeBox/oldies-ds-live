using System;
using DuckstazyLive.app.game.level;

namespace DuckstazyLive.app.game
{
    public class VersusGame : BaseGame
    {
        private const int CHILD_GAME = 0;
        private const int CHILD_STAGE_SELECT = 1;
        private const int CHILD_RESULT = 2;
        private const int CHILD_PAUSE = 3;

        public void selectStage()
        {
            //clearViews();

            //VersusStageSelect stageSelect = new VersusStageSelect(this);
            //showView(stageSelect);
            throw new NotImplementedException();
        }

        public void newGame(int levelIndex)
        {
            //VersusLevel level = new VersusLevel(this, levelIndex);
            //level.start();
            //showNextView(level);
            throw new NotImplementedException();
        }

        public void showDraw()
        {
            //VersusResultView resultView = new VersusResultView(this);
            //resultView.setDraw();
            //showNextView(resultView);
            throw new NotImplementedException();
        }

        public void showWinner(int playerIndex)
        {
            //VersusResultView resultView = new VersusResultView(this);
            //resultView.setWinner(playerIndex);
            //showNextView(resultView);
            throw new NotImplementedException();
        }
    }
}
