using app.menu;
using asap.app;
using asap.ui;
using asap.sound;
using asap.resources;

namespace app
{
    public class Application : BaseApp
    {
        public static CheatManager sharedCheatMgr;

        public static InputManager sharedScreenMgr;

        public static ScreensView sharedScreensView;                

        public static AppRootController sharedRootController;

        public Application(int width, int height) : base(width, height)
        {
            sharedCheatMgr = new CheatManager();
            sharedCheatMgr.AddCheatListener(this);
            sharedScreenMgr = new InputManager(this);
            sharedScreensView = new ScreensView();            
            sharedRootController = new AppRootController();
            SetTickListener(sharedRootController);
        }

        public override void Tick(float deltaTime)
        {
            base.Tick(deltaTime);
            sharedScreensView.Tick(deltaTime);
        }

        public override void Start()
        {
            base.Start();
            sharedRootController.OnStart();
        }

        public static Application Instance
        {
            get { return (Application)GetInstance(); }
        }

        protected override ResourceMgr CreateResourceMgr()
        {
            return new AppResourceMgr();
        }

        protected override SoundMgr CreateSoundMgr()
        {
            return new SoundMgr(16); // fix me
        }
    }
}
