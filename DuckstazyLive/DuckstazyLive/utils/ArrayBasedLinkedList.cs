using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuckstazyLive.utils
{
    public class ArrayBaseLinkedList
    {
        static readonly int UNDEFINED = -1;
        
        int[] itemsNext;
        int[] itemsPrev;
        int itemFreeRoot;
        int itemFirst;
        int itemLast;
        int size;

        public ArrayBaseLinkedList(int capacity)
        {
            itemsNext = new int[capacity];
            itemsPrev = new int[capacity];            

            for (int i = 0; i < capacity; i++)
            {
                itemsNext[i] = i + 1;
                itemsPrev[i] = i - 1;
            }
            itemsNext[capacity - 1] = UNDEFINED;
            itemsPrev[0] = UNDEFINED;

            itemFirst = UNDEFINED;
            itemLast = UNDEFINED;
            itemFreeRoot = 0;
        }

        public int Add()
        {
            if (itemFreeRoot == UNDEFINED)
            {
                return -1;
            }

            int item = itemFreeRoot;
            itemFreeRoot = itemsNext[item];

            if (itemFirst == UNDEFINED)
            {
                itemFirst = item;
            }
            itemLast = item;
            size++;

            return item;
        }

        public void Remove(int item)
        {
            if (itemFirst == UNDEFINED)
                throw new InvalidOperationException("Cannot delete item from empty list: " + item);

            if (itemFirst == item)
            {
                if (size > 1)
                {
                    itemFirst = itemsNext[item];
                    itemsPrev[itemFirst] = UNDEFINED;
                }
                else
                {
                    itemFirst = UNDEFINED;
                    itemLast = UNDEFINED;
                }
            }
            else
            {
                itemLast = itemsPrev[item];
            }

            if (itemFreeRoot == UNDEFINED)
            {
                linkThem(itemLast, item);
                itemsNext[item] = UNDEFINED;
            }
            else
            {
                linkThem(itemsPrev[itemFreeRoot], item);
                linkThem(item, itemFreeRoot);
            }
            itemFreeRoot = item;
            size--;
        }

        private void linkThem(int firstItem, int secondItem)
        {
            if (firstItem != UNDEFINED)
                itemsNext[firstItem] = secondItem;
            if (secondItem != UNDEFINED)
                itemsPrev[secondItem] = firstItem;
        }        

        public int First
        {
            get { return itemFirst; }
        }

        public int Last
        {
            get { return itemLast; }
        }

        public bool HasNext(int item)
        {
            return item != UNDEFINED && item != itemFreeRoot;
        }        

        public int Next(int item)
        {
            return itemsNext[item];
        }

        public int Size
        {
            get { return size; }
        }
    }
}
