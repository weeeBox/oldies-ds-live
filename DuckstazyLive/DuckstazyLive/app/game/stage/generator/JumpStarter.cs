using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckstazyLive.app.game;

namespace DuckstazyLive.game.levels.generator
{
    public class JumpStarter : Setuper
	{
		public JumpStarter() { }

		public override Pill start(float x, float y, Pill pill)
		{
			pill.user = userCallback;
			pill.startJump(x, y);
			return pill;
		}
		
	}

}
