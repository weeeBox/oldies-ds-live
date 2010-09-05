using Microsoft.Xna.Framework;
namespace DuckstazyLive.core.input
{
    public interface InputListener
    {
        void playerDisconnected(PlayerIndex playerIndex);
        void playerConnected(PlayerIndex playerIndex);

        void buttonPressed(InputEvent e);
        void buttonReleased(InputEvent e);        
    }
}
