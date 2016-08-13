using System;
using System.Collections.Generic;
using System.Linq;
using MikrotikApi.Protocol;

namespace MikrotikApi
{
    public class CommandBuilder
    {
        private readonly List<Word> _commandSentence;
        private QueryBuilder _queryBuilder;
        private readonly List<Word> _attributeSentence;

        public CommandBuilder(string command)
        {
            _commandSentence = new List<Word> {new CommandWord(command)};
            _attributeSentence = new List<Word>();
        }

        public void Attribute(string key, string value)
        {
            _attributeSentence.Add(new AttributeWord(key, value));
        }

        public void Query(Func<QueryBuilder, QueryBuilder> func)
        {
            _queryBuilder = func.Invoke(new QueryBuilder());
        }

        internal Sentence Sentence => (Sentence)_commandSentence
            .Concat(_attributeSentence)
            .Concat(_queryBuilder.Sentence)
            .ToList();
    }
}