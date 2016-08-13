using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MikrotikApi.Protocol
{
    internal class Word
    {
        internal static Word Done = new Word("!done");
        internal static Word Trap = new Word("!trap");
        internal static Word Re = new Word("!re");
        internal static Word Fatal = new Word("!fatal");

        public byte[] Buffer
        {
            get
            {
                var word = Encoding.ASCII.GetBytes(String).ToList();
                var lengthPrefix = EncodeLengthPrefix((uint)String.Length);

                return lengthPrefix.Concat(word).ToArray();
            }
        }

        public string String { get; }

        public bool Empty => String.Length == 0;

        public override bool Equals(object other)
        {
            var word = other as Word;
            if (word != null)
            {
                return String == word.String;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        private static IEnumerable<byte> EncodeLengthPrefix(uint length)
        {
            var len = 0;
            ulong val = 0;

            if (length <= 0x7F)
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
                val = (ulong)length | 0xF000000000;
            }

            return BitConverter.GetBytes(val).Take(len).ToList();
        }

        public Word (string word)
        {
            String = word;
        }
    }
}
