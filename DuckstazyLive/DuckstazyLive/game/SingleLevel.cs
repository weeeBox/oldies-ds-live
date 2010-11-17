using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckstazyLive.game.levels;
using DuckstazyLive.app;
using Framework.visual;
using Framework.core;

namespace DuckstazyLive.game
{
    public class SingleLevel : Level
    {
        public SingleLevel(GameState gameState) : base(gameState)
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

        public override void drawUI(Canvas canvas)
        {
            DrawMatrix mat = new DrawMatrix(true);
            float sc = 1.0f + 0.3f * hpPulse;

            mat.tx = -25.0f;
            mat.ty = -23.0f;
            mat.scale(sc, sc);
            mat.translate(22.0f, 410 + 18);//463.0f);
            canvas.draw(imgHP1, mat);

            mat.identity();
            mat.tx = -24.0f;
            mat.ty = -24.0f;
            sc = 1.0f + 0.3f * scoreCounter;
            mat.scale(sc, sc);
            mat.translate(620.0f, 410 + 18);//463.0f);
            canvas.draw(imgScore, mat);

            mat.identity();

            mat.translate(40.0f, 410.0f);//445.0f;
            String str = state.health.ToString() + "/" + state.maxHP.ToString();
            canvas.draw(Res.FNT_BIG, str, mat);

            Font font = Application.sharedResourceMgr.getFont(Res.FNT_BIG);
            mat.translate(600.0f - font.stringWidth(scoreText), 410.0f);
            canvas.draw(Res.FNT_BIG, scoreText, mat);
        }
    }
}
