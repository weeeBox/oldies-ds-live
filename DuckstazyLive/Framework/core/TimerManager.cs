using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.core
{
    public interface TimerManager
    {
        void registerTimer(Timer timer);
        void deregisterTimer(Timer timer);        
    }
}
