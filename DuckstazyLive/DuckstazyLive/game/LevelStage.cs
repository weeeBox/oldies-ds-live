﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using DuckstazyLive.app;
using System.Diagnostics;

namespace DuckstazyLive.game
{
	public class LevelStage
	{
		// кол-во времени для прохождения
		public float goalTime;
		
		// обратить флаг в true, если прошли уровень.
		public bool win;
		
		// уровень
		protected Level level;
		protected Pills pills;
		protected Particles particles;
		protected Hero hero;
		protected Env env;
		
		
		// 0 - накачай утку
		// 1 - бонус уровень (собирай данное время)
		// 2 - трип
		protected int type;
		
		protected float pumpProg; // прогресс накачки 0->1 после power==1
		protected float pumpVel; // скорость накачки
		public int collected;
		
		protected float startX;
		protected bool heroStarted;
		
		public StageMedia media;
		
		protected int startTitle = Constants.UNDEFINED;
		protected float startCounter;
		
		protected bool end;
		protected int endImg;
		protected float endCounter;
				
		public LevelStage(int _type)
		{
			level = Level.instance;
			media = level.stageMedia;
			pills = level.pills;
			particles = level.pills.ps;
			hero = level.hero;
			env = level.env;
			
			if(_type==0)
			{
				goalTime = 2.0f;
				pumpVel = 1.0f;
			}
			else if(_type==1)
			{

			}
			else
			{

			}
			
			type = _type;
		}
		
		public virtual void start()
		{
			win = false;
			startCounter = 0.0f;
			if(type==0)
			{
				startTitle = media.imgPump;
			}
			else if(type==1)
			{
				startTitle = media.imgParty;
			}
			else
				startTitle = media.imgTrip;
				
			pumpProg = 0.0f;
			collected = 0;
			
			startX = utils.rnd()*(640-54);
			heroStarted = false;
			
			end = false;
		}
		
		public virtual void onWin()
		{
		}
		
		public virtual void draw1(Canvas canvas)
		{
			
		}
		
		public virtual void draw2(Canvas canvas)
		{
            //ColorTransform color = new ColorTransform();
            //Matrix mat = new Matrix();
            //float a = startCounter;
            //float b = startCounter;
            //TextField text = level.infoText;
			
            //if(startTitle!=null && a<5.0f)
            //{
            //    if(b>4.0f) b = 5.0f - b;
            //    else if(b>2.0f) b = 1.0f;
            //    else b*=0.5;
            //    color.alphaMultiplier = b;
            //    mat.tx = 320 - startTitle.width*0.5;
            //    mat.ty = 180;
            //    canvas.draw(startTitle, mat, color);
				
            //    if(text.text.length!=0)
            //    {
            //        mat.tx = -text.textWidth*0.5;
            //        mat.ty = -text.textHeight*0.5;
					
            //        if(a<2)
            //            b = Math.Sin(2.355*a/2)*1.4148;
            //        else if(a<4)
            //            b = 1.0f;
            //        else
            //            b = 5.0f-a;
						
            //        mat.scale(b, b);
            //        mat.translate(320, 230);
										
            //        canvas.draw(text, mat);
					
            //        if(a>4)
            //        {
            //            b = a-4;
            //            mat.identity();
            //            mat.tx = -text.textWidth*0.5;
            //            mat.ty = -text.textHeight*0.5;
            //            mat.scale(b, b);
            //            mat.translate(320, 410+text.textHeight*0.5);
            //            canvas.draw(text, mat);
            //        }
            //    }
            //}
            //else
            //{
            //    if(text.text.length!=0)
            //    {
            //        mat.tx = 320 - text.textWidth*0.5;
            //        mat.ty = 410;
            //        canvas.draw(text, mat);
            //    }
            //}
			
            //if(end && endCounter<2)
            //{
            //    mat.identity();
            //    mat.tx = 320 - endImg.width*0.5;
            //    mat.ty = 180;
            //    a = endCounter;

            //    if(a>1) color.alphaMultiplier = Math.Cos(3.14*(a-1))*0.5+0.5;
            //    else color.alphaMultiplier = 1;
				
            //    canvas.draw(endImg, mat, color);
            //}
            // Implement me
		}
		
		public virtual void update(float dt)
		{
			float t;
			int i;
			String str;
			
			
			if(!heroStarted)
			{
				heroStarted = true;
				hero.start(startX);
			}
			
			if(!win)
			{
				
				if(type==0)
				{
					level.progress.updateProgress(level.power+pumpProg);
					if(level.power>=1.0f)
					{
						pumpProg+=dt*pumpVel;
						if(pumpProg>1.0f)
							pumpProg = 1.0f;
					}
					
					str = ((int)level.progress.perc*100).ToString() + "%";
					if(level.infoText.text!= str) level.infoText.text = str;
				}
				else if(type==1)
				{
					level.progress.updateProgress(pumpProg);
					if(startTitle==null && pumpProg<goalTime)
					{
						pumpProg+=dt;
						if(pumpProg>goalTime)
							pumpProg = goalTime;
					}
					
					t = (1.0f-level.progress.perc)*goalTime;
					i = (int)(t/60);
					if(i<10) str = "0" + i.ToString() + ":";
					else str = i.ToString() + ":";
					i = ((int)t)%60;
					if(i<10) str+="0"+ i.ToString();
					else str+=i.ToString();
					
					if(level.infoText.text!= str) level.infoText.text = str;
				}
				else if(type==2)
				{
					level.progress.updateProgress(collected);
					str = collected.ToString() + " OF " + ((int)goalTime).ToString();
					if(level.infoText.text!= str) level.infoText.text = str;
				}
				
				if(level.progress.full)
				{
					win = true;
					level.infoText.text = "";
					this.onWin();
					end = true;
					endImg = media.imgStageEnd;
					endCounter = 0.0f;
				}
				else if(!end && hero.state.health<=0)
				{
					level.infoText.text = "";
					end = true;
					endImg = media.imgTheEnd;
					endCounter = 0.0f;
				}
			}
			
			if(end)
				endCounter+=dt;
			
			if(startTitle!=Constants.UNDEFINED && startCounter<5.0f)
			{
				startCounter+=dt;
				if(startCounter>=5.0f)
				{
					startCounter = 5.0f;
					startTitle = Constants.UNDEFINED;
				}
			}
		}		
	}
}
