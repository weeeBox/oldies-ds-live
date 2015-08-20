using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Framework.core
{
    public class View : BaseElementContainer
    {
        private bool drawInnactive;
        private bool updateInnactive;
        private bool active;

        public View() : base(0, 0, FrameworkConstants.SCREEN_WIDTH, FrameworkConstants.SCREEN_HEIGHT)
        {
            drawInnactive = false;
            updateInnactive = false;
        }       

        public virtual void onShow()
        {
            active = true;
        }

        public virtual void onHide()
        {
            active = false;
        }

        public virtual void onReveal()
        {
            active = true;
        }               

        public virtual void onOvertop()
        {
            active = false;
        }        

        public void setDrawInnactive(bool b)
        {
            drawInnactive = b;
        }

        public bool isDrawInnactive()
        {
            return drawInnactive;
        }

        public void setUpdateInnactive(bool b)
        {
            updateInnactive = b;
        }

        public bool isUpdateInnactive()
        {
            return updateInnactive;
        }

        public bool isActive()
        {
            return active;
        }
    }
}
