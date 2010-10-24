using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.utils;
using Microsoft.Xna.Framework;
using DuckstazyLive.app;
using Framework.core;

namespace DuckstazyLive.game
{
    public class Pill
    {
        public const int POWER = 0;
        public const int TOXIC = 1;
        public const int SLEEP = 2;
        public const int HEALTH = 3;
        public const int MATRIX = 4;
        public const int JUMP = 5;

        /*public const int DEAD = 0;
        public const int BORNING = 1;
        public const int ALIVE = 2;
        public const int DYING = 3;*/

        /*public const int HAPPY = 0;
        public const int SHAKE = 1;
        public const int SMILE = 2;*/

        private Rect RC = new Rect(0, 0, 20, 20);
        private Point POINT = new Point(0, 0);
        private ColorTransform COLOR = new ColorTransform();
        private ColorTransform BLACK = new ColorTransform(0, 0, 0);
        private Matrix MAT = new Matrix();


        // временный идентификатор
        public int id;

        // Прирост силы от таблетки
        public float power;

        // Урон от таблетки
        public int damage;

        // Очки за таблетку при максимальной силе
        public int scores;

        // Время предупреждения
        public float warning;
        public bool enabled;

        // захват героя
        public bool hook;
        private float hookTime;
        private float hookCounter;

        // Направление на героя
        public bool spy;
        public float hx;
        public float hy;

        // Счётчик появления
        private float appear;

        public bool emo;
        private int emoType;
        private float emoCounter;
        private float emoPause;
        private float emoParam;

        // состояние табла
        public int state;

        public float x;
        public float y;

        // округлённые координаты
        public float dx;
        public float dy;

        // временные вспомогательные переменные, используемые только внешними классами
        public float t1;
        public float t2;

        public float r;
        public float rMax;

        public bool move;
        public float v;
        public float vx;
        public float vy;

        public bool high;
        public float highCounter;

        public int type;

        private PillsMedia media;
        private Particles ps;
        private Hero hero;
        private Level level;

        private int imgMain;
        private int imgEmo;
        private int imgNid;

        // используется для оповещения генератора-родителя
        public PillListener2 parent;

        // используется для оповещения пользовательских событий
        public PillListener user;

        // Инициализируемся в массиве
        public Pill(PillsMedia pillsMedia, Hero duckHero, Particles particles, Level _level)
        {
            media = pillsMedia;
            hero = duckHero;
            ps = particles;
            level = _level;
            init();
        }

        // Сбрасываемся
        public void init()
        {
            state = 0;
            move = false;
            highCounter = 0.0f;

            // parent = null;
            // user = null;

            /*emoCounter = 0.0f;
            emoPause = 0.0f;
            hook = false;
			
            emo = false;
						
            vx = 0.0f;
            vy = 0.0f;
			
            hx = 0.0f;
            hy = 0.0f;*/
        }

        // Стартуем анимацию эмоции
        private void startEmo(int emotionType)
		{
			switch(emotionType)
			{
                case 0:
                    {
                        emoParam = 1 + (int)(utils.rnd() * 3.0);
                        if (utils.rnd() < 0.5f)
                            emoParam = -emoParam;
                        emoCounter = 3.0f;
                    }
                    break;
			    case 1:
			    case 2:
				    emoCounter = 3.0f;
				    break;
			}
			emoType = emotionType;
		}

        // Обновляем появление
        private void setState(int newState)
        {
            switch (newState)
            {
                case 0:
                    if (user != null)
                    {
                        user.foo(this, "dead", 0.0f);
                        user = null;
                    }
                    if (parent != null)
                    {
                        parent.foo(this);
                        parent = null;
                    }
                    move = false;
                    break;
                case 1:
                    if (user != null)
                        user.foo(this, "born", 0.0f);
                    appear = 0.0f;
                    r = 0.0f;
                    break;
                case 2:
                    appear = 1.0f;
                    r = rMax;
                    break;
            }

            state = newState;
        }

