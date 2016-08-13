using System.Collections.Generic;
using System.Linq;
using MikrotikApi.Protocol;

namespace MikrotikApi
{
    public class QueryBuilder
    {
        private readonly Stack<Word> _query;

        public QueryBuilder()
        {
            _query = new Stack<Word>();
        }

        private QueryBuilder(Word word)
        {
            _query = new Stack<Word>();
            _query.Push(word);
        }

        public static QueryBuilder operator !(QueryBuilder builder)
        {
            var queryBuilder = new QueryBuilder();
            queryBuilder.Push(builder);
            queryBuilder.Push(new QueryWord("#!"));

            return queryBuilder;
        }

        public static QueryBuilder operator |(QueryBuilder leftBuilder, QueryBuilder rightBuilder)
        {
            var queryBuilder = new QueryBuilder();
            queryBuilder.Push(leftBuilder);
            queryBuilder.Push(rightBuilder);
            queryBuilder.Push(new QueryWord("#|"));

            return queryBuilder;
        }

        public static QueryBuilder operator &(QueryBuilder leftBuilder, QueryBuilder rightBuilder)
        {
            var queryBuilder = new QueryBuilder();
            queryBuilder.Push(leftBuilder);
            queryBuilder.Push(rightBuilder);
            queryBuilder.Push(new QueryWord("#&"));

            return queryBuilder;
        }

        public QueryBuilder Exists(string prop)
        {
            return new QueryBuilder(new QueryWord(prop));
        }

        public QueryBuilder NotExists(string prop)
        {
            return new QueryBuilder(new QueryWord("-" + prop));
        }

        public QueryBuilder Equals(string prop, string value)
        {
            return new QueryBuilder(new QueryWord(prop + "=" + value));
        }

        public QueryBuilder LessThan(string prop, string value)
        {
            return new QueryBuilder(new QueryWord("<" + prop + "=" + value));
        }

        public QueryBuilder GreaterThan(string prop, string value)
        {
            return new QueryBuilder(new QueryWord(">" + prop + "=" + value));
        }

        public QueryBuilder HasValue(string prop)
        {
            return GreaterThan(prop, "");
        }

        private void Push(Word queryWord)
        {
            _query.Push(queryWord);
        }

        private void Push(QueryBuilder queryBuilder)
        {
            queryBuilder.Sentence.ForEach(w => _query.Push(w));
        }

        internal List<Word> Sentence
        {
            get
            {
                var sentence = _query.ToList();
                sentence.Reverse();
                return sentence;
            }
        }
    }
}