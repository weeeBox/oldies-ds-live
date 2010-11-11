using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.core;
using Framework.visual;
using DuckstazyLive.app;

namespace DuckstazyLive.game.env
{
	public class EnvEffect
	{		
		public float power;
		public uint c1;
		public uint c2;
		public float peak;

        private CustomGeomerty geomEffect;
		
		public EnvEffect(float x, float y, float w, float h)
		{
			power = 0.0f;
			c1 = 0x000000;
			c2 = 0x000000;
			peak = 0.0f;

            geomEffect = utils.createSolidRect(x, y, w, h, utils.makeColor(0), false);
		}
		
		public virtual void update(float dt)
		{
		}

		public virtual void draw(Canvas canvas)
		{
            geomEffect.colorize(utils.makeColor(c2));
            canvas.drawGeometry(geomEffect);
		}

	}
}
