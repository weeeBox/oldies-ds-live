using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using app.menu;
using DuckstazyLive.app.game.level;
using asap.graphics;

namespace DuckstazyLive.app.game
{
    public class SingleGameScreen : EnvScreen
    {
        StoryLevel level;

        public SingleGameScreen(GameController controller) : base(ScreenId.SINGLE_GAME)
        {
            level = new SingleLevel(null);
            level.start();
        }

        public override void Draw(Graphics g)
        {
            PreDraw(g);
            level.Draw(g);
            PostDraw(g);
        }

        public override void Update(float delta)
        {
            base.Update(delta);
            level.Update(delta);
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
