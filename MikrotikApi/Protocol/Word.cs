using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikrotikApi.Protocol
{

    class Word
    {
        private string _word;
        private List<byte> _buffer;
        internal static Word Done = new Word("!done");
        internal static Word Trap = new Word("!trap");
        internal static Word Re = new Word("!re");
        internal static Word Fatal = new Word("!fatal");

        public byte[] Buffer
        {
            get
            {
                List<byte> word = Encoding.ASCII.GetBytes(_word).ToList();
                List<byte> lengthPrefix = EncodeLengthPrefix((UInt32)_word.Length);

                return lengthPrefix.Concat(word).ToArray();
            }
        }

        public string String
        {
            get
            {
                return _word;
            }
        }

        public bool Empty { 
            get
            {
                return this._word.Length == 0;
            }
        }

        internal bool Equals(Word other)
        {
            return this.String == other.String;
        }

        private static List<byte> EncodeLengthPrefix(UInt32 length)
        {
            int len = 0;
            UInt64 val = 0;

            if (length >= 0 && length <= 0x7F)
            {
                len = 1;
                val = length;
            }
            else if (length >= 0x80 && length <= 0x3FFF)
            {
                len = 2;
                val = length | 0x8000;
            }
            else if (length >= 0x4000 && length <= 0x1FFFFF)
            {
                len = 3;
                val = length | 0xC00000;
            }
            else if (length >= 0x200000 && length <= 0xFFFFFF)
            {
                len = 4;
                val = length | 0xE0000000;
            }
            else if (length >= 0x10000000)
            {
                len = 5;
                val = (UInt64)length | 0xF000000000;
            }

            return BitConverter.GetBytes(val).Take(len).ToList();
        }

        public Word (string word)
        {
            _word = word;
        }
    }
}
