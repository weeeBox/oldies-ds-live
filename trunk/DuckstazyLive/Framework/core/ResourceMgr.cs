using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework;
using Framework.visual;

namespace Framework.core
{
    public enum ResourceType
    {
        RESOURCE_TYPE_TEXTURE,
        RESOURCE_TYPE_SOUND,
        RESOURCE_TYPE_SONG,
        RESOURCE_TYPE_PIXEL_FONT,
        RESOURCE_TYPE_VECTOR_FONT,
        RESOURCE_TYPE_BINARY,
        RESOURCE_TYPE_ATLAS
    }    

    public struct ResourceLoadInfo
    {        
        public ResourceLoadInfo(String fileName, ResourceType resType, int resId, Object[] resParams)
        {
            this.fileName = fileName;
            this.resType = resType;
            this.resId = resId;
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

        public int resId;
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

        public ResourceMgr(ContentManager cm)
        {
            contentManager = cm;

            resources = new Object[getCapacity()];
            loadQueue = new List<ResourceLoadInfo>(getCapacity());            
            setTimerInterval(LOADING_TIME_INTERVAL);
        }

        public void initLoading()
        {
            loadQueue.Clear();
            loaded = 0;         
            stopTimer();
        }

        public void addResourceToLoadQueue(String fileName, ResourceType resType, int resId, Object[] p)
        {
            Debug.Assert(resources[resId] == null, "Resource already loaded: " + resId);

            ResourceLoadInfo r = new ResourceLoadInfo(fileName, resType, resId, p);
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
            return isTimerStarted();
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

        public SpriteTexture getTexture(int resName)
        {
            return (SpriteTexture)resources[resName];
        }

        public Font getFont(int resName)
        {
            return (Font)resources[resName];
        }

        public void freeResource(int resName)
        {            
            resources[resName] = null;            
        }

        public bool isResourceLoaded(int resName)
        {
            return resources[resName] != null;
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

                case ResourceType.RESOURCE_TYPE_SONG:
                    res = loadSong(r);
                    break;

                case ResourceType.RESOURCE_TYPE_PIXEL_FONT:
                    res = loadPixelFont(r);
                    break;               

                case ResourceType.RESOURCE_TYPE_VECTOR_FONT:
                    res = loadVectorFont(r);
                    break;

                case ResourceType.RESOURCE_TYPE_BINARY:
                    res = loadBinary(r);
                    break;

                case ResourceType.RESOURCE_TYPE_ATLAS:
                    res = loadAtlas(r);
                    break;
            }

            if (r.resId >= 0)
                resources[r.resId] = res;

            return res;
        }

        private Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();

        public SpriteTexture loadTextureImage(ResourceLoadInfo r)
        {
            String name = r.getResContentName();
            Texture2D texture = loadTexture(contentManager, name);
            return new SingleTexture(texture);
        }

        public Texture2D loadTexture(ContentManager contentManager, String name)
        {            
            if (textures.ContainsKey(name))
            {
                return textures[name];
            }

            Texture2D texture = contentManager.Load<Texture2D>(name);
            textures.Add(name, texture);
            return texture;
        }

        public SoundEffect loadSound(ResourceLoadInfo r)
        {         
            return contentManager.Load<SoundEffect>(r.fileName);
        }

        public Song loadSong(ResourceLoadInfo r)
        {            
            return contentManager.Load<Song>(r.fileName);            
        }        

        public VectorFont loadVectorFont(ResourceLoadInfo r)
        {
            SpriteFont font = contentManager.Load<SpriteFont>(r.fileName);
            return new VectorFont(font);
        }

        public PixelFont loadPixelFont(ResourceLoadInfo r)
        {
            return contentManager.Load<PixelFont>(r.fileName);
        }

        public object loadBinary(ResourceLoadInfo r)
        {
            return contentManager.Load<byte[]>(r.getResContentName());
        }

        public Atlas loadAtlas(ResourceLoadInfo r)
        {            
            Atlas atlas = contentManager.Load<Atlas>(r.getResContentName());
            AtlasImage[] images = atlas.Images;
            for (int imageIndex = 0, resId = r.resId + 1; imageIndex < images.Length; ++imageIndex, ++resId)
            {                
                resources[resId] = images[imageIndex];
            }

            return atlas;
        }

        public override void update()
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
