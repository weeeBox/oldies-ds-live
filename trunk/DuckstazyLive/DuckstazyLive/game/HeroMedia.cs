﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        //[Embed(source="gfx/eye1.png")]
        //private var rEyeImg1:Class;

        //[Embed(source="gfx/eye2.png")]
        //private var rEyeImg2:Class;

        private Texture2D imgDuck;
        //private var imgDuckFlip:Texture2D;

        private Texture2D imgSleep;
        // private var imgSleepFlip:Texture2D;

        private Texture2D imgWing;
        private Texture2D imgEye1;
        private Texture2D imgEye2;

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

        private Sound sndStep1;
        private Sound sndStep2;
        private Sound sndWing1;
        private Sound sndWing2;
        private Sound sndJump;
        private Sound sndLand;
        private Sound sndAwake;
        private Sound sndSleep;
        public Sound sndAttack;
        public Sound sndToxic;

        private SoundTransform transformPan;
        private SoundTransform transformPanVol;
        private bool trigStep;
        private bool trigWing;

        public function HeroMedia()
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

            imgEye1 = (new rEyeImg1()).bitmapData;
            imgEye2 = (new rEyeImg2()).bitmapData;
            imgWing = (new rWingImg()).bitmapData;

            /*imgDuck = new Texture2D(54, 42, true, 0x00000000);
            imgDuck.lock();
            imgDuck.copyPixels(duckBitmap.bitmapData, rcBody, new Point());
            imgDuck.copyPixels(footsBitmap.bitmapData, rcFoots, new Point(30.0, 39.0));
            imgDuck.unlock();
			
            imgSleep = new Texture2D(54, 42, true, 0x00000000);
            imgSleep.lock();
            imgSleep.copyPixels(sleepBitmap.bitmapData, rcBody, new Point());
            imgSleep.unlock();*/

            imgDuck = (new rDuckImg()).bitmapData;
            imgSleep = (new rSleepImg()).bitmapData;

            /*mat.scale(-1.0, 1.0);
            mat.translate(54.0, 0.0);
			
            imgDuckFlip = new Texture2D(54, 42, true, 0x00000000);			
            imgDuckFlip.lock();
            imgDuckFlip.draw(duckBitmap.bitmapData, mat);
            imgDuckFlip.copyPixels(footsBitmap.bitmapData, rcFoots, new Point(12.0, 39.0));
            imgDuckFlip.unlock();
					
            imgSleepFlip = new Texture2D(54, 42, true, 0x00000000);			
            imgSleepFlip.lock();
            imgSleepFlip.draw(sleepBitmap.bitmapData, mat);
            imgSleepFlip.unlock();*/
        }

        private void initSFX()
        {
            sndStep1 = new rStepSnd1();
            sndStep2 = new rStepSnd2();
            sndWing1 = new rWingSnd1();
            sndWing2 = new rWingSnd2();
            sndJump = new rJumpSnd();
            sndLand = new rLandSnd();
            sndAwake = new rAwakeSnd();
            sndSleep = new rSleepSnd();
            sndAttack = new rAttackSnd();
            sndToxic = new rToxicSnd();

            transformPan = new SoundTransform();
            transformPanVol = new SoundTransform();
        }

        public void drawDuck(B dest, N x, N y, N power, B flip, N wingsAngle)
        {
            Matrix mat = new Matrix(1, 0, 0, 1, x, y);
            /*Matrix eye_mat = new Matrix();
            Matrix wing_mat = new Matrix();*/
            ColorTransform eye = new ColorTransform(1.0, 1.0, 1.0, 1.0 - power);
            //var eye2_color:ColorTransform = new ColorTransform(1.0, 1.0, 1.0, power);

            if (flip)
            {
                mat.a = -1;
                mat.tx = x + 54.0;
                //dest.copyPixels(imgDuckFlip, rcHero, new Point(x, y));
                dest.draw(imgDuck, mat, null, null, null, true);

                mat.tx = 44.0 + x;
                mat.ty = 5.0 + y;
                dest.draw(imgEye1, mat, eye, null, null, true);
                eye.alphaMultiplier = power;
                dest.draw(imgEye2, mat, eye, null, null, true);

                mat.tx = 3.0;
                mat.ty = -7.0;
                mat.rotate(-wingsAngle);
                mat.translate(21.0 + x, 26.0 + y);
                dest.draw(imgWing, mat, null, null, null, true);
            }
            else
            {
                //dest.copyPixels(imgDuck, rcHero, new Point(x, y));
                dest.draw(imgDuck, mat, null, null, null, true);

                mat.tx = 10.0 + x;
                mat.ty = 5.0 + y;
                dest.draw(imgEye1, mat, eye, null, null, true);
                eye.alphaMultiplier = power;
                dest.draw(imgEye2, mat, eye, null, null, true);

                mat.tx = -3.0;
                mat.ty = -7.0;
                mat.rotate(wingsAngle);
                mat.translate(33.0 + x, 26.0 + y);
                dest.draw(imgWing, mat, null, null, null, true);
            }

            //dest.draw(imgEye1.bitmapData, eye_mat, eye1_color);
            //dest.draw(imgEye2.bitmapData, eye_mat, eye2_color);
            //dest.draw(imgWing.bitmapData, wing_mat, null, null, null, wingsAngle!=0.0);
        }

        public void drawSleep(B dest, N x, N y, B flip)
        {
            /*if(flip)
                dest.copyPixels(imgSleepFlip, rcHero, new Point(x, y));
            else
                dest.copyPixels(imgSleep, rcHero, new Point(x, y));*/

            Matrix mat = new Matrix(1, 0, 0, 1, x, y);

            if (flip)
            {
                mat.a = -1;
                mat.tx = x + 54.0;
            }

            dest.draw(imgSleep, mat, null, null, null, true);
        }

        public void updateSFX(N x)
        {
            float pan = utils.pos2pan(x);

            if (pan > 1.0) pan = 1.0;
            else if (pan < -1.0) pan = -1.0;

            transformPan.pan = pan;

            transformPanVol.pan = pan;
            transformPanVol.volume = 0.3 + Math.random() * 0.7;
        }

        public void playAwake()
        {
            sndAwake.play(49.0, 0, transformPan);
        }

        public void playSleep()
        {
            sndSleep.play(49.0, 0, transformPan);
        }

        public void playJump()
        {
            sndJump.play(49.0, 0, transformPan);
        }

        public void playLand()
        {
            sndLand.play(49.0, 0, transformPan);
            //utils.playSound(land_snd, (power+0.3)*Math.abs(jumpVel)/200.0, pos.x+27);
        }

        public void playStep()
        {
            Sound snd;

            if (trigStep)
                snd = sndStep1;
            else
                snd = sndStep2;

            trigStep = !trigStep;

            snd.play(49.0, 0, transformPanVol);
            //var trans:SoundTransform = new SoundTransform(utils.rnd_float(0.3, 1.0), utils.pos2pan(pos.x+duck_w));
        }

        public void playWing()
        {
            Sound snd;

            if (trigWing)
                snd = sndWing1;
            else
                snd = sndWing2;

            trigWing = !trigWing;

            snd.play(49.0, 0, transformPanVol);

            //var trans:SoundTransform = new SoundTransform(utils.rnd_float(0.5, 1.0)*(0.3 + power*0.7), utils.pos2pan(pos.x+duck_w));
        }

    }

}
