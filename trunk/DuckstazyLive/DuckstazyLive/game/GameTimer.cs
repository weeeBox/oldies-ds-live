using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.core;

namespace DuckstazyLive.game
{
	// "тайминг такой ваще крутой нидаибацца" (C)
	public class GameTimer
	{
		
		// прошлое
		private int last;
		
		// миллисекунды
		public int ms;
		// секунды
		public float s;
		
		public int fps;
		private int frames;
		private int framesTime;
		
		public GameTimer()
		{
			reset();
		}
		
		public void reset()
		{
			last = getTimer();
			ms = 1;
			s = 0.001f;
	
			fps = 0;
			frames = 0;
			framesTime = 0;
		}
		
		public void update()
		{
			int now = getTimer();
			       	
			ms = now - last;
			
			if(ms>300)
			{
				ms = 300;
				s = 0.3f;
			}
			else if(ms<=0)
			{
				ms = 1;
				s = 0.001f;
			}
			else
				s = ms*0.001f;
						
			last = now;

			framesTime+=ms;
			frames++;
			
			if(framesTime>1000)
			{
				fps = frames;
				frames = 0;
				framesTime = 0;
			}
		}

        private int getTimer()
        {
            return GameClock.ElapsedTimeMs;
        }
	}
}
