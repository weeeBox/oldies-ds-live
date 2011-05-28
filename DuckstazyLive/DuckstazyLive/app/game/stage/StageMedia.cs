using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using app;
using asap.graphics;

namespace DuckstazyLive.app.game.stage
{
    public class StageMedia
    {
        //[Embed(source="gfx/pedestal_l.png")]
        //private Class gfxPedestalL;

        //[Embed(source="gfx/pedestal_r.png")]
        //private Class gfxPedestalR;

        public int imgPedestalL;
        public int imgPedestalR;

        //[Embed(source="gfx/cat_r.png")]
        //private Class gfxCatR;

        //[Embed(source="gfx/cat_l.png")]
        //private Class gfxCatL;

        //[Embed(source="gfx/cat_smile.png")]
        //private Class gfxCatSmile;

        //[Embed(source="gfx/cat_hum.png")]
        //private Class gfxCatHum;

        public int imgCatR;
        public int imgCatL;
        public int imgCatSmile;
        public int imgCatHum;

        //[Embed(source="gfx/hint_arrow.png")]
        //private Class gfxHintArrow;

        public GameTexture imgHintArrow;

        //[Embed(source="gfx/frog_body.png")]
        //private Class gfxFrogBody;

        //[Embed(source="gfx/frog_emo1.png")]
        //private Class gfxFrogEmo1;

        //[Embed(source="gfx/frog_emo2.png")]
        //private Class gfxFrogEmo2;

        //[Embed(source="gfx/frog_eye1.png")]
        //private Class gfxFrogEye1;

        //[Embed(source="gfx/frog_eye2.png")]
        //private Class gfxFrogEye2;

        //[Embed(source="gfx/frog_hand1.png")]
        //private Class gfxFrogHand1;

        //[Embed(source="gfx/frog_hand2.png")]
        //private Class gfxFrogHand2;

        //[Embed(source="gfx/frog_head.png")]
        //private Class gfxFrogHead;

        public int imgFrogBody;
        public int imgFrogEmo1;
        public int imgFrogEmo2;
        public int imgFrogEye1;
        public int imgFrogEye2;
        public int imgFrogHand1;
        public int imgFrogHand2;
        public int imgFrogHead;

        public StageMedia()
        {
            imgPedestalL = Res.IMG_GFX_PEDESTAL_L;
            imgPedestalR = Res.IMG_GFX_PEDESTAL_R;

            imgCatR = Res.IMG_GFX_CAT_R;
            imgCatL = Res.IMG_GFX_CAT_L;
            imgCatSmile = Res.IMG_GFX_CAT_SMILE;
            imgCatHum = Res.IMG_GFX_CAT_HUM;

            imgHintArrow = GetTexture(Res.IMG_FX_HINT_ARROW);

            imgFrogBody = Res.IMG_GFX_FROG_BODY;
            imgFrogEmo1 = Res.IMG_GFX_FROG_EMO1;
            imgFrogEmo2 = Res.IMG_GFX_FROG_EMO2;
            imgFrogEye1 = Res.IMG_GFX_FROG_EYE1;
            imgFrogEye2 = Res.IMG_GFX_FROG_EYE2;
            imgFrogHand1 = Res.IMG_GFX_FROG_HAND1;
            imgFrogHand2 = Res.IMG_GFX_FROG_HAND2;
            imgFrogHead = Res.IMG_GFX_FROG_HEAD;
        }

        private GameTexture GetTexture(int id)
        {
            return Application.sharedResourceMgr.GetTexture(id);
        }
    }
}
