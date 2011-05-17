using asap.ui;
using asap.visual;

namespace app.menu
{
    public class ScreenFactory
    {
        public UiComponent CreateControllerButtons(string aButtonLabel, string bButtonLabel)
        {
            UiComponent container = new UiComponent();

            if (aButtonLabel != null)
            {
                //container.AddChild(createbu)
            }

            return container;
        }

        private UiComponent createButtonWithLabel(int imageId, string text)
        {
            UiComponent container = new UiComponent();

            Image button = new Image(Application.sharedResourceMgr.GetTexture(imageId));
            Text label = new Text(Application.sharedResourceMgr.GetFont(Res.FNT_INFO), text);            

            container.AddChild(button);
            container.AddChild(label);

            container.ArrangeHor(10);
            container.ResizeToFitChilds();

            return container;
        }
    }    
}