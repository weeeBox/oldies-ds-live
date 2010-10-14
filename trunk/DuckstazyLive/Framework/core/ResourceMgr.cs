using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Framework.core
{
    public enum ResourceType
    {
        RESOURCE_TYPE_TEXTURE,        
        RESOURCE_TYPE_SOUND,        
        RESOURCE_TYPE_FONT,        
        RESOURCE_TYPE_BINARY
    }        

    public struct ResourceLoadInfo
    {        
        public ResourceLoadInfo(String fileName, ResourceType resType, int resName, Object[] resParams)
        {
            this.fileName = fileName;
            this.resType = resType;
            this.resName = resName;
            this.resParams = resParams;
        }

        public String getResContentName()
        {
            int idx = fileName.IndexOf('.');
            if (idx < 0)
                return fileName;
            else
                return fileName.Substring(0, idx);
        }

        public int resName;
        public ResourceType resType;
        public String fileName;
        public Object[] resParams;
    }

    public interface ResourceMgrDelegate
    {
        void resourceLoaded(ResourceLoadInfo res);
        void allResourcesLoaded();
    }

    public abstract class ResourceMgr : Timer
    {
        Object[] resources;
        public ResourceMgrDelegate resourcesDelegate;
        List<ResourceLoadInfo> loadQueue;        
        int loaded;

        ContentManager contentManager;

        private const float LOADING_TIME_INTERVAL = 1.0f / 20.0f;

        public ResourceMgr(ContentManager cm) : base(LOADING_TIME_INTERVAL)
        {
            contentManager = cm;

            resources = new Object[getCapacity()];
            loadQueue = new List<ResourceLoadInfo>(getCapacity());            
        }

        public void initLoading()
        {
            loadQueue.Clear();
            loaded = 0;         
            stopTimer();
        }

        public void addResourceToLoadQueue(String fileName, ResourceType resType, int resName, Object[] p)
        {
            Debug.Assert(resources[resName] == null, "Resource already loaded: " + resName);

            ResourceLoadInfo r = new ResourceLoadInfo(fileName, resType, resName, p);
            loadQueue.Add(r);            
        }

        public void startLoading()
        {
            GC.Collect();
            startTimer();
        }

        public void loadImmediately()
        {
            foreach (ResourceLoadInfo r in loadQueue)
            {
                if (loadResource(r) != null)
                {
                    loaded++;
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        public bool isBusy()
        {
            return isTimerRunning();
        }

        public int getPercentLoaded()
        {
            if (loadQueue.Count == 0)
            {
                return 100;
            }
            else
            {
                return ((100 * loaded) / loadQueue.Count);
            }
        }

        public object getResource(int resName)
        {
            return resources[resName];
        }

        public void freeResource(int resName)
        {
            if (resources[resName] != null && resources[resName] is Texture2D)
            {
                ((Texture2D)resources[resName]).setTexture(null);
            }
            else
            {
                resources[resName] = null;
            }
        }

        public abstract int getCapacity();

        public object loadResource(ResourceLoadInfo r)
        {
            object res = null;

            switch (r.resType)
            {
                case ResourceType.RESOURCE_TYPE_TEXTURE:
                    res = loadTextureImage(r);
                    break;              

                case ResourceType.RESOURCE_TYPE_SOUND:                
                    res = loadSound(r);
                    break;

                case ResourceType.RESOURCE_TYPE_FONT:
                    res = loadFont(r);
                    break;               

                case ResourceType.RESOURCE_TYPE_BINARY:
                    res = loadBinary(r);
                    break;
            }

            if (r.resName >= 0)
                resources[r.resName] = res;

            return res;
        }

        private Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();

        public Texture2D loadTextureImage(ResourceLoadInfo r)
        {
            String name = r.getResContentName();            
            if (textures.ContainsKey(name))
            {
                return textures[name];
            }

            Texture2D texture = cm.Load<Texture2D>(name);
            textures.Add(name, texture);
            return texture;            
        }

        public object loadSound(ResourceLoadInfo r)
        {
            String name = r.fileName;

            if (name.EndsWith(".wav"))
            {
                name = name.Substring(0, name.Length - 4);
                return contentManager.Load<SoundEffect>(name);                         
            }
            else if (name.EndsWith(".mp3"))
            {
                name = name.Substring(0, name.Length - 4);
                object m = contentManager.Load<Song>(name);
                System.Threading.Thread.Sleep(500);
                return m;
            }

            return null;
        }

        public object loadFont(ResourceLoadInfo r)
        {
            throw NotImplementedException();
        }

        public object loadBinary(ResourceLoadInfo r)
        {
            return contentManager.Load<byte[]>(r.getResContentName());
        }

        public override void tickTimer(float dt)
        {
            ResourceLoadInfo r = loadQueue[loaded];
            if (loadResource(r) != null)
            {
                loaded++;
                if (resourcesDelegate != null)
                    resourcesDelegate.resourceLoaded(r);

                if (loaded == loadQueue.Count)
                {
                    if (resourcesDelegate != null)
                    {
                        GC.Collect();
                        resourcesDelegate.allResourcesLoaded();
                    }
                    stopTimer();
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
