using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;

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

    public class ResourceManager
    {
        private ContentManager contentManager;
        private object[] resources;
        private List<ResourceLoadingInfo> loadingQueue;

        public ResourceManager(int maxResourcesCount)
        {
            resources = new object[maxResourcesCount];
            loadingQueue = new List<ResourceLoadingInfo>(maxResourcesCount);
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
