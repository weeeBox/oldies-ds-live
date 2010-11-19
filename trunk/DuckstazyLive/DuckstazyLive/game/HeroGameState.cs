using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.visual;
using Framework.core;
using DuckstazyLive.app;
using Microsoft.Xna.Framework;

namespace DuckstazyLive.game
{
    public class HeroGameState
    {
        public int def; // коэффициент урона
        public int maxHP; // максимальное здоровье

        public int health;        
        public int scores;
        public int scoreOld;

        public float scoreCounter;

        public float hpPulse;
        public float hpCounter;

        private string scoreText;
        public bool leftOriented;

        public Color fontColor;

        public HeroGameState()
        {
            fontColor = Color.White;
            reset();
        }

        // Все вернуть как сначала.
        public void reset()
        {
            def = 0;
            maxHP = 25;
            health = maxHP;            
            scores = scoreOld = 0;
            scoreCounter = 0.0f;
            hpCounter = 0.0f;
            hpPulse = 0.0f;
        }

        public void syncScores()
        {
            scoreOld = scores;
            scoreText = scoreOld.ToString();
        }

        public void draw(Canvas canvas, float x, float y)
        {
            DrawMatrix mat = new DrawMatrix();
            float sc = 1.0f + 0.3f * hpPulse;

            mat.tx = -0.5f * utils.imageWidth(Res.IMG_UI_HP);
            mat.ty = -0.5f * utils.imageHeight(Res.IMG_UI_HP);
            mat.scale(sc, sc);
            mat.translate(x, y);//463.0f);
            canvas.draw(Res.IMG_UI_HP, mat);

            mat.identity();
            mat.tx = -0.5f * utils.imageWidth(Res.IMG_UI_SCORE);
            mat.ty = -0.5f * utils.imageHeight(Res.IMG_UI_SCORE);
            sc = 1.0f + 0.3f * scoreCounter;
            mat.scale(sc, sc);
            mat.translate(x, y + utils.imageHeight(Res.IMG_UI_HP));//463.0f);
            canvas.draw(Res.IMG_UI_SCORE, mat);

            AppGraphics.SetColor(fontColor);

            mat.identity();
            String str = health.ToString() + "/" + maxHP.ToString();
            if (leftOriented)
                mat.translate(x, y - 0.5f * utils.fontHeight(Res.FNT_BIG));
            else
                mat.translate(x - utils.stringWidth(Res.FNT_BIG, str), y - 0.5f * utils.fontHeight(Res.FNT_BIG));            
            canvas.draw(Res.FNT_BIG, str, mat);

            Font font = Application.sharedResourceMgr.getFont(Res.FNT_BIG);
            if (leftOriented)
                mat.translate(x, y + utils.imageHeight(Res.IMG_UI_HP) - 0.5f * utils.fontHeight(Res.FNT_BIG));
            else
                mat.translate(x - utils.stringWidth(Res.FNT_BIG, scoreText), y + utils.imageHeight(Res.IMG_UI_HP) - 0.5f * utils.fontHeight(Res.FNT_BIG));
            canvas.draw(Res.FNT_BIG, scoreText, mat);

            AppGraphics.SetColor(Color.White);
        }

        public void update(float dt, float power)
        {
            if (hpPulse > 0.0f) { hpPulse -= 4.0f * dt; if (hpPulse < 0.0f) hpPulse = 0.0f; }
            hpCounter += 4.0f * dt;
            if (power < 0.33)
            {
                if (hpCounter > 4.0f) { hpCounter -= 4.0f; hpPulse = 1.0f; }
            }
            else if (power < 0.66)
            {
                if (hpCounter > 2.0f) { hpCounter -= 2.0f; hpPulse = 1.0f; }
            }
            else
            {
                if (hpCounter > 1.0f) { hpCounter -= 1.0f; hpPulse = 1.0f; }
            }

            if (scores > scoreOld)
            {
                scoreCounter += 30.0f * dt;
                if (scoreCounter > 1.0f)
                {
                    int i = (scores - scoreOld) / 5;
                    if (i == 0)
                    {
                        scoreOld = scores;
                        scoreCounter = 0.0f;
                    }
                    else
                    {
                        scoreOld += i;
                        scoreCounter -= (int)scoreCounter;
                    }

                    scoreText = scoreOld.ToString();
                }
            }
        }
    }
}
