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
		public const int SND_ENV_POWER = 12;
		public const int SONG_ENV_TEX = 13;
		public const int SND_HERO_ATTACK = 14;
		public const int SND_HERO_AWAKE = 15;
		public const int SND_HERO_JUMP = 16;
		public const int SND_HERO_LAND = 17;
		public const int SND_HERO_SLEEP = 18;
		public const int SND_HERO_STEP1 = 19;
		public const int SND_HERO_STEP2 = 20;
		public const int SND_HERO_TOXIC = 21;
		public const int SND_HERO_WING1 = 22;
		public const int SND_HERO_WING2 = 23;
		public const int SND_LEVEL_START = 24;
		public const int SND_PILL_GENERATE = 25;
		public const int SND_PILL_HIGH = 26;
		public const int SND_PILL_JUMPER = 27;
		public const int SND_PILL_POWER1 = 28;
		public const int SND_PILL_POWER2 = 29;
		public const int SND_PILL_POWER3 = 30;
		public const int SND_PILL_TOXIC_BORN = 31;
		public const int SND_PILL_WARNING = 32;
		public const int SND_UI_CLICK = 33;
		public const int SND_UI_FOCUS = 34;
		public const int RES_COUNT = 35;
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
				new ResourceBaseInfo(Res.SND_ENV_POWER, ResourceType.RESOURCE_TYPE_SOUND, "Env_rPowerSnd"),
				new ResourceBaseInfo(Res.SONG_ENV_TEX, ResourceType.RESOURCE_TYPE_SONG, "Env_rTex2Snd"),
				new ResourceBaseInfo(Res.SND_HERO_ATTACK, ResourceType.RESOURCE_TYPE_SOUND, "HeroMedia_rAttackSnd"),
				new ResourceBaseInfo(Res.SND_HERO_AWAKE, ResourceType.RESOURCE_TYPE_SOUND, "HeroMedia_rAwakeSnd"),
				new ResourceBaseInfo(Res.SND_HERO_JUMP, ResourceType.RESOURCE_TYPE_SOUND, "HeroMedia_rJumpSnd"),
				new ResourceBaseInfo(Res.SND_HERO_LAND, ResourceType.RESOURCE_TYPE_SOUND, "HeroMedia_rLandSnd"),
				new ResourceBaseInfo(Res.SND_HERO_SLEEP, ResourceType.RESOURCE_TYPE_SOUND, "HeroMedia_rSleepSnd"),
				new ResourceBaseInfo(Res.SND_HERO_STEP1, ResourceType.RESOURCE_TYPE_SOUND, "HeroMedia_rStepSnd1"),
				new ResourceBaseInfo(Res.SND_HERO_STEP2, ResourceType.RESOURCE_TYPE_SOUND, "HeroMedia_rStepSnd2"),
				new ResourceBaseInfo(Res.SND_HERO_TOXIC, ResourceType.RESOURCE_TYPE_SOUND, "HeroMedia_rToxicSnd"),
				new ResourceBaseInfo(Res.SND_HERO_WING1, ResourceType.RESOURCE_TYPE_SOUND, "HeroMedia_rWingSnd1"),
				new ResourceBaseInfo(Res.SND_HERO_WING2, ResourceType.RESOURCE_TYPE_SOUND, "HeroMedia_rWingSnd2"),
				new ResourceBaseInfo(Res.SND_LEVEL_START, ResourceType.RESOURCE_TYPE_SOUND, "Level_rStartSnd"),
				new ResourceBaseInfo(Res.SND_PILL_GENERATE, ResourceType.RESOURCE_TYPE_SOUND, "PillsMedia_rGenerateSnd"),
				new ResourceBaseInfo(Res.SND_PILL_HIGH, ResourceType.RESOURCE_TYPE_SOUND, "PillsMedia_rHighSnd"),
				new ResourceBaseInfo(Res.SND_PILL_JUMPER, ResourceType.RESOURCE_TYPE_SOUND, "PillsMedia_rJumperSnd"),
				new ResourceBaseInfo(Res.SND_PILL_POWER1, ResourceType.RESOURCE_TYPE_SOUND, "PillsMedia_rPower1Snd"),
				new ResourceBaseInfo(Res.SND_PILL_POWER2, ResourceType.RESOURCE_TYPE_SOUND, "PillsMedia_rPower2Snd"),
				new ResourceBaseInfo(Res.SND_PILL_POWER3, ResourceType.RESOURCE_TYPE_SOUND, "PillsMedia_rPower3Snd"),
				new ResourceBaseInfo(Res.SND_PILL_TOXIC_BORN, ResourceType.RESOURCE_TYPE_SOUND, "PillsMedia_rToxicBornSnd"),
				new ResourceBaseInfo(Res.SND_PILL_WARNING, ResourceType.RESOURCE_TYPE_SOUND, "PillsMedia_rWarningSnd"),
				new ResourceBaseInfo(Res.SND_UI_CLICK, ResourceType.RESOURCE_TYPE_SOUND, "ui_sfxClick"),
				new ResourceBaseInfo(Res.SND_UI_FOCUS, ResourceType.RESOURCE_TYPE_SOUND, "ui_sfxOn"),
			},
		};
	}
}
