using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.core;
using Microsoft.Xna.Framework;
using Framework.visual;

namespace DuckstazyLive.app
{
    public delegate void ButtonDelegate(UiButton button);

    public class UiButton : BaseElement
    {
        private bool focused;

        private const int CHILD_STROKE = 0;
        private const int CHILD_ROTATION = 1;

        private Color targetColor;
        private Vector2 targetScale;
        private float omega;        

        public ButtonDelegate onPressed;
        public ButtonDelegate onFocusGain;
        public ButtonDelegate onFocusLost;

        public UiButton(float x, float y)
        {
            this.x = x;
            this.y = y;

            width = utils.imageWidth(Res.IMG_BUTTON_STROKE_DEFAULT);
            height = utils.imageHeight(Res.IMG_BUTTON_STROKE_DEFAULT);

            // button stroke part            
            Image strokeImage = new Image(Application.sharedResourceMgr.getTexture(Res.IMG_BUTTON_STROKE_FOCUSED));
            strokeImage.toParentCenter();
            addChildWithId(strokeImage, CHILD_STROKE);

            // button rotating part
            Image baseImage = new Image(Application.sharedResourceMgr.getTexture(Res.IMG_BUTTON_BASE));
            baseImage.toParentCenter();
            addChildWithId(baseImage, CHILD_ROTATION);

            focusLost();
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
        }        

        public void press()
        {
            if (onPressed != null)
                onPressed(this);
        }

        public void setFocused(bool f)
        {
            if (f && !focused)
                focusGained();
            else if (!f && focused)
                focusLost();
            focused = f;
        }

        private void focusLost()
        {
            if (onFocusLost != null)
                onFocusLost(this);

            targetColor = Color.Black;
            targetScale = new Vector2(1.0f, 1.0f);
            omega = 350.0f / 5.0f;
        }

        private void focusGained()
        {
            if (onFocusGain != null)
                onFocusGain(this);

            targetColor = Color.White;
            targetScale = new Vector2(1.2f, 1.2f);
            omega = 360.0f / 2.5f;
        }
    }
}
