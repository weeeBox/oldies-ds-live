using System;
using app;
using app.menu;
using Microsoft.Xna.Framework;
using asap.visual;
using asap.graphics;

namespace DuckstazyLive.app.menu
{
    public class MenuButton : Button
    {
        private Color targetColor;
        private Vector2 targetScale;
        private float omega;

        private static float[] MULTIPLIERS = new float[] { 1.0f, 0.975f };
        private float colorCounter;

        private Color color1;
        private Color color2;

        private BaseElement strokeImage;
        private BaseElement rotationElement1;
        private BaseElement rotationElement2;
        private Text label;

        public string name;

        public MenuButton(String text, int code, ButtonListener listener)
            : this(text, code, 0x99ccff, 0xd5f2ff, listener)
        {

        }

        public MenuButton(String text, int code, int c1, int c2, ButtonListener listener) : base(code, listener)
        {
            name = text;

            color1 = ColorUtils.MakeColor(c1);
            color2 = ColorUtils.MakeColor(c2);

            // button stroke part            
            strokeImage = new Circle(81, Color.Black);            
            AddChild(strokeImage);

            //// button rotating part
            rotationElement1 = new Circle(77, color1);
            AddChild(rotationElement1);

            rotationElement2 = new Image(Application.sharedResourceMgr.GetTexture(Res.IMG_BUTTON_BASE));
            rotationElement2.Color = color2;
            AddChild(rotationElement2);

            // button label
            BaseFont font = Application.sharedResourceMgr.GetFont(Res.FNT_BIG);
            label = new Text(font, text, 100);
            label.SetAlign(TextAlign.CENTER);
            AddChild(label);

            ResizeToFitChilds();
            AttachCenterAll();            

            Reset();
        }

        private void Reset()
        {
            targetColor = ColorUtils.MakeColor(0x0f0540);
            targetScale = new Vector2(1.0f, 1.0f);
            omega = MathHelper.TwoPi / 5.0f;
            colorCounter = 0.0f;
            label.Color = ColorUtils.MakeColor(0x95c9ff);
        }

        public override void Update(float delta)
        {
            base.Update(delta);
            //Color strokeColor;

            //strokeImage.color.A = (byte)(0.5f * (strokeImage.color.A + targetColor.A));
            //strokeImage.color.R = (byte)(0.5f * (strokeImage.color.R + targetColor.R));
            //strokeImage.color.G = (byte)(0.5f * (strokeImage.color.G + targetColor.G));
            //strokeImage.color.B = (byte)(0.5f * (strokeImage.color.B + targetColor.B));

            scaleX = 0.5f * (scaleX + targetScale.X);
            scaleY = 0.5f * (scaleY + targetScale.Y);

            rotationElement2.rotation += omega * delta;

            //colorCounter += delta;
            //int colorIndex = ((int)(colorCounter / 0.05f)) % MULTIPLIERS.Length;
            //float multiplier = MULTIPLIERS[colorIndex];
            //rotationElement1.color = color1 * multiplier;
            //rotationElement2.color = color2 * multiplier;
        }

        public override void FocusLost()
        {
            base.FocusLost();
            Reset();
        }

        public override void FocusGained()
        {
            base.FocusGained();

            label.Color = Color.White;
            targetColor = Color.White;
            targetScale = new Vector2(1.2f, 1.2f);
            omega = MathHelper.TwoPi / 2.5f;
        }
    }
}