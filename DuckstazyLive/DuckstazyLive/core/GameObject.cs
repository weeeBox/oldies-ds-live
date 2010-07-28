using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckstazyLive.graphics;

namespace DuckstazyLive.core
{
    public interface GameObject
    {
        void Update(float dt);
        void Draw(RenderContext context);
    }
}
