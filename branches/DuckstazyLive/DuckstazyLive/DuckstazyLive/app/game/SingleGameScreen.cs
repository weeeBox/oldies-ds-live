using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using app.menu;
using DuckstazyLive.app.game.level;
using asap.graphics;
using app;
using asap.core;

namespace DuckstazyLive.app.game
{
    public class SingleGameScreen : Screen
    {
        StoryLevel level;

        public SingleGameScreen(GameController controller) : base(ScreenId.SINGLE_GAME)
        {
            level = new SingleLevel(null, 960, 600);            
            //level.alignX = level.parentAlignX = ALIGN_CENTER;
            level.drawBorder = true;
            AddChild(level);

            level.start();            
        }

        public override bool KeyPressed(KeyEvent evt)
        {
            return level.KeyPressed(evt);
        }

        public override bool KeyReleased(KeyEvent evt)
        {
            return level.KeyReleased(evt);
        }

        //protected override Hud createHud()
        //{
        //    throw new NotImplementedException();
        //}

        //protected override stage.LevelStage createStage(int stageIndex)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
