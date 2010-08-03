using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;
using DuckstazyLive.framework.core;

namespace DuckstazyLive.framework.resources
{
    public enum ResourceType
    {
        IMAGE,
        FONT,
        TEXTURE,
        SOUND,
        BINARY,
    }

    public interface ResourcesLoadingListener
    {
        void resourceLoaded(ResourceLoadingInfo info);
        void resourcesLoadingCompleted();
    }

    public struct ResourceLoadingInfo
    {
        public int resourceId;
        public string resourceName;
        public ResourceType type;
    }

    public class ResourceManager : Timer
    {
        private ContentManager contentManager;
        private object[] resources;
        private List<ResourceLoadingInfo> loadingQueue;
        private int loadedResourcesCount;

        public ResourceManager(int maxResourcesCount)
        {
            resources = new object[maxResourcesCount];
            loadingQueue = new List<ResourceLoadingInfo>(maxResourcesCount);
        }

        public override void Update(float dt)
        {
            
        }

        private void onResourceLoad(ResourceLoadingInfo info)
        {
            object resource = null;

            switch (info.type)
            {                
                default:
                    Debug.Assert(false, "Resource type not supported: " + info.type);
                    break;
            }
        }
    }
}
