using DuckstazyLive.framework.core;
using System.Diagnostics;
using DuckstazyLive.core.input;
using Microsoft.Xna.Framework;

namespace DuckstazyLive.foobar
{
    class InputAdapter : InputListener
    {
        public void playerDisconnected(PlayerIndex playerIndex)
        {
            Trace.WriteLine("Player disconnected: " + playerIndex);
        }

        public void playerConnected(PlayerIndex playerIndex)
        {
            Trace.WriteLine("Player connected: " + playerIndex);
        }

        public void buttonPressed(InputEvent e)
        {
            Trace.WriteLine("Button pressed: " + e.Button);
        }

        public void buttonReleased(InputEvent e)
        {
            Trace.WriteLine("Button released: " + e.Button);
        }     
    }

    public class App
    {
        private int width;
        private int height;

        private float appTime;

        private InputManager inputManager;
        private TimerManager timerManager;

        public App(int width, int height)
        {
            this.width = width;
            this.height = height;            
            
            timerManager = new TimerManager(16);
            inputManager = new InputManager(2);
            inputManager.startTimer();
        }

        /************************************************************************/
        /* Life cycle                                                           */
        /************************************************************************/
        public virtual void onStart()
        {
            InputAdapter inputListener = new InputAdapter();
            inputManager.AddInputListener(inputListener);
        }

        public virtual void onStop()
        {

        }

        public virtual void onSuspend()
        {

        }

        public virtual void onResume()
        {

        }

        /************************************************************************/
        /*                                                                      */
        /************************************************************************/

        public void tick(float dt)
        {
            appTime += dt;
            timerManager.update(dt);
            onUpdate(dt);
        }

        public virtual void onUpdate(float dt)
        {

        }

        public virtual void handleEvent()
        {

        }

        /************************************************************************/
        /* InputListener                                                        */
        /************************************************************************/

        // TODO

        /************************************************************************/
        /* Helpers                                                              */
        /************************************************************************/

        public float getAppTime()
        {
            return appTime;
        }
    }
}
