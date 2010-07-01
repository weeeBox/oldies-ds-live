using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace DuckstazyLive.core.input
{
    public interface InputListener
    {
        void KeyPressed();
        void KeyRepeated();
        void KeyReleased();
        void ButtonDown(Buttons button);
        void ButtonUp(Buttons button);        
    }
}
