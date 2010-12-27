using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Framework.core
{
    public abstract class BaseElement : InputListener
    {
        public const float ALIGN_MIN = 0.0f;
        public const float ALIGN_CENTER = 0.5f;
        public const float ALIGN_MAX = 1.0f;        

        public enum Timeline
        {
            NO_LOOP,
            PING_PONG,
            REPLAY
        }

        public interface TimelineDelegate
        {
            void elementTimelineFinished(BaseElement e);
        }

        public struct KeyFrame
        {
            public float x;
            public float y;
            public float scaleX;
            public float scaleY;
            public float rotation;
            public Color color;
            public float time;

            public KeyFrame(float x, float y, Color color, float scaleX, float scaleY, float rotation, float time)
            {
                this.x = x;
                this.y = y;
                this.scaleX = scaleX;
                this.scaleY = scaleY;
                this.rotation = rotation;
                this.color = color;
                this.time = time;
            }
        }

        public bool visible;        
        public bool updateable;
        private bool focusable;
        private bool focused;

        public float x;
        public float y;
        public float drawX;
        public float drawY;

        public int width;
        public int height;

        public float rotation;
        public float rotationCenterX;
        public float rotationCenterY;

        public float scaleX;
        public float scaleY;

        public Color color;        

        public float translateX;
        public float translateY;

        public float alignX;
        public float alignY;

        public float parentAlignX;
        public float parentAlignY;
                
        private BaseElement parent;

        // timeline support
        protected KeyFrame[] keyFrames;
        protected int nextKeyFrame;
        protected int keyFramesCount;
        protected int keyFramesCapacity;
        protected KeyFrame currentStepPerSecond;
        protected float keyFrameTimeLeft;
        public TimelineDelegate timelineDelegate;
        protected Timeline timelineLoopType;
        protected bool timelineDirReverse;

        public BaseElement() : this(0, 0)
        {
        }

        public BaseElement(int width, int height) : this(0, 0, width, height)
        {
        }

        public BaseElement(float x, float y, int width, int height)
        {
            visible = true;
            updateable = true;
            focusable = false;
            focused = false;

            this.x = x;
            this.y = y;

            this.width = width;
            this.height = height;

            rotation = 0;
            rotationCenterX = 0;
            rotationCenterY = 0;
            scaleX = 1.0f;
            scaleY = 1.0f;
            color = Color.White; //solidOpaqueRGBA;
            translateX = 0;
            translateY = 0;

            parentAlignX = parentAlignY = alignX = alignY = ALIGN_MIN;
            parent = null;            

            nextKeyFrame = FrameworkConstants.UNDEFINED;
            keyFrames = null;
            keyFramesCount = keyFramesCapacity = 0;            
        }

        public void setTimelineDelegate(TimelineDelegate td)
        {
            timelineDelegate = td;
        }

        public virtual void update(float delta)
        {
            if (nextKeyFrame != FrameworkConstants.UNDEFINED)
            {
                updateTimeline(delta);
            }            
        }

        public void restoreTransformations()
        {
            if (color != Color.White)
            {
                AppGraphics.SetColor(Color.White);
            }

            // if any transformation
            if (rotation != 0.0 || scaleX != 1.0 || scaleY != 1.0 || translateX != 0.0 || translateY != 0.0)
            {
                AppGraphics.PopMatrix();
            }
        }

        public virtual void preDraw()
        {
            // align to parent
            drawX = x - width * alignX;
            drawY = y - height * alignY;            

            if (parent != null)
            {
                drawX += parent.drawX + parent.width * parentAlignX;
                drawY += parent.drawY + parent.height * parentAlignY;
            }

            bool changeScale = (scaleX != 1.0 || scaleY != 1.0);
            bool changeRotation = (rotation != 0.0);
            bool changeTranslate = (translateX != 0.0 || translateY != 0.0);

            // apply transformations
            if (changeScale || changeRotation || changeTranslate)
            {
                AppGraphics.PushMatrix();

                if (changeScale || changeRotation)
                {
                    float rotationOffsetX = drawX + (width >> 1) + rotationCenterX;
                    float rotationOffsetY = drawY + (height >> 1) + rotationCenterY;

                    AppGraphics.Translate(rotationOffsetX, rotationOffsetY, 0);

                    if (changeRotation)
                    {
                        AppGraphics.Rotate(rotation, 0, 0, 1);
                    }

                    if (changeScale)
                    {
                        AppGraphics.Scale(scaleX, scaleY, 1);
                    }
                    AppGraphics.Translate(-rotationOffsetX, -rotationOffsetY, 0);
                }

                if (changeTranslate)
                {
                    AppGraphics.Translate(translateX, translateY, 0);
                }
            }

            if (color != Color.White)
            {
                AppGraphics.SetColor(color);
            }            
        }

        public virtual void postDraw()
        {           
            restoreTransformations();            
        }

        public virtual void draw()
        {
            preDraw();
            postDraw();
        }        

        public BaseElement Parent
        {
            get { return parent; }
            set { parent = value; }
        }        

        // timeline
        public void turnTimelineSupportWithMaxKeyFrames(int m)
        {
            keyFramesCapacity = m + 1; // 1 for the current state
            keyFrames = new KeyFrame[m + 1];
            timelineLoopType = Timeline.NO_LOOP;
            resetTimeline();
        }

        public void setTimelineLoopType(Timeline l)
        {
            timelineLoopType = l;
        }

        public void resetTimeline()
        {
            keyFramesCount = 1;
            nextKeyFrame = FrameworkConstants.UNDEFINED;
            timelineDirReverse = false;
        }

        public void deleteTimeline()
        {
            resetTimeline();
            keyFrames = null;
            keyFramesCapacity = 0;
        }

        public void addKeyFrame(KeyFrame k)
        {
            keyFrames[keyFramesCount++] = k;
        }

        public void updateTimeline(float delta)
        {
            keyFrameTimeLeft -= delta;

            color.R += (byte)(currentStepPerSecond.color.R * delta);
            color.G += (byte)(currentStepPerSecond.color.G * delta);
            color.B += (byte)(currentStepPerSecond.color.B * delta);
            color.A += (byte)(currentStepPerSecond.color.A * delta);
            rotation += currentStepPerSecond.rotation * delta;
            scaleX += currentStepPerSecond.scaleX * delta;
            scaleY += currentStepPerSecond.scaleY * delta;
            x += currentStepPerSecond.x * delta;
            y += currentStepPerSecond.y * delta;

            if (keyFrameTimeLeft <= 0)
            {
                color = keyFrames[nextKeyFrame].color;
                rotation = keyFrames[nextKeyFrame].rotation;
                scaleX = keyFrames[nextKeyFrame].scaleX;
                scaleY = keyFrames[nextKeyFrame].scaleY;
                x = keyFrames[nextKeyFrame].x;
                y = keyFrames[nextKeyFrame].y;

                timelineKeyFrameFinished();
            }
        }

        public void playTimeline()
        {
            nextKeyFrame = 1;
            KeyFrame currentState = new KeyFrame(x, y, color, scaleY, scaleY, rotation, 0);
            keyFrames[0] = currentState;
            initKeyFrameStep(keyFrames[0], keyFrames[nextKeyFrame], keyFrames[nextKeyFrame].time);
            //[self initKeyFrameStepFrom:&keyFrames[0] To:&keyFrames[nextKeyFrame] withTime:keyFrames[nextKeyFrame].time];
        }

        public void stopTimeline()
        {
            nextKeyFrame = FrameworkConstants.UNDEFINED;
        }

        public void initKeyFrameStep(KeyFrame src, KeyFrame dst, float t)
        {
            keyFrameTimeLeft = t;
            currentStepPerSecond.color.R = (byte)((dst.color.R - src.color.R) / keyFrameTimeLeft);
            currentStepPerSecond.color.G = (byte)((dst.color.G - src.color.G) / keyFrameTimeLeft);
            currentStepPerSecond.color.B = (byte)((dst.color.B - src.color.B) / keyFrameTimeLeft);
            currentStepPerSecond.color.A = (byte)((dst.color.A - src.color.A) / keyFrameTimeLeft);
            currentStepPerSecond.rotation = (dst.rotation - src.rotation) / keyFrameTimeLeft;
            currentStepPerSecond.scaleX = (dst.scaleX - src.scaleX) / keyFrameTimeLeft;
            currentStepPerSecond.scaleY = (dst.scaleY - src.scaleY) / keyFrameTimeLeft;
            currentStepPerSecond.x = (dst.x - src.x) / keyFrameTimeLeft;
            currentStepPerSecond.y = (dst.y - src.y) / keyFrameTimeLeft;
        }

        public void timelineKeyFrameFinished()
        {
            switch (timelineLoopType)
            {
                case Timeline.PING_PONG:
                    if (timelineDirReverse && nextKeyFrame == 0)
                    {
                        timelineDirReverse = false;
                    }
                    else if (!timelineDirReverse && nextKeyFrame == keyFramesCount - 1)
                    {
                        timelineDirReverse = true;
                    }

                    if (timelineDirReverse)
                    {
                        initKeyFrameStep(keyFrames[nextKeyFrame], keyFrames[nextKeyFrame - 1], keyFrames[nextKeyFrame].time);
                        //[self initKeyFrameStepFrom:&keyFrames[nextKeyFrame] To:&keyFrames[nextKeyFrame - 1] withTime:keyFrames[nextKeyFrame].time];
                        nextKeyFrame--;
                    }
                    else
                    {
                        initKeyFrameStep(keyFrames[nextKeyFrame], keyFrames[nextKeyFrame + 1], keyFrames[nextKeyFrame + 1].time);
                        //[self initKeyFrameStepFrom:&keyFrames[nextKeyFrame] To:&keyFrames[nextKeyFrame + 1] withTime:keyFrames[nextKeyFrame + 1].time];
                        nextKeyFrame++;
                    }
                    break;

                case Timeline.REPLAY:
                case Timeline.NO_LOOP:
                    if (nextKeyFrame < keyFramesCount - 1)
                    {
                        initKeyFrameStep(keyFrames[nextKeyFrame], keyFrames[nextKeyFrame + 1], keyFrames[nextKeyFrame + 1].time);
                        //[self initKeyFrameStepFrom:&keyFrames[nextKeyFrame] To:&keyFrames[nextKeyFrame + 1] withTime:keyFrames[nextKeyFrame + 1].time];
                        nextKeyFrame++;
                    }
                    else
                    {
                        if (timelineLoopType == Timeline.REPLAY)
                        {
                            nextKeyFrame = 0;
                            initKeyFrameStep(keyFrames[nextKeyFrame], keyFrames[nextKeyFrame + 1], keyFrames[nextKeyFrame + 1].time);
                            //[self initKeyFrameStepFrom:&keyFrames[nextKeyFrame] To:&keyFrames[nextKeyFrame + 1] withTime:keyFrames[nextKeyFrame + 1].time];
                            nextKeyFrame++;
                        }
                        else
                        {
                            stopTimeline();
                            //[self stopTimeline];
                            if (timelineDelegate != null)
                            {
                                timelineDelegate.elementTimelineFinished(this);
                                //[timelineDelegate elementTimelineFinished:self];
                            }
                        }
                    }
                    break;
            }
        }

        public void setEnabled(bool e)
        {
            visible = e;            
            updateable = e;            
        }

        public bool isEnabled()
        {
            return (visible && updateable);
        }

        public void setFocusable(bool f)
        {
            focusable = true;
        }

        public bool isFocusable()
        {
            return focusable;
        }

        public virtual bool isAcceptingInput()
        {
            return isFocusable() && isEnabled();
        }

        public void setFocused(bool f)
        {
            Debug.Assert(isFocusable());
            bool oldFocused = focused;
            focused = f;
            if (oldFocused)
            {
                if (!focused)
                {
                    focusLost();
                }
            }
            else // !oldFocused
            {
                if (focused)
                {
                    focusGained();
                }
            }
        }

        public bool isFocused()
        {
            return focused;
        }

        protected virtual void focusGained()
        {

        }

        protected virtual void focusLost()
        {

        }

        public void toParentCenter()
        {
            setAlign(ALIGN_CENTER, ALIGN_CENTER);
            setParentAlign(ALIGN_CENTER, ALIGN_CENTER);
        }

        public void setAlign(float alignX, float alignY)
        {
            this.alignX = alignX;
            this.alignY = alignY;
        }

        public void setParentAlign(float alignX, float alignY)
        {
            this.parentAlignX = alignX;
            this.parentAlignY = alignY;
        }

        public virtual bool buttonPressed(ref ButtonEvent e)
        {
            return false;
        }

        public virtual bool buttonReleased(ref ButtonEvent e)
        {   
            return false;
        }        
    }
}
