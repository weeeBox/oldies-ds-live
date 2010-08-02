using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using DuckstazyLive.app;
using DuckstazyLive.framework.graphics;

namespace DuckstazyLive.env
{
    public sealed class Grass
    {     
        public void Draw(GameGraphics g)
        {
            Image grass = Resources.GetImage(Res.IMG_GRASS);
            grass.DrawTiled(g, 0, App.Height - (Constants.GROUND_HEIGHT + grass.Height), App.Width, grass.Height);
        }

        private GraphicsDevice GraphicsDevice
        {
            get { return App.GraphicsDevice; }
        }

        private SpriteBatch SpriteBatch
        {
            get { return App.SpriteBatch; }
        }

        private Application App
        {
            get { return Application.Instance; }
        }
    }
}
