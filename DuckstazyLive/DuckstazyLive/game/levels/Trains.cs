using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckstazyLive.game.levels.generator;
using DuckstazyLive.game.levels.fx;
using DuckstazyLive.app;
using Framework.utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DuckstazyLive.game.levels
{
	public class Trains : LevelStage
	{
		private FrogActor frog;
		private FrogActor cat;
				
		private Generator jumpGen;
		private float catGen;

		private float frogCounter;
		
		public Trains() : base(2)
		{			
			goalTime = 100.0f;
			
			frog = new FrogActor(media);
			
			frog.speedHands = 2.0f;
			
			frog.x = 10.0f;
			frog.y = 253;
		}
		
		public override void start()
		{
			//var placer:Placer;
			JumpStarter jumper = new JumpStarter();
			
			base.start();
			
			jumpGen = new Generator();
			jumpGen.regen = false;
			jumpGen.speed = 4.0f;
			
			jumpGen.map.Add(new Placer(jumper, 160.0f, 370.0f));
			jumpGen.map.Add(new Placer(jumper, 480.0f, 370.0f));
			
			jumpGen.map.Add(new Placer(jumper, 200.0f, 340.0f));
			jumpGen.map.Add(new Placer(jumper, 440.0f, 340.0f));
			
			jumpGen.map.Add(new Placer(jumper, 240.0f, 310.0f));
			jumpGen.map.Add(new Placer(jumper, 400.0f, 310.0f));
			
			jumpGen.map.Add(new Placer(jumper, 280.0f, 280.0f));
			jumpGen.map.Add(new Placer(jumper, 360.0f, 280.0f));
			
			jumpGen.map.Add(new Placer(jumper, 320.0f, 250.0f));

			jumpGen.start();
			
			env.day = false;
			env.updateNorm();
			
			frog.openCounter = 0.0f;
			frog.open = true;
			frogCounter = 1.0f;
			
			catGen = 1.0f;
			
			startX = 293;
		}
		
		public override void onWin()
		{
			jumpGen.finish();
			
		}
		
		public override void update(float dt)
		{			
			int i = 0;
			Pill p;
			int newPills = 0;
			
			base.update(dt);
			
			jumpGen.update(dt);

			frog.update(dt);
			
			if(frog.open && frog.openCounter>=0.5f && !win)
			{
				frogCounter+=dt*(0.5f+level.power*2.0f);
				if(frogCounter>=1.0f)
				{
					p = pills.findDead();
					if(p!=null)
					{
						p.user = toxicLogic;
						if(utils.rnd()>0.2f)
							p.startMissle(80, 260, 1);
						else
							p.startSleep(80, 260);
						newPills++;
						
					}
					frogCounter-=(int)(frogCounter);
				}
			}
			else frogCounter = 1.0f;
			
			frog.speedHands = 2.0f+level.power*2.0f;
			
			if(level.power>=0.5f && !win)
			{
				catGen+=dt*(0.25f+0.75f*(level.power-0.5f));
				if(catGen>1)
				{
					p = pills.findDead();
					if(p!=null)
					{
						p.user = rocketLogic;
						p.startMissle(548, 228, 0);
						p.t2 = 0.1f;
						catGen -= 1.0f;
						newPills++;
					}
				}
			}
			else catGen = 1.0f;
			
			frog.open = (hero.y<=250 && !win);
			
			pills.actives+=newPills;
		}
		
		public override void draw1(Canvas canvas)
		{
			Rect rc = new Rect();
			Vector2 p = new Vector2();
			int bm;			

			bm = media.imgCatL;
            Texture2D bmTex = utils.getImage(bm);
			rc.Width = bmTex.Width;
			rc.Height = bmTex.Height;
			p.X = 495;
			p.Y = 140;
			canvas.copyPixels(bm, rc, p);
			
			if(catGen<0.5f)
			{
				bm = media.imgCatHum;
				rc.Width = bmTex.Width;
				rc.Height = bmTex.Height;
				p.X = 533;
				p.Y = 212;
				canvas.copyPixels(bm, rc, p);
			}
			else
			{
				bm = media.imgCatSmile;
				rc.Width = bmTex.Width;
				rc.Height = bmTex.Height;
				p.X = 533;
				p.Y = 219;
				canvas.copyPixels(bm, rc, p);
			}

			
			frog.draw(canvas);
			
			bm = media.imgPedestalL;
			rc.Width = bmTex.Width;
			rc.Height = bmTex.Height;
			p.X = -27;
			p.Y = 400-115;
			canvas.copyPixels(bm, rc, p);
			
			bm = media.imgPedestalR;
			rc.Width = bmTex.Width;
			rc.Height = bmTex.Height;
			p.X = 432;
			p.Y = 400-113;
			canvas.copyPixels(bm, rc, p);
		}
		
		public void toxicLogic(Pill pill, String msg, float dt)
		{
			int i;
			Pill p;
			if(msg==null && pill.state==2)
			{
				if(pill.x>=630 || pill.x<=10)
				{
					pill.kill();
					if(pill.type==1)
						particles.explStarsToxic(pill.x, pill.y, 1, false);
					else if(pill.type==2)
						particles.explStarsSleep(pill.x, pill.y);
				}
				else
					pill.x+=pill.t1*(1.0f+4.0f*level.power)*dt;
			}
			else if(msg=="attack")
			{
				i = 1 + (int)(level.power*5);
				while(i>0)
				{
					p = pills.findDead();
					if(p!=null)
					{
						p.user = partyLogic;
						p.startPower(pill.x, pill.y, (int)(utils.rnd()*3), false);
						pills.actives++;
					}
					--i;
				}
			}
			else if(msg=="born")
			{
				if(pill.x>320) pill.t1 = -40;
				else pill.t1 = 40;
			}
		}
				
		public void partyLogic(Pill pill, String msg, float dt)
		{
			float pow = level.power;
			float friction = 0.8f-pow*0.1f;
			float dx;
			float dy;
			
			if(msg==null && pill.enabled)
			{
				dx = hero.x - pill.x + 27;
				dy = hero.y - pill.y + 20;
                pill.vy += (300.0f + dy * 10) * dt;
				pill.vx += dx*5*dt;
				pill.x+=pill.vx*dt;
				pill.y+=pill.vy*dt;
				
				pill.t2-=dt;
				
				if(pill.t2<0.0f)
				{
					pill.t2 = 0.05f;
					particles.startStarPower(pill.x,  pill.y, -pill.vx, -pill.vy, pill.id);
				}
				
				if(pill.x > 630)
				{
					pill.vx = -pill.vx*friction;
					pill.vy = pill.vy*friction;
					pill.x = 630;
				}
				if(pill.x < 10)
				{
					pill.vx = -pill.vx*friction;
					pill.vy = pill.vy*friction;
					pill.x = 10;
				}
				
				if(pill.y < 10)
				{
					pill.vy = -pill.vy*friction;
					pill.vx = pill.vx*friction;
					pill.y = 10;
				}
				if(pill.y > 390)
				{
					pill.vy = -pill.vy*friction;
					pill.vx = pill.vx*friction;
					pill.y = 390;
				}
			}
			else if(msg=="born")
			{
				pill.vx = (150.0f+150.0f*pow)*(utils.rnd()*2.0f-1.0f);
				pill.vy = 75.0f+ utils.rnd()*50.0f;
				pill.t2 = 0.05f;
			}
		}
		
		public void rocketLogic(Pill pill, String msg, float dt)
		{
			int i;
			Pill p;
			float pow = level.power;
			if(msg==null && pill.state==2)
			{
				if(pow>=0.5f)
				{
					pill.t2-=dt;
					
					if(pill.t2<0.0f)
					{
						pill.t2 = 0.1f;
						particles.startStarToxic(pill.x+12,  pill.y, 100*pow, 0, 0);
					}
					
					if(pill.x<=10)
					{
						pill.kill();
						particles.explStarsToxic(pill.x, pill.y, 0, true);
					}
					else
					{
						pill.x-=100*pow*dt;
					}
				}
			}
			else if(msg=="attack")
			{
				i = 1 + (int)(pow*5);
				while(i>0)
				{
					p = pills.findDead();
					if(p!=null)
					{
						p.user = partyLogic;
						p.startPower(pill.x, pill.y, (int)(utils.rnd()*3), false);
						pills.actives++;
					}
					--i;
				}
			}
		}
	}
}
