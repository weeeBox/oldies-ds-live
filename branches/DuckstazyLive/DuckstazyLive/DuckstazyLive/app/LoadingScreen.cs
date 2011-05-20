using app;
using app.menu;
using asap.graphics;
using Framework.core;
using Framework.visual;
using Microsoft.Xna.Framework;
using asap.visual;
using asap.ui;

namespace DuckstazyLive.app
{
    public class LoadingScreen : Screen
    {
        private Color linesColor;
        private float offset;

        private StartupController controller;
        private Text progressText;

        public LoadingScreen(StartupController controller) : base(ScreenId.START_LOADING)
        {
            this.controller = controller;
            linesColor = Color.Gray;

            BaseFont font = Application.sharedResourceMgr.GetFont(Res.FNT_BIG);
            UiComponent container = new UiComponent(0, 0);

            Text loadingText = new Text(font, "LOADING");            

            progressText = new Text(font, "100");            
            progressText.SetAlign(TextAlign.CENTER);
            Text percentText = new Text(font, "%...");

            container.AddChild(loadingText);
            container.AddChild(progressText);
            container.AddChild(percentText);

            container.ArrangeHor(0);
            container.ResizeToFitChilds();

            AddChild(container);
            AttachHCenter(container);
            container.y = Constants.TITLE_SAFE_BOTTOM_Y - container.height;            
        }

        public override void Update(float delta)
        {
            GameTexture line = Application.sharedResourceMgr.GetTexture(Res.IMG_LOAD_LINE);

            offset += delta * 24;
            if (offset > line.GetHeight())
                offset -= line.GetHeight();
                        
            progressText.SetText("" + controller.getPercentLoaded());            
        }
        
        public override void Draw(Graphics g)
        {
            PreDraw(g);

            // back
            AppGraphics.FillRect(0, 0, Constants.SCREEN_WIDTH, Constants.SCREEN_HEIGHT, Color.White);

            // lines
            AppGraphics.SetColor(linesColor);

            GameTexture line = Application.sharedResourceMgr.GetTexture(Res.IMG_LOAD_LINE);

            float x = 0.5f * (Application.Width - line.GetWidth());
            float y = -line.GetHeight() + offset;
            while (y < Application.Height)
            {
                g.DrawImage(line, x, y);                
                y += line.GetHeight();
            }

            AppGraphics.SetColor(Color.White);

            PostDraw(g);
        }
    }
}
