using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.core;
using Microsoft.Xna.Framework;
using Framework.visual;
using Microsoft.Xna.Framework.Graphics;

namespace DuckstazyLive.app
{
    public class MenuButton : AbstractButton
    {
        private const int IMG_STOKE_ID = Res.IMG_BUTTON_STROKE_FOCUSED;

        private const int CHILD_STROKE = 0;
        private const int CHILD_ROTATION1 = 1;
        private const int CHILD_ROTATION2 = 2;
        private const int CHILD_TEXT = 3;

        private Color targetColor;
        private Vector2 targetScale;
        private float omega;

        private static float[] MULTIPLIERS = new float[] { 1.0f, 0.975f };        
        private float colorCounter;

        private Color color1;
        private Color color2;

        public MenuButton(String text, int buttonID, float x, float y) : this(text, buttonID, x, y, 0x99ccff, 0xd5f2ff)
        {

        }

        public MenuButton(String text, int buttonID, float x, float y, uint c1, uint c2) : base(buttonID, x, y, utils.textureWidth(IMG_STOKE_ID), utils.textureHeight(IMG_STOKE_ID))
        {
            color1 = utils.makeColor(c1);
            color2 = utils.makeColor(c2);

            // button stroke part            
            Image strokeImage = new Image(utils.getTexture(IMG_STOKE_ID));
            strokeImage.toParentCenter();
            addChild(strokeImage, CHILD_STROKE);

            // button rotating part
            Image baseImage1 = new Image(utils.getTexture(Res.IMG_BUTTON_BASE1));            
            baseImage1.toParentCenter();            
            addChild(baseImage1, CHILD_ROTATION1);

            Image baseImage2 = new Image(utils.getTexture(Res.IMG_BUTTON_BASE2));
            baseImage2.toParentCenter();            
            addChild(baseImage2, CHILD_ROTATION2);

            // button label
            Font font = Application.sharedResourceMgr.getFont(Res.FNT_BIG);
            Text label = new Text(font);
            label.setString(text, baseImage1.width);
            label.setParentAlign(ALIGN_CENTER, ALIGN_CENTER);
            label.setAlign(TextAlign.HCENTER | TextAlign.VCENTER);
            addChild(label, CHILD_TEXT);

            reset();
        }
        
        private void reset()
        {
            targetColor = utils.makeColor(0x0f0540);
            targetScale = new Vector2(1.0f, 1.0f);
            omega = MathHelper.TwoPi / 5.0f;            
            colorCounter = 0.0f;
            getChild(CHILD_TEXT).color = utils.makeColor(0x95c9ff);
        }

        public override void update(float delta)
        {
            BaseElement stroke = getChild(CHILD_STROKE);
            BaseElement rotation1 = getChild(CHILD_ROTATION1);
            BaseElement rotation2 = getChild(CHILD_ROTATION2);

            stroke.color.A = (byte)(0.5f * (stroke.color.A + targetColor.A));
            stroke.color.R = (byte)(0.5f * (stroke.color.R + targetColor.R));
            stroke.color.G = (byte)(0.5f * (stroke.color.G + targetColor.G));
            stroke.color.B = (byte)(0.5f * (stroke.color.B + targetColor.B));

            scaleX = 0.5f * (scaleX + targetScale.X);
            scaleY = 0.5f * (scaleY + targetScale.Y);

            rotation2.rotation += omega * delta;

            colorCounter += delta;
            int colorIndex = ((int)(colorCounter / 0.05f)) % MULTIPLIERS.Length;
            float multiplier = MULTIPLIERS[colorIndex];
            rotation1.color = color1 * multiplier;
            rotation2.color = color2 * multiplier;
        }       

        protected override void focusLost()
        {
            base.focusLost();            
            reset();
        }
        
        protected override void  focusGained()
        {
            base.focusGained();

            getChild(CHILD_TEXT).color = Color.White;
            targetColor = Color.White;
            targetScale = new Vector2(1.2f, 1.2f);
            omega = MathHelper.TwoPi / 2.5f;
        }
    }
}
