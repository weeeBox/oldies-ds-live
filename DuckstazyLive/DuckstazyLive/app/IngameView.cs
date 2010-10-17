using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.core;
using DuckstazyLive.game;

namespace DuckstazyLive.app
{
    public class IngameView : View
    {
        private const int VIEW_GAMEWORLD = 0;
        private const int VIEW_PLAYZONE = 1;

        public IngameView()
        {
            GameWorld world = new GameWorld();
            addChildWithId(world, VIEW_GAMEWORLD);
        }

        private GameWorld getGameWorld()
        {
            return (GameWorld)getChild(VIEW_GAMEWORLD);
        }        
    }
}
