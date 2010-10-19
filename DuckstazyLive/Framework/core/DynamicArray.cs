using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Framework.core
{
    public class DynamicArray<T> : IEnumerable<T>
    {
        private const int INITIAL_SIZE = 10;
        private const int INCREMENT = 10;

        private T[] data;
        private int rc;

        public DynamicArray()
        {
            data = new T[INITIAL_SIZE];
            rc = 0;
        }

        public DynamicArray(int size)
        {
            data = new T[size];
            rc = 0;
        }

        public void removeAllObjects()
        {
            data = new T[INITIAL_SIZE];
            rc = 0;
        }

        public T this[int idx]
        {
            get
            {
                if (idx < 0 || idx >= data.Length)
                    return default(T);
                return data[idx];
            }

            set
            {
                if (idx < 0)
                    throw new IndexOutOfRangeException();
                if (data.Length <= idx)
                {
                    int len = data.Length;
                    while (len <= idx)
                        len += INCREMENT;

                    T[] newData = new T[len];
                    Array.Copy(data, newData, data.Length);
                    data = newData;
                }

                data[idx] = value;

                if (value != null)
                {
                    if (rc <= idx)
                        rc = idx + 1;
                }
                else
                {
                    while (rc > 0 && data[rc - 1] == null)
                        rc--;
                }
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] != null)
                    yield return data[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int getFirstEmptyIndex()
        {
            int i = 0;
            while (this[i] != null)
                i++;
            return i;
        }

        public void addObject(T obj)
        {
            this[count()] = obj;
        }

        public int getObjectIndex(T obj)
        {
            for (int i = 0; i < data.Length; i++)
            {
                // TODO: Is this really right ?
                if (data[i] != null && data[i].GetHashCode() == obj.GetHashCode())
                    return i;
            }
            return FrameworkConstants.UNDEFINED;
        }

        public int count()
        {
            return rc;
        }

        public void removeObject(T obj)
        {
            int idx = getObjectIndex(obj);
            if (idx >= 0)
                this[idx] = default(T);
        }
    }
}
