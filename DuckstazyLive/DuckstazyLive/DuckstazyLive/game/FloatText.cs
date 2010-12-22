using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace DuckstazyLive.game
{
    public class FloatText
	{
		public Texture2D img;
		public float x;
		public float y;
		public float t;
		public ColorTransform color;
		
		public FloatText()
		{
			t = 0.0f;
			color = new ColorTransform();
		}

	}

}
