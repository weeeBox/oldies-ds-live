using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.core;

namespace DuckstazyLive.app
{
    public class StartupController : ViewController, ResourceMgrDelegate
    {
        private const int VIEW_MAIN = 0;

        public StartupController(ViewController p) : base(p)
        {
            StartupView startView = new StartupView();
            addViewWithId(startView, VIEW_MAIN);
        }

        public override void activate()
        {
            base.activate();            

            DuckstazyResourceMgr rm = (DuckstazyResourceMgr) Application.sharedResourceMgr;
            rm.initLoading();
            rm.addPackToLoad(Packs.PACK_START);
            rm.loadImmediately();

            showView(VIEW_MAIN);

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
            deactivate();
        }        
    }
}
