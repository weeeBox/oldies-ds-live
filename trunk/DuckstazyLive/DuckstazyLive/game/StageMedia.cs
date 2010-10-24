using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace DuckstazyLive.game
{
   	public class StageMedia
	{
        //[Embed(source="gfx/pedestal_l.png")]
        //private Class gfxPedestalL;
        
        //[Embed(source="gfx/pedestal_r.png")]
        //private Class gfxPedestalR;
               
        public Texture2D imgPedestalL;
        public Texture2D imgPedestalR;
        
        //[Embed(source="gfx/cat_r.png")]
        //private Class gfxCatR;
        
        //[Embed(source="gfx/cat_l.png")]
        //private Class gfxCatL;
        
        //[Embed(source="gfx/cat_smile.png")]
        //private Class gfxCatSmile;
        
        //[Embed(source="gfx/cat_hum.png")]
        //private Class gfxCatHum;

        public Texture2D imgCatR;
        public Texture2D imgCatL;
        public Texture2D imgCatSmile;
        public Texture2D imgCatHum;
        
        //[Embed(source="gfx/hint_arrow.png")]
        //private Class gfxHintArrow;

        public Texture2D imgHintArrow;
        
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

		public Texture2D imgFrogBody;
		public Texture2D imgFrogEmo1;
		public Texture2D imgFrogEmo2;
		public Texture2D imgFrogEye1;
		public Texture2D imgFrogEye2;
		public Texture2D imgFrogHand1;
		public Texture2D imgFrogHand2;
		public Texture2D imgFrogHead;
		
        //[Embed(source="gfx/pump.png")]
        //private Class gfxPump;

        //[Embed(source="gfx/party.png")]
        //private Class gfxParty;

        //[Embed(source="gfx/trip.png")]
        //private Class gfxTrip;

		public Texture2D imgPump;
		public Texture2D imgParty;
		public Texture2D imgTrip;

        //[Embed(source="gfx/theend.png")]
        //private Class gfxTheEnd;
        
        //[Embed(source="gfx/stageend.png")]
        //private Class gfxStageEnd;
        
		public Texture2D imgTheEnd;
		public Texture2D imgStageEnd;
		
		public StageMedia()
		{
            //imgPedestalL = (new gfxPedestalL()).bitmapData;
            //imgPedestalR = (new gfxPedestalR()).bitmapData;
			
            //imgCatR = (new gfxCatR()).bitmapData;
            //imgCatL = (new gfxCatL()).bitmapData;
            //imgCatSmile = (new gfxCatSmile()).bitmapData;
            //imgCatHum = (new gfxCatHum()).bitmapData;
			
            //imgHintArrow = (new gfxHintArrow()).bitmapData;
			
            //imgFrogBody = (new gfxFrogBody()).bitmapData;
            //imgFrogEmo1 = (new gfxFrogEmo1()).bitmapData;
            //imgFrogEmo2 = (new gfxFrogEmo2()).bitmapData;
            //imgFrogEye1 = (new gfxFrogEye1()).bitmapData;
            //imgFrogEye2 = (new gfxFrogEye2()).bitmapData;
            //imgFrogHand1 = (new gfxFrogHand1()).bitmapData;
            //imgFrogHand2 = (new gfxFrogHand2()).bitmapData;
            //imgFrogHead = (new gfxFrogHead()).bitmapData;
			
            //imgPump = (new gfxPump()).bitmapData;
            //imgParty = (new gfxParty()).bitmapData;
            //imgTrip = (new gfxTrip()).bitmapData;
			
            //imgTheEnd = (new gfxTheEnd()).bitmapData;
            //imgStageEnd = (new gfxStageEnd()).bitmapData;
		}
	}

}
