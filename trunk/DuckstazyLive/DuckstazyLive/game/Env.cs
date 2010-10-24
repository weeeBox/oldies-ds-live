using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckstazyLive.game.env;
using DuckstazyLive.app;
using System.Diagnostics;
using Framework.core;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Framework.visual;

namespace DuckstazyLive.game
{
public class Env
	{
        //// Импорт графических ресурсов
        //[Embed(source="gfx/cloud1.png")]
        //private Class rCloudImg1;
        
        //[Embed(source="gfx/cloud2.png")]
        //private Class rCloudImg2;
        
        //[Embed(source="gfx/cloud3.png")]
        //private Class rCloudImg3;
        
        //[Embed(source="gfx/grass.png")]
        //private Class rGrassImg;
        
        //[Embed(source="gfx/grass2.png")]
        //private Class rGrass2Img;
        
        //[Embed(source="gfx/fx_star.png")]
        //private Class rStarImg;
        
        ////[Embed(source="sfx/tex2.mp3")]
        ////private var rTex1Snd:Class;
        
        //[Embed(source="sfx/tex3.mp3")]
        //private Class rTex2Snd;
        
        //[Embed(source="sfx/power.mp3")]
        //private Class rPowerSnd;
                
		private int[] imgClouds;
		private int imgGrass;
		private int imgGrass2;
		private int imgGround;
		private int imgStar;
		private int imgSky;
        private CustomGeomerty geomSky;
		
		private int sndPower;
		//private var sndTex1:Sound;
		private int sndTex2;
		private int music;
		private float musicLenght;
		private SoundChannel channel;
		private SoundTransform musicTrans;
		private float musicAttack;
		public float x;
		public float y;
		
		public float blanc;
		// private Shape shBlanc;
		
		// Состояние окружения
		private float power;
		
		// Время для нормального состояния. Циклит для смены ДЕНЬ/НОЧЬ
		public bool day;
		// Время для глюков. Циклит палитру.
		private float time;		
		
		// Счётчик для эффекта с травой
		private float grassCounter;		
		
		// Облака
		private EnvCloud[] clouds;
		private EnvStar[] nightSky;
		
		private EnvColor norm;

        private static EnvColor[] hell = 
        {
			new EnvColor(0xFF0000, 0xFFFF00), 
			new EnvColor(0xFFFF00, 0xFF0000), 
			new EnvColor(0x00FF00, 0x0000FF),
			new EnvColor(0x00FFFF, 0xFFFF00),
			new EnvColor(0x0000FF, 0xFFFF00),
			new EnvColor(0xFF00FF, 0xFFFF00),
			new EnvColor(0xFF0000, 0xFFFF00)
        };
			
		private const int hellCount = 7;
			
		// Текущие цвета.
		public EnvColor colors;
		
		public uint colGrass;
		public uint colGround;
		public uint colProgress;
		
		public ColorTransform ctGrass;
		public ColorTransform ctProgress;
				
		// Темп-переменная для рисования
		// private Shape shape;
		
		// эффекты
		private EnvEffect[] effects;
		private EnvEffect curEffect;
		
		public Env()
		{
			// Временные переменные.
			float x = 0;
			int i;
			
			// shape = new Shape();
			norm = new EnvColor(0x3FB5F2, 0x000000);
			
			// Текущие цвета
			colors = new EnvColor(0,0);
			
			effects = new EnvEffect[] { new EnvEffect1(), new EnvEffect2(), new EnvEffect3(), new EnvEffect4() };	
			
			// shBlanc = new Shape();
			
			blanc = 0.0f;
			// Инициализируем траву
			grassCounter = 0.0f;
			
			// Инициализируем переменные окружения
			power = 0.0f;
			day = true;
			updateNorm();
			time = 0.0f;			
			
			// Инициализируем
			initGrass();
			initDay();
			initNight();
			
			curEffect = effects[3];//effects[int(Math.random()*effects.length)];
			
			colGrass = 0xff00ff00;
			colGround = 0xff371d06;
			colProgress = 0xff5d310c;
			ctGrass = new ColorTransform();
			utils.ARGB2ColorTransform(colGrass, ctGrass);
			ctProgress = new ColorTransform();
			utils.ARGB2ColorTransform(colProgress, ctProgress);
			
			sndPower = Res.SND_ENV_POWER;
			//sndTex1 = new rTex1Snd();
            sndTex2 = Res.SONG_ENV_TEX;
			
			musicTrans = new SoundTransform(0);
			
			music = sndTex2;
			// channel = music.play(0.0f, 0, musicTrans);
			// channel.addEventListener(Event.SOUND_COMPLETE, loopMusic);
			musicAttack = 0.0f;
		}
	
