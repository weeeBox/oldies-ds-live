using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.core;

namespace DuckstazyLive.app
{
    public struct ResourceBaseInfo
    {
        public DuckstazyResource resId;
        public ResourceType resType;
        public string filename;

        public ResourceBaseInfo(DuckstazyResource resId, ResourceType resType, String filename)
        {
            this.resId = resId;
            this.resType = resType;
            this.filename = filename;
        }
    }

    public enum DuckstazyPack
    {
        PACK_COMMON,
        PACK_MENU,
        PACK_GAME,
        PACK_SOUNDS,
        PACKS_COUNT,
    }

    public enum DuckstazyResource
    {
        IMG_BUTTON_BASE,
        IMG_BUTTON_STROKE_DEFAULT,
        IMG_BUTTON_STROKE_FOCUSED,
        IMG_BUTTON_CLOUD_1,
        IMG_BUTTON_CLOUD_2,
        IMG_BUTTON_CLOUD_3,
        RESOURCES_COUNT,
    }

    public class DuckstazyResources
    {
        public static ResourceBaseInfo[][] RESOURCES_PACKS =
        {
            // PACK_COMMON
            new ResourceBaseInfo[]
            {
                new ResourceBaseInfo(DuckstazyResource.IMG_BUTTON_BASE, ResourceType.RESOURCE_TYPE_TEXTURE, "button"),
                new ResourceBaseInfo(DuckstazyResource.IMG_BUTTON_STROKE_DEFAULT, ResourceType.RESOURCE_TYPE_TEXTURE, "button_black"),
                new ResourceBaseInfo(DuckstazyResource.IMG_BUTTON_STROKE_FOCUSED, ResourceType.RESOURCE_TYPE_TEXTURE, "button_white"),
                new ResourceBaseInfo(DuckstazyResource.IMG_BUTTON_CLOUD_1, ResourceType.RESOURCE_TYPE_TEXTURE, "cloud_1"),
                new ResourceBaseInfo(DuckstazyResource.IMG_BUTTON_CLOUD_2, ResourceType.RESOURCE_TYPE_TEXTURE, "cloud_2"),
                new ResourceBaseInfo(DuckstazyResource.IMG_BUTTON_CLOUD_3, ResourceType.RESOURCE_TYPE_TEXTURE, "cloud_3"),
            },
            // PACK_MENU
            new ResourceBaseInfo[]
            {
            },
            // PACK_GAME
            new ResourceBaseInfo[]
            {
            },
            // PACK_SOUNDS            
            new ResourceBaseInfo[]
            {
            },
        };
    }
}
