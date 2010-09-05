using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DuckstazyLive.core.input
{
    public struct InputEvent
    {
        public PlayerIndex PlayerIndex;
        public GamePadState GamePadState;
        public Buttons Button;

        public InputEvent(Buttons button, PlayerIndex player, GamePadState state)
        {
            PlayerIndex = player;
            GamePadState = state;
            Button = button;
        }
    }
}
