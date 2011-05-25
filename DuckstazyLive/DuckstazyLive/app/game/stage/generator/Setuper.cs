using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckstazyLive.app.game;

namespace DuckstazyLive.game.levels.generator
{
    public abstract class Setuper
	{        
        public UserCallback userCallback;
		public abstract Pill start(float x, float y, Pill pill);
	}
}
