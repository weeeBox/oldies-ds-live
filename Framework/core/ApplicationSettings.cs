using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.core
{
    public class ApplicationSettings
    {        
        public const int APP_SETTING_FPS = 0;

        public void initSettingsDefaults()
        {
            // TODO
        }

        public virtual int getValue(int s)
        {
            switch (s)
            {
                case APP_SETTING_FPS:
                    return 60;                
            }
            return -1;
        }

        public bool getBoolValue(int s)
        {
            return getValue(s) != 0;
        }

        public virtual void setValue(int s, int v)
        {
        }

        public void setBoolValue(int s, bool v)
        {
            setValue(s, v ? 1 : 0);
        }
    }
}
