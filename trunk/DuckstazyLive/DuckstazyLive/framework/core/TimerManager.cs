using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace DuckstazyLive.framework.core
{
    public class TimerManager
    {
        private static TimerManager instance;

        private Timer[] timers;
        private int timersCount;

        public TimerManager(int maxTimersCount)
        {
            Debug.Assert(instance == null, "Instance already initialized");
            instance = this;

            timers = new Timer[maxTimersCount];
            timersCount = 0;
        }

        public void addTimer(Timer timer)
        {
            Debug.Assert(timersCount < timers.Length, "Max timers count reached");
            timers[timersCount] = timer;
            timersCount++;

            Trace.WriteLine("TimerManager. Add timer. Total timers count: " + timersCount);
        }

        public void update(float dt)
        {
            for (int timerIndex = 0; timerIndex < getTimersCount(); ++timerIndex)
            {
                Timer timer = getTimer(timerIndex);

                if (timer.isTimerPaused())
                    continue;

                if (timer.isTimerStopped())
                {
                    removeTimer(timerIndex);
                    timerIndex--;
                    continue;
                }

                timer.addToTickTime(dt);

                if (timer.getTimerTickTime() >= timer.getTimerDelay())
                {
                    timer.fireTimer();
                    if (timer.isTimerStopped())
                    {
                        removeTimer(timerIndex);
                        timerIndex--;
                    }
                }
            }
        }

        private Timer getTimer(int timerIndex)
        {
            Debug.Assert(timerIndex >= 0 && timerIndex < timersCount, timerIndex + "<" + timersCount);
            return timers[timerIndex];
        }

        private void removeTimer(int timerIndex)
        {
            Debug.Assert(timerIndex >= 0 && timerIndex < timersCount, timerIndex + "<" + timersCount);        

	        Timer timer = timers[timerIndex];
	        timers[timerIndex] = timers[timersCount - 1];
	        timersCount--;
	        Trace.WriteLine("TimerManager. Remove timer. Total timers count: " + timersCount);
        }

        private int getTimersCount()
        {
            return timersCount;
        }

        public static TimerManager getInstance()
        {
            return instance;
        }
    }
}
