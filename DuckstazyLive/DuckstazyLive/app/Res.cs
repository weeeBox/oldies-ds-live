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
		public const int IMG_GRASS1 = 3;
		public const int IMG_GRASS2 = 4;
		public const int IMG_CLOUD_1 = 5;
		public const int IMG_CLOUD_2 = 6;
		public const int IMG_CLOUD_3 = 7;
		// PACK_MENU
		public const int IMG_MENU_TITLE = 8;
		public const int IMG_MENU_TITLE_BACK = 9;
		// PACK_GAME
		public const int IMG_DUCK = 10;
		public const int IMG_DUCK_SLEEP = 11;
		public const int IMG_EYE1 = 12;
		public const int IMG_EYE2 = 13;
		public const int IMG_WING = 14;
		public const int IMG_STAR = 15;
		public const int IMG_PILL_1 = 16;
		public const int IMG_PILL_2 = 17;
		public const int IMG_PILL_3 = 18;
		public const int IMG_PILL_1P = 19;
		public const int IMG_PILL_2P = 20;
		public const int IMG_PILL_3P = 21;
		public const int IMG_PILL_TOXIC_1 = 22;
		public const int IMG_PILL_TOXIC_2 = 23;
		public const int IMG_PILL_SLEEP = 24;
		public const int IMG_PILL_HIGH = 25;
		public const int IMG_SMILE_1 = 26;
		public const int IMG_SMILE_2 = 27;
		public const int IMG_SMILE_3 = 28;
		public const int IMG_EYES_1 = 29;
		public const int IMG_EYES_2 = 30;
		public const int IMG_POWER_1 = 31;
		public const int IMG_POWER_2 = 32;
		public const int IMG_POWER_3 = 33;
		public const int IMG_POWER_4 = 34;
		public const int IMG_FX_ACID = 35;
		public const int IMG_FX_BUBBLE = 36;
		public const int IMG_FX_STAR = 37;
		public const int IMG_FX_WARNING = 38;
		public const int IMG_FX_HINT_ARROW = 39;
		// PACK_SOUNDS
		public const int SND_ENV_POWER = 40;
		public const int SONG_ENV_TEX = 41;
		public const int SND_HERO_ATTACK = 42;
		public const int SND_HERO_AWAKE = 43;
		public const int SND_HERO_JUMP = 44;
		public const int SND_HERO_LAND = 45;
		public const int SND_HERO_SLEEP = 46;
		public const int SND_HERO_STEP1 = 47;
		public const int SND_HERO_STEP2 = 48;
		public const int SND_HERO_TOXIC = 49;
		public const int SND_HERO_WING1 = 50;
		public const int SND_HERO_WING2 = 51;
		public const int SND_LEVEL_START = 52;
		public const int SND_PILL_GENERATE = 53;
		public const int SND_PILL_HIGH = 54;
		public const int SND_PILL_JUMPER = 55;
		public const int SND_PILL_POWER1 = 56;
		public const int SND_PILL_POWER2 = 57;
		public const int SND_PILL_POWER3 = 58;
		public const int SND_PILL_TOXIC_BORN = 59;
		public const int SND_PILL_WARNING = 60;
		public const int SND_UI_CLICK = 61;
		public const int SND_UI_FOCUS = 62;
		public const int RES_COUNT = 63;
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
				new ResourceBaseInfo(Res.IMG_GRASS1, ResourceType.RESOURCE_TYPE_TEXTURE, "grass1"),
				new ResourceBaseInfo(Res.IMG_GRASS2, ResourceType.RESOURCE_TYPE_TEXTURE, "grass2"),
				new ResourceBaseInfo(Res.IMG_CLOUD_1, ResourceType.RESOURCE_TYPE_TEXTURE, "cloud_1"),
				new ResourceBaseInfo(Res.IMG_CLOUD_2, ResourceType.RESOURCE_TYPE_TEXTURE, "cloud_1"),
				new ResourceBaseInfo(Res.IMG_CLOUD_3, ResourceType.RESOURCE_TYPE_TEXTURE, "cloud_1"),
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
				new ResourceBaseInfo(Res.IMG_DUCK, ResourceType.RESOURCE_TYPE_TEXTURE, "duck_fake"),
				new ResourceBaseInfo(Res.IMG_DUCK_SLEEP, ResourceType.RESOURCE_TYPE_TEXTURE, "duck_fake"),
				new ResourceBaseInfo(Res.IMG_EYE1, ResourceType.RESOURCE_TYPE_TEXTURE, "eye1"),
				new ResourceBaseInfo(Res.IMG_EYE2, ResourceType.RESOURCE_TYPE_TEXTURE, "eye2"),
				new ResourceBaseInfo(Res.IMG_WING, ResourceType.RESOURCE_TYPE_TEXTURE, "wing"),
				new ResourceBaseInfo(Res.IMG_STAR, ResourceType.RESOURCE_TYPE_TEXTURE, "star"),
				new ResourceBaseInfo(Res.IMG_PILL_1, ResourceType.RESOURCE_TYPE_TEXTURE, "pill_1"),
				new ResourceBaseInfo(Res.IMG_PILL_2, ResourceType.RESOURCE_TYPE_TEXTURE, "pill_2"),
				new ResourceBaseInfo(Res.IMG_PILL_3, ResourceType.RESOURCE_TYPE_TEXTURE, "pill_3"),
				new ResourceBaseInfo(Res.IMG_PILL_1P, ResourceType.RESOURCE_TYPE_TEXTURE, "pill_1p"),
				new ResourceBaseInfo(Res.IMG_PILL_2P, ResourceType.RESOURCE_TYPE_TEXTURE, "pill_2p"),
				new ResourceBaseInfo(Res.IMG_PILL_3P, ResourceType.RESOURCE_TYPE_TEXTURE, "pill_3p"),
				new ResourceBaseInfo(Res.IMG_PILL_TOXIC_1, ResourceType.RESOURCE_TYPE_TEXTURE, "toxic1"),
				new ResourceBaseInfo(Res.IMG_PILL_TOXIC_2, ResourceType.RESOURCE_TYPE_TEXTURE, "toxic2"),
				new ResourceBaseInfo(Res.IMG_PILL_SLEEP, ResourceType.RESOURCE_TYPE_TEXTURE, "pill_sleep"),
				new ResourceBaseInfo(Res.IMG_PILL_HIGH, ResourceType.RESOURCE_TYPE_TEXTURE, "pill_high"),
				new ResourceBaseInfo(Res.IMG_SMILE_1, ResourceType.RESOURCE_TYPE_TEXTURE, "smile1"),
				new ResourceBaseInfo(Res.IMG_SMILE_2, ResourceType.RESOURCE_TYPE_TEXTURE, "smile2"),
				new ResourceBaseInfo(Res.IMG_SMILE_3, ResourceType.RESOURCE_TYPE_TEXTURE, "smile3"),
				new ResourceBaseInfo(Res.IMG_EYES_1, ResourceType.RESOURCE_TYPE_TEXTURE, "eyes1"),
				new ResourceBaseInfo(Res.IMG_EYES_2, ResourceType.RESOURCE_TYPE_TEXTURE, "eyes2"),
				new ResourceBaseInfo(Res.IMG_POWER_1, ResourceType.RESOURCE_TYPE_TEXTURE, "power1"),
				new ResourceBaseInfo(Res.IMG_POWER_2, ResourceType.RESOURCE_TYPE_TEXTURE, "power2"),
				new ResourceBaseInfo(Res.IMG_POWER_3, ResourceType.RESOURCE_TYPE_TEXTURE, "power3"),
				new ResourceBaseInfo(Res.IMG_POWER_4, ResourceType.RESOURCE_TYPE_TEXTURE, "power4"),
				new ResourceBaseInfo(Res.IMG_FX_ACID, ResourceType.RESOURCE_TYPE_TEXTURE, "fx_acid"),
				new ResourceBaseInfo(Res.IMG_FX_BUBBLE, ResourceType.RESOURCE_TYPE_TEXTURE, "fx_bubble"),
				new ResourceBaseInfo(Res.IMG_FX_STAR, ResourceType.RESOURCE_TYPE_TEXTURE, "fx_star"),
				new ResourceBaseInfo(Res.IMG_FX_WARNING, ResourceType.RESOURCE_TYPE_TEXTURE, "fx_warning"),
				new ResourceBaseInfo(Res.IMG_FX_HINT_ARROW, ResourceType.RESOURCE_TYPE_TEXTURE, "fx_hint_arrow"),
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
