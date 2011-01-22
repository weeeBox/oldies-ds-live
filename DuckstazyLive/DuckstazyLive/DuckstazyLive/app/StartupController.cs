using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.core;
using DuckstazyLive.game;

namespace DuckstazyLive.app
{
    public class StartupController : ViewController, ResourceMgrDelegate
    {
        public StartupController(ViewController p) : base(p)
        {            
        }

        public override void activate()
        {
            base.activate();            

            DuckstazyResourceMgr rm = (DuckstazyResourceMgr) Application.sharedResourceMgr;
            rm.initLoading();
            rm.addPackToLoad(Packs.PACK_START);
            rm.addPackToLoad(Packs.PACK_COMMON1);
            rm.loadImmediately();

            LoadingView loadingView = new LoadingView(this);
            showView(loadingView);

            rm.resourcesDelegate = this;
            rm.initLoading();
            rm.addPackToLoad(Packs.PACK_COMMON2);
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

            GameElements.init();           

            IntroView introView = new IntroView(this);
            showView(introView);
        }        

        public int getPercentLoaded()
        {
            DuckstazyResourceMgr rm = (DuckstazyResourceMgr)Application.sharedResourceMgr;
            return rm.getPercentLoaded();
        }
    }
}
