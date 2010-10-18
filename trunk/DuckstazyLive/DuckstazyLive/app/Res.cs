using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.core;

namespace DuckstazyLive.app
{
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
        IMG_MENU_TITLE,
        IMG_MENU_TITLE_BACK,
        IMG_GRASS,
        IMG_DUCK_FAKE,
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
                new ResourceBaseInfo(DuckstazyResource.IMG_GRASS, ResourceType.RESOURCE_TYPE_TEXTURE, "grass"),
            },
            // PACK_MENU
            new ResourceBaseInfo[]
            {
                new ResourceBaseInfo(DuckstazyResource.IMG_MENU_TITLE, ResourceType.RESOURCE_TYPE_TEXTURE, "title"),
                new ResourceBaseInfo(DuckstazyResource.IMG_MENU_TITLE_BACK, ResourceType.RESOURCE_TYPE_TEXTURE, "title_back"),
            },
            // PACK_GAME
            new ResourceBaseInfo[]
            {
                new ResourceBaseInfo(DuckstazyResource.IMG_DUCK_FAKE, ResourceType.RESOURCE_TYPE_TEXTURE, "duck_fake"),
            },
            // PACK_SOUNDS            
            new ResourceBaseInfo[]
            {
            },
        };
    }
}
