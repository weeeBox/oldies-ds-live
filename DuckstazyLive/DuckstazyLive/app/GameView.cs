using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.core;
using DuckstazyLive.game;
using Microsoft.Xna.Framework.Input;

namespace DuckstazyLive.app
{
    public class GameView : View
    {
        private Game game;

        public GameView()
        {
            game = new Game();
            addChildWithId(game, 0);
        }

        public void newGame(GameMode gameMode)
        {
            game.newGame();
        }

        public override void buttonPressed(ref ButtonEvent e)
        {
            game.buttonPressed(ref e);
        }

        public override void buttonReleased(ref ButtonEvent e)
        {
            game.buttonReleased(ref e);
        }

        public override void keyPressed(Keys key)
        {
            game.keyPressed(key);
        }

        public override void keyReleased(Keys key)
        {
            game.keyReleased(key);
        }
    }
}
