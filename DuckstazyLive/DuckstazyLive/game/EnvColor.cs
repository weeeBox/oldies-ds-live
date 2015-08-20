using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckstazyLive.app;

namespace DuckstazyLive.game
{
	public class EnvColor
	{
		public uint bg;
		public uint text;

		public EnvColor(uint sky, uint floatText)
		{
			bg = sky;
			text = floatText;
		}

        public void lerp(float x, EnvColor c1, EnvColor c2)
		{
			bg = utils.lerpColor(c1.bg, c2.bg, x);
			text = utils.lerpColor(c1.text, c2.text, x);
		}

	}

}
