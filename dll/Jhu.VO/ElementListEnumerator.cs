using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jhu.VO
{
    public class ElementListEnumerator<T> : IEnumerator<T>
    {
        private IList items;
        private int current;

        public T Current
        {
            get { return (T)items[current]; }
        }
        
        object IEnumerator.Current
        {
            get { return items[current]; }
        }

        public ElementListEnumerator(IList items)
        {
            this.items = items;
            this.current = -1;
        }

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            for (int i = current + 1; i < items.Count; i++)
            {
                if (items[i] is T)
                {
                    current = i;
                    return true;
                }
            }

            return false;
        }

        public void Reset()
        {
            throw new NotImplementedException();   
        }
    }
}
