using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.core;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;

namespace DuckstazyLive.app
{
    public class DuckstazyResourceMgr : ResourceMgr
    {
        public DuckstazyResourceMgr(ContentManager cm)
            : base(cm)
        {
        }

        public int getPacksCount()
        {
            return (int)DuckstazyPack.PACKS_COUNT;
        }

        public override int getCapacity()
        {
            return (int)DuckstazyResource.RESOURCES_COUNT;
        }

        public void addPackToLoad(int n)
        {
            Debug.Assert(n >= 0 && n < DuckstazyResources.RESOURCES_PACKS.Length);

            ResourceBaseInfo[] pack = DuckstazyResources.RESOURCES_PACKS[n];
            for (int resIndex = 0; resIndex < pack.Length; ++resIndex)
            {
                object[] resParams = null;
                DuckstazyResource resId = pack[resIndex].resId;
                ResourceType resType = pack[resIndex].resType;
                string filename = pack[resIndex].filename;
                addResourceToLoadQueue(filename, resType, (int)resId, resParams);
            }
        }

        public void freePack(int n)
        {
            Debug.Assert(n >= 0 && n < DuckstazyResources.RESOURCES_PACKS.Length);

            ResourceBaseInfo[] pack = DuckstazyResources.RESOURCES_PACKS[n];
            for (int resIndex = 0; resIndex < pack.Length; ++resIndex)
            {
                freeResource((int)pack[resIndex].resId);
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        public void freeAll()
        {
            for (int packIndex = 0; packIndex < getPacksCount(); packIndex++)
            {
                freePack(packIndex);
            }
        }
    }
}
