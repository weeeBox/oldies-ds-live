using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace DuckstazyLive.framework.core
{
    public abstract class Timer
    {
        private float elapsedTime;
        private float delay;
        private bool running;

        public Timer() : this(0)
        {            
        }

        public Timer(float delay)
        {
            this.delay = delay;
        }

        public abstract void Update(float dt);

        public void StartTimer()
        {
            running = true;
            elapsedTime = 0;

            Application.Instance.TimerManager.AddTimer(this);
        }       

        public void StopTimer()
        {
            running = false;
        }

        public bool Running
        {
            get { return running; }
        }

        public float ElapsedTime
        {
            get { return elapsedTime; }
            set { elapsedTime = value; }
        }

        public float Delay
        {
            get { return delay; }
            set 
            {
                Debug.Assert(value > 0, "Value must be positive: " + value);
                delay = value;
            }
        }
    }
}
