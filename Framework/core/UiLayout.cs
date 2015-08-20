using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Framework.core
{
    public struct AttachStyle
    {
        public float align;
        public float indentLT;
        public float indentRB;

        public static readonly AttachStyle MIN = new AttachStyle(0.0f);
        public static readonly AttachStyle CENTER = new AttachStyle(0.5f);
        public static readonly AttachStyle MAX = new AttachStyle(1.0f);        

        public AttachStyle(float align)
            : this(align, 0.0f, 0.0f)
        {
        }

        public AttachStyle(AttachStyle style, float indentLT, float indentRB) : this(style.align, indentLT, indentRB)
        {
        }

        public AttachStyle(float align, float indentLT, float indentRB)
        {
            this.align = align;
            this.indentLT = indentLT;
            this.indentRB = indentRB;
        }        
    }

    public class UiLayout
    {
        public const int UNDEFINED_DISTANCE = -1;        

        public static void attachHor(BaseElement item, BaseElement itemLT, BaseElement itemRB, AttachStyle style)
        {
            attachVertOrHor(item, itemLT, itemRB, style, true);
        }

        public static void attachVert(BaseElement item, BaseElement itemLT, BaseElement itemRB, AttachStyle style)
        {
            attachVertOrHor(item, itemLT, itemRB, style, false);
        }

        public static void attachCenter(BaseElement item, BaseElement itemLTRB)
        {
            attachHor(item, itemLTRB, itemLTRB, AttachStyle.CENTER);
            attachVert(item, itemLTRB, itemLTRB, AttachStyle.CENTER);
        }

        private static void attachVertOrHor(BaseElement item, BaseElement itemLT, BaseElement itemRB, AttachStyle style, bool attachHor)
        {
            Debug.Assert(itemLT != null || itemRB != null);


            BaseElement parent = item.getParent();

            float leftOrTop = 0;
            if (itemLT != null)
            {
                if (itemLT == parent)
                {
                    leftOrTop = 0;
                }
                else
                {
                    leftOrTop = attachHor ?
                                itemLT.x + itemLT.width :
                                itemLT.y + itemLT.height;
                }

                leftOrTop += style.indentLT;
            }

            float rightOrBottom = 0;
            if (itemRB != null)
            {
                if (itemRB == parent)
                {
                    rightOrBottom = attachHor ? parent.width : parent.height;
                }
                else
                {
                    rightOrBottom = attachHor ? itemRB.x : itemRB.y;
                }

                rightOrBottom -= style.indentRB;
            }

            if (itemLT != null && itemRB != null)
            {
                float dif = rightOrBottom - leftOrTop;
                dif -= attachHor ? item.width : item.height;
                leftOrTop += dif * style.align;
            }
            else
            {
                if (itemRB != null)
                {
                    leftOrTop = rightOrBottom;
                    leftOrTop -= attachHor ? item.width : item.height;
                }
            }

            if (attachHor)
            {
                item.x = leftOrTop;
            }
            else
            {

                item.y = leftOrTop;
            }
        }

        public static void resizeToFitItems(BaseElementContainer container)
        {
            resizeToFitItems(container, 0, 0, 0, 0);
        }

        public static void resizeToFitItemsHor(BaseElementContainer container, int leftIndent, int rightIndent)
        {
            resizeToFitItems(container, leftIndent, 0, rightIndent, 0);
        }

        public static void resizeToFitItemsVer(BaseElementContainer container, int topIndent, int bottomIndent)
        {
            resizeToFitItems(container, 0, topIndent, 0, bottomIndent);
        }

        public static void resizeToFitItems(BaseElementContainer container, int leftIndent, int topIndent, int rightIndent, int bottomIndent)
        {
            float rx = 0;
            float ry = 0;
            float rw = 0;
            float rh = 0;

            DynamicArray<BaseElement> childs = container.getChilds();
            foreach (BaseElement c in childs)
            {
                if (c == null)
                    continue;

                int iw = c.width;
                int ih = c.height;
                if (iw != 0 && ih != 0)
                {
                    float ix = c.x;
                    float iy = c.y;
                    if (rw == 0)
                    {
                        rx = ix;
                        ry = iy;
                        rw = iw;
                        rh = ih;
                    }
                    else
                    {
                        if (rx > ix)
                        {
                            rw += rx - ix;
                            rx = ix;
                        }
                        if (ry > iy)
                        {
                            rh += ry - iy;
                            ry = iy;
                        }
                        if ((rx + rw) < (ix + iw))
                            rw = ix + iw - rx;
                        if ((ry + rh) < (iy + ih))
                            rh = iy + ih - ry;
                    }
                }
            }

            rx -= leftIndent;
            ry -= topIndent;
            rw += leftIndent + rightIndent;
            rh += topIndent + bottomIndent;
            if (rx != 0 || ry != 0)
            {
                container.x += rx;
                container.y += ry;

                foreach (BaseElement c in childs)
                {
                    if (c != null)
                    {
                        c.x -= rx;
                        c.y -= ry;
                    }
                }
            }

            container.width = (int)rw;
            container.height = (int)rh;
        }

        public static void arrangeVertically(BaseElementContainer container)
        {
            arrangeVertically(container, UNDEFINED_DISTANCE, UNDEFINED_DISTANCE);
        }

        public static void arrangeVertically(BaseElementContainer container, int minDist, int maxDist)
        {
            arrangeInTable(container, minDist, maxDist, false);
        }

        public static void arrangeHorizontally(BaseElementContainer container)
        {
            arrangeHorizontally(container, UNDEFINED_DISTANCE, UNDEFINED_DISTANCE);
        }

        public static void arrangeHorizontally(BaseElementContainer container, int minDist, int maxDist)
        {
            arrangeInTable(container, minDist, maxDist, true);
        }

        private static void arrangeInTable(BaseElementContainer container, int minDist, int maxDist, bool isHoriz)
        {
            int sum = 0;
            int itemsCount = 0;

            DynamicArray<BaseElement> childs = container.getChilds();

            foreach (BaseElement c in childs)
            {
                if (c != null)
                {
                    sum += isHoriz ? c.width : c.height;
                    itemsCount++;
                }
            }

            if (itemsCount == 0)
                return;

            int cont = isHoriz ? container.width : container.height;

            if (itemsCount == 1)
            {
                minDist = UNDEFINED_DISTANCE;
                maxDist = (cont > 0) ? UNDEFINED_DISTANCE : 0;
            }

            float dist;
            if (minDist == UNDEFINED_DISTANCE)
                dist = (cont - sum) / ((float)(itemsCount + 1));
            else
                dist = (cont - sum) / ((float)(itemsCount - 1));

            if (maxDist != UNDEFINED_DISTANCE && dist > maxDist)
                dist = maxDist;
            if (minDist != UNDEFINED_DISTANCE && dist < minDist)
                dist = minDist;

            float pos = (minDist == UNDEFINED_DISTANCE) ? dist : 0;

            foreach (BaseElement c in childs)
            {
                if (c != null)
                {
                    if (isHoriz)
                    {
                        c.x = pos;
                        pos += c.width;
                    }
                    else
                    {
                        c.y = pos;
                        pos += c.height;
                    }

                    pos += dist;
                }
            }
        }
    }
}
