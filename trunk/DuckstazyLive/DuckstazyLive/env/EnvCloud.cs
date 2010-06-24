using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuckstazyLive.env
{
    class EnvCloud
    {
        private float x;
		private float y;
		private float counter;
		private int id;
        private Random random; 
				
		public void init(int x)
		{
			this.x = x;
            this.y = GetRandomY();
            this.id = GetRandomId();
			counter = random.Next(100) / 100.0f;
		}
		
		public void Update(float dt, float power)
		{
			x -= (float)(0.75f + 0.25f * Math.Sin(counter * 6.2832f)) * (30.0f + power * 200.0f) * dt;
			if(x<=-50.0)
			{
				x += 740;
                y = GetRandomY();
                id = GetRandomId();
			}
			counter += (0.1f + 0.9f * power) * dt;
			if(counter >= 1.0f)
				counter -= (int)counter;					
		}

        private int GetRandomId()
        {
            return random.Next(3);
        }

        private int GetRandomY()
        {
            return 40 + random.Next(90);
        }       
    }

    
}
