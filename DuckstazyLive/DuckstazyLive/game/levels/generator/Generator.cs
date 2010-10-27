using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuckstazyLive.game.levels.generator
{
    public class Generator
	{
		public List<Placer> map; // карта генератора
		public List<Pill> pills; // массив таблов
		
		public int pillsCount; // текущее кол-во живых таблов
		public bool begining; // начало генерации
		public bool finished; // все таблы убиты, не генерируем
		
		private Pills pillsMan;
				
		public bool regen; // регенерировать таблы бесконечно
		
		public float speed; // скорость генерации таблов
		public float counter; // счётчик генерации 0 -> 1
		
		private int mapPointer;
		
		public float heroSqrDist; // минимальный квадрат расстояния до утки при регенерации
		
		public Generator()
		{
			counter = 0.0f;
			speed = 4.0f;
			mapPointer = 0;
			heroSqrDist = 1200;
			
			regen = true;
			
			pillsMan = Pills.instance;
			map = new List<Placer>();
            pills = new List<Pill>();
		}
		
		public void start()
		{
			clearPills();
			mapPointer = 0;
			counter = 0.0f;
			pillsCount = 0;
			begining = true;
			finished = false;
		}
		
		public void finish()
		{
			foreach(Pill it in pills)
			{
				if(it!=null)
					it.kill();
			}
					
			mapPointer = map.Count;
			pillsCount = 0;
			finished = true;
		}
		
		public void clearMap()
		{
            map.Clear();
			mapPointer = 0;
		}
		
		public void clearPills()
		{
			if(pills.Count>0)
			{
				foreach(Pill it in pills)
				{
					if(it!=null)
						it.kill();
				}

                pills.Clear();
				pillsCount = 0;
			}
		}
		
		public void update(float dt)
		{			
			int i;
			Pill p;
			int news = 0;
			
			counter+=speed*dt;
			if(!finished && counter>1.0f)
			{
				if(pills.Count<map.Count)
				{
					if((p = pillsMan.findDead())!=null)
					{
						map[mapPointer].place(p);
						pills.Add(p);
						p.parent = parentCallback;
						++news;
						++mapPointer;
					}
				}
				else
				{
					begining = false;
					if(regen)
					{
						i = 0;
						// foreach(Pill it in pills)
                        for (i = 0; i < pills.Count; ++i)
                        {
                            Pill it = pills[i];
                            if (it == null)
                            {
                                it = pillsMan.findDead();
                                if (it != null)
                                {
                                    if (map[i].placeAvoidHero(it, heroSqrDist) != null)
                                    {
                                        it.parent = parentCallback;
                                        pills[i] = it;
                                        ++news;
                                        break;
                                    }
                                }
                            }                            
                        }
					}
				}
				counter-=(int)(counter);
				pillsMan.actives+=news;
				pillsCount+=news;
			}
		}
		
		public void parentCallback(Pill pill)
		{
			int i = 0;
			
			foreach(Pill it in pills)
			{
				if(it==pill)
				{
					pills[i] = null;
					--pillsCount;
					break;
				}
				++i;
			}
		}
		
		public void addCircle(Setuper setuper, float cx, float cy, float r, int count, float da, float a0)
		{
			int i = count;
			float a = a0;
			
			if(i==0) i = (int)(6.28f/da);
			while(i>0)
			{
				map.Add(new Placer(setuper, (float)(cx + r*Math.Cos(a)), (float)(cy + r*Math.Sin(a))));
				--i;
				a+=da;
			}
		}
		
		public void addLine(Setuper setuper, float ox, float oy, float dx, float dy, int count)
		{
			int i = count;
			float x = ox;
			float y = oy;

			while(i>0)
			{
				map.Add(new Placer(setuper, x, y));
				x+=dx;
				y+=dy;
				--i;
			}
		}
	}
}
