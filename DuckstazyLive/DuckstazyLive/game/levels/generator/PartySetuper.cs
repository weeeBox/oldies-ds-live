using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckstazyLive.app;

namespace DuckstazyLive.game.levels.generator
{
    public class PartySetuper : Setuper
	{
		public float powers;
		public float sleeps;
		public float toxics;
		
		public float dangerH;
		
		public float jump;
		
		public PartySetuper()
		{
			powers = 0.8f;
			sleeps = 0.9f;
			toxics = 1.0f;
			
			dangerH = 200.0f; 
			
			jump = 0.0f;
		}
		
		public override Pill start(float x, float y, Pill pill)
		{
			float t = utils.rnd();
			pill.user = userCallback;
			
			if(t<powers || y>dangerH)
				pill.startPower(x, y, (int)(utils.rnd()*3), utils.rnd()<jump);
			else if(t<sleeps)
				pill.startSleep(x, y);
			else
				pill.startToxic(x, y, (int)(utils.rnd()*2));
				
			return pill;
		}		
	}
}