        // Обновляемся
        public bool update(float dt)
		{
			if(state!=3 && enabled && hero.state.health>0)
			{
				if(y+r > hero.y || y-r < hero.y + 40)
					if(x+r > hero.x || x-r < hero.x + 54)
						if(hero.overlapsCircle(x, y, r))
							heroTouch();
			}
			
			switch(state)
			{
			case 1:
				if(!enabled)
				{
					warning-=dt;
					if(warning<=0.0f)
					{
						enabled = true;
						ps.startAcid(x, y);
						if(type==TOXIC)
							utils.playSound(media.sndToxicBorn, 1.0f, x);
					}
				}
				else
				{
					appear+=10*dt;
					r = rMax*appear;
					if(appear>=1.0f)
						setState(2);	
				}
				break;
			case 2:
				if(spy)
					updateSpy();
					
				if(hook)
					updateHook(dt);
					
				if(move) { x += vx*dt; y += vy*dt; }
				
				if(emo && media.power>0.5f)
				{
					if(emoCounter>0.0f)
					{
						emoCounter -= dt;
						if(emoCounter<0.0f)
						{
							emoCounter = 0.0f;
							emoPause = utils.rnd()*3.0f+2.0f;
						}
					}
					else
					{
						emoPause-=dt;
						if(emoPause<=0.0f)
							startEmo((int)(utils.rnd()*3.0));
					}
				}
					
				if(high)
				{
					if(level.power>=0.5)
						highCounter+=dt;//*(1.0f + 3.0*level.power);
					else
						highCounter+=dt*(1.0f + 7.0f*level.power);
					if(highCounter>=1.0f)
						highCounter-=(int)(highCounter);
				}
				
				if(type==JUMP && highCounter>0.0f)
				{
					highCounter-=dt;
					if(highCounter<0.0f) highCounter = 0.0f;
				}
				
				break;
			case 3:
				appear-=10*dt;
				if(appear<=0.0f)
					setState(0);
				break;
			}
			
			if(user!=null)
				user.foo(this, null, dt);
				
			return state==0;
		}

        public void updateSpy()
        {
            float dx = hero.x - x + 27.0f;
            float dy = hero.y - y + 20.0f;
            float i = (float)(1.0 / Math.Sqrt(dx * dx + dy * dy));

            hx = dx * i;
            hy = dy * i;
        }

        public void heroTouch()
		{
			int i;
			GameInfo info = level.info;
						
			switch(type)
			{
			case POWER:
				if(!hero.sleep)
					level.gainPower(power);
				if(level.power>=0.5)
				{
					i = id+level.state.hell;
					level.state.scores+=level.state.calcHellScores(i);
					//else if(i==1) level.state.scores+=10;
					//else if(i==2) level.state.scores+=25;
					info.add(x, y, info.powers[i]);
					level.env.beat();
				}
				else
				{
					i = level.state.norm;
					if(i==0)
					{
						level.state.scores++;
						info.add(x, y, info.one);
					}
					else
					{
						info.add(x, y, info.powers[i-1]);
						level.state.scores+=level.state.calcHellScores(i-1);
					}
				}
				utils.playSound(media.sndPowers[id], 1.0f, x);
				
				
				if(high && hero.doHigh(x, y))
				{
					// media.sndHigh.play();                    
					ps.explStarsPower(x, y-r, id);
                    throw new NotImplementedException();
				}
				else
					ps.explStarsPower(x, y, id);
					
				level.stage.collected++;
					
				break;
			case TOXIC:
				i = hero.doToxicDamage(x, y, damage, id);
				if(i>=0)
				{
					if(level.power>=0.5)
					{
						if(i==0) level.state.scores+=100;
						else if(i==1) level.state.scores+=150;
						else if(i==2) level.state.scores+=200;
						info.add(x, y, info.toxics[i]);
						level.env.beat();
					}
					else
					{
						if(i==0) level.state.scores+=5;
						else if(i==1) level.state.scores+=10;
						else if(i==2) level.state.scores+=25;
						info.add(x, y, info.powers[i]);
					}
					if(user!=null)
						user.foo(this, "attack", 0);
				}
				else info.add(x, y, info.damages[(int)(utils.rnd()*3.0)]);
				break;
			case SLEEP:
				//--delay_count;
				if(!hero.sleep)
				{
					hero.doSleep();
					level.gainSleep();
					level.env.beat();
				}
				ps.explStarsSleep(x, y);
				info.add(x, y, info.sleeps[(int)(utils.rnd()*3.0)]);
				break;
			case HEALTH:
				// media.sndHeal.play();
                Application.sharedSoundMgr.playSound(media.sndHeal);
				hero.doHeal(5);
				level.env.beat();
				break;
			case MATRIX:
				level.switchEvnPower();
				level.env.beat();
				break;
			case JUMP:
				if(highCounter<=0.0f && hero.doHigh(x, y))
				{
					// media.sndJumper.play();
                    Application.sharedSoundMgr.playSound(media.sndJumper);
					highCounter = 1.0f;
					level.env.beat();
					if(user!=null)
						user.foo(this, "jump", 0.0f);
				}
				return;
			}
			
			kill();
		
		}

