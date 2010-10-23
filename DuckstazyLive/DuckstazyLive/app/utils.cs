using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuckstazyLive.app
{
    public class utils
	{

		public static float lerp(float x, float a, float b)
		{
			return a + x*(b - a);
		}
	
		public static float vec2angle(P v1, P v2)
		{
			return Math.atan2(v1.y, v1.x) - Math.atan2(v2.y, v2.x);
		}
		
		public static Point vec2norm(P vec)
		{
			float inv_len = 1.0 / Math.Sqrt(vec.x*vec.x + vec.y*vec.y);
			
			return new Point(vec.x*inv_len, vec.y*inv_len);
		}
		
		public static float vec2lenSqr(P vec)
		{
			return vec.x*vec.x + vec.y*vec.y;
		}
		
		public static float vec2distSqr(float x1, float y1, float x2, float y2)
		{
			float dx = x1 - x2;
			float dy = y1 - y2;
			
			return dx*dx + dy*dy;
		}
		
		public static Point vec2norm2(P vec1, P vec2)
		{
			float dx = vec1.x - vec2.x;
			float dy = vec1.y - vec2.y;
			float inv_len = 1.0 / Math.Sqrt(dx*dx + dy*dy);
			
			return new Point(dx*inv_len, dy*inv_len);
		}
		
		public static Point vec2multScalar(P vec, float a)
		{
			float dx = a*vec.x;
			float dy = a*vec.y;
	
			return new Point(dx, dy);
		}

	
		public static float pos2pan(float x)
		{
			float p = (x-320.0)/320.0;
			
			if(p>1) p=1;
			else if(p<-1) p=-1;
			
			return p;
		}

		public static uint lerpColor(int fromColor, int toColor, float progress)
		{
			float q = 1 - progress;
			uint fromA = (fromColor >> 24) & 0xFF;
			uint fromR = (fromColor >> 16) & 0xFF;
			uint fromG = (fromColor >>  8) & 0xFF;
			uint fromB =  fromColor        & 0xFF;
	
			uint toA = (toColor >> 24) & 0xFF;
			uint toR = (toColor >> 16) & 0xFF;
			uint toG = (toColor >>  8) & 0xFF;
			uint toB =  toColor        & 0xFF;
			
			uint resultA = fromA*q + toA*progress;
			uint resultR = fromR*q + toR*progress;
			uint resultG = fromG*q + toG*progress;
			uint resultB = fromB*q + toB*progress;
			uint resultColor = resultA << 24 | resultR << 16 | resultG << 8 | resultB;
			
			return resultColor;		
		}
		
		public static uint mixAlpha(int color, float alpha)
		{
			uint a = (color >> 24) & 0xFF;
			uint rgb = color & 0xFFFFFF;
			
			uint resultA = a*alpha;
			
			uint resultColor = resultA << 24 | rgb;
			
			return resultColor;		
		}
		
		public static uint multColorScalar(int color, float value)
		{
			uint a = (color >> 24) & 0xFF;
			uint r = (color >> 16) & 0xFF;
			uint g = (color >>  8) & 0xFF;
			uint b =  color        & 0xFF;
				
			uint resultA = a*value;
			uint resultR = r*value;
			uint resultG = g*value;
			uint resultB = b*value;
			
			uint resultColor = resultA << 24 | resultR << 16 | resultG << 8 | resultB;
			
			return resultColor;
		}
		
		public static void ctSetRGB(C ct, int rgb)
		{
			ct.redMultiplier = ((rgb >> 16) & 0xFF)/255.0;
			ct.greenMultiplier = ((rgb >> 8) & 0xFF)/255.0;
			ct.blueMultiplier = (rgb & 0xFF)/255.0;
		}
		
		public static void ARGB2ColorTransform(int argb, C ct)
		{
			uint a = (argb >> 24) & 0xFF;
			uint r = (argb >> 16) & 0xFF;
			uint g = (argb >>  8) & 0xFF;
			uint b =  argb        & 0xFF;
			
			ct.redMultiplier = r*0.0039216;
			ct.greenMultiplier = g*0.0039216;
			ct.blueMultiplier = b*0.0039216;
			ct.alphaMultiplier = a*0.0039216;
		}
		
		public static uint calcARGB(float r, float g, float b, float a)
		{
			uint resultA = a*255.0;
			uint resultR = r*255.0;
			uint resultG = g*255.0;
			uint resultB = b*255.0;
			
			uint resultColor = resultA << 24 | resultR << 16 | resultG << 8 | resultB;
			
			return resultColor;
		}
		
		public static float splineInter(float v0, float v1, float v2, float v3, float x)
		{ 
			float P = (v3 - v2) - (v0 - v1); 
			float Q = (v0 - v1) - P; 
			float R = v2 - v0; 
			float S = v1; 
		
			float x2 = x*x;
			
			return (P*(x2*x)) + (Q*(x2)) + (R*x) + S; 
		}


		public static bool rnd_bool()
		{
			return Math.random()>=0.5;
		}
		
		// [x1, x2)
		public static float rnd_float(float x1, float x2)
		{
			return Math.random()*(x2 - x1) + x1;
		}
		
		//[x1, x2]
		public static int rnd_int(int x1, int x2)
		{
			return int(rnd_float(x1, x2+1));
		}
		
		//[x1, x2]
		public static void playSound(S snd, float vol, float x)
		{
			float p = (x-320.0)/320.0;
			
			if(p>1) p=1;
			else if(p<-1) p=-1;
			
			SoundTransform tr = new SoundTransform(vol, p);
			snd.play(49, 0, tr);
		}
	}
}
