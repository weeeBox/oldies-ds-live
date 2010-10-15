using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.core;

namespace DuckstazyLive.game
{
    class DuckstazyApp : Application
    {
        private ContentManager contentManager;

        public DuckstazyApp(ContentManager contentManager)
        {
            this.contentManager = contentManager;
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
    }
}
