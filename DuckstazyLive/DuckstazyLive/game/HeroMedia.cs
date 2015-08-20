﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.core;
using DuckstazyLive.app;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace DuckstazyLive.game
{
    public class HeroMedia
    {
        //// ГРАФИКА //
        //[Embed(source="gfx/duck.png")]
        //private var rDuckImg:Class;

        //[Embed(source="gfx/sleep.png")]
        //private var rSleepImg:Class;

        ////[Embed(source="gfx/foots.png")]
        ////private var rFootsImg:Class;

        //[Embed(source="gfx/wing.png")]
        //private var rWingImg:Class;

        //[Embed(source="gfx/eyeTex.png")]
        //private var rEyeImg1:Class;

        //[Embed(source="gfx/eye2.png")]
        //private var rEyeImg2:Class;

        public int imgDuck;
        //private var imgDuckFlip:SpriteTexture;

        public int imgSleep;
        public int imgDead;
        // private var imgSleepFlip:SpriteTexture;

        public int imgWing;
        public int imgEye1;
        public int imgEye2;

        //static private const rcHero:Rect = new Rect(0.0, 0.0, 54.0, 42.0);


        // ЗВУКИ //

        //[Embed(source = "sfx/step1.mp3")]
        //private Class rStepSnd1;

        //[Embed(source = "sfx/step2.mp3")]
        //private Class rStepSnd2;

        //[Embed(source = "sfx/wing1.mp3")]
        //private Class rWingSnd1;

        //[Embed(source = "sfx/wing2.mp3")]
        //private Class rWingSnd2;

        //[Embed(source = "sfx/land.mp3")]
        //private Class rLandSnd;

        //[Embed(source = "sfx/jump.mp3")]
        //private Class rJumpSnd;

        //[Embed(source = "sfx/awake.mp3")]
        //private Class rAwakeSnd;

        //[Embed(source = "sfx/sleep.mp3")]
        //private Class rSleepSnd;

        //[Embed(source = "sfx/attack.mp3")]
        //private Class rAttackSnd;

        //[Embed(source = "sfx/toxic.mp3")]
        //private Class rToxicSnd;

        private int sndStep1;
        private int sndStep2;
        private int sndWing1;
        private int sndWing2;
        private int sndJump;
        private int sndLand;
        private int sndLandHeavy;
        private int sndAwake;
        private int sndSleep;
        public int sndAttack;
        public int sndToxic;

        private SoundTransform transformPan;
        private SoundTransform transformPanVol;
        private bool trigStep;
        private bool trigWing;

        public HeroMedia()
        {
            initGFX();
            initSFX();
        }

        private void initGFX()
        {
            /*Bitmap duckBitmap = new rDuckImg();
            Bitmap footsBitmap = new rFootsImg();
            Bitmap sleepBitmap = new rSleepImg();*/

            //var mat:Matrix = new Matrix();

            //var rcFoots:Rect = new Rect(0.0, 0.0, 13.0, 3.0);
            //var rcBody:Rect = new Rect(0.0, 0.0, 54.0, 40.0);

            imgEye1 = Res.IMG_EYE1;
            imgEye2 = Res.IMG_EYE2;
            imgWing = Res.IMG_WING;

            /*imgDuck = new SpriteTexture(54, 42, true, 0x00000000);
            imgDuck.lock();
            imgDuck.copyPixels(duckBitmap.bitmapData, rcBody, new Point());
            imgDuck.copyPixels(footsBitmap.bitmapData, rcFoots, new Point(30.0, 39.0));
            imgDuck.unlock();
			
            imgSleep = new SpriteTexture(54, 42, true, 0x00000000);
            imgSleep.lock();
            imgSleep.copyPixels(sleepBitmap.bitmapData, rcBody, new Point());
            imgSleep.unlock();*/

            //imgDuck = (new rDuckImg()).bitmapData;
            imgSleep = Res.IMG_DUCK_SLEEP;
            imgDead = Res.IMG_DUCK_DEAD;

            /*mat.scale(-1.0f, 1.0f);
            mat.translate(54.0, 0.0);
			
            imgDuckFlip = new SpriteTexture(54, 42, true, 0x00000000);			
            imgDuckFlip.lock();
            imgDuckFlip.draw(duckBitmap.bitmapData, mat);
            imgDuckFlip.copyPixels(footsBitmap.bitmapData, rcFoots, new Point(12.0, 39.0));
            imgDuckFlip.unlock();
					
            imgSleepFlip = new SpriteTexture(54, 42, true, 0x00000000);			
            imgSleepFlip.lock();
            imgSleepFlip.draw(sleepBitmap.bitmapData, mat);
            imgSleepFlip.unlock();*/

            imgDuck = Res.IMG_DUCK;
        }

        private void initSFX()
        {
            sndStep1 = Res.SND_HERO_STEP1;
            sndStep2 = Res.SND_HERO_STEP2;
            sndWing1 = Res.SND_HERO_WING1;
            sndWing2 = Res.SND_HERO_WING2;
            sndJump = Res.SND_HERO_JUMP;
            sndLand = Res.SND_HERO_LAND;
            sndLandHeavy = Res.SND_HERO_LAND_HEAVY;
            sndAwake = Res.SND_HERO_AWAKE;
            sndSleep = Res.SND_HERO_SLEEP;
            sndAttack = Res.SND_HERO_ATTACK;
            sndToxic = Res.SND_HERO_TOXIC;

            transformPan = new SoundTransform();
            transformPanVol = new SoundTransform();
        }

        public void drawDuck(int playerIndex, Canvas dest, float x, float y, float power, bool flip, float wingsAngle, float trans)
        {
            DrawMatrix mat = DrawMatrix.ScaledInstance;

            ColorTransform eyeColor = new ColorTransform(1.0f, 1.0f, 1.0f, trans * (1.0f - power));
            ColorTransform duckColor = new ColorTransform(1.0f, 1.0f, 1.0f, trans);            

            if (flip)
            {
                mat.flip(true, false);
                
                mat.translate(x, y);
                mat.flip(true, false);
                dest.draw(imgDuck + playerIndex, mat, duckColor);

                mat.translate(37.0f + x, 6.0f + y);
                dest.draw(imgEye1, mat, eyeColor);
                eyeColor.alphaMultiplier = power * trans;
                dest.draw(imgEye2, mat, eyeColor);

                mat.tx = -12.0f;
                mat.ty = -7.0f;
                mat.rotate(-wingsAngle);
                mat.translate(21.0f + x, 26.0f + y);
                duckColor.alphaMultiplier *= trans;
                dest.draw(imgWing + playerIndex, mat, duckColor);
            }
            else
            {
                //dest.copyPixels(imgDuck, rcHero, new Point(x, y));
                mat.translate(x, y);
                dest.draw(imgDuck + playerIndex, mat, duckColor);

                mat.translate(11.0f + x, 6.0f + y);
                dest.draw(imgEye1, mat, eyeColor);
                eyeColor.alphaMultiplier = power * trans;
                dest.draw(imgEye2, mat, eyeColor);

                mat.tx = -3.0f;
                mat.ty = -7.0f;
                mat.rotate(wingsAngle);
                mat.translate(33.0f + x, 26.0f + y);
                duckColor.alphaMultiplier *= trans;
                dest.draw(imgWing + playerIndex, mat, duckColor);
            }

            //dest.draw(imgEye1.bitmapData, eye_mat, eye1_color);
            //dest.draw(imgEye2.bitmapData, eye_mat, eye2_color);
            //dest.draw(imgWing.bitmapData, wing_mat, null, null, null, wingsAngle!=0.0);
        }

        public void drawSleep(Canvas dest, float x, float y, bool flip, float trans)
        {
            DrawMatrix mat = DrawMatrix.ScaledInstance;
            ColorTransform color = new ColorTransform(1.0f, 1.0f, 1.0f, trans);
            
            mat.translate(x, y);

            if (flip)
            {
                mat.flip(true, false);
            }

            dest.draw(imgSleep, mat, color);
        }

        public void drawDead(Canvas dest, float x, float y, bool flip, float trans)
        {
            DrawMatrix mat = DrawMatrix.ScaledInstance;
            ColorTransform color = new ColorTransform(1.0f, 1.0f, 1.0f, trans);

            mat.translate(x, y);

            if (flip)
            {
                mat.flip(true, false);
            }

            dest.draw(imgDead, mat, color);
        }

        public void updateSFX(float x)
        {
            float pan = utils.pos2pan(x);

            if (pan > 1.0f) pan = 1.0f;
            else if (pan < -1.0f) pan = -1.0f;

            transformPan.Pan = pan;

            transformPanVol.Pan = pan;
            transformPanVol.Volume = 0.3f + utils.rnd() * 0.7f;
        }

        public void playAwake()
        {
            // sndAwake.play(49.0, 0, transformPan);
            Application.sharedSoundMgr.playSound(sndAwake);
        }

        public void playSleep()
        {
            // sndSleep.play(49.0, 0, transformPan);
            Application.sharedSoundMgr.playSound(sndSleep);
        }

        public void playJump()
        {
            // sndJump.play(49.0, 0, transformPan);
            Application.sharedSoundMgr.playSound(sndJump);
        }

        public void playLand()
        {
            // sndLand.play(49.0, 0, transformPan);
            Application.sharedSoundMgr.playSound(sndLand);

            //utils.playSound(land_snd, (power+0.3)*Math.abs(jumpVel)/200.0, pos.x+27);
        }

        public void playLandHeavy()
        {
            Application.sharedSoundMgr.playSound(sndLandHeavy);
        }

        public void playStep()
        {
            int snd;
            if (trigStep)
                snd = sndStep1;
            else
                snd = sndStep2;

            trigStep = !trigStep;

            // snd.play(49.0, 0, transformPanVol);
            Application.sharedSoundMgr.playSound(snd, transformPanVol);
            //var trans:SoundTransform = new SoundTransform(utils.rnd_float(0.3, 1.0f), utils.pos2pan(pos.x+duck_w));
        }

        public void playFlip()
        {
            Application.sharedSoundMgr.playSound(Res.SND_HERO_SALTO);
        }

        public void playWing()
        {
            int snd;

            if (trigWing)
                snd = sndWing1;
            else
                snd = sndWing2;

            trigWing = !trigWing;

            // snd.play(49.0, 0, transformPanVol);
            Application.sharedSoundMgr.playSound(snd, transformPanVol);

            //var trans:SoundTransform = new SoundTransform(utils.rnd_float(0.5, 1.0f)*(0.3 + power*0.7), utils.pos2pan(pos.x+duck_w));
        }

    }

}
