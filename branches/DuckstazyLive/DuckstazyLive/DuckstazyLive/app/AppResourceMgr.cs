using System;

using System.Collections.Generic;

using asap.resources;
using asap.graphics;
using asap.anim;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;

namespace app
{
    public class AppResourceMgr : ResourceMgr
    {
        public AppResourceMgr() : base(Res.RES_COUNT, Resources.PACKS)
        {
        }        
    }    
}