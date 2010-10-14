using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.core
{
    public class View : BaseElement
    {
        public View()
        {
            // Always full screen ???
            width = Constants.SCREEN_WIDTH;
            height = Constants.SCREEN_HEIGHT;
        }

        public virtual void show()
        {
        }

        public virtual void hide()
        {
        }
    }
}