        public void startPower(float px, float py, int ID, bool h)
		{
			x = (int)(px);
			y = (int)(py);
			type = POWER;
			
			switch(ID)
			{
			case 0:
				scores = 5;
				power = 0.01f;
				imgMain = media.imgPower1;
				imgEmo = media.imgPPower1;
				break;
			case 1:
				scores = 25;
				power = 0.025f;
				imgMain = media.imgPower2;
				imgEmo = media.imgPPower2;
				break;
			case 2:
				scores = 50;
				power = 0.05f;
				imgMain = media.imgPower3;
				imgEmo = media.imgPPower3;
				break;
			}
			
			rMax = 10.0f;
			
			damage = 0;
			
			id = ID;
			
			emo = true;
			emoPause = utils.rnd()*3.0f+2.0f;
			emoCounter = 0.0f;
			hx = 0.0f;
			hy = 0.0f;
			
			imgNid = media.imgNids[(int)(utils.rnd()*4)];
			
			spy = true;
			
			hook = false;
			
			high = h;
						
			enabled = true;
			
			setState(1);
			
			ps.startAcid(x, y);
			utils.playSound(media.sndGenerate, 1.0f, x);
		}

        public void startJump(float px, float py)
		{
			x = (int)(px);
			y = (int)(py);
			type = JUMP;
			
			imgMain = media.imgHigh;
			
			rMax = 10.0f;
			
			damage = 0;

			spy = false;
			hook = false;
			high = false;
			enabled = true;
			
			setState(1);
			
			highCounter = 0.0f;
			
			ps.startAcid(x, y, 0xffffffff);
			utils.playSound(media.sndGenerate, 1.0f, x);
		}

        public void startMatrix(float px, float py)
		{
			x = (int)(px);
			y = (int)(py);
			type = MATRIX;
			
			imgMain = media.imgHole;
			//imgNid = null;
		
			rMax = 10.0f;
			
			
			spy = false;
			hook = false;
			high = false;
						
			enabled = true;
			
			setState(1);
			
			ps.startAcid(x, y);
			utils.playSound(media.sndGenerate, 1.0f, x);
		}

