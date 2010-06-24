using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuckstazyLive
{
    public class Utils
    {
        public static float lerp(float x, float a, float b)
		{
			return a + x * (b - a);
		}
    }
}
