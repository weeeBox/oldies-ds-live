using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
			t = 0.0;
			color = new ColorTransform();
		}

	}

}
