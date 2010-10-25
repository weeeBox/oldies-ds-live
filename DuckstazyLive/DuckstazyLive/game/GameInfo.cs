using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace DuckstazyLive.game
{
	public class GameInfo
	{
		
		private const int ftSize = 50;
		private FloatText[] ftPool;
		private int ftCount;

		// private TextField text;
		public Texture2D one;
		public Texture2D[] powers;
		public Texture2D[] toxics;
		public Texture2D[] sleeps;
		public Texture2D[] damages;
		
		
		public float r;
		public float g;
		public float b;
		
		//public var state:GameState;
		 
		public GameInfo()
		{
			int i = 0;
			Texture2D bm;
			
			ftPool = new FloatText[ftSize];
			for( ; i<ftSize; ++i)
				ftPool[i] = new FloatText();
				
			ftCount = 0;
			
            //text = new TextField();
            //text.defaultTextFormat = new TextFormat("_mini", 15, 0xffffffff);
            //text.embedFonts = true;
            //text.cacheAsBitmap = true;
            //text.autoSize = TextFieldAutoSize.LEFT;
 			
            //powers = new Array();
            //toxics = new Array();
            //sleeps = new Array();
            //damages = new Array();
 			
            //text.text = "+1";
            //one = new Texture2D(text.width, text.height, true, 0x00000000);
            //one.draw(text);
 			
            //text.text = "+5";
            //bm = new Texture2D(text.width, text.height, true, 0x00000000);
            //bm.draw(text);
            //powers.push(bm);
 			
            //text.text = "+10";
            //bm = new Texture2D(text.width, text.height, true, 0x00000000);
            //bm.draw(text);
            //powers.push(bm);
 			
            //text.text = "+25";
            //bm = new Texture2D(text.width, text.height, true, 0x00000000);
            //bm.draw(text);
            //powers.push(bm);
 			
            //text.text = "+50";
            //bm = new Texture2D(text.width, text.height, true, 0x00000000);
            //bm.draw(text);
            //powers.push(bm);
 			
            //text.text = "+100";
            //bm = new Texture2D(text.width, text.height, true, 0x00000000);
            //bm.draw(text);
            //powers.push(bm);
 			
            //text.text = "+150";
            //bm = new Texture2D(text.width, text.height, true, 0x00000000);
            //bm.draw(text);
            //powers.push(bm);
 			
            //text.text = "FIRST BLOOD! +100";
            //bm = new Texture2D(text.width, text.height, true, 0x00000000);
            //bm.draw(text);
            //toxics.push(bm);
 			
            //text.text = "MANIACALISTIC! +150";
            //bm = new Texture2D(text.width, text.height, true, 0x00000000);
            //bm.draw(text);
            //toxics.push(bm);
 			
            //text.text = "SUPER RESISTANCE! +200";
            //bm = new Texture2D(text.width, text.height, true, 0x00000000);
            //bm.draw(text);
            //toxics.push(bm);
 			
            //text.text = "WAKE UP!";
            //bm = new Texture2D(text.width, text.height, true, 0x00000000);
            //bm.draw(text);
            //sleeps.push(bm);
 			
            //text.text = "LULLABY...";
            //bm = new Texture2D(text.width, text.height, true, 0x00000000);
            //bm.draw(text);
            //sleeps.push(bm);
 			
            //text.text = "FALLING ASLEEP..";
            //bm = new Texture2D(text.width, text.height, true, 0x00000000);
            //bm.draw(text);
            //sleeps.push(bm);
 			
            //text.text = "OOPS!";
            //bm = new Texture2D(text.width, text.height, true, 0x00000000);
            //bm.draw(text);
            //damages.push(bm);
 			
            //text.text = "REALLY HARD...";
            //bm = new Texture2D(text.width, text.height, true, 0x00000000);
            //bm.draw(text);
            //damages.push(bm);
 			
            //text.text = "BE CAREFUL!";
            //bm = new Texture2D(text.width, text.height, true, 0x00000000);
            //bm.draw(text);
            //damages.push(bm);
            Debug.WriteLine("Implement me: GameInfo.GameInfo()");
 		}
		
		public void reset()
		{
			foreach (FloatText it in ftPool)
			{
				it.t = 0.0f;
				it.img = null;
			}
			ftCount = 0;
		}
		
		public void drawFT(Canvas canvas)
		{
            //int i = 0;
            //Matrix mat = new Matrix();

            //foreach (FloatText ft in ftPool)
            //{
            //    if (i == ftCount)
            //        break;

            //    if (ft.t > 0.0f)
            //    {
            //        mat.tx = ft.x;
            //        mat.ty = ft.y;

            //        canvas.draw(ft.img, mat, ft.color, null, null, false);
            //        ++i;
            //    }
            //}            
            Debug.WriteLine("Implement me: GameInfo.drawFT");
		}
		
		public void add(float x, float y, Texture2D bm)
		{
            //foreach (FloatText ft in ftPool)
            //{
            //    if(ft.t<=0.0f)
            //    {
            //        ft.t = 1.0f;
            //        ft.x = x - (bm.Width>>1);
            //        ft.y = y - (bm.Height>>1);
            //        ft.img = bm;
					
            //        ++ftCount; 
					 
            //        break;
            //    }
            //}
            Debug.WriteLine("Implement me: GameInfo.add()");
		}
		
		public void setRGB(uint color)
		{
			r = (uint)(((color >> 16) & 0xFF)*0.003921569f);
			g = (uint)(((color >>  8) & 0xFF)*0.003921569f);
			b = (uint)((color & 0xFF)*0.003921569f);
		}
			
		private void ctCalc(ColorTransform color, float t)
		{
			float x = (int)(0.5f*(1.0f + Math.Sin(t*6.28f*4.0f)));
			color.redMultiplier = r*x;
			color.greenMultiplier = g*x;
			color.blueMultiplier = b*x;
		}
		
		public void update(float power, float dt)
		{
			int i = 0;
			int ft_proc = ftCount;
			float a;
			
			foreach (FloatText ft in ftPool)
			{
				if(i==ft_proc)
					break;
					
				if(ft.t>0.0f)
				{
					ft.t -= dt;
					 
					if(ft.t<=0.0f)
						--ftCount;
					else
					{
						ft.y -= 50.0f*dt;
						a = 0.25f;
						if(ft.t>0.75) a = 1.0f - ft.t;
						else if(ft.t<0.25f) a = ft.t;
						a*=4.0f;
						ctCalc(ft.color, ft.t);
						ft.color.alphaMultiplier = a;

					}
					
					++i;
				}
			}
		}

	}

}
