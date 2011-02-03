using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.core;
using Framework.visual;

namespace DuckstazyLive.app
{
    public class UiControllerButtons : BaseElementContainer
    {
        public UiControllerButtons(String aButtonLabel, String bButtonLabel)
        {
            if (aButtonLabel != null)
            {
                addChild(createButtonWithLabel(Res.IMG_UI_BUTTON_A, aButtonLabel));
            }
            if (bButtonLabel != null)
            {
                addChild(createButtonWithLabel(Res.IMG_UI_BUTTON_B, bButtonLabel));
            }
            arrangeHorizontally(50, 50);
            resizeToFitItems();
        }

        private BaseElement createButtonWithLabel(int imageId, String text)
        {
            BaseElementContainer container = new BaseElementContainer();            

            Image button = new Image(Application.sharedResourceMgr.getTexture(imageId));            
            Text label = new Text(Application.sharedResourceMgr.getFont(Res.FNT_INFO));
            label.setString(text);
            label.alignY = label.parentAlignY = ALIGN_CENTER;            

            container.addChild(button);
            container.addChild(label);

            container.arrangeHorizontally(10, 10);
            container.resizeToFitItems();

            return container;
        }
    }
}