		private void initGrass()
		{
            //Rect rc = new Rect(0, 0, 128, 16);
            //Texture2D data = (new rGrassImg()).bitmapData;
            //Texture2D data2 = (new rGrass2Img()).bitmapData;
            //Point dest = new Point();
            //int i = 5;
            //Matrix mat = new Matrix();
            //mat.createGradientBox(640, 80, 1.57, 0, 0);
			
            //imgGrass = new Texture2D(640, 8, true, 0x00000000);
            //imgGrass2 = new Texture2D(640, 8, true, 0x00000000);
            //imgGround = new Texture2D(640, 80, false, 0x00000000); 
            //imgGrass.lock();
            //imgGrass2.lock();
			
            //while(i>0)
            //{
            //    imgGrass.copyPixels(data, rc, dest, null, null, true);
            //    imgGrass2.copyPixels(data2, rc, dest, null, null, true);
            //    dest.x+=128.0;
            //    --i;
            //} 

            //imgGrass.unlock();
            //imgGrass2.unlock();
			
            //shape.graphics.clear();
            //shape.graphics.beginGradientFill(GradientType.LINEAR, [0x371d06, 0x5d310c], [1.0f, 1.0f], [0x00, 0xFF], mat);
            //shape.graphics.drawRect(0.0f, 0.0f, 640.0, 80.0);
            //shape.graphics.endFill();
            //imgGround.draw(shape);
            Debug.WriteLine("Implement me: Env.initGrass");
		}
			
		private void initDay()
		{
            //float x = 0.0f;
            //Matrix mat = new Matrix();
            //mat.createGradientBox(640, 400, 1.57, 0, 0);
		
            //shape.graphics.clear();
            //shape.graphics.beginGradientFill(GradientType.LINEAR, [0x3FB5F2, 0xDDF2FF], [1.0f, 1.0f], [0x00, 0xFF], mat);
            //shape.graphics.drawRect(0.0f, 0.0f, 640.0, 480.0);
            //shape.graphics.endFill();
			
            //imgSky = new Texture2D(640, 400, false);
            //imgSky.draw(shape);
            geomSky = GeometryFactory.createGradient(0, 0, 640, 480, utils.makeColor(0x3FB5F2), utils.makeColor(0xDDF2FF));
			
            imgClouds = new int[] { Res.IMG_CLOUD_1, Res.IMG_CLOUD_2, Res.IMG_CLOUD_3 };
			
            clouds = new EnvCloud[] { new EnvCloud(), new EnvCloud(), new EnvCloud(), new EnvCloud(), new EnvCloud() };			
            foreach (EnvCloud it in clouds)
            {
                it.init(x);
                x+=128.0f + utils.rnd()*22.0f;
            }			
		}
		
		private void initNight()
		{
            int starsCount = 30;
            int i = starsCount - 1;

            imgStar = Res.IMG_STAR;
            nightSky = new EnvStar[starsCount];

            while (i >= 0)
            {
                nightSky[i] = new EnvStar();
                --i;
            }
            
		}
		
		public void updateNorm()
		{
			if(day)
			{
				norm.bg = 0x3FB5F2;
				norm.text = 0x000000;
			}
			else
			{
				norm.bg = 0x111133;
				norm.text = 0xFFFFFF;
			}
		}
		
		private void updateColors()
		{
			//var pal:EnvColor = new EnvColor(0,0);
			int c;
			float x = time;
			float p2 = power*power;
		
			c = (int)x;	x-=c;
			//pal.lerp(x, hell[c], hell[c+1]);
			colors.lerp(x, hell[c], hell[c+1]);
			//colors.lerp(power, norm, pal);
				
			colGrass = 0xff000000|utils.lerpColor(utils.multColorScalar(0x177705, 1.0f-p2), colors.bg, p2*grassCounter);
			colGround = 0xff000000|utils.lerpColor(0x371d06, utils.multColorScalar(colors.bg, grassCounter*power), p2);
			colProgress = 0xff000000|utils.lerpColor(0x5d310c, colors.bg, p2);
			
			utils.ARGB2ColorTransform(colGrass, ctGrass);
			utils.ARGB2ColorTransform(colProgress, ctProgress);		
		}
						
