using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using asap.graphics;
using app;
using asap.util;
using asap.visual;

namespace DuckstazyLive.app.game.env
{
    public class NightSky : Sky
    {
        private const int SKY_COLOR = 0x111133;
        private const int STARS_COUNT = 30;

        private EnvStar[] stars;
        private Image starImage;

        private float MIN_X;
        private float MIN_Y;
        private float MAX_X;
        private float MAX_Y;        

        public NightSky(float width, float height)
            : base(width, height, ColorUtils.MakeColor(SKY_COLOR))
        {
            starImage = new Image(Application.sharedResourceMgr.GetTexture(Res.IMG_STAR));
            
            stars = new EnvStar[STARS_COUNT];
            for (int i = 0; i < STARS_COUNT; ++i)
            {
                EnvStar star = new EnvStar();
                star.x = width * RandomHelper.rnd();
                star.y = height * RandomHelper.rnd();
                star.a = RandomHelper.rnd() * 6.28f;
                star.t = RandomHelper.rnd();
                star.vx = 0.5f * (float)(width * Math.Cos(star.a));
                star.vy = 0.5f * (float)(height * Math.Sin(star.a));

                stars[i] = star;                
            }

            MIN_X = -starImage.width;
            MIN_Y = -starImage.height;
            MAX_X = width + starImage.width;
            MAX_Y = height + starImage.height;
        }
        
        protected override void Update(float delta, float power)
        {
            foreach (EnvStar star in stars)
            {
                star.update(delta, power);

                if (star.x < MIN_X) star.x += MAX_X;
                else if (star.x > MAX_X) star.x -= MAX_X;

                if (star.y < -MIN_Y) star.y += MAX_Y;
                else if (star.y > MAX_Y) star.y -= MAX_Y;
            }
        }

        public override void Draw(Graphics g)
        {
            base.Draw(g);

            foreach (EnvStar star in stars)
            {
                starImage.x = star.x;
                starImage.y = star.y;
                starImage.rotation = star.a;
                starImage.ctForm = star.color;
                starImage.scaleX = 0.75f + 0.25f * (float)Math.Sin(x * 6.28);
                starImage.scaleY = 0.75f + 0.25f * (float)Math.Sin(x * 6.28);
                starImage.Draw(g);
            }
        }
    }
}
