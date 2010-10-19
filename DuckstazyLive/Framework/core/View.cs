using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.core
{
    public class View : BaseElement
    {
        public View() : this (0, 0, FrameworkConstants.SCREEN_WIDTH, FrameworkConstants.SCREEN_HEIGHT)
        {
        }

        public View(int x, int y, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public virtual void show()
        {
        }

        public virtual void hide()
        {
        }
    }
}
