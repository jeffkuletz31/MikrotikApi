using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikrotikApi.Protocol
{
    class Reply : Sentence
    {
        public bool Valid
        {
            get
            {
                if (this.Count > 0)
                {
                    return this.Last().Empty;
                }

                return false;
            }
        }

        internal static bool IsDone(Reply reply)
        {
            if (reply.Valid)
            {
                if (reply.First().Equals(Word.Done))
                {
                    return true;
                }
            }

            return false;
        }

        internal static bool IsRe(Reply reply)
        {
            if (reply.Valid)
            {
                if (reply.First().Equals(Word.Re))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
