using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikrotikApi.Protocol
{
    class Command : Word
    {
        public Command (List<string> command) : 
            base("/" + String.Join("/", command))
        {
            
        }

        public Command (string command) :
            base(command)
        {

        }
    }
}
