using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Framework.core
{
    public class View : BaseElementContainer
    {
        public View() : base(0, 0, FrameworkConstants.SCREEN_WIDTH, FrameworkConstants.SCREEN_HEIGHT)
        {
        }       

        public virtual void onShow()
        {
        }

        public virtual void onHide()
        {
        }        
    }
}
