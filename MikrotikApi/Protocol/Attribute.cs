using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikrotikApi.Protocol
{
    class Attribute : Word
    {
        private string _name;
        private string _value;

        public Attribute(string name, string value) :
            base("=" + name + "=" + value)
        {
            _name = name;
            _value = value;
        }

        public string Value
        {
            get
            {
                return _value;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
        }
    }
}
