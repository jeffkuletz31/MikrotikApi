using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikrotikApi.Protocol
{
    class Response : IList<Reply>
    {
        public List<Reply> replies = new List<Reply>();

        public Reply this[int index]
        {
            get
            {
                return ((IList<Reply>)replies)[index];
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public int Count
        {
            get
            {
                return ((IList<Reply>)replies).Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return ((IList<Reply>)replies).IsReadOnly;
            }
        }

        public bool Valid
        {
            get
            {
                bool valid = false;

                if (this.Count > 0)
                {
                    valid = Reply.IsDone(this.Last());
                }

                return valid;
            }
        }

        public void Add(Reply item)
        {
            ((IList<Reply>)replies).Add(item);
        }

        public void Clear()
        {
            ((IList<Reply>)replies).Clear();
        }

        public bool Contains(Reply item)
        {
            return ((IList<Reply>)replies).Contains(item);
        }

        public void CopyTo(Reply[] array, int arrayIndex)
        {
            ((IList<Reply>)replies).CopyTo(array, arrayIndex);
        }

        public IEnumerator<Reply> GetEnumerator()
        {
            return ((IList<Reply>)replies).GetEnumerator();
        }

        public int IndexOf(Reply item)
        {
            return ((IList<Reply>)replies).IndexOf(item);
        }

        public void Insert(int index, Reply item)
        {
            ((IList<Reply>)replies).Insert(index, item);
        }

        public bool Remove(Reply item)
        {
            return ((IList<Reply>)replies).Remove(item);
        }

        public void RemoveAt(int index)
        {
            ((IList<Reply>)replies).RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IList<Reply>)replies).GetEnumerator();
        }
    }
}
