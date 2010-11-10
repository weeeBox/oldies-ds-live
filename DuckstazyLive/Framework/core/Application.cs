using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.core
{
    public abstract class Application
    {
        public abstract RootController createRootController();

        public abstract ApplicationSettings createAppSettings();

        public abstract ResourceMgr createResourceMgr();

        public virtual void onApplicationStart()
        {
            sharedPreferences = createPreferences();
            sharedAppSettings = createAppSettings();
            sharedResourceMgr = createResourceMgr();            
            sharedSoundMgr = createSoundMgr();
            sharedRootController = createRootController();
            sharedInputMgr = createInputManager();

            sharedInputMgr.addInputListener(sharedRootController);
        }

        public virtual void onApplicationStop()
        {
        }

        public SoundMgr createSoundMgr()
        {
            return new SoundMgr();
        }       

        public Preferences createPreferences()
        {
            return new Preferences();
        }

        public InputManager createInputManager()
        {
            return new InputManager();
        }

        public static RootController sharedRootController;
        public static ApplicationSettings sharedAppSettings;
        public static ResourceMgr sharedResourceMgr;        
        public static SoundMgr sharedSoundMgr;        
        public static Preferences sharedPreferences;
        public static InputManager sharedInputMgr;
    }
}
