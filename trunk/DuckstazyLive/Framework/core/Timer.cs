using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Framework.core
{
    public class Timer
    {
        private static TimerManager timerManager;

        public delegate void TimerDelegate();

        public TimerDelegate targetSelector;

        public float desiredInterval;
        public float lastFired = 0;

        private bool started;

        public Timer()
        {
        }

        public bool isTimerStarted()
        {
            return started;
        }

        public void startTimer()
        {
            started = true;
            timerManager.registerTimer(this);
        }

        public void stopTimer()
        {
            started = false;
            timerManager.deregisterTimer(this);
        }

        public virtual void update()
        {
            // Do nothing by default
        }

        public void setTimerInterval(float interval)
        {
            desiredInterval = interval;
        }

        public void internalUpdate()
        {
            if (targetSelector == null)
                update();
            else
                targetSelector();
        }

        public static TimerManager TimerManager
        {
            set 
            {
                Debug.Assert(timerManager == null);
                timerManager = value;
            }
        }
    }
}
