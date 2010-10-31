using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.core;
using Framework.visual;
using Microsoft.Xna.Framework.Graphics;
using DuckstazyLive.app;
using Microsoft.Xna.Framework;

using Framework.utils;

namespace DuckstazyLive.game
{
    public class GameWorld : BaseElement
    {
        Game game;
        public GameWorld()            
        {
            game = new Game();
            game.clickNewGame();

            Application.sharedInputMgr.addInputListener(game);
        }

        public override void update(float dt)
        {
            game.update(dt);
        }

        public override void draw()
        {
            preDraw();
            game.draw();
            postDraw();
        }        
    }
}
