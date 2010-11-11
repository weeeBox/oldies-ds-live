using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using DuckstazyLive.game;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using Framework.core;
using Framework.visual;

namespace DuckstazyLive.app
{
    public class utils
	{
        private static Random random = new Random();

		public static float lerp(float x, float a, float b)
		{
			return a + x*(b - a);
		}
	
		public static float vec2angle(Vector2 v1, Vector2 v2)
		{
			return (float)(Math.Atan2(v1.Y, v1.X) - Math.Atan2(v2.Y, v2.X));
		}
		
		public static Vector2 vec2norm(Vector2 vec)
		{
			float inv_len = (float)(1.0 / Math.Sqrt(vec.X*vec.X + vec.Y*vec.Y));
			
			return new Vector2(vec.X*inv_len, vec.Y*inv_len);
		}
		
		public static float vec2lenSqr(Vector2 vec)
		{
			return vec.X*vec.X + vec.Y*vec.Y;
		}
		
		public static float vec2distSqr(float x1, float y1, float x2, float y2)
		{
			float dx = x1 - x2;
			float dy = y1 - y2;
			
			return dx*dx + dy*dy;
		}
		
		public static Vector2 vec2norm2(Vector2 vec1, Vector2 vec2)
		{
			float dx = vec1.X - vec2.X;
			float dy = vec1.Y - vec2.Y;
			float inv_len = (float)(1.0 / Math.Sqrt(dx*dx + dy*dy));
			
			return new Vector2(dx*inv_len, dy*inv_len);
		}
		
		public static Vector2 vec2multScalar(Vector2 vec, float a)
		{
			float dx = a*vec.X;
			float dy = a*vec.Y;
	
			return new Vector2(dx, dy);
		}

	
		public static float pos2pan(float x)
		{
			float p = (x-320.0f)/320.0f;
			
			if(p>1) p=1;
			else if(p<-1) p=-1;
			
			return p;
		}

		public static uint lerpColor(uint fromColor, uint toColor, float progress)
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
			
			uint resultA = (uint)(fromA*q + toA*progress);
			uint resultR = (uint)(fromR*q + toR*progress);
			uint resultG = (uint)(fromG*q + toG*progress);
			uint resultB = (uint)(fromB*q + toB*progress);
			uint resultColor = resultA << 24 | resultR << 16 | resultG << 8 | resultB;
			
			return resultColor;		
		}
		
		public static uint mixAlpha(uint color, float alpha)
		{
			uint a = (color >> 24) & 0xFF;
			uint rgb = color & 0xFFFFFF;
			
			uint resultA = (uint)(a*alpha);
			
			uint resultColor = resultA << 24 | rgb;
			
			return resultColor;		
		}
		
		public static uint multColorScalar(uint color, float value)
		{
			uint a = (color >> 24) & 0xFF;
			uint r = (color >> 16) & 0xFF;
			uint g = (color >>  8) & 0xFF;
			uint b =  color        & 0xFF;
				
			uint resultA = (uint)(a*value);
			uint resultR = (uint)(r*value);
			uint resultG = (uint)(g*value);
			uint resultB = (uint)(b*value);
			
			uint resultColor = resultA << 24 | resultR << 16 | resultG << 8 | resultB;
			
			return resultColor;
		}
		
		public static void ctSetRGB(ColorTransform ct, uint rgb)
		{
			ct.redMultiplier = ((rgb >> 16) & 0xFF)/255.0f;
			ct.greenMultiplier = ((rgb >> 8) & 0xFF)/255.0f;
			ct.blueMultiplier = (rgb & 0xFF)/255.0f;
		}
		
		public static void ARGB2ColorTransform(uint argb, ColorTransform ct)
		{
			uint a = (argb >> 24) & 0xFF;
			uint r = (argb >> 16) & 0xFF;
			uint g = (argb >>  8) & 0xFF;
			uint b =  argb        & 0xFF;
			
			ct.redMultiplier = r*0.0039216f;
			ct.greenMultiplier = g*0.0039216f;
			ct.blueMultiplier = b*0.0039216f;
			ct.alphaMultiplier = a*0.0039216f;
		}
		
