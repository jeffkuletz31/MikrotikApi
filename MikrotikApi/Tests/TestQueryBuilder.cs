using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MikrotikApi.Protocol;
using MikrotikApi;

namespace MikrotikApi.Tests
{
    [TestFixture]
    public class TestCommandBuilder
    {
        [Test]
        public void TestBuild()
        {
            var expect = new Sentence
            {
                new CommandWord("/ip/interfaces/print"),
                new QueryWord("propa"),
                new QueryWord("propb"),
                new QueryWord("#&")
            };

            var commandBuilder = new CommandBuilder("/ip/interfaces/print");
            commandBuilder.Query(qb => qb.Exists("propa") & qb.Exists("propb"));
            var actual = commandBuilder.Sentence;

            CollectionAssert.AreEqual(expect, actual);
        }

        [Test]
        public void TestComplexBuild()
        {
            var expect = new Sentence()
            {
                new CommandWord("/ip/interfaces/print"),
                new QueryWord("propa"),
                new QueryWord("propb"),
                new QueryWord("#&"),
                new QueryWord("propc"),
                new QueryWord("propd"),
                new QueryWord("#!"),
                new QueryWord("#&"),
                new QueryWord("#|")
            };

            var commandBuilder = new CommandBuilder("/ip/interfaces/print");
            commandBuilder.Query(qb =>
                (qb.Exists("propa") & qb.Exists("propb")) | (qb.Exists("propc") & !qb.Exists("propd"))
            );

            var actual = commandBuilder.Sentence;

            CollectionAssert.AreEqual(expect, actual);
        }
    }
}
