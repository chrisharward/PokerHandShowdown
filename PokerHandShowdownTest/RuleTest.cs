using Microsoft.VisualStudio.TestTools.UnitTesting;

using PokerHandShowdown;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PokerHandShowdownTest
{
    [TestClass]
    public class RuleTest
    {
        [TestMethod]
        public void HighCardCompareTest()
        {
            const string testcase0 = "Player1 8H 8C 8D QH QC\r\nplayer2 8H 8C 8D QH QC";
            var builder = new PlayerBuilder(new Tokenizer(new StringReader(testcase0)));
            Assert.AreEqual(0, RuleUtils.HighCardCompare(builder.Build(), builder.Build()));

            const string testcase1 = "Player1 7H 8C 9D 10H JC\r\nplayer2 7H 8C 9D 10H JC";
            builder = new PlayerBuilder(new Tokenizer(new StringReader(testcase1)));
            Assert.AreEqual(0, RuleUtils.HighCardCompare(builder.Build(), builder.Build()));

            const string testcase2 = "Player1 7H 8C 9D 10H JC\r\nplayer2 6H 8C 9D 10H JC";
            builder = new PlayerBuilder(new Tokenizer(new StringReader(testcase2)));
            Assert.AreEqual(1, RuleUtils.HighCardCompare(builder.Build(), builder.Build()));

            const string testcase3 = "Player1 6H 8C 9D 10H JC\r\nplayer2 7H 8C 9D 10H JC";
            builder = new PlayerBuilder(new Tokenizer(new StringReader(testcase3)));
            Assert.AreEqual(-1, RuleUtils.HighCardCompare(builder.Build(), builder.Build()));

            const string testcase4 = "Player1 7H 9C 9D 10H JC\r\nplayer2 7H 8C 9D 10H JC";
            builder = new PlayerBuilder(new Tokenizer(new StringReader(testcase4)));
            Assert.AreEqual(1, RuleUtils.HighCardCompare(builder.Build(), builder.Build()));

            const string testcase5 = "Player1 7H 9C 9D 10H JC\r\nplayer2 7H 8C 10D 10H JC";
            builder = new PlayerBuilder(new Tokenizer(new StringReader(testcase5)));
            Assert.AreEqual(-1, RuleUtils.HighCardCompare(builder.Build(), builder.Build()));

            const string testcase6 = "Player1 7H 9C 10D 10H JC\r\nplayer2 7H 8C 9D 10H QC";
            builder = new PlayerBuilder(new Tokenizer(new StringReader(testcase6)));
            Assert.AreEqual(-1, RuleUtils.HighCardCompare(builder.Build(), builder.Build()));

            const string testcase7 = "Player1 7H 9C 10D 10H 10C\r\nplayer2 7H 8C 9D 10H 10C";
            builder = new PlayerBuilder(new Tokenizer(new StringReader(testcase7)));
            Assert.AreEqual(1, RuleUtils.HighCardCompare(builder.Build(), builder.Build()));
        }

        [TestMethod]
        public void HighCardRuleTest()
        {
            var rule = new HighCardRule();

            const string testcase0 = "Player1 8H ";
            var builder = new RoundBuilder(new StringReader(testcase0));
            var ex = Assert.ThrowsException<Exception>(() => builder.Build());
            Assert.AreEqual(ex.Message, "Invalid player format");

            const string testcase1 = "Player1 8H 7C 9D AH KC\r\nplayer2 7D 6C 5H QS 10H";
            builder = new RoundBuilder(new StringReader(testcase1));
            var winners = rule.Apply(builder.Build());
            Assert.IsNotNull(winners);
            Assert.AreEqual(1, winners.Count);
            Assert.AreEqual("Player1", winners[0].Name);

            const string testcase2 = "Player1 8H 7C 9D AH KC\r\nplayer2 7D 6C 5H AS 10H";
            builder = new RoundBuilder(new StringReader(testcase2));
            winners = rule.Apply(builder.Build());
            Assert.IsNotNull(winners);
            Assert.AreEqual(1, winners.Count);
            Assert.AreEqual("Player1", winners[0].Name);

            const string testcase3 = "Player1 8H 7C 9D AH KC\r\nplayer2 7D 6C 5H AS KH";
            builder = new RoundBuilder(new StringReader(testcase3));
            winners = rule.Apply(builder.Build());
            Assert.IsNotNull(winners);
            Assert.AreEqual(1, winners.Count);
            Assert.AreEqual("Player1", winners[0].Name);

            const string testcase4 = "Player1 8H 7C 9D AH KC\r\nplayer2 7D 8C 9H AS KH";
            builder = new RoundBuilder(new StringReader(testcase4));
            winners = rule.Apply(builder.Build());
            Assert.IsNotNull(winners);
            Assert.AreEqual(2, winners.Count);
            Assert.AreEqual("Player1", winners[0].Name);
            Assert.AreEqual("player2", winners[1].Name);

            const string testcase5 = "Player1 8H 7C 9D JH 10C\r\nPlayer2 7D 6C 5H QS 10H\r\nPlayer3 7D KC 5H QS 10H\r\nPlayer4 7D KC AH QS 10H";
            builder = new RoundBuilder(new StringReader(testcase5));
            winners = rule.Apply(builder.Build());
            Assert.IsNotNull(winners);
            Assert.AreEqual(1, winners.Count);
            Assert.AreEqual("Player4", winners[0].Name);
        }

        [TestMethod]
        public void MultiCompareTest()
        {
            const string testcase0 = "Player1 8H 8C 8D QH QC\r\nplayer2 8H 8C 8D QH QC";
            var builder = new PlayerBuilder(new Tokenizer(new StringReader(testcase0)));
            Assert.AreEqual(0, RuleUtils.MultiCompare(builder.Build(), builder.Build()));

            const string testcase1 = "Player1 8H 8C 8D QH KC\r\nplayer2 8H 8C 8D QH QC";
            builder = new PlayerBuilder(new Tokenizer(new StringReader(testcase1)));
            Assert.AreEqual(1, RuleUtils.MultiCompare(builder.Build(), builder.Build()));

            const string testcase2 = "Player1 6H 8C 8D QH QC\r\nplayer2 7H 8C 8D QH QC";
            builder = new PlayerBuilder(new Tokenizer(new StringReader(testcase2)));
            Assert.AreEqual(-1, RuleUtils.MultiCompare(builder.Build(), builder.Build()));

            const string testcase3 = "Player1 6H 8C 8D QH QC\r\nplayer2 5H 8C 8D QH QC";
            builder = new PlayerBuilder(new Tokenizer(new StringReader(testcase3)));
            Assert.AreEqual(1, RuleUtils.MultiCompare(builder.Build(), builder.Build()));

            const string testcase4 = "Player1 6H 6C 6D 6H QC\r\nplayer2 7H 7C 7D 7H QC";
            builder = new PlayerBuilder(new Tokenizer(new StringReader(testcase4)));
            Assert.AreEqual(-1, RuleUtils.MultiCompare(builder.Build(), builder.Build()));

            const string testcase5 = "Player1 6H 6C 6D 6H QC\r\nplayer2 6H 6C 6D 6H QC";
            builder = new PlayerBuilder(new Tokenizer(new StringReader(testcase5)));
            Assert.AreEqual(0, RuleUtils.MultiCompare(builder.Build(), builder.Build()));

            const string testcase6 = "Player1 6H 6C 6D 6H QC\r\nplayer2 6H 6C 6D 6H KC";
            builder = new PlayerBuilder(new Tokenizer(new StringReader(testcase6)));
            Assert.AreEqual(-1, RuleUtils.MultiCompare(builder.Build(), builder.Build()));

            const string testcase7 = "Player1 8H 7C 9D KH KC\r\nplayer2 7D 6C 5H KS KH";
            builder = new PlayerBuilder(new Tokenizer(new StringReader(testcase7)));
            Assert.AreEqual(1, RuleUtils.MultiCompare(builder.Build(), builder.Build()));
        }

        [TestMethod]
        public void HighCardBigTest()
        {
            var rule = new HighCardRule();

            var builder = new RoundBuilder(new StringReader(testcase_bighighcard));
            var round = builder.Build();
            var winners = rule.Apply(round);
            Assert.AreEqual(100000, winners.Count);
        }

        [TestMethod]
        public void OnePairRuleTest()
        {
            var rule = new OnePairRule();

            const string testcase0 = "Player1 8H ";
            var builder = new RoundBuilder(new StringReader(testcase0));
            var ex = Assert.ThrowsException<Exception>(() => builder.Build());
            Assert.AreEqual(ex.Message, "Invalid player format");

            const string testcase1 = "Player1 8H 7C 9D AH KC\r\nplayer2 7D 6C 5H QS 10H";
            builder = new RoundBuilder(new StringReader(testcase1));
            var winners = rule.Apply(builder.Build());
            Assert.IsNotNull(winners);
            Assert.AreEqual(0, winners.Count);

            const string testcase2 = "Player1 8H 7C 9D KH KC\r\nplayer2 7D 6C 5H KS KH";
            builder = new RoundBuilder(new StringReader(testcase2));
            winners = rule.Apply(builder.Build());
            Assert.IsNotNull(winners);
            Assert.AreEqual(1, winners.Count);
            Assert.AreEqual("Player1", winners[0].Name);

            const string testcase3 = "Player1 8H 7C 9D 9H KC\r\nplayer2 7D 6C 9H 9S KH";
            builder = new RoundBuilder(new StringReader(testcase3));
            winners = rule.Apply(builder.Build());
            Assert.IsNotNull(winners);
            Assert.AreEqual(1, winners.Count);
            Assert.AreEqual("Player1", winners[0].Name);

            const string testcase4 = "Player1 8H 7C 9D 9H KC\r\nPlayer2 8D 8C 9H 9S KH";
            builder = new RoundBuilder(new StringReader(testcase4));
            winners = rule.Apply(builder.Build());
            Assert.IsNotNull(winners);
            Assert.AreEqual(1, winners.Count);
            Assert.AreEqual("Player2", winners[0].Name);

            const string testcase5 = "Player1 8H 8C 9D 9H AC\r\nPlayer2 8D 8C 9H 9S KH";
            builder = new RoundBuilder(new StringReader(testcase5));
            winners = rule.Apply(builder.Build());
            Assert.IsNotNull(winners);
            Assert.AreEqual(1, winners.Count);
            Assert.AreEqual("Player1", winners[0].Name);

            const string testcase6 = "Player1 8H 8C 9D 9H AC\r\nPlayer2 8D 8C 9H 9S KH\r\nPlayer3 8D 8C 9H 9S AH";
            builder = new RoundBuilder(new StringReader(testcase6));
            winners = rule.Apply(builder.Build());
            Assert.IsNotNull(winners);
            Assert.AreEqual(2, winners.Count);
            Assert.AreEqual("Player1", winners[0].Name);
            Assert.AreEqual("Player3", winners[1].Name);
        }

        [TestMethod]
        public void OnePairBigTest()
        {
            var rule = new OnePairRule();

            var builder = new RoundBuilder(new StringReader(testcase_bigonepair));
            var round = builder.Build();
            var winners = rule.Apply(round);
            Assert.AreEqual(100000, winners.Count);
        }

        [TestMethod]
        public void ThreeOfAKindRuleTest()
        {
            var rule = new ThreeOfAKindRule();

            const string testcase0 = "Player1 8H ";
            var builder = new RoundBuilder(new StringReader(testcase0));
            var ex = Assert.ThrowsException<Exception>(() => builder.Build());
            Assert.AreEqual(ex.Message, "Invalid player format");

            const string testcase1 = "Player1 8H 7C 9D AH KC\r\nplayer2 7D 6C 5H QS 10H";
            builder = new RoundBuilder(new StringReader(testcase1));
            var winners = rule.Apply(builder.Build());
            Assert.IsNotNull(winners);
            Assert.AreEqual(0, winners.Count);

            const string testcase2 = "Player1 8H 7C 9D AH 7C\r\nplayer2 7D 6C 5H QS 6H";
            builder = new RoundBuilder(new StringReader(testcase2));
            winners = rule.Apply(builder.Build());
            Assert.IsNotNull(winners);
            Assert.AreEqual(0, winners.Count);

            const string testcase3 = "Player1 8H 6C 9D AH 6C\r\nplayer2 7D 6C 5H QS 6H";
            builder = new RoundBuilder(new StringReader(testcase3));
            winners = rule.Apply(builder.Build());
            Assert.IsNotNull(winners);
            Assert.AreEqual(0, winners.Count);

            const string testcase4 = "Player1 8H 6C 9D 6H 6C\r\nplayer2 7D 6C 5H QS 6H";
            builder = new RoundBuilder(new StringReader(testcase4));
            winners = rule.Apply(builder.Build());
            Assert.IsNotNull(winners);
            Assert.AreEqual(1, winners.Count);
            Assert.AreEqual("Player1", winners[0].Name);

            const string testcase5 = "Player1 8H 6C 8D 6H 6C\r\nplayer2 7D 6C 8H 8S 6H";
            builder = new RoundBuilder(new StringReader(testcase5));
            winners = rule.Apply(builder.Build());
            Assert.IsNotNull(winners);
            Assert.AreEqual(1, winners.Count);
            Assert.AreEqual("Player1", winners[0].Name);

            const string testcase6 = "Player1 8H 6C 8D 6H 6C\r\nPlayer2 6D 6C 8H 8S 6H";
            builder = new RoundBuilder(new StringReader(testcase6));
            winners = rule.Apply(builder.Build());
            Assert.IsNotNull(winners);
            Assert.AreEqual(2, winners.Count);
            Assert.AreEqual("Player1", winners[0].Name);
            Assert.AreEqual("Player2", winners[1].Name);

            const string testcase7 = "Player1 8H 6C 8D 6H 6C\r\nPlayer2 6D 6C 9H 8S 6H";
            builder = new RoundBuilder(new StringReader(testcase7));
            winners = rule.Apply(builder.Build());
            Assert.IsNotNull(winners);
            Assert.AreEqual(1, winners.Count);
            Assert.AreEqual("Player2", winners[0].Name);

            const string testcase8 = "Player1 8H 6C 8D 6H 6C\r\nPlayer2 7D 7C 8H 8S 7H";
            builder = new RoundBuilder(new StringReader(testcase8));
            winners = rule.Apply(builder.Build());
            Assert.IsNotNull(winners);
            Assert.AreEqual(1, winners.Count);
            Assert.AreEqual("Player2", winners[0].Name);

            const string testcase9 = "Player1 8H 6C 8D 6H 6C\r\nPlayer2 7D 7C 8H 8S 7H\r\nPlayer3 7D 7C 8H 8S 7H";
            builder = new RoundBuilder(new StringReader(testcase9));
            winners = rule.Apply(builder.Build());
            Assert.IsNotNull(winners);
            Assert.AreEqual(2, winners.Count);
            Assert.AreEqual("Player2", winners[0].Name);
            Assert.AreEqual("Player3", winners[1].Name);
        }

        [TestMethod]
        public void ThreeOfAKindRuleBigTest()
        {
            var rule = new ThreeOfAKindRule();

            var builder = new RoundBuilder(new StringReader(testcase_bigthree));
            var round = builder.Build();
            var winners = rule.Apply(round);
            Assert.AreEqual(100000, winners.Count);
        }

        [TestMethod]
        public void FlushRuleTest()
        {
            var rule = new FlushRule();

            const string testcase0 = "Player1 8H ";
            var builder = new RoundBuilder(new StringReader(testcase0));
            var ex = Assert.ThrowsException<Exception>(() => builder.Build());
            Assert.AreEqual(ex.Message, "Invalid player format");

            const string testcase1 = "Player1 8H 7C 9D AH KC\r\nplayer2 7D 6C 5H QS 10H";
            builder = new RoundBuilder(new StringReader(testcase1));
            var winners = rule.Apply(builder.Build());
            Assert.IsNotNull(winners);
            Assert.AreEqual(0, winners.Count);

            const string testcase2 = "Player1 8C 7C 9C AC KH\r\nplayer2 7C 6C 5C QC 10H";
            builder = new RoundBuilder(new StringReader(testcase2));
            winners = rule.Apply(builder.Build());
            Assert.IsNotNull(winners);
            Assert.AreEqual(0, winners.Count);

            const string testcase3 = "Player1 8C 7C 9C AC KC\r\nPlayer2 7C 6C 5C QC 10H";
            builder = new RoundBuilder(new StringReader(testcase3));
            winners = rule.Apply(builder.Build());
            Assert.IsNotNull(winners);
            Assert.AreEqual(1, winners.Count);
            Assert.AreEqual("Player1", winners[0].Name);

            const string testcase4 = "Player1 8C 7C 9C AC KC\r\nPlayer2 7C 6C 5C QC AC";
            builder = new RoundBuilder(new StringReader(testcase4));
            winners = rule.Apply(builder.Build());
            Assert.IsNotNull(winners);
            Assert.AreEqual(1, winners.Count);
            Assert.AreEqual("Player1", winners[0].Name);

            const string testcase5 = "Player1 8C 7C 9C AC KC\r\nPlayer2 7C 6C 5C KC AC";
            builder = new RoundBuilder(new StringReader(testcase5));
            winners = rule.Apply(builder.Build());
            Assert.IsNotNull(winners);
            Assert.AreEqual(1, winners.Count);
            Assert.AreEqual("Player1", winners[0].Name);

            const string testcase6 = "Player1 8C 7C 9C QC JC\r\nPlayer2 7C 6C 5C QC KC";
            builder = new RoundBuilder(new StringReader(testcase6));
            winners = rule.Apply(builder.Build());
            Assert.IsNotNull(winners);
            Assert.AreEqual(1, winners.Count);
            Assert.AreEqual("Player2", winners[0].Name);

            const string testcase7 = "Player1 8C 7C 9C QC JC\r\nPlayer2 9C 8C 7C QC JC";
            builder = new RoundBuilder(new StringReader(testcase7));
            winners = rule.Apply(builder.Build());
            Assert.IsNotNull(winners);
            Assert.AreEqual(2, winners.Count);
            Assert.AreEqual("Player1", winners[0].Name);
            Assert.AreEqual("Player2", winners[1].Name);

            const string testcase8 = "Player1 8C 7C 9C QC JC\r\nPlayer2 9C 8C 7C QC JC\r\nPlayer3 9C 8C 7C QC KC";
            builder = new RoundBuilder(new StringReader(testcase8));
            winners = rule.Apply(builder.Build());
            Assert.IsNotNull(winners);
            Assert.AreEqual(1, winners.Count);
            Assert.AreEqual("Player3", winners[0].Name);
        }

        [TestMethod]
        public void ParseBigRoundTest()
        {
            var rule = new FlushRule();

            var builder = new RoundBuilder(new StringReader(testcase_bigflush));
            var round = builder.Build();
            Assert.AreEqual(100000, round.Players.Count);
        }

        [TestMethod]
        public void FlushRuleBigTest()
        {
            var rule = new FlushRule();

            var builder = new RoundBuilder(new StringReader(testcase_bigflush));
            var round = builder.Build();
            var winners = rule.Apply(round);
            Assert.AreEqual(100000, winners.Count);
        }

        public RuleTest()
        {
            StringBuilder sb = new StringBuilder();
            const string testcase0 = "Player1 10C 8H 9C QC JC ";
            for (int i = 0; i < 100000; ++i)
            {
                sb.Append(testcase0);
            }
            testcase_bighighcard = sb.ToString();

            sb = new StringBuilder();
            const string testcase1 = "Player1 10C 10H 9C QC JC ";
            for (int i = 0; i < 100000; ++i)
            {
                sb.Append(testcase1);
            }
            testcase_bigonepair = sb.ToString();

            sb = new StringBuilder();
            const string testcase2 = "Player1 10C 10H 10C QC JC ";
            for (int i = 0; i < 100000; ++i)
            {
                sb.Append(testcase2);
            }
            testcase_bigthree = sb.ToString();

            sb = new StringBuilder();
            const string testcase3 = "Player1 10C 10C 10C QC JC ";
            for (int i = 0; i < 100000; ++i)
            {
                sb.Append(testcase3);
            }
            testcase_bigflush = sb.ToString();
        }

        private readonly string testcase_bighighcard;
        private readonly string testcase_bigonepair;
        private readonly string testcase_bigthree;
        private readonly string testcase_bigflush;
    }
}
