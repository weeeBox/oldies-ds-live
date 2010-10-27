using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckstazyLive.app;

namespace DuckstazyLive.game.levels.fx
{
	public class FrogActor
	{
		protected StageMedia media;
		
		// координаты относительно LT картинки тела
		public float x;
		public float y;
		
		public float openCounter;
		public bool open;
		public float openVel;
		
		public float aHands;
		public float speedHands;
		
		public float angleEyes;
		public float phaseEyes;
		
		public bool visible;
		
		public FrogActor(StageMedia stageMedia)
		{
			media = stageMedia;
			
			x = 0;
			y = 0;
			
			visible = true;
			
			openCounter = 0.0f;
			open = false;
			openVel = 1.5f;
			
			aHands = 0.0f;
			speedHands = 0.0f;

			phaseEyes = utils.rnd();
			angleEyes = utils.rnd();
		}
		
		public void draw(Canvas canvas)
		{
            DrawMatrix mat = new DrawMatrix();
            mat.tx = x + 2;
            mat.ty = y;
			float hy = y-17-42*(float)(1-Math.Cos(openCounter*3.14f))*0.5f;
			ColorTransform color = new ColorTransform(1, 1, 1, 0.5f+openCounter*0.5f);
			float ha1 = (float)(1-Math.Cos(aHands))*0.5f*1.57f;
			float ha2 = (float)(1-Math.Cos(aHands+openCounter*3.14f))*0.5f*1.57f;
			canvas.draw(media.imgFrogBody, mat);
			
			mat.tx = -5; mat.ty = -2;
			mat.rotate(-ha1);
			mat.translate(x + 48+5, y + 58 - 17-2);
			canvas.draw(media.imgFrogHand1, mat);
			
			mat.identity();
			mat.tx = -13;
			mat.rotate(ha2);
			mat.translate(x + 92-3, y + 55 - 17);
			canvas.draw(media.imgFrogHand2, mat);

			mat.identity();
			mat.tx = x+20; mat.ty = hy;
			canvas.draw(media.imgFrogHead, mat);
		
			mat.tx = -12; mat.ty = -10;
            mat.scale(1.0f + 0.1f * (float)Math.Sin(angleEyes * 6.28f), 1.0f + 0.1f * (float)Math.Cos(angleEyes * 6.28f));
			mat.translate(x+ 58, 20+hy);
			canvas.draw(media.imgFrogEye1, mat);
			
			mat.identity();
			mat.tx = -15; mat.ty = -13;
            mat.scale(1.0f + 0.1f * (float)Math.Sin((angleEyes + phaseEyes) * 6.28f), 1.0f + 0.1f * (float)Math.Cos((angleEyes + phaseEyes) * 6.28f));
			mat.translate(x+ 87, 19+hy);
			canvas.draw(media.imgFrogEye2, mat);
			
			mat.identity();
			mat.tx = x + 51; mat.ty = hy+14;
			canvas.draw(media.imgFrogEmo1, mat, color);
			
			color.alphaMultiplier = openCounter;
			mat.tx = x+47; mat.ty = hy;
			canvas.draw(media.imgFrogEmo2, mat, color);
		}
		
		public void update(float dt)
		{
			angleEyes+=dt*openCounter;
			if(angleEyes>1.0f) angleEyes-=(int)(angleEyes);
			
			if(open)
			{
				if(openCounter<1.0f)
				{
					openCounter+=dt*openVel;
					if(openCounter>1.0f) openCounter = 1.0f;
				}
				
				aHands+=dt*speedHands*openCounter;
				if(aHands>6.28f)
					aHands-=6.28f;
			}
			else
			{
				if(openCounter>0.0f)
				{
					openCounter-=dt*openVel;
					if(openCounter<0.0f) openCounter = 0.0f;
				}
				
				if(aHands>0.0f && aHands<6.28f)
				{
					aHands+=dt*speedHands;
					if(aHands>=6.28f) aHands = 0.0f;
				}
			}			
		}
	}
}