        public void startToxic(float px, float py, int ID)
		{
			x = (int)(px);
			y = (int)(py);
			type = TOXIC;
						
			switch(ID)
			{
			case 0:
				damage = 20;
				imgMain = media.imgToxic;
				hook = true;
				hookTime = 3.0f;
				hookCounter = 0.0f;
				break;
			case 1:
				damage = 20;
				imgMain = media.imgToxic2;
				hook = false;
				break;
			}
			
			id = ID;
			
			warning = 3.0f;
			enabled = false;
						
			spy = false;
			
			rMax = 10.0f;
			
			v = 20.0f;
			
			emo = false;
			
			high = false;
			
			setState(1);
			if(level.power<0.5f && !level.env.day)
				ps.startWarning(x, y, 3.0f, 1.0f, 1.0f, 1.0f);
			else
				ps.startWarning(x, y, 3.0f, 0.0f, 0.0f, 0.0f);
			// media.sndWarning.play();
            Application.sharedSoundMgr.playSound(media.sndWarning);
		}

        public void startMissle(float px, float py, int ID)
		{
			x = (int)(px);
			y = (int)(py);
			type = TOXIC;
						
			switch(ID)
			{
			case 0:
				damage = 20;
				imgMain = media.imgToxic;
				break;
			case 1:
				damage = 20;
				imgMain = media.imgToxic2;
				break;
			}
			
			hook = false;
			
			id = ID;
			
			enabled = true;
						
			spy = false;
			
			rMax = 10.0f;
			
			v = 20.0f;
			
			emo = false;
			
			high = false;
			
			setState(1);
		}

        public void startSleep(float px, float py)
		{
			x = (int)(px);
			y = (int)(py);
			type = SLEEP;
			
			emo = false;
			spy = false;
			hook = false;
			high = false;
			enabled = true;
						
			rMax = 10.0f;
						
			imgMain = media.imgSleep;

			setState(1);
			
			ps.startAcid(x, y);
			utils.playSound(media.sndGenerate, 1.0f, x);
		}

        public void startCure(float px, float py)
		{
			x = (int)(px);
			y = (int)(py);
			type = HEALTH;
			
			//damage = 5;
			
			emo = false;
			spy = false;
			hook = false;
			high = false;
			enabled = true;
						
			rMax = 10.0f;
						
			imgMain = media.imgCure;

			setState(1);
			
			ps.startAcid(x, y);
			utils.playSound(media.sndGenerate, 1.0f, x);
		}

        public void kill()
        {
            setState(3);
            appear = 0.5f;
        }

        public void die()
        {
            setState(0);
        }

        public void updateHook(float dt)
        {
            if (hookCounter > 0.0f)
            {
                hookCounter -= dt;
                if (hookCounter > 0.0f)
                {
                    updateSpy();
                    vx = hx * v;
                    vy = hy * v;
                }
                else
                {
                    // хук закончился
                    move = false;

                    // въебать эффект
                    ps.startRing(x, y, -1.0f, 0.25f, 0.25f, 0xff000000);
                    //mWaves->Start(world_draw_pos(p->GetPos()), 0xff000000, 30.0f, 5.0f);
                }
            }
            else
            {
                if (utils.vec2distSqr(x, y, hero.x + 27, hero.y + 20) < 10000)
                {
                    hookCounter = hookTime;
                    move = true;
                    updateSpy();
                    vx = hx * v;
                    vy = hy * v;

                    // въебать эффект
                    //mWaves->Start(worl_draw_pos(p->GetPos()), 0xff000000, 30.0f, 5.0f);
                    ps.startRing(x, y, 1.0f, 0.25f, 0.125f, 0xff000000);
                }
            }
        }

