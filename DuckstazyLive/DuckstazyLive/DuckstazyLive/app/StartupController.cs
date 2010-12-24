using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.core;

namespace DuckstazyLive.app
{
    public class StartupController : ViewController, ResourceMgrDelegate
    {
        private const int VIEW_LOADING = 0;
        private const int VIEW_INTRO = 1;

        public StartupController(ViewController p) : base(p)
        {
            LoadingView startView = new LoadingView();
            addViewWithId(startView, VIEW_LOADING);

            IntroView introView = new IntroView(this);
            addViewWithId(introView, VIEW_INTRO);
        }

        public override void activate()
        {
            base.activate();            

            DuckstazyResourceMgr rm = (DuckstazyResourceMgr) Application.sharedResourceMgr;
            rm.initLoading();
            rm.addPackToLoad(Packs.PACK_START);
            rm.loadImmediately();

            showView(VIEW_LOADING);

            rm.resourcesDelegate = this;
            rm.initLoading();
            rm.addPackToLoad(Packs.PACK_COMMON);
            rm.addPackToLoad(Packs.PACK_MENU);
            rm.addPackToLoad(Packs.PACK_GAME);
            rm.addPackToLoad(Packs.PACK_SOUNDS);            
            rm.startLoading();
        }

        public void resourceLoaded(ResourceLoadInfo res)
        {
            
        }

        public void allResourcesLoaded()
        {
            DuckstazyResourceMgr rm = (DuckstazyResourceMgr)Application.sharedResourceMgr;
            rm.freePack(Packs.PACK_START);

            showView(VIEW_INTRO);
        }        
    }
}
