using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Framework.core;
using System.Diagnostics;

namespace Framework.visual
{
    public interface ButtonDelegate
    {
        void onButtonPressed(int id);
    }

    public class Button : BaseElement
    {        
        public const int BUTTON_UP = 0;
        public const int BUTTON_DOWN = 1;
        public const int BUTTON_FOCUSED = 2;

        int state;
        public ButtonDelegate buttonDelegate;
        int buttonID;

        float touchLeftInc;
        float touchRightInc;
        float touchTopInc;
        float touchBottomInc;

        public Button(Texture2D up, Texture2D down, Texture2D focused, int bID)
            : this(new Image(up), new Image(down), new Image(focused), bID)
        {
        }

        public Button(BaseElement up, BaseElement down, BaseElement focused, int n)
        {
            buttonID = n;
            state = BUTTON_UP;

            touchLeftInc = 0;
            touchRightInc = 0;
            touchTopInc = 0;
            touchBottomInc = 0;

            up.parentAnchor = down.parentAnchor = ANCHOR_TOP | ANCHOR_LEFT;
            width = up.width;
            height = up.height;
            addChildWithId(up, BUTTON_UP);
            addChildWithId(down, BUTTON_DOWN);
            addChildWithId(focused, BUTTON_FOCUSED);
            setState(BUTTON_UP);
        }

        public void setTouchIncrease(float l, float r, float t, float b)
        {
            touchLeftInc = l;
            touchRightInc = r;
            touchTopInc = t;
            touchBottomInc = b;
        }

        public void setState(int s)
        {
            Debug.Assert(s == BUTTON_UP || s == BUTTON_DOWN || s == BUTTON_FOCUSED);

            state = s;
            BaseElement up = getChild(BUTTON_UP);
            BaseElement down = getChild(BUTTON_DOWN);
            BaseElement focused = getChild(BUTTON_DOWN);

            up.setEnabled(s == BUTTON_UP);
            down.setEnabled(s == BUTTON_DOWN);
            focused.setEnabled(s == BUTTON_FOCUSED);
        }

        //public override bool onTouchDown(float tx, float ty)
        //{
        //    base.onTouchDown(tx, ty);

        //    if (state == BUTTON_UP)
        //    {
        //        if (FwMathHelper.pointInRect(tx, ty, drawX - touchLeftInc, drawY - touchTopInc,
        //                        width + (touchLeftInc + touchRightInc), height + (touchTopInc + touchBottomInc)))
        //        {
        //            setState(BUTTON_DOWN);
        //            return true;
        //        }
        //    }

        //    return false;
        //}

        //public override bool onTouchUp(float tx, float ty)
        //{
        //    base.onTouchUp(tx, ty);

        //    if (state == BUTTON_DOWN)
        //    {
        //        setState(BUTTON_UP);

        //        if (FwMathHelper.pointInRect(tx, ty, drawX - touchLeftInc, drawY - touchTopInc,
        //                        width + (touchLeftInc + touchRightInc), height + (touchTopInc + touchBottomInc)))
        //        {
        //            if (buttonDelegate != null)
        //            {
        //                buttonDelegate.onButtonPressed(buttonID);
        //            }
        //            return true;
        //        }
        //    }

        //    return false;
        //}

        //public override bool onTouchMove(float tx, float ty)
        //{
        //    base.onTouchMove(tx, ty);

        //    if (state == BUTTON_DOWN)
        //    {
        //        if (!FwMathHelper.pointInRect(tx, ty, drawX - touchLeftInc, drawY - touchTopInc,
        //                        width + (touchLeftInc + touchRightInc), height + (touchTopInc + touchBottomInc)))
        //        {
        //            setState(BUTTON_UP);
        //            return true;
        //        }
        //    }

        //    return false;
        //}
    }
}
