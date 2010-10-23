using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuckstazyLive.game
{
	public class GameInfo
	{
		
		private const int ftSize = 50;
		private Array ftPool;
		private int ftCount;

		private TextField text;
		public Texture2D one;
		public Array powers;
		public Array toxics;
		public Array sleeps;
		public Array damages;
		
		
		public float r;
		public float g;
		public float b;
		
		//public var state:GameState;
		 
		public GameInfo()
		{
			int i;
			Texture2D bm;
			
			ftPool = new Array(ftSize);
			for( ; i<ftSize; ++i)
				ftPool[i] = new FloatText();
				
			ftCount = 0;
			
			text = new TextField();
 			text.defaultTextFormat = new TextFormat("_mini", 15, 0xffffffff);
 			text.embedFonts = true;
 			text.cacheAsBitmap = true;
 			text.autoSize = TextFieldAutoSize.LEFT;
 			
 			powers = new Array();
 			toxics = new Array();
 			sleeps = new Array();
 			damages = new Array();
 			
 			text.text = "+1";
 			one = new Texture2D(text.width, text.height, true, 0x00000000);
 			one.draw(text);
 			
 			text.text = "+5";
 			bm = new Texture2D(text.width, text.height, true, 0x00000000);
 			bm.draw(text);
 			powers.push(bm);
 			
 			text.text = "+10";
 			bm = new Texture2D(text.width, text.height, true, 0x00000000);
 			bm.draw(text);
 			powers.push(bm);
 			
 			text.text = "+25";
 			bm = new Texture2D(text.width, text.height, true, 0x00000000);
 			bm.draw(text);
 			powers.push(bm);
 			
 			text.text = "+50";
 			bm = new Texture2D(text.width, text.height, true, 0x00000000);
 			bm.draw(text);
 			powers.push(bm);
 			
 			text.text = "+100";
 			bm = new Texture2D(text.width, text.height, true, 0x00000000);
 			bm.draw(text);
 			powers.push(bm);
 			
 			text.text = "+150";
 			bm = new Texture2D(text.width, text.height, true, 0x00000000);
 			bm.draw(text);
 			powers.push(bm);
 			
 			text.text = "FIRST BLOOD! +100";
 			bm = new Texture2D(text.width, text.height, true, 0x00000000);
 			bm.draw(text);
 			toxics.push(bm);
 			
 			text.text = "MANIACALISTIC! +150";
 			bm = new Texture2D(text.width, text.height, true, 0x00000000);
 			bm.draw(text);
 			toxics.push(bm);
 			
 			text.text = "SUPER RESISTANCE! +200";
 			bm = new Texture2D(text.width, text.height, true, 0x00000000);
 			bm.draw(text);
 			toxics.push(bm);
 			
 			text.text = "WAKE UP!";
 			bm = new Texture2D(text.width, text.height, true, 0x00000000);
 			bm.draw(text);
 			sleeps.push(bm);
 			
 			text.text = "LULLABY...";
 			bm = new Texture2D(text.width, text.height, true, 0x00000000);
 			bm.draw(text);
 			sleeps.push(bm);
 			
 			text.text = "FALLING ASLEEP..";
 			bm = new Texture2D(text.width, text.height, true, 0x00000000);
 			bm.draw(text);
 			sleeps.push(bm);
 			
 			text.text = "OOPS!";
 			bm = new Texture2D(text.width, text.height, true, 0x00000000);
 			bm.draw(text);
 			damages.push(bm);
 			
 			text.text = "REALLY HARD...";
 			bm = new Texture2D(text.width, text.height, true, 0x00000000);
 			bm.draw(text);
 			damages.push(bm);
 			
 			text.text = "BE CAREFUL!";
 			bm = new Texture2D(text.width, text.height, true, 0x00000000);
 			bm.draw(text);
 			damages.push(bm);
 		}
		
		public void reset()
		{
			foreach (FloatText it in ftPool)
			{
				it.t = 0.0;
				it.img = null;
			}
			ftCount = 0;
		}
		
		public void drawFT(bool canvas)
		{
			int i = 0;
			Matrix mat = new Matrix();
			
			foreach (FloatText ft in ftPool)
			{
				if(i==ftCount)
					break;
					
				if(ft.t>0.0)
				{
					mat.tx = ft.x;
					mat.ty = int(ft.y);

					canvas.draw(ft.img, mat, ft.color, null, null, false);
					++i;
				}
			}
		}
		
		public void add(float x, float y, bool bm)
		{
			foreach (FloatText ft in ftPool)
			{
				if(ft.t<=0.0)
				{
					ft.t = 1.0;
					ft.x = x - (bm.width>>1);
					ft.y = y - (bm.height>>1);
					ft.img = bm;
					
					++ftCount; 
					 
					break;
				}
			}
		}
		
		public void setRGB(int color)
		{
			r = ((color >> 16) & 0xFF)*0.003921569;
			g = ((color >>  8) & 0xFF)*0.003921569;
			b =  (color        & 0xFF)*0.003921569;
		}
			
		private void ctCalc(C color, float t)
		{
			float x = 0.5*(1.0 + Math.Sin(t*6.28*4.0));
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
					
				if(ft.t>0.0)
				{
					ft.t -= dt;
					 
					if(ft.t<=0.0)
						--ftCount;
					else
					{
						ft.y -= 50.0*dt;
						a = 0.25;
						if(ft.t>0.75) a = 1.0 - ft.t;
						else if(ft.t<0.25) a = ft.t;
						a*=4.0;
						ctCalc(ft.color, ft.t);
						ft.color.alphaMultiplier = a;

					}
					
					++i;
				}
			}
		}

	}

}
