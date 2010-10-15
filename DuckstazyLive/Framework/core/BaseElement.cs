using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Framework.core
{
    public enum HAlign
    {
        ALIGN_LEFT,
        ALIGN_CENTER,
        ALIGN_RIGHT
    }

    public enum VAlign
    {
        ALIGN_TOP,
        ALIGN_MIDDLE,
        ALIGN_BOTTOM
    }

    public abstract class BaseElement
    {
        public static int ANCHOR_LEFT = 1;
        public static int ANCHOR_HCENTER = 2;
        public static int ANCHOR_RIGHT = 4;
        public static int ANCHOR_TOP = 8;
        public static int ANCHOR_VCENTER = 16;
        public static int ANCHOR_BOTTOM = 32;
        public static int ANCHOR_CENTER = 2 | 16;

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
        public bool isAdditive;

        public float translateX;
        public float translateY;

        public int anchor;
        public int parentAnchor;

        public bool passTransformationsToChilds;
        public bool passTouchEventsToAllChilds;

        protected DynamicArray<BaseElement> childs;
        protected BaseElement parent;

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

        public BaseElement()
        {
            visible = true;            
            updateable = true;

            x = 0;
            y = 0;

            width = 0;
            height = 0;

            rotation = 0;
            rotationCenterX = 0;
            rotationCenterY = 0;
            scaleX = 1;
            scaleY = 1;
            color = Color.White; //solidOpaqueRGBA;
            translateX = 0;
            translateY = 0;

            parentAnchor = Constants.UNDEFINED; //# UNDEFINED; // TODO: Really UNDEFINED ???	
            parent = null;

            anchor = ANCHOR_TOP | ANCHOR_LEFT;

            childs = new DynamicArray<BaseElement>();

            nextKeyFrame = Constants.UNDEFINED;
            keyFrames = null;
            keyFramesCount = keyFramesCapacity = 0;

            passTransformationsToChilds = true;
            passTouchEventsToAllChilds = false;
        }

        public void setTimelineDelegate(TimelineDelegate td)
        {
            timelineDelegate = td;
        }

        public virtual void update(float delta)
        {
            if (nextKeyFrame != Constants.UNDEFINED)
            {
                updateTimeline(delta);
            }

            foreach (BaseElement c in childs)
            {
                if (c != null && c.updateable)
                {
                    c.update(delta);
                }
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
            if (parentAnchor != Constants.UNDEFINED)
            {
                if ((parentAnchor & ANCHOR_LEFT) != 0)
                {
                    drawX = parent.drawX + x;
                }
                else if ((parentAnchor & ANCHOR_HCENTER) != 0)
                {
                    drawX = parent.drawX + x + (parent.width >> 1);
                }
                else if ((parentAnchor & ANCHOR_RIGHT) != 0)
                {
                    drawX = parent.drawX + x + parent.width;
                }

                if ((parentAnchor & ANCHOR_TOP) != 0)
                {
                    drawY = parent.drawY + y;
                }
                else if ((parentAnchor & ANCHOR_VCENTER) != 0)
                {
                    drawY = parent.drawY + y + (parent.height >> 1);
                }
                else if ((parentAnchor & ANCHOR_BOTTOM) != 0)
                {
                    drawY = parent.drawY + y + parent.height;
                }
            }
            else
            {
                drawX = x;
                drawY = y;
            }

            // align self anchor
            if ((anchor & ANCHOR_TOP) == 0)
            {
                if ((anchor & ANCHOR_VCENTER) != 0)
                {
                    drawY -= height >> 1;
                }
                else if ((anchor & ANCHOR_BOTTOM) != 0)
                {
                    drawY -= height;
                }
            }

            if ((anchor & ANCHOR_LEFT) == 0)
            {
                if ((anchor & ANCHOR_HCENTER) != 0)
                {
                    drawX -= width >> 1;
                }
                else if ((anchor & ANCHOR_RIGHT) != 0)
                {
                    drawX -= width;
                }
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

            if (isAdditive)
                AppGraphics.SetBlendMode(AppBlendMode.Additive);
        }

        public virtual void postDraw()
        {
            if (!passTransformationsToChilds)
            {
                restoreTransformations();
            }

            foreach (BaseElement c in childs)
            {
                if (c != null && c.visible)
                {
                    c.draw();
                }
            }

            if (passTransformationsToChilds)
            {
                restoreTransformations();
            }

            if (isAdditive)
                AppGraphics.SetBlendMode(AppBlendMode.AlphaBlend);
        }

        public virtual void draw()
        {
            preDraw();
            postDraw();
        }

        public virtual void addChildWithId(BaseElement c, int i)
        {
            c.parent = this;
            childs[i] = c;
        }

        public virtual int addChild(BaseElement c)
        {
            int index = childs.getFirstEmptyIndex();
            addChildWithId(c, index);
            return index;
        }

        public void removeChildWithId(int i)
        {
            BaseElement c = childs[i];
            c.parent = null;
            childs[i] = null;
        }

        public void removeChild(BaseElement c)
        {
            int index = childs.getObjectIndex(c);
            removeChildWithId(index);
        }

        public void removeAllChilds()
        {
            childs = new DynamicArray<BaseElement>();
        }

        public BaseElement getChild(int i)
        {
            return childs[i];
        }

        public DynamicArray<BaseElement> getChilds()
        {
            return childs;
        }

        public int childsCount()
        {
            return childs.count();
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
            nextKeyFrame = Constants.UNDEFINED;
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
            nextKeyFrame = Constants.UNDEFINED;
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
    }
}
