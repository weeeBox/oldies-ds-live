using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckstazyLive.game.levels;
using DuckstazyLive.app;
using Framework.visual;
using Framework.core;
using Microsoft.Xna.Framework.Graphics;

namespace DuckstazyLive.game
{
    public class MultiplayerLevel : Level
    {
        public MultiplayerLevel(GameState gameState) : base(gameState)
        {
            stages.Add(LevelStages.Harvesting);
            stages.Add(LevelStages.PartyTime);
            stages.Add(LevelStages.Bubbles);
            stages.Add(LevelStages.DoubleFrog);
            stages.Add(LevelStages.PartyTime2);
            stages.Add(LevelStages.BetweenCatsStage);
            stages.Add(LevelStages.Bubbles2);
            stages.Add(LevelStages.AirAttack);
            stages.Add(LevelStages.PartyTime3);
            stages.Add(LevelStages.Trains);
            stages.Add(LevelStages.Bubbles3);
            stagesCount = stages.Count;
        }

        protected override void initHero()
        {
            heroes = new Heroes();
            Hero heroInstance = new Hero(heroes, 0);
            heroInstance.state = state;
            heroes.addHero(heroInstance);

            heroInstance = new Hero(heroes, 1);
            heroInstance.state = state;
            heroes.addHero(heroInstance);

            pills = new Pills(heroes, ps, this);
            heroes.particles = ps;
            heroes.env = env;
            heroes.init();
        }

        public override void drawUI(Canvas canvas)
        {
            DrawMatrix mat = new DrawMatrix();
            float sc = 1.0f + 0.3f * hpPulse;

            Texture2D tex = Application.sharedResourceMgr.getTexture(imgHP1);
            mat.tx = -0.5f * tex.Width;
            mat.ty = -0.5f * tex.Height;
            mat.scale(sc, sc);
            mat.translate(Constants.TITLE_SAFE_LEFT_X, utils.unscale(Constants.TITLE_SAFE_TOP_Y));
            canvas.draw(imgHP1, mat);

            mat.identity();
            tex = Application.sharedResourceMgr.getTexture(imgScore);
            mat.tx = -0.5f * tex.Width;
            mat.ty = -0.5f * tex.Height;            
            sc = 1.0f + 0.3f * scoreCounter;
            mat.scale(sc, sc);
            mat.translate(Constants.TITLE_SAFE_LEFT_X, Constants.TITLE_SAFE_TOP_Y);
            canvas.draw(imgScore, mat);

            //mat.identity();

            //mat.translate(40.0f, 410.0f);//445.0f;
            //String str = state.health.ToString() + "/" + state.maxHP.ToString();
            //canvas.draw(Res.FNT_BIG, str, mat);

            //Font font = Application.sharedResourceMgr.getFont(Res.FNT_BIG);
            //mat.translate(600.0f - font.stringWidth(scoreText), 410.0f);
            //canvas.draw(Res.FNT_BIG, scoreText, mat);
        }
    }
}
