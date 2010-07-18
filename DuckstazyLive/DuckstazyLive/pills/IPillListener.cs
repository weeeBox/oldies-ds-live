using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuckstazyLive.pills
{
    public interface IPillListener
    {
        void PillAdded(Pill pill);
        void PillRemoved(Pill pill);
    }
}