		public static uint calcARGB(float r, float g, float b, float a)
		{
			uint resultA = (uint)(a*255);
			uint resultR = (uint)(r*255);
			uint resultG = (uint)(g*255);
			uint resultB = (uint)(b*255);
			
			uint resultColor = resultA << 24 | resultR << 16 | resultG << 8 | resultB;
			
			return resultColor;
		}
		
		public static float splineInter(float v0, float v1, float v2, float v3, float x)
		{ 
			float Vector2 = (v3 - v2) - (v0 - v1); 
			float Q = (v0 - v1) - Vector2; 
			float R = v2 - v0; 
			float Sound = v1; 
		
			float x2 = x*x;
			
			return (Vector2*(x2*x)) + (Q*(x2)) + (R*x) + Sound; 
		}

        public static float rnd()
        {
            return (float)random.NextDouble();
        }

		public static bool rnd_bool()
		{
			return random.NextDouble()>=0.5;
		}
		
		// [x1, x2)
		public static float rnd_float(float x1, float x2)
		{
			return (float)random.NextDouble()*(x2 - x1) + x1;
		}
		
		//[x1, x2]
		public static int rnd_int(int x1, int x2)
		{
			return (int) rnd_float(x1, x2+1);
		}
		
		//[x1, x2]
		public static void playSound(int snd, float vol, float x)
		{
			float p = (x-320)/320;
			
			if(p>1) p=1;
			else if(p<-1) p=-1;
			
			SoundTransform tr = new SoundTransform(vol, p);
			// snd.play(49, 0, tr);
            Application.sharedSoundMgr.playSound(snd, tr);            
		}

        public static Color makeColor(uint color)
        {
            return makeColor(color, false);
        }

        public static Color makeColor(uint color, bool processAlpha)
        {            
            byte a = processAlpha ? (byte)((color >> 24) & 0xff) : (byte)255;
            byte r = (byte)((color >> 16) & 0xff);
            byte g = (byte)((color >> 8) & 0xff);
            byte b = (byte)((color >> 0) & 0xff);
            return new Color(r, g, b, a);
        }

        public static Texture2D getImage(int imageId)
        {
            return Application.sharedResourceMgr.getTexture(imageId);
        }

        public static CustomGeomerty createRect(float x, float y, float w, float h, Color borderColor)
        {
            return createRect(x, y, w, h, borderColor, true);
        }

        public static CustomGeomerty createRect(float x, float y, float w, float h, Color borderColor, bool applyScale)
        {
            if (applyScale)
            {
                return GeometryFactory.createRect(scale(x), scale(y), scale(w), scale(h), borderColor);
            }
            else
            {
                return GeometryFactory.createRect(x, y, w, h, borderColor);
            }
        }

        public static CustomGeomerty createGradient(float x, float y, float w, float h, Color upperColor, Color lowerColor)
        {
            return createGradient(x, y, w, h, upperColor, lowerColor, true);
        }

        public static CustomGeomerty createGradient(float x, float y, float w, float h, Color upperColor, Color lowerColor, bool applyScale)
        {
            if (applyScale)
            {
                return GeometryFactory.createGradient(scale(x), scale(y), scale(w), scale(h), upperColor, lowerColor);
            }
            else
            {
                return GeometryFactory.createGradient(x, y, w, h, upperColor, lowerColor);
            }
        }

        public static CustomGeomerty createSolidRect(float x, float y, float w, float h, Color fillColor)
        {
            return createSolidRect(x, y, w, h, fillColor, true);
        }

        public static CustomGeomerty createSolidRect(float x, float y, float w, float h, Color fillColor, bool applyScale)
        {
            if (applyScale)
            {
                return GeometryFactory.createSolidRect(scale(x), scale(y), scale(w), scale(h), fillColor);
            }
            else
            {
                return GeometryFactory.createSolidRect(x, y, w, h, fillColor);
            }
        }

        public static int scale(int val)
        {
            return (int)(Constants.SCALE * val);
        }

        public static float scale(float val)
        {
            return Constants.SCALE * val;
        }

        public static float unscale(float val)
        {
            return Constants.SCALE_INV * val;
        }
    }
}
