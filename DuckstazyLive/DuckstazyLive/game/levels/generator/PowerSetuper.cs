using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckstazyLive.app;

namespace DuckstazyLive.game.levels.generator
{
	public class PowerSetuper : Setuper
	{
		public static int[] POWER1 = new int[] {0};
		public static int[] POWER2 = new int[] {1};
		public static int[] POWER3 = new int[] {2};
		public static int[] POWERS = new int[] {0,1,2};
		
		public int[] ids;
		public float probJump;
		
		public PowerSetuper(float jump, int[] powerIDs)
		{
			ids = powerIDs;
			probJump = jump;
		}

        public override Pill start(float x, float y, Pill pill)
		{
			int lenght = ids.Length;
			int id;
			if(lenght>0)
			{
				if(lenght>1) id = ids[(int)(utils.rnd()*lenght)];
				else id = ids[0];
			}
			else id = 0;

			pill.user = userCallback;
			pill.startPower(x, y, id, utils.rnd()<probJump);
					
			return pill;
		}
	}
}
