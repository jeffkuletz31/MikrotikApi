using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikrotikApi.Protocol
{
    class Sentence : IList<Word>
    {
        private List<byte> _buffer = new List<byte>();
        private List<Word> _words = new List<Word>();

        public Word this[int index]
        {
            get
            {
                return ((IList<Word>)_words)[index];
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public byte[] Buffer
        {
            get
            {
                byte[] buffer = new byte[_buffer.Count + 1];
                _buffer.CopyTo(buffer);
                buffer[buffer.Length - 1] = 0;
                return buffer;
            }
        }

        public int Count
        {
            get
            {
                return ((IList<Word>)_words).Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return ((IList<Word>)_words).IsReadOnly;
            }
        }

        public void Add(Word item)
        {
            ((IList<Word>)_words).Add(item);
            _buffer = _buffer.Concat(item.Buffer.ToList()).ToList();
        }

        public void Clear()
        {
            ((IList<Word>)_words).Clear();
            _buffer = new List<Byte>();
        }

        public bool Contains(Word item)
        {
            return ((IList<Word>)_words).Contains(item);
        }

        public void CopyTo(Word[] array, int arrayIndex)
        {
            ((IList<Word>)_words).CopyTo(array, arrayIndex);
        }

        public IEnumerator<Word> GetEnumerator()
        {
            return ((IList<Word>)_words).GetEnumerator();
        }

        public int IndexOf(Word item)
        {
            return ((IList<Word>)_words).IndexOf(item);
        }

        public void Insert(int index, Word item)
        {
            throw new NotImplementedException();
        }

        public bool Remove(Word item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IList<Word>)_words).GetEnumerator();
        }
    }
}
