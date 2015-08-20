using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckstazyLive.app;

namespace DuckstazyLive.game
{
    public class Pills
    {
        public const int poolSize = 120;

        public Pill[] pool;

        public Particles ps;
        public PillsMedia media;
        public Heroes heroes;

        public int actives;

        public float harvesting;
        public int harvestCount;

        public Pills(Heroes heroes, Particles particles)
        {            
            this.heroes = heroes;

            // Временные переменные
            int i = poolSize - 1;

            media = new PillsMedia();
            ps = particles;

            // Инициализируем массив(пул) для таблеток
            pool = new Pill[poolSize];
            while (i >= 0)
            {
                pool[i] = new Pill(media, heroes, particles);
                --i;
            }

            clear();

            harvesting = 0.0f;
        }

        public void clear()
        {
            foreach (Pill it in pool)
                it.clear();

            actives = 0;
        }

        public void finish()
        {            
            foreach (Pill p in pool)
            {
                if (p.state != Pill.DEAD)
                {
                    if (p.isPower())
                    {
                        ps.explStarsPower(p.x, p.y, p.id);
                        p.die();                        
                    }
                    else if (p.state == Pill.BORNING || p.state == Pill.ALIVE)
                    {
                        ps.explStarsSleep(p.x, p.y);
                        p.die();                        
                    }
                    actives--;                    
                    if (actives == 0)
                        break;
                }
                
            }
        }

        public void harvest()
        {
            int i = 0;
            int process;

            process = actives;
            harvestCount = 0;
            foreach (Pill p in pool)
            {
                if (i == process)
                    break;

                if (p.state != Pill.DEAD)
                {
                    if (p.isPower())
                    {
                        if (p.state == Pill.BORNING || p.state == Pill.ALIVE)
                            harvestCount++;
                    }
                    else if (p.state == Pill.BORNING || p.state == Pill.ALIVE)
                    {
                        ps.explStarsSleep(p.x, p.y);
                        p.die();
                        actives--;
                    }
                    ++i;
                }
            }
        }

        public void updateHarvest(float dt)
        {
            int i = 0;
            bool to_touch = true;

            harvesting += dt * 8.0f;
            if (harvesting >= 1.0f)
            {
                harvesting -= 1.0f;
                harvestCount = 0;
                if (actives > 0)
                {
                    foreach (Pill p in pool)
                    {
                        if (i == actives)
                            break;

                        if (p.state > Pill.DEAD)
                        {
                            if (p.isPower())
                            {
                                harvestCount++;
                                if (to_touch && p.isAlive())
                                {
                                    Hero hero = p.getClosestHero();
                                    p.heroTouch(hero);
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
            int i = 0;
            int process;

            media.power = power;

            process = actives;
            foreach (Pill p in pool)
            {
                if (i == process)
                    break;

                if (p.state != Pill.DEAD)
                {
                    if (p.update(dt))
                        actives--;
                    ++i;
                }
            }
        }

        public void draw(Canvas canvas)
        {
            int i = 0;

            bool hasBlanc = GameElements.Env.isHitFaded();
            foreach (Pill p in pool)
            {
                if (i == actives)
                    break;

                if (p.state != Pill.DEAD)
                {
                    p.dx = (int)(p.x);
                    p.dy = (int)(p.y);
                    if (p.isPower())
                    {
                        p.drawEmo(canvas);                        
                    }
                    else if (p.type == Pill.JUMP)
                    {
                        p.drawJump(canvas);
                    }
                    else
                    {
                        p.draw(canvas);
                    }

                    if (hasBlanc)
                        p.drawBlanc(canvas);
                    ++i;
                }
            }            
        }

        public Pill findDead()
        {
            foreach (Pill p in pool)
            {
                if (!p.isActive())
                    return p;
            }

            return null;
        }

        public bool isBusy(float x, float y)
        {
            for (int heroIndex = 0; heroIndex < heroes.getHeroesCount(); ++heroIndex)
            {
                if (isBusy(heroes[heroIndex], x, y))
                    return true;
            }

            return false;
        }

        public bool isBusy(Hero hero, float x, float y)
        {
            bool busy = false;
            int i = 0;
            if (utils.vec2distSqr(hero.x + 27.0f, hero.y + 20.0f, x, y) >= 3600.0f)
            {
                foreach (Pill p in pool)
                {
                    if (i == actives)
                        break;

                    if (p.isActive())
                    {
                        if (utils.vec2distSqr(p.x, p.y, x, y) < 900.0f)
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
            for (int heroIndex = 0; heroIndex < heroes.getHeroesCount(); ++heroIndex)
            {
                if (tooCloseHero(heroes[heroIndex], x, y, sqrDist))
                    return true;
            }

            return false;
        }

        public bool tooCloseHero(Hero hero, float x, float y, float sqrDist)
        {
            float dx = x - hero.x - 27;
            float dy = y - hero.y - 20;

            return dx * dx + dy * dy < sqrDist;
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
                            if(p.toxic_warning>0.0f)
                                pick = false;
                            else
                            {
                                if(duck.toxicDamage(p.x, p.y))
                                {
                                    pick = false;
                                    p.kill();
                                    utils.playSound(attack_snd, 1.0f, p.x);
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
