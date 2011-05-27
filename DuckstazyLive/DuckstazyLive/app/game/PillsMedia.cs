using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using app;
using asap.graphics;

namespace DuckstazyLive.app.game
{
    public class PillsMedia
    {
        // [Embed(source="gfx/p1.png")]
        // private var rPowerImg1:Class;

        // [Embed(source="gfx/p2.png")]
        // private var rPowerImg2:Class;

        // [Embed(source="gfx/p3.png")]
        // private var rPowerImg3:Class;

        // [Embed(source="gfx/pp1.png")]
        // private var rSuperPowerImg1:Class;

        // [Embed(source="gfx/pp2.png")]
        // private var rSuperPowerImg2:Class;

        // [Embed(source="gfx/pp3.png")]
        // private var rSuperPowerImg3:Class;

        // //[Embed(source="gfx/pn.png")]
        // //private var rEmoImg:Class;

        // [Embed(source="gfx/pn1.png")]
        // private var rNidImg1:Class;

        // [Embed(source="gfx/pn2.png")]
        // private var rNidImg2:Class;

        // [Embed(source="gfx/pn3.png")]
        // private var rNidImg3:Class;

        // [Embed(source="gfx/pn4.png")]
        // private var rNidImg4:Class;

        // [Embed(source="gfx/ph.png")]
        // private var rHighImg:Class;

        // [Embed(source="gfx/pt.png")]
        // private var rToxicImg:Class;

        // [Embed(source="gfx/pt2.png")]
        // private var rToxic2Img:Class;

        // [Embed(source="gfx/pcure.png")]
        // private var rCureImg:Class;

        // [Embed(source="gfx/phole.png")]
        // private var gfxHole:Class;


        // [Embed(source="gfx/psleep.png")]
        // private var rSleepImg:Class;

        // [Embed(source="gfx/emo_smile1.png")]
        // private var rSmileImg1:Class;

        // [Embed(source="gfx/emo_smile2.png")]
        // private var rSmileImg2:Class;

        // [Embed(source="gfx/emo_smile3.png")]
        // private var rSmileImg3:Class;

        // [Embed(source="gfx/emo_eyes1.png")]
        // private var rEyesImg1:Class;

        // [Embed(source="gfx/emo_eyes2.png")]
        // private var rEyesImg2:Class;

        // [Embed(source="sfx/warning.mp3")]
        // private var rWarningSnd:Class;

        // [Embed(source="sfx/toxic_born.mp3")]
        // private var rToxicBornSnd:Class;

        // [Embed(source="sfx/generate.mp3")]
        // private var rGenerateSnd:Class;

        // [Embed(source="sfx/p1.mp3")]
        // private var rPower1Snd:Class;

        // [Embed(source="sfx/p2.mp3")]
        // private var rPower2Snd:Class;

        // [Embed(source="sfx/p3.mp3")]
        // private var rPower3Snd:Class;

        // //[Embed(source="sfx/heal.mp3")]
        //// private var rHealSnd:Class;

        // [Embed(source="sfx/jumper.mp3")]
        // private var rJumperSnd:Class;

        // [Embed(source="sfx/high.mp3")]
        // private var rHighSnd:Class;

        public int[] sndPowers;
        public int sndHeal;
        public int sndJumper;
        public int sndHigh;
        public int sndWarning;
        public int sndToxicBorn;
        public int sndGenerate;

        // Основы таблеток
        public GameTexture imgPower1;
        public GameTexture imgPower2;
        public GameTexture imgPower3;

        public GameTexture imgPPower1;
        public GameTexture imgPPower2;
        public GameTexture imgPPower3;

        public GameTexture[] imgNids;

        public GameTexture imgHigh;

        public GameTexture imgToxic;
        public GameTexture imgToxic2;

        public GameTexture imgCure;
        public GameTexture imgHole;
        public GameTexture imgSleep;

        // Эмоции таблеток
        //public var imgEmo:SpriteTexture;
        public GameTexture imgSmile1;
        public GameTexture imgSmile2;
        public GameTexture imgSmile3;
        public GameTexture imgEyes1;
        public GameTexture imgEyes2;

        public float power;

        public PillsMedia()
        {
            imgPower1 = GetTexture(Res.IMG_PILL_1);
            imgPower2 = GetTexture(Res.IMG_PILL_2);
            imgPower3 = GetTexture(Res.IMG_PILL_3);
            imgPPower1 = GetTexture(Res.IMG_PILL_1P);
            imgPPower2 = GetTexture(Res.IMG_PILL_2P);
            imgPPower3 = GetTexture(Res.IMG_PILL_3P);

            imgHole = GetTexture(Res.IMG_PILL_HAL_1);

            imgNids = new GameTexture[] 
            { 
                GetTexture(Res.IMG_POWER_1), 
                GetTexture(Res.IMG_POWER_2), 
                GetTexture(Res.IMG_POWER_3), 
                GetTexture(Res.IMG_POWER_4)
            };

            imgHigh = GetTexture(Res.IMG_PILL_HIGH);

            imgToxic = GetTexture(Res.IMG_PILL_TOXIC_1);
            imgToxic2 = GetTexture(Res.IMG_PILL_TOXIC_2);
            imgSleep = GetTexture(Res.IMG_PILL_SLEEP);

            imgSmile1 = GetTexture(Res.IMG_SMILE_1);
            imgSmile2 = GetTexture(Res.IMG_SMILE_2);
            imgSmile3 = GetTexture(Res.IMG_SMILE_3);
            imgEyes1 = GetTexture(Res.IMG_EYES_1);
            imgEyes2 = GetTexture(Res.IMG_EYES_2);

            sndPowers = new int[] { Res.SND_PILL_POWER1, Res.SND_PILL_POWER2, Res.SND_PILL_POWER3 };
            sndWarning = Res.SND_PILL_WARNING;
            sndGenerate = Res.SND_PILL_GENERATE;

            sndJumper = Res.SND_PILL_JUMPER;
            sndHigh = Res.SND_PILL_HIGH;
            sndToxicBorn = Res.SND_PILL_TOXIC_BORN;

            power = 0.0f;
        }

        private GameTexture GetTexture(int id)
        {
            return Application.sharedResourceMgr.GetTexture(id);
        }
    }
}