        public void drawEmo(Canvas canvas)
        {
            //if (media.power < 0.5)
            //{
            //    if (state != 2)
            //    {
            //        MAT.identity();
            //        MAT.tx = MAT.ty = -10;
            //        MAT.scale(appear, appear);
            //        MAT.translate(dx, dy);
            //        canvas.draw(imgMain, MAT, null, null, null, true);
            //        canvas.draw(imgNid, MAT, null, null, null, true);
            //    }
            //    else
            //    {
            //        POINT.x = dx - 10;
            //        POINT.y = dy - 10;
            //        canvas.copyPixels(imgMain, RC, POINT);
            //        canvas.copyPixels(imgNid, RC, POINT);
            //    }
            //}
            //else
            //{
            //    if (state != 2)
            //    {
            //        MAT.identity();
            //        MAT.tx = MAT.ty = -10;
            //        MAT.scale(appear, appear);
            //        MAT.translate(dx, dy);
            //        canvas.draw(imgEmo, MAT, null, null, null, true);
            //    }
            //    else
            //    {
            //        POINT.x = dx - 10;
            //        POINT.y = dy - 10;
            //        canvas.copyPixels(imgEmo, RC, POINT);
            //    }

            //    if (emoCounter > 0.0f)
            //    {
            //        switch (emoType)
            //        {
            //            case 0:
            //                drawEmoHappy(canvas);
            //                break;
            //            case 1:
            //                drawEmoShake(canvas);
            //                break;
            //            case 2:
            //                drawEmoSmile(canvas);
            //                break;
            //        }
            //    }
            //    else
            //        drawNid(canvas);
            //}

            //if (high && highCounter > 0.5 && state == 2)
            //{
            //    MAT.identity();
            //    MAT.tx = dx - 12;
            //    MAT.ty = dy - 12;
            //    COLOR.alphaMultiplier = 2.0 - highCounter * 2.0;
            //    canvas.draw(media.imgHigh, MAT, COLOR, null, null, false);
            //}
            throw new NotImplementedException();
        }

        public void draw(Canvas canvas)
        {
            //if (state != 2)
            //{
            //    MAT.identity();
            //    MAT.tx = MAT.ty = -10;
            //    MAT.scale(appear, appear);
            //    MAT.translate(dx, dy);
            //    canvas.draw(imgMain, MAT, null, null, null, true);
            //}
            //else
            //{
            //    POINT.x = dx - 10;
            //    POINT.y = dy - 10;
            //    canvas.copyPixels(imgMain, RC, POINT);
            //}
            throw new NotImplementedException();
        }

        public void drawJump(Canvas canvas)
        {
            //float s = 0.8 + 0.4 * Math.sin(highCounter * 1.57);

            //if (state != 2)
            //    s *= appear;

            //MAT.identity();
            //MAT.tx = MAT.ty = -12;
            //MAT.scale(s, s);
            //MAT.translate(x, y);

            //if (level.power >= 0.5)
            //{
            //    canvas.draw(imgMain, MAT, null, BlendMode.INVERT, null, true);
            //}
            //else
            //{
            //    if (level.env.day)
            //        canvas.draw(imgMain, MAT, BLACK, null, null, true);
            //    else
            //        canvas.draw(imgMain, MAT, null, null, null, true);
            //}
            throw new NotImplementedException();
        }

        private void drawNid(Canvas canvas)
        {
            //MAT.identity();
            //MAT.tx = -4;
            //MAT.ty = 2;
            //MAT.scale(appear, appear);
            //MAT.translate(dx, dy);

            //canvas.draw(media.imgSmile1, MAT, null, null, null, true);

            //MAT.identity();
            //MAT.tx = -5;
            //MAT.ty = -3;
            //MAT.scale(appear, appear);
            //MAT.translate(dx + hx, dy + hy);

            //canvas.draw(media.imgEyes1, MAT, null, null, null, true);
            throw new NotImplementedException();
        }

        private void drawEmoDefault(Canvas canvas, float alpha, float angle)
        {
            //COLOR.alphaMultiplier = alpha;

            //MAT.identity();
            //MAT.tx = -4;
            //MAT.ty = 2;
            //MAT.rotate(angle);
            //MAT.scale(appear, appear);
            //MAT.translate(dx, dy);

            //canvas.draw(media.imgSmile1, MAT, COLOR, null, null, true);

            //MAT.identity();
            //MAT.tx = -5;
            //MAT.ty = -3;
            //MAT.rotate(angle);
            //MAT.scale(appear, appear);
            //MAT.translate(dx + hx, dy + hy);

            //canvas.draw(media.imgEyes1, MAT, COLOR, null, null, true);
            throw new NotImplementedException();
        }

