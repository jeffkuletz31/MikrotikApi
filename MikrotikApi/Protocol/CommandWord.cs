using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikrotikApi.Protocol
{
    class CommandWord : Word
    {
        public CommandWord (List<string> command) : 
            base("/" + String.Join("/", command))
        {
            
        }

        public CommandWord (string command) :
            base(command)
        {

        }
    }
}
