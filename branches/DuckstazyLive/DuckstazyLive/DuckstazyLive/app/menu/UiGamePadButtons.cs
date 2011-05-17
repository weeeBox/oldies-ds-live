using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using asap.ui;
using asap.visual;
using app;

namespace DuckstazyLive.app.menu
{
    public class UiGamePadButtons : UiComponent
    {
        public UiGamePadButtons(string aButtonLabel, string bButtonLabel)
        {
            if (aButtonLabel != null)
            {
                AddChild(createButtonWithLabel(Res.IMG_UI_BUTTON_A, aButtonLabel));
            }
            if (bButtonLabel != null)
            {
                AddChild(createButtonWithLabel(Res.IMG_UI_BUTTON_B, bButtonLabel));
            }
            ArrangeHor(50);
            ResizeToFitChilds();
        }

        private UiComponent createButtonWithLabel(int imageId, String text)
        {
            UiComponent container = new UiComponent();

            Image button = new Image(Application.sharedResourceMgr.GetTexture(imageId));
            Text label = new Text(Application.sharedResourceMgr.GetFont(Res.FNT_INFO), text);
            
            label.alignY = label.parentAlignY = ALIGN_CENTER;

            container.AddChild(button);
            container.AddChild(label);

            container.ArrangeHor(10);
            container.ResizeToFitChilds();

            return container;
        }
    }
}