        private void drawEmoHappy(Canvas canvas)
        {
            ////var mat:Matrix = new Matrix(1.0f, 0.0f, 0.0f, 1.0f, -6.0, 1.0f);
            //ColorTransform col;
            //float a = 0.5;
            //float ang = emoCounter / 3.0;

            //if (emoCounter > 2.5) a = 3.0 - emoCounter;
            //else if (emoCounter < 0.5) a = emoCounter;
            //a *= 2.0;

            //if (emoParam > 0.0f)
            //    ang = 1.0f - ang;

            //ang *= Math.abs(emoParam) * 6.28;

            //if (a < 1.0f) drawEmoDefault(canvas, 1.0f - a, ang);
            //COLOR.alphaMultiplier = a;

            //MAT.identity();
            //MAT.tx = -6;
            //MAT.ty = 1;
            //MAT.rotate(ang);
            //MAT.scale(appear, appear);
            //MAT.translate(dx, dy);

            //canvas.draw(media.imgSmile3, MAT, COLOR, null, null, true);

            //MAT.identity();
            //MAT.tx = -7;
            //MAT.ty = -5;
            //MAT.rotate(ang);
            //MAT.scale(appear, appear);
            //MAT.translate(dx, dy);

            //canvas.draw(media.imgEyes2, MAT, COLOR, null, null, true);
            throw new NotImplementedException();
        }

        private void drawEmoShake(Canvas canvas)
        {
            ////var mat:Matrix = new Matrix(1.0f, 0.0f, 0.0f, 1.0f, -6.0, 1.0f);
            ////var col:ColorTransform;
            //float a = 0.5;
            //float off = Math.sin(emoCounter * 6.28);

            //if (emoCounter > 2.5) a = 3.0 - emoCounter;
            //else if (emoCounter < 0.5) a = emoCounter;
            //a *= 2.0;

            //if (a < 1.0f) drawEmoDefault(canvas, 1.0f - a, 0.0f);
            //COLOR.alphaMultiplier = a;

            //if (off < 0.0f)
            //    off = 0.0f;
            //else if (off >= 0.0f)
            //    off = 0.5;

            //MAT.identity();
            //MAT.tx = -6;
            //MAT.ty = 1;
            //MAT.scale(appear, appear);
            //MAT.translate(dx, dy);



            //canvas.draw(media.imgSmile3, MAT, COLOR, null, null, true);

            //MAT.identity();
            //MAT.tx = -7;
            //MAT.ty = -5;
            //MAT.scale(appear, appear);
            //MAT.translate(dx, dy + off);

            //canvas.draw(media.imgEyes2, MAT, COLOR, null, null, true);
            throw new NotImplementedException();
        }

        private void drawEmoSmile(Canvas canvas)
        {
            //float a = 0.5;

            //if (emoCounter > 2.5) a = 3.0 - emoCounter;
            //else if (emoCounter < 0.5) a = emoCounter;
            //a *= 2.0;

            //if (a < 1.0f) drawEmoDefault(canvas, 1.0f - a, 0.0f);
            //COLOR.alphaMultiplier = a;

            //MAT.identity();
            //MAT.tx = -8;
            //MAT.ty = 1;
            //MAT.scale(appear, appear);
            //MAT.translate(dx, dy);

            //canvas.draw(media.imgSmile2, MAT, COLOR, null, null, true);

            //MAT.identity();
            //MAT.tx = -5;
            //MAT.ty = -3;
            //MAT.scale(appear, appear);
            //MAT.translate(dx, dy);

            //canvas.draw(media.imgEyes1, MAT, COLOR, null, null, true);
            throw new NotImplementedException();
        }

        public bool isActive()
        {
            return state != 0;
        }

    }
}
