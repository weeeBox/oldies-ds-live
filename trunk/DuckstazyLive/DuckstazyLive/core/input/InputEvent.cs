using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DuckstazyLive.core.input
{
    public struct InputEvent
    {
        public PlayerIndex PlayerIndex;
        public GamePadState GamePadState;
        public Buttons Button;

        public InputEvent(PlayerIndex player, GamePadState state, Buttons button)
        {
            PlayerIndex = player;
            GamePadState = state;
            Button = button;
        }
    }
}
