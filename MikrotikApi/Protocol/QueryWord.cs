using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MikrotikApi.Protocol
{
    [DebuggerDisplay("{String}")]
    class QueryWord : Word
    {
        public QueryWord(string query)
            : base("?" + query)
        {
        }
    }
}
