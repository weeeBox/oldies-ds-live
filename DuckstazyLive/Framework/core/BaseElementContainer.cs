using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Framework.core
{
    public class BaseElementContainer : BaseElement
    {
        public bool passTransformationsToChilds;
        public bool passButtonEventsToAllChilds;

        protected DynamicArray<BaseElement> childs;

        public BaseElementContainer(int width, int height) : this(0, 0, width, height)
        {
        }

        public BaseElementContainer(float x, float y, int width, int height) : base(x, y, width, height)
        {
            childs = new DynamicArray<BaseElement>();
            passTransformationsToChilds = true;
            passButtonEventsToAllChilds = true;
        }

        public override void update(float delta)
        {
            base.update(delta);

            foreach (BaseElement c in childs)
            {
                if (c != null && c.updateable)
                {
                    c.update(delta);
                }
            }
        }

        public override void postDraw()
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
        }

        public virtual void addChildWithId(BaseElement c, int i)
        {
            c.Parent = this;
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
            c.Parent = null;
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

        public override bool buttonPressed(ref ButtonEvent e)
        {
            if (passButtonEventsToAllChilds)
            {
                foreach (BaseElement c in childs)
                {
                    if (c != null && c.isAcceptingInput())
                    {
                        if (c.buttonPressed(ref e))
                            return true;
                    }
                }
            }

            return base.buttonPressed(ref e);
        }

        public override bool buttonReleased(ref ButtonEvent e)
        {
            if (passButtonEventsToAllChilds)
            {
                foreach (BaseElement c in childs)
                {
                    if (c != null && c.isAcceptingInput())
                    {
                        if (c.buttonReleased(ref e))
                            return true;
                    }
                }
            }

            return base.buttonReleased(ref e);
        }

        public override bool keyPressed(Keys key)
        {
            if (passButtonEventsToAllChilds)
            {
                foreach (BaseElement c in childs)
                {
                    if (c != null && c.isAcceptingInput())
                    {
                        if (c.keyPressed(key))
                            return true;
                    }
                }
            }

            return base.keyPressed(key);
        }

        public override bool keyReleased(Keys key)
        {
            if (passButtonEventsToAllChilds)
            {
                foreach (BaseElement c in childs)
                {
                    if (c != null && c.isAcceptingInput())
                    {
                        if (c.keyReleased(key))
                            return true;
                    }
                }
            }

            return base.keyReleased(key);
        }
    }
}
