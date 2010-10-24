using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuckstazyLive.game
{
    public interface PillLogicListener
    {
        void pillLogic(Pill pill, String msg, float dt);
    }
}
