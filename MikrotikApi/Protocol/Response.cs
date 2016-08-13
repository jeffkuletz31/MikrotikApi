using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikrotikApi.Protocol
{
    class Response : IList<ReplySentence>
    {
        public List<ReplySentence> replies = new List<ReplySentence>();

        public ReplySentence this[int index]
        {
            get
            {
                return ((IList<ReplySentence>)replies)[index];
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
                return ((IList<ReplySentence>)replies).Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return ((IList<ReplySentence>)replies).IsReadOnly;
            }
        }

        public bool Valid
        {
            get
            {
                if (this.Count > 0)
                {
                    return this.Last().Done;
                }

                return false;
            }
        }

        public void Add(ReplySentence item)
        {
            ((IList<ReplySentence>)replies).Add(item);
        }

        public void Clear()
        {
            ((IList<ReplySentence>)replies).Clear();
        }

        public bool Contains(ReplySentence item)
        {
            return ((IList<ReplySentence>)replies).Contains(item);
        }

        public void CopyTo(ReplySentence[] array, int arrayIndex)
        {
            ((IList<ReplySentence>)replies).CopyTo(array, arrayIndex);
        }

        public IEnumerator<ReplySentence> GetEnumerator()
        {
            return ((IList<ReplySentence>)replies).GetEnumerator();
        }

        public int IndexOf(ReplySentence item)
        {
            return ((IList<ReplySentence>)replies).IndexOf(item);
        }

        public void Insert(int index, ReplySentence item)
        {
            ((IList<ReplySentence>)replies).Insert(index, item);
        }

        public bool Remove(ReplySentence item)
        {
            return ((IList<ReplySentence>)replies).Remove(item);
        }

        public void RemoveAt(int index)
        {
            ((IList<ReplySentence>)replies).RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IList<ReplySentence>)replies).GetEnumerator();
        }
    }
}
