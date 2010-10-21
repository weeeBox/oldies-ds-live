// This file was generated. Do not modify.
using Framework.core;

namespace DuckstazyLive.app
{
	public class Packs
	{
		public const int PACK_COMMON = 0;
		public const int PACK_MENU = 1;
		public const int PACK_GAME = 2;
		public const int PACK_SOUNDS = 3;
		public const int PACKS_COUNT = 4;
	}
	public class Res
	{
		// PACK_COMMON
		public const int IMG_BUTTON_BASE = 0;
		public const int IMG_BUTTON_STROKE_DEFAULT = 1;
		public const int IMG_BUTTON_STROKE_FOCUSED = 2;
		public const int IMG_BUTTON_CLOUD_1 = 3;
		public const int IMG_BUTTON_CLOUD_2 = 4;
		public const int IMG_BUTTON_CLOUD_3 = 5;
		public const int IMG_GRASS = 6;
		public const int IMG_PILL_FAKE = 7;
		public const int IMG_PILL_GLOW_FAKE = 8;
		// PACK_MENU
		public const int IMG_MENU_TITLE = 9;
		public const int IMG_MENU_TITLE_BACK = 10;
		// PACK_GAME
		public const int IMG_DUCK_FAKE = 11;
		// PACK_SOUNDS
		public const int RES_COUNT = 12;
	}
	public class DuckstazyResources
	{
		public static ResourceBaseInfo[][] RESOURCES_PACKS =
		{
			// PACK_COMMON
			new ResourceBaseInfo[]
			{
				new ResourceBaseInfo(Res.IMG_BUTTON_BASE, ResourceType.RESOURCE_TYPE_TEXTURE, "button"),
				new ResourceBaseInfo(Res.IMG_BUTTON_STROKE_DEFAULT, ResourceType.RESOURCE_TYPE_TEXTURE, "button_black"),
				new ResourceBaseInfo(Res.IMG_BUTTON_STROKE_FOCUSED, ResourceType.RESOURCE_TYPE_TEXTURE, "button_white"),
				new ResourceBaseInfo(Res.IMG_BUTTON_CLOUD_1, ResourceType.RESOURCE_TYPE_TEXTURE, "cloud_1"),
				new ResourceBaseInfo(Res.IMG_BUTTON_CLOUD_2, ResourceType.RESOURCE_TYPE_TEXTURE, "cloud_2"),
				new ResourceBaseInfo(Res.IMG_BUTTON_CLOUD_3, ResourceType.RESOURCE_TYPE_TEXTURE, "cloud_3"),
				new ResourceBaseInfo(Res.IMG_GRASS, ResourceType.RESOURCE_TYPE_TEXTURE, "grass"),
				new ResourceBaseInfo(Res.IMG_PILL_FAKE, ResourceType.RESOURCE_TYPE_TEXTURE, "pill_fake"),
				new ResourceBaseInfo(Res.IMG_PILL_GLOW_FAKE, ResourceType.RESOURCE_TYPE_TEXTURE, "pill_glow_fake"),
			},
			// PACK_MENU
			new ResourceBaseInfo[]
			{
				new ResourceBaseInfo(Res.IMG_MENU_TITLE, ResourceType.RESOURCE_TYPE_TEXTURE, "title"),
				new ResourceBaseInfo(Res.IMG_MENU_TITLE_BACK, ResourceType.RESOURCE_TYPE_TEXTURE, "title_back"),
			},
			// PACK_GAME
			new ResourceBaseInfo[]
			{
				new ResourceBaseInfo(Res.IMG_DUCK_FAKE, ResourceType.RESOURCE_TYPE_TEXTURE, "duck_fake"),
			},
			// PACK_SOUNDS
			new ResourceBaseInfo[]
			{
			},
		};
	}
}
