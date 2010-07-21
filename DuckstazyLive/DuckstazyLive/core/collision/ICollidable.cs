using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuckstazyLive.core.collision
{
    public interface ICollidable : IDisposable
    {
        bool Collides(CollisionRect other);
        bool Collides(CollisionCircle other);
    }
}
