using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuckstazyLive.framework.core
{
    public interface TimerListener
    {
        void timerStarted(Timer t);
	    void timerPaused(Timer t);
	    void timerResumed(Timer t);
	    void timerStopped(Timer t);       
    }
}
