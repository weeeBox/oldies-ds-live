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

    public abstract class Timer
    {
        private const int INITIAL_LISTENERS_LIST_CAPACITY = 16;

        private float delay;
        private float elapsedTime;
        private float tickTime;
        private int repeatCount;
        private int repeatCompleted;
        private TimerState timerState;        

        private List<TimerListener> listeners;

        public Timer(float delay, int repeatCount)
        {
            this.delay = delay;
            this.repeatCount = repeatCount;            
            timerState = TimerState.CREATED;            
        }

        public Timer(float delay) : this(delay, 0)
        {            
        }

        public Timer() : this(0)
        {
        }

        protected abstract void tickTimer(float dt);
	
	    public void addToTickTime(float dt)
        {
            tickTime += dt;
            elapsedTime += dt;
        }

	    public void fireTimer()
        {
            repeatCompleted++;
            tickTimer(tickTime);

            tickTime = 0;            
            if (repeatCount != 0 && repeatCompleted == repeatCount)
            {
                stopTimer();
            }
        }

        public float getTimerTickTime()
        {
            return tickTime;
        }

        public float getTimerDelay()
        {
            return delay;
        }

        public float getTimerElapsedTime()
        {
            return elapsedTime;
        }

	    public float getTimerNumRepeated()
        {
            return repeatCompleted;
        }

        public void startTimer()
        {
            Debug.Assert(timerState == TimerState.CREATED, "Bad timer state: " + timerState);

            TimerManager.getInstance().addTimer(this);
            timerState = TimerState.RUNNING;

            fireTimerStarted();
        }

        public void pauseTimer()
        {
            Debug.Assert(isTimerPaused());

            timerState = TimerState.PAUSED;
            fireTimerPaused();
        }

        public void resumeTimer()
        {
            Debug.Assert(isTimerPaused());
            timerState = TimerState.RUNNING;

            fireTimerResumed();
        }

        public void stopTimer()
        {
            Debug.Assert(!isTimerStopped());
            timerState = TimerState.STOPPED;

            fireTimerStopped();
        }

	    public void restartTimer()
        {
            repeatCompleted = 0;
            tickTime = 0;
            elapsedTime = 0;
            if (isTimerStopped())
            {
                startTimer();
            }
            else
            {
                timerState = TimerState.RUNNING;
            }
        }

        public bool isTimerPaused()
        {
            return timerState == TimerState.PAUSED;
        }

        public bool isTimerRunning()
        {
            return timerState == TimerState.RUNNING;
        }

	    public bool isTimerStopped()
        {
            return timerState == TimerState.STOPPED;
        }       

        public void addTimerListener(TimerListener listener)
        {
            if (listeners == null)
                listeners = new List<TimerListener>(INITIAL_LISTENERS_LIST_CAPACITY);

            if (!listeners.Contains(listener))            
                listeners.Add(listener);            
        }        

        public void removeTimerListener(TimerListener listener)
        {
            Debug.Assert(listeners != null);
            listeners.Remove(listener);
        }

        private void fireTimerStarted()
        {
            Debug.Assert(listeners != null);
            foreach (TimerListener listener in listeners)
            {
                listener.timerStarted(this);
            }
        }

        private void fireTimerPaused()
        {
            Debug.Assert(listeners != null);
            foreach (TimerListener listener in listeners)
            {
                listener.timerPaused(this);
            }
        }

        private void fireTimerResumed()
        {
            Debug.Assert(listeners != null);
            foreach (TimerListener listener in listeners)
            {
                listener.timerResumed(this);
            }
        }

        private void fireTimerStopped()
        {
            Debug.Assert(listeners != null);
            foreach (TimerListener listener in listeners)
            {
                listener.timerStopped(this);
            }
        }

        private int getListenersCount()
        {
            if (listeners == null)
                return 0;

            return listeners.Count;
        }
    }
}
