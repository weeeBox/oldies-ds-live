using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuckstazyLive.game
{
public class Pills
	{
		public static const int poolSize = 120;
		
		public static Pills instance;
		
		public Array pool;
	
		public Particles ps;
		public PillsMedia media;
		public Hero hero;
		
		public int actives;
		
		public float harvesting;
		public int harvestCount;

		public Pills(Hero gameHero, Particles particles, Level level)
		{
			instance = this;
			
			// Временные переменные
			int i = poolSize;
			
			media = new PillsMedia();
			ps = particles;
			hero = gameHero;
			
			// Инициализируем массив(пул) для таблеток
			pool = new Array(poolSize);
			while(i>0)
			{
				pool[i] = new Pill(media, gameHero, particles, level);
				--i;
			}
			
			clear();
			
			harvesting = 0.0;
		}
		
		public void clear()
		{			
			foreach (Pill it in pool)
				it.init();
				
			actives = 0;
		}
		
		public void finish()
		{
			int i;
			int process;
						
			process = actives;
			harvestCount = 0;
			foreach (Pill p in pool)
			{
				if(i==process)
					break;
				
				if(p.state!=0)
				{
					if(p.type==0)
					{
						if(p.state==1 || p.state==2)
							harvestCount++;
					}
					else if(p.type==1 || p.type==2)
					{
						ps.explStarsSleep(p.x, p.y);
						p.die();
						actives--;
					}
					++i;
				}
			}
		}
		
		public void harvest(float dt)
		{
			int i = 0;
			bool to_touch = true;
			
			harvesting+=dt*8.0;
			if(harvesting>=1.0)
			{
				harvesting-=1.0;
				harvestCount = 0;
				if(actives>0)
				{
					foreach (Pill p in pool)
					{
						if(i==actives)
							break;
						
						if(p.state>0)
						{
							if(p.type==0)
							{
								harvestCount++;
								if(to_touch && p.state==2)
								{
									p.heroTouch();
									to_touch = false;
								}
							}					
							++i;
						}
					}
				}
			}
		}
		
		public void update(float dt, float power)
		{
			int i;
			int process;
			
			media.power = power;
			
			process = actives;
			foreach (Pill p in pool)
			{
				if(i==process)
					break;
					
				if(p.state)
				{
					if(p.update(dt))
						actives--;
					++i;
				}
			}
		}
		
		public void draw(B canvas)
		{
			int i;
			
			foreach (Pill p in pool)
			{
				if(i==actives)
					break;
				
				if(p.state)
				{
					p.dx = (int)(p.x);
					p.dy = (int)(p.y);
					if(p.type==0)
						p.drawEmo(canvas);
					else if(p.type==5)
						p.drawJump(canvas);
					else
						p.draw(canvas);
						
					++i;
				}
			}
		}

		public Pill findDead()
		{
			* o;
			
			foreach (Pill p in pool)
			{
				if(!p.state)
					return p;
			}
			
			return null;
		}
		
		public bool isBusy(float x, float y)
		{
			bool busy = false;
			int i;
			
			if(utils.vec2distSqr(hero.x+27.0, hero.y+20.0, x, y) >= 3600.0)
			{
				foreach (Pill p in pool)
				{
					if(i==actives)
						break;
						
					if(p.state)
					{
						if(utils.vec2distSqr(p.x, p.y, x, y) < 900.0)
						{
							busy = true;
							break;
						}
						++i;
					}
				}
			}
			else busy = true;
		
			return busy;
		}
		
		public bool tooCloseHero(float x, float y, float sqrDist)
		{
			float dx = x-hero.x-27;
			float dy = y-hero.y-20;
				
			return dx*dx+dy*dy < sqrDist;
		}
			
		/*public int checkDuck(H duck, i startIndex)
		{		
			duck_prev_pos = duck.pos;
			int i = startIndex;
			bool pick;
			
			for(; i<pills_count; ++i)
			{
				Pill p = pool[i];
				if(p.state!=Pill.PILL_DEAD && p.state!=Pill.PILL_DYING)
				{
					if(duck.overlapsCircle(p.x, p.y, p.r))
					{
						pick = true;
						switch(p.type)
						{
						case Pill.PILL_POWER:
							powers_info[p.powerID].count--;
							//mBoard->AddPowerScores(pill_pos, p->GetPowerID());
							break;
						case Pill.PILL_TOXIC:
							if(p.toxic_warning>0.0)
								pick = false;
							else
							{
								if(duck.toxicDamage(p.x, p.y))
								{
									pick = false;
									p.kill();
									utils.playSound(attack_snd, 1.0, p.x);
								}
								--toxic_count;
							}
							break;
						case Pill.PILL_DELAY:
							--delay_count; 
							break;
						}
						if(pick)
							break;
					}
				}
			}
			return i;
		}*/

	}
}
}
