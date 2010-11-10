using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Framework.core
{
    public class View : BaseElement, InputListener
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

        public virtual void buttonPressed(ref ButtonEvent e)
        {
        }

        public virtual void buttonReleased(ref ButtonEvent e)
        {
        }

        public virtual void keyPressed(Keys key)
        {
        }

        public virtual void keyReleased(Keys key)
        {
        }
    }
}
