using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jhu.VO
{
    public class ItemList<T> : ICollection<T>
    {
        private List<object> items;

        public int Count
        {
            get
            {
                return items.Where(i => i is T).Count();
            }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public ItemList(List<object> items)
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
            foreach (var e in items.Where(i => i is T).Cast<T>())
            {
                array[arrayIndex + q] = e;
                q++;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return items.Where(i => i is T).Cast<T>().GetEnumerator();
        }

        public bool Remove(T item)
        {
            return items.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return items.Where(i => i is T).GetEnumerator();
        }
    }
}
