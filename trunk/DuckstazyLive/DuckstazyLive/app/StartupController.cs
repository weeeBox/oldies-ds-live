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

            showView(VIEW_MAIN);

            DuckstazyResourceMgr rm = (DuckstazyResourceMgr) Application.sharedResourceMgr;
            rm.initLoading();
            rm.resourcesDelegate = this;
            rm.initLoading();
            rm.addPackToLoad((int)DuckstazyPack.PACK_COMMON);
            rm.addPackToLoad((int)DuckstazyPack.PACK_MENU);
            rm.addPackToLoad((int)DuckstazyPack.PACK_GAME);
            rm.addPackToLoad((int)DuckstazyPack.PACK_SOUNDS);            
            rm.startLoading();
        }

        public void resourceLoaded(ResourceLoadInfo res)
        {
            
        }

        public void allResourcesLoaded()
        {
            deactivate();
        }        
    }
}
