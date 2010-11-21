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

        public Color color;

        DrawMatrix mat;

        public HeroGameState()
        {
            color = Color.White;
            mat = new DrawMatrix();
            
            reset();
        }

        // Все вернуть как сначала.
        public void reset()
        {
            def = 0;
            maxHP = 3;
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
            //mat.tx = -0.5f * utils.imageWidth(Res.IMG_UI_HP);
            //mat.ty = -0.5f * utils.imageHeight(Res.IMG_UI_HP);
            //mat.scale(sc, sc);
            //mat.translate(x, y);//463.0f);           

            float drawX = x;
            float drawY = y;

            float dx = 0.65f * utils.imageWidth(Res.IMG_UI_HEALTH_EMO_BASE);
            
            if (!leftOriented)
                drawX -= 2 * dx;

            for (int i = 0; i < 3; ++i)
            {                
                bool alive = i <= health - 1;
                drawHealthEmo(canvas, drawX, drawY, alive);

                drawX += dx;
            }

            //mat.identity();
            //mat.tx = -0.5f * utils.imageWidth(Res.IMG_UI_SCORE);
            //mat.ty = -0.5f * utils.imageHeight(Res.IMG_UI_SCORE);
            //sc = 1.0f + 0.3f * scoreCounter;
            //mat.scale(sc, sc);
            //mat.translate(x, y + utils.imageHeight(Res.IMG_UI_HP));//463.0f);
            //canvas.draw(Res.IMG_UI_SCORE, mat);

            //AppGraphics.SetColor(fontColor);

            //mat.identity();
            //String str = health.ToString() + "/" + maxHP.ToString();
            //if (leftOriented)
            //    mat.translate(x, y - 0.5f * utils.fontHeight(Res.FNT_BIG));
            //else
            //    mat.translate(x - utils.stringWidth(Res.FNT_BIG, str), y - 0.5f * utils.fontHeight(Res.FNT_BIG));
            //canvas.draw(Res.FNT_BIG, str, mat);            

            //AppGraphics.SetColor(Color.White);
        }

        private void drawHealthEmo(Canvas canvas, float cx, float cy, bool alive)
        {
            mat.identity();            

            if (alive)
            {
                float sc = 1.0f + 0.20f * hpPulse;

                mat.tx = -0.5f * utils.imageWidth(Res.IMG_UI_HEALTH_EMO_BASE);
                mat.ty = -0.5f * utils.imageHeight(Res.IMG_UI_HEALTH_EMO_BASE);
                mat.scale(sc, sc);
                mat.translate(cx, cy);

                // base
                canvas.draw(leftOriented ? Res.IMG_UI_HEALTH_EMO_BASE : Res.IMG_UI_HEALTH_EMO_BASE2, mat);
                // eyes
                int eyes = health == 1 ? Res.IMG_UI_HEALTH_EMO_EYES2 : Res.IMG_UI_HEALTH_EMO_EYES1;
                canvas.draw(eyes, mat);
                // smile
                int smile = health == 3 ? Res.IMG_UI_HEALTH_EMO_SMILE1 :
                            health == 2 ? Res.IMG_UI_HEALTH_EMO_SMILE2 : Res.IMG_UI_HEALTH_EMO_SMILE3;
                canvas.draw(smile, mat);
            }
            else
            {
                mat.tx = -0.5f * utils.imageWidth(Res.IMG_UI_HEALTH_EMO_DEAD);
                mat.ty = -0.5f * utils.imageHeight(Res.IMG_UI_HEALTH_EMO_DEAD);                
                mat.translate(cx, cy);

                // base
                canvas.draw(Res.IMG_UI_HEALTH_EMO_DEAD, mat);
            }
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