		public void update(float dt, float newPower)
		{
			// Временные переменные.
			float x;
			int i;
			SoundTransform st;
			
			if(newPower!=power)
			{
				if(newPower>=0.5f && power<0.5f)
				{
					day = !day;
					updateNorm();
					blanc = 1.0f;
					
					/*x = channel.position;
					channel.stop();
					channel.removeEventListener(Event.SOUND_COMPLETE, loopMusic);
					music = sndTex2;
					channel = music.play(x);
					channel.addEventListener(Event.SOUND_COMPLETE, loopMusic);*/
					musicTrans.volume = 1;
					// channel.soundTransform = musicTrans;					
					// sndPower.play();
                    Debug.WriteLine("Implement me. Env.update(float dt, float newPower)");
				}
				else if(power>=0.5f && newPower<0.5f)
				{
					blanc = 1.0f;
					colGrass = 0xff00ff00;
					colGround = 0xff371d06;
					colProgress = 0xff5d310c;
					utils.ARGB2ColorTransform(colGrass, ctGrass);
					utils.ARGB2ColorTransform(colProgress, ctProgress);
					curEffect = effects[(int)(utils.rnd()*effects.Length)];
					
					/*x = channel.position;
					channel.stop();
					channel.removeEventListener(Event.SOUND_COMPLETE, loopMusic);
					music = sndTex2;
					channel = music.play(x);
					channel.addEventListener(Event.SOUND_COMPLETE, loopMusic);*/
				}
				
				power = newPower;
				
				if(power<0.5f)
				{
					musicTrans.volume = power*0.3f;
					channel.soundTransform = musicTrans;
				}
			}
			
			/*if(channel.position >= 63900.0)
			{
				st = channel.soundTransform;
				
				channel.stop();
				channel = music.play(0.0f, 0, st);
			}*/

			// Обноляем счётчик с травой
			if(grassCounter>0)
			{
				grassCounter-=dt*4.0f;
				if(grassCounter<0.0f)
					grassCounter = 0.0f;
			}
			
			if(power<0.5f)
			{
				if(day)
					foreach (EnvCloud c in clouds)
					    c.update(dt, power);
				else
					foreach (EnvStar s in nightSky)
					    s.update(dt, power);
			}
			else
			{
				curEffect.power = power;
				curEffect.c1 = colors.bg;
				curEffect.c2 = utils.multColorScalar(colors.bg, 0.5f);
				curEffect.update(dt);
				// Прокручиваем время день/ночь
				time+=dt*0.1f;
				while(time>hellCount-1)
				time-=hellCount-1;
				
				x = (channel.leftPeak + channel.rightPeak)*0.5f;
				// Обновляем текущие цвета
				updateColors();
								
				/*if(musicAttack<x)
					musicAttack = x;
				else if(musicAttack>0.0f)
				{
					musicAttack-=dt*10.0;
					if(musicAttack<0.0f)
						musicAttack = 0.0f;
				}*/
				musicAttack = musicAttack*0.7f + x*0.7f;
				
				curEffect.peak = musicAttack;
			}

		}

		public void draw1(Canvas canvas)
		{
            //// Временные переменные.
            //Rect rc = new Rect(0.0f, 0.0f, 640.0, 400.0);
            ////Graphics gr = shape.graphics;			
			
            //if(power<0.5f)
            //{
            //    if(day)
            //    {
            //        canvas.copyPixels(imgSky, rc, new Point(0.0f, 0.0f));
					 
            //        drawSky(canvas);
            //    }
            //    else
            //    {
            //        canvas.fillRect(rc, 0x111133);
            //        drawNight(canvas);
            //    }
            //}
            //else
            //{
            //    curEffect.draw(canvas);
            //    gr.clear();
            //    gr.beginFill(colors.bg, 0.4*musicAttack);
            //    gr.drawCircle(613.0 - x, 380.0 - y, musicAttack*30.0);
            //    gr.drawCircle(320.0 - (x - 293.0)*0.97, 200.0 - (y - 180.0)*0.97, musicAttack*25.0);
            //    gr.drawCircle(320.0 + (x - 293.0)*0.7, 200.0 + (y - 180.0)*0.7, musicAttack*10.0);
            //    gr.endFill();
            //    canvas.draw(shape);
            //    //canvas.applyFilter(canvas, new Rect(0, 0, 640, 400), new Point(), new ConvolutionFilter(3,3,null, 9));
            //}			
		}
		
