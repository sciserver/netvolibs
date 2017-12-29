using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jhu.VO
{
    public class ElementList<T> : IList<T>
    {
        private IList items;

        public int Count
        {
            get
            {
                int q = 0;
                for (int i = 0; i < items.Count; i++)
                {
                    if (items[i] is T)
                    {
                        q++;
                        
                    }
                }
                return q;
            }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public T this[int index]
        {
            get
            {
                int q = 0;
                for (int i = 0; i < items.Count; i++)
                {
                    if (items[i] is T)
                    {
                        if (q == index)
                        {
                            return (T)items[i];
                        }
                        q++;
                    }
                }

                throw new IndexOutOfRangeException();
            }
            set
            {
                int q = 0;
                for (int i = 0; i < items.Count; i++)
                {
                    if (items[i] is T)
                    {
                        if (q == index)
                        {
                            items[i] = value;
                            return;
                        }
                        q++;
                    }
                }

                throw new IndexOutOfRangeException();
            }
        }

        public ElementList(IList items)
        {
            this.items = items;
        }

        public void Add(T item)
        {
            items.Add(item);
        }

        public void Clear()
        {
            for (int i = items.Count - 1; i >= 0; i--)
            {
                if (items[i] is T)
                {
                    items.RemoveAt(i);
                }
            }
        }

        public bool Contains(T item)
        {
            return items.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            int q = 0;
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i] is T)
                {
                    array[arrayIndex + q] = (T)items[i];
                    q++;
                }
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new ElementListEnumerator<T>(items);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new ElementListEnumerator<T>(items);
        }

        public bool Remove(T item)
        {
            var c = items.Count;
            items.Remove(item);
            return c != items.Count;
        }
        
        public int IndexOf(T item)
        {
            int q = 0;
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i] is T)
                {
                    if (items[i].Equals(item))
                    {
                        return q;
                    }
                    q++;
                }
            }

            return -1;
        }

        public void Insert(int index, T item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }
    }
}
