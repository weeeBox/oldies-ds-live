using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using asap.visual;
using asap.graphics;
using app;

namespace DuckstazyLive.app.game.env
{
    public class Ground : DisplayObjectContainer
    {
        private TiledImage grass;
        private RectShape ground;

        public Ground(float width, float height) : base(width, height)
        {
            GameTexture texture = Application.sharedResourceMgr.GetTexture(Res.IMG_GRASS1);
            grass = new TiledImage(texture, width, texture.GetHeight());
            grass.Color = ColorUtils.MakeColor(0x00ff00);
            ground = new RectShape(width, height, ColorUtils.MakeColor(0x371d06), ColorUtils.MakeColor(0x5d310c));

            AddChild(ground);
            AddChild(grass);

            grass.y = -grass.height;
        }

        public override void Update(float delta)
        {
            base.Update(delta);


        }
    }
}
