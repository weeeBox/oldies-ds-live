using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.core;
using Microsoft.Xna.Framework.Content;

namespace DuckstazyLive.app
{
    class DuckstazyApp : Application, TimerManager
    {
        private ContentManager contentManager;        
        private List<Timer> timers = new List<Timer>();

        public DuckstazyApp(ContentManager contentManager)
        {
            this.contentManager = contentManager;
            Timer.TimerManager = this;
        }

        public override RootController createRootController()
        {
            return new DuckstazyRootController(null);
        }

        public override ApplicationSettings createAppSettings()
        {
            return new DuckstazyApplicationSettings();
        }

        public override ResourceMgr createResourceMgr()
        {
            return new DuckstazyResourceMgr(contentManager);
        }

        public override void onApplicationStart()
        {
            base.onApplicationStart();

            //float soundsVol = Application.sharedAppSettings.getBoolValue(DuckstazyApplicationSettings.BLOCKIT_APP_SETTING_SOUND_ON) ? 1 : 0;
            //SoundMgr sm = Application.sharedSoundMgr;
            //sm.setVolume(soundsVol, 1);
            //sm.setVolume(soundsVol, 2);
            //sm.setVolume(soundsVol, 3);

            //float musicVol = Application.sharedAppSettings.getBoolValue(DuckstazyApplicationSettings.BLOCKIT_APP_SETTING_MUSIC_ON) ? 1 : 0;
            //sm = Application.sharedSoundMgr;
            //sm.setVolume(musicVol, 0);

            sharedRootController.activate();
        }
        
        public void processDraw()
        {
            Application.sharedRootController.processDraw();
        }

        public void update(float dt)
        {
            GameClock.update(dt);
            fireTimers();
            Application.sharedRootController.processUpdate();
        }

        private void fireTimers()
        {
            List<Timer> newTimers = new List<Timer>(timers);
            foreach (Timer timer in newTimers)
            {
                if ((GameClock.ElapsedTime - timer.lastFired) > timer.desiredInterval)
                {
                    timer.lastFired += timer.desiredInterval;
                    if ((GameClock.ElapsedTime - timer.lastFired) > timer.desiredInterval)
                        timer.lastFired = GameClock.ElapsedTime;
                    timer.internalUpdate();
                }
            }
        }

        public void registerTimer(Timer timer)
        {
            timers.Add(timer);
            timer.lastFired = GameClock.ElapsedTime;
        }

        public void deregisterTimer(Timer timer)
        {
            timers.Remove(timer);
        }        
    }
}
