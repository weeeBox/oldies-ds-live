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
        private const int IMG_BUTTON_BASE_ID = Res.IMG_BUTTON_BASE;

        private const int CHILD_STROKE = 0;
        private const int CHILD_ROTATION = 1;
        private const int CHILD_TEXT = 2;

        private Color targetColor;
        private Vector2 targetScale;
        private float omega;

        private static Color[] COLORS = new Color[] { Color.White, new Color(0.975f, 0.975f, 0.975f) };        
        private float colorCounter;

        public MenuButton(String text, int buttonID, float x, float y)
            : base(buttonID, x, y, utils.textureWidth(IMG_STOKE_ID), utils.textureHeight(IMG_STOKE_ID))
        {          
            // button stroke part            
            Image strokeImage = new Image(utils.getTexture(IMG_STOKE_ID));
            strokeImage.toParentCenter();
            addChild(strokeImage, CHILD_STROKE);

            // button rotating part
            Image baseImage = new Image(utils.getTexture(IMG_BUTTON_BASE_ID));
            baseImage.toParentCenter();
            addChild(baseImage, CHILD_ROTATION);

            // button label
            Font font = Application.sharedResourceMgr.getFont(Res.FNT_BIG);
            Text label = new Text(font);
            label.setString(text, baseImage.width);
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
            BaseElement rotation = getChild(CHILD_ROTATION);

            stroke.color.A = (byte)(0.5f * (stroke.color.A + targetColor.A));
            stroke.color.R = (byte)(0.5f * (stroke.color.R + targetColor.R));
            stroke.color.G = (byte)(0.5f * (stroke.color.G + targetColor.G));
            stroke.color.B = (byte)(0.5f * (stroke.color.B + targetColor.B));

            scaleX = 0.5f * (scaleX + targetScale.X);
            scaleY = 0.5f * (scaleY + targetScale.Y);

            rotation.rotation += omega * delta;

            colorCounter += delta;
            int colorIndex = ((int)(colorCounter / 0.05f)) % COLORS.Length;
            rotation.color = COLORS[colorIndex];
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
