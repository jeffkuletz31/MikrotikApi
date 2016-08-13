using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikrotikApi.Protocol
{
    class AttributeWord : Word
    {
        private string _key;
        private string _value;

        public AttributeWord(string name, string value) :
            base("=" + name + "=" + value)
        {
            _key = name;
            _value = value;
        }

        public AttributeWord(KeyValuePair<string, string> pair) :
            base("=" + pair.Key + "=" + pair.Value)
        {
            _key = pair.Key;
            _value = pair.Value;
        }

        public string Value
        {
            get
            {
                return _value;
            }
        }

        public string Key
        {
            get
            {
                return _key;
            }
        }
    }
}
