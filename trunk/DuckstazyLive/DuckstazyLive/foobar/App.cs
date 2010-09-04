using DuckstazyLive.framework.core;
using System.Diagnostics;

namespace DuckstazyLive.foobar
{
    public class App
    {
        private int width;
        private int height;

        private float appTime;        

        public App(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        /************************************************************************/
        /* Life cycle                                                           */
        /************************************************************************/
        public virtual void onStart()
        {
            
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
            onUpdate(dt);
        }

        public virtual void onUpdate(float dt)
        {

        }

        public virtual void handleEvent()
        {

        }

        /************************************************************************/
        /* Helpers                                                              */
        /************************************************************************/

        public float getAppTime()
        {
            return appTime;
        }
    }
}
