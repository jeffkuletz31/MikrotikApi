using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikrotikApi.Protocol
{
    class ReplySentence : Sentence
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

        public bool Done
        {
            get
            {
                if (this.Valid)
                {
                    return this.First().Equals(Word.Done);
                }

                return false;
            }
        }

        public bool Re
        {
            get
            {
                if (this.Valid)
                {
                    return this.First().Equals(Word.Re);
                }

                return false;
            }
        }
    }
}
