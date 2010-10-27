using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckstazyLive.app;
using DuckstazyLive.game.levels.generator;

namespace DuckstazyLive.game.levels
{
    public class PartyTime : LevelStage
	{
		public Generator gen;
		public PartySetuper setuper;
		
		private int danger;
	
		public PartyTime(float goalTime, int danger) : base(1)
		{			
			this.goalTime = goalTime;
			this.danger = danger;
		}
		
		public override void start()
		{
			float y = 350.0f;
			float x0 = 40.0f;
			float dx = 110.0f;
            float dy = 30.0f;
			
			base.start();
			
			setuper = new PartySetuper();
			if(danger==0)
			{
				//setuper.sleeps = 1.0;
				//setuper.toxics = 1.0;
				//setuper.sleeps = 1.0;
				setuper.dangerH = 0.0f;
				setuper.jump = 0.1f;
			}
			else if(danger==1)
			{
				setuper.powers = 0.8f;
				setuper.sleeps = 0.9f;
				setuper.toxics = 1.0f;
			
				setuper.dangerH = 200.0f;
				
				setuper.jump = 0.1f;
			}
			else if(danger==2)
			{
				setuper.powers = 0.6f;
				setuper.sleeps = 0.8f;
				setuper.toxics = 1.0f;
			
				setuper.dangerH = 300.0f;
				
				setuper.jump = 0.1f;
			}
			gen = new Generator();
			gen.regen = true;
			gen.speed = 8.0f;
			
			
			while(y>=50)
			{
				gen.addLine(setuper, x0+dx*0.5f, y, dx, 0, 5);
				gen.addLine(setuper, x0, y-dy, dx, 0, 6);
				y-=dy*2;
			}
			
			gen.start();
			
			startX = 20.0f + 600.0f*utils.rnd();
			//startPause = true;
		}

        public override void onWin()
		{
			//gen.finish();
			gen.regen = false;
		}
		
		override public void update(float dt)
		{		
			int i = 0;
			
			base.update(dt);
		
			if(gen.speed>2.0f)
			{
				gen.speed-=dt*0.5f;
				if(gen.speed<2.0f) gen.speed = 2.0f;
			}
				
			gen.update(dt);							
		}

        public override void draw1(Canvas canvas)
		{
	
		}		
	}
}
