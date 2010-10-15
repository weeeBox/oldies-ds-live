using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.core
{
    public class TimerManager
    {
        float appTime;
        List<Timer> timers = new List<Timer>();

        public void update(float dt)
        {
            appTime += dt;
            fireTimers();
        }

        private void fireTimers()
        {            
            List<Timer> newTimers = new List<Timer>(timers);
            foreach (Timer timer in newTimers)
            {
                if ((appTime - timer.lastFired) > timer.desiredInterval)
                {
                    timer.lastFired += timer.desiredInterval;
                    if ((appTime - timer.lastFired) > timer.desiredInterval)
                        timer.lastFired = appTime;
                    timer.internalUpdate();
                }
            }
        }


        public void registerTimer(Timer timer)
        {
            timers.Add(timer);
            timer.lastFired = appTime;
        }

        public void deregisterTimer(Timer timer)
        {
            timers.Remove(timer);
        }

        public float AppTime
        {
            get { return appTime; }
        }
    }
}
