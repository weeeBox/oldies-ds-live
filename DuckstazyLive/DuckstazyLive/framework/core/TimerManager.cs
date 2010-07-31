using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace DuckstazyLive.framework.core
{
    public class TimerManager
    {        
        private Timer[] timers;
        private int timersCount;
        
        public TimerManager(int maxTimersCount)
        {
            timers = new Timer[maxTimersCount];
        }

        public void AddTimer(Timer timer)
        {
            Debug.Assert(timersCount < timers.Length, "Out of timers: " + timersCount + "<" + timers.Length);
            timers[timersCount] = timer;
            timersCount++;
        }

        public void Update(float dt)
        {
            for (int timerIndex = 0; timerIndex < timersCount; timerIndex++)
            {
                Timer timer = timers[timerIndex];

                timer.ElapsedTime += dt;
                if (timer.ElapsedTime >= timer.Delay)
                {                    
                    timer.Update(timer.ElapsedTime);
                    timer.ElapsedTime = 0;

                    if (timer.Stopped)
                    {                        
                        timers[timerIndex] = timers[timersCount - 1];
                        timersCount--;
                        timerIndex--;
                    }
                }
            }
        }
    }
}