		public void drawNight(Canvas canvas)
		{
            //// Временные переменные.
            //float x;
            //Matrix mat = new Matrix();
            //* o;
            //EnvStar c;
			
            //// Рисуем ОБЛАКА
            //foreach (o in nightSky)
            //{
            //    c = EnvStar(o);
            //    x = c.t;
								
            //    mat.identity();
            //    mat.translate(-7.0, -7.0);
            //    mat.rotate(c.a);
            //    mat.scale(0.75 + 0.25*Math.sin(x*6.28), 0.75 + 0.25*Math.sin(x*6.28));
            //    mat.translate(c.x, c.y);
				
            //    canvas.draw(imgStar, mat, c.color, null, null, true);
            //}
		}
		
		public void drawSky(Canvas canvas)
		{
            //// Временные переменные.
            //float x;
            //Matrix mat = new Matrix();
            //Texture2D img;
			
            //// Рисуем ОБЛАКА
            //foreach (EnvCloud c in clouds)
            //{
            //    x = c.counter;
            //    img = Texture2D(imgClouds[c.id]); 
				
            //    mat.identity();
            //    mat.translate(-img.width*0.5f, -img.height*0.5f);
            //    mat.scale(0.9 + 0.1*Math.sin(x*6.28), 0.95 + 0.05*Math.sin(x*6.28 + 3.14));
            //    mat.translate(c.x, c.y);
				
            //    canvas.draw(img, mat, null, null, null, true);
            //}
		}
	
		public void draw2(Canvas canvas)
		{
            //// Временные переменные.
            //Matrix mat = new Matrix(1.0f, 0.0f, 0.0f, 1.0f, 0.0f, 392.0);
            //Rect rc = new Rect(0.0f, 400.0, 640.0, 80.0);
			
            ///**** ОСНОВА ДЛЯ ЗЕМЛИ ****/
			
			
			
            ///**** ТРАВА ****/

            if (power < 0.5f)
            {
                // TODO Optimize
                // canvas.draw(imgGround, new Matrix(1.0f, 0.0f, 0.0f, 1.0f, 0.0f, 400.0));
                // canvas.draw(imgGrass, mat, ctGrass);
                Texture2D tex = Application.sharedResourceMgr.getTexture(Res.IMG_GRASS1);
                Rectangle src = new Rectangle(0, 0, tex.Width, tex.Height);
                Rectangle dst = new Rectangle(0, 400, FrameworkConstants.SCREEN_WIDTH, FrameworkConstants.SCREEN_HEIGHT);
                AppGraphics.DrawImageTiled(tex, ref src, ref dst);
            }
            else
            {
                //canvas.fillRect(rc, colGround);
                //canvas.draw(imgGrass2, mat, ctGrass);
                Texture2D tex = Application.sharedResourceMgr.getTexture(Res.IMG_GRASS1);
                Rectangle src = new Rectangle(0, 0, tex.Width, tex.Height);
                Rectangle dst = new Rectangle(0, 400, FrameworkConstants.SCREEN_WIDTH, FrameworkConstants.SCREEN_HEIGHT);
                AppGraphics.DrawImageTiled(tex, ref src, ref dst);
            }	
		}

		public void beat()
		{
			grassCounter = 1.0f;
		}
		
		public void updateBlanc(float dt)
		{
			if(blanc>0.0f)
				blanc-=0.5f*dt;
		}
		
		public void drawBlanc(Canvas canvas)
		{
            //shBlanc.graphics.clear();
            //shBlanc.graphics.beginFill(0xffffff, blanc);
            //shBlanc.graphics.drawRect(0.0f, 0.0f, 640.0, 480.0);
            //shBlanc.graphics.endFill();
            //canvas.draw(shBlanc);
		}

		public void loopMusic()
		{
            //if(power<0.5f)
            //    musicTrans.volume = power*0.3f;
				
            //if(channel!=null)
            //{
            //    channel.stop();
            //    channel.removeEventListener(Event.SOUND_COMPLETE, loopMusic);
            //    channel = music.play(0.0f, 0, musicTrans);
            //    channel.addEventListener(Event.SOUND_COMPLETE, loopMusic);
            //}
		}
	}

}
