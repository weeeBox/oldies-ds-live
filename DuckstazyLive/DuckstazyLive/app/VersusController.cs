using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.core;
using DuckstazyLive.game;

namespace DuckstazyLive.app
{
    public class VersusController : GameController
    {
        private const int CHILD_GAME = 0;
        private const int CHILD_STAGE_SELECT = 1;        
        private const int CHILD_RESULT = 2;
        private const int CHILD_PAUSE = 3;        

        public VersusController(ViewController parent) : base(parent)
        {        
        }
        
        public void selectStage()        
        {
            clearViews();

            VersusStageSelect stageSelect = new VersusStageSelect(this);
            showView(stageSelect);
        }        

        public void newGame(int levelIndex)
        {
            VersusLevel level = new VersusLevel(this, levelIndex);
            level.start();
            showNextView(level);
        }

        public void showDraw()
        {
            VersusResultView resultView = new VersusResultView(this);
            resultView.setDraw();
            showNextView(resultView);
        }

        public void showWinner(int playerIndex)
        {
            VersusResultView resultView = new VersusResultView(this);
            resultView.setWinner(playerIndex);
            showNextView(resultView);
        }        
    }
}
