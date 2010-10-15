using System.Collections.Generic;
using System.Diagnostics;

namespace DuckstazyLive.framework.core
{
    enum TimerState
    {
        CREATED,
        RUNNING,
        PAUSED,
        STOPPED
    }

   public class Timer
    {
        public static TimerManager timerManager;

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
    }
}
