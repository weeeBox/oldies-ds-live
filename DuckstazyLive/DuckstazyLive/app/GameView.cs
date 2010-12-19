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
            game.newGame(gameMode);
        }

        public override bool buttonPressed(ref ButtonEvent e)
        {
            return game.buttonPressed(ref e);            
        }

        public override bool buttonReleased(ref ButtonEvent e)
        {
            return game.buttonReleased(ref e);            
        }

        public override bool keyPressed(Keys key)
        {
            return game.keyPressed(key);            
        }

        public override bool keyReleased(Keys key)
        {
            return game.keyReleased(key);            
        }
    }
}
