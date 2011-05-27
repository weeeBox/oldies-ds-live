﻿using app;
using asap.core;
using asap.resources;
using System;
using DuckstazyLive.app.game;

namespace DuckstazyLive.app
{
    public class StartupController : Controller, ResourceMgrListener
    {
        public override void Start(int param)
        {
            base.Start(param);            

            ResourceMgr rm = Application.sharedResourceMgr;
            rm.InitLoading();
            rm.AddPackToLoad(ResPacks.PACK_START);
            rm.AddPackToLoad(ResPacks.PACK_COMMON1);
            rm.LoadImmediately();

            LoadingScreen loadingView = new LoadingScreen(this);
            Application.sharedScreensView.StartScreen(loadingView);
                        
            rm.InitLoading();            
            rm.AddPackToLoad(ResPacks.PACK_MENU);
            rm.AddPackToLoad(ResPacks.PACK_GAME);
            rm.AddPackToLoad(ResPacks.PACK_SOUNDS);            
            rm.StartLoading(this);
        }        

        public void allResourcesLoaded()
        {
            Application.sharedResourceMgr.UnloadPack(ResPacks.PACK_START);

            GameElements.init();

            IntroScreen introView = new IntroScreen(this);
            Application.sharedScreensView.StartScreen(introView);            
        }        

        public int getPercentLoaded()
        {            
            return Application.sharedResourceMgr.GetPercentLoaded();
        }

        public void resourceLoaded(ref ResourceLoadInfo res)
        {            
        }

        public override void Stop(int param)
        {
            base.Stop(param);            
            AppRootController.StartController(AppRootController.menuController, 0);
        }
    }
}