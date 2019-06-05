using Microsoft.VisualStudio.TestTools.UnitTesting;

using PokerHandShowdown;
using System;
using System.IO;
using System.Text;

namespace PokerHandShowdownTest
{
    [TestClass]
    public class RoundTest
    {
        [TestMethod]
        public void TestRoundBuilder()
        {
            const string testcase0 = "";
            var builder = new RoundBuilder(new StringReader(testcase0));
            var round = builder.Build();
            Assert.IsNotNull(round);
            Assert.AreEqual(0, round.Players.Count);

            const string testcase1 = "Player1 8Q";
            builder = new RoundBuilder(new StringReader(testcase1));
            var ex = Assert.ThrowsException<Exception>(() => builder.Build());
            Assert.AreEqual(ex.Message, "Invalid card format");

            const string testcase2 = "Player1 8H 7C 9D AH KC";
            builder = new RoundBuilder(new StringReader(testcase2));
            round = builder.Build();
            Assert.IsNotNull(round);
            Assert.AreEqual(1, round.Players.Count);

            const string testcase3 = "Player1 8H 7C 9D AH KC \r\n ";
            builder = new RoundBuilder(new StringReader(testcase3));
            round = builder.Build();
            Assert.IsNotNull(round);
            Assert.AreEqual(1, round.Players.Count);

            round = builder.Build();
            Assert.IsNotNull(round);
            Assert.AreEqual(0, round.Players.Count);

            const string testcase4 = "Player1 8H 7C 9D AH KC \r\n player2 7D 6C 5H QS 10H \r\n\r\n Player1 9C 10H 8C 7C 5C ";
            builder = new RoundBuilder(new StringReader(testcase4));
            round = builder.Build();
            Assert.IsNotNull(round);
            Assert.AreEqual(2, round.Players.Count);

            round = builder.Build();
            Assert.IsNotNull(round);
            Assert.AreEqual(1, round.Players.Count);

            round = builder.Build();
            Assert.IsNotNull(round);
            Assert.AreEqual(0, round.Players.Count);

            const string testcase5 = "Player1 8H 7C 9D AH KC \r\n player2 7D 6C 5H QS 10H \r\n Player3 9C 10H 8C 7C";
            builder = new RoundBuilder(new StringReader(testcase5));
            ex = Assert.ThrowsException<Exception>(() => builder.Build());
            Assert.AreEqual(ex.Message, "Invalid player format");
        }

        [TestMethod]
        public void TestRound()
        {
            const string testcase0 = "";
            var builder = new RoundBuilder(new StringReader(testcase0));
            var round = builder.Build();
            Assert.IsNotNull(round);
            Assert.AreEqual(0, round.Players.Count);
            var winners = round.Play();
            Assert.AreEqual(0, winners.Count);

            const string testcase1 = "Player1 8C 7C 9C QC JC\r\n";
            builder = new RoundBuilder(new StringReader(testcase1));
            round = builder.Build();
            Assert.IsNotNull(round);
            Assert.AreEqual(1, round.Players.Count);
            winners = round.Play();
            Assert.AreEqual(1, winners.Count);
            Assert.AreEqual("Player1", winners[0].Name);

            const string testcase2 = "Player1 8C 7H 9C QC JC\r\n";
            builder = new RoundBuilder(new StringReader(testcase2));
            round = builder.Build();
            Assert.IsNotNull(round);
            Assert.AreEqual(1, round.Players.Count);
            winners = round.Play();
            Assert.AreEqual(1, winners.Count);
            Assert.AreEqual("Player1", winners[0].Name);

            const string testcase3 = "Player1 8C 7H 9C QC JC\r\nPlayer2 8C 7H 9S QC JD\r\n";
            builder = new RoundBuilder(new StringReader(testcase3));
            round = builder.Build();
            Assert.IsNotNull(round);
            Assert.AreEqual(2, round.Players.Count);
            winners = round.Play();
            Assert.AreEqual(2, winners.Count);
            Assert.AreEqual("Player1", winners[0].Name);
            Assert.AreEqual("Player2", winners[1].Name);

            const string testcase4 = "Player1 8C 9H 9C 9C 9C\r\nPlayer2 8C 7H 9S 9C 9D\r\n";
            builder = new RoundBuilder(new StringReader(testcase4));
            round = builder.Build();
            Assert.IsNotNull(round);
            Assert.AreEqual(2, round.Players.Count);
            winners = round.Play();
            Assert.AreEqual(1, winners.Count);
            Assert.AreEqual("Player1", winners[0].Name);

            const string testcase5 = "Player1 8C 9H 9C 9C 9C\r\nPlayer2 8C 10H 9S 9C 9D\r\n";
            builder = new RoundBuilder(new StringReader(testcase5));
            round = builder.Build();
            Assert.IsNotNull(round);
            Assert.AreEqual(2, round.Players.Count);
            winners = round.Play();
            Assert.AreEqual(1, winners.Count);
            Assert.AreEqual("Player2", winners[0].Name);

            const string testcase6 = "Player1 8C 7H 9C 10C JC\r\nPlayer2 8C 7H 9S QC JD\r\n";
            builder = new RoundBuilder(new StringReader(testcase6));
            round = builder.Build();
            Assert.IsNotNull(round);
            Assert.AreEqual(2, round.Players.Count);
            winners = round.Play();
            Assert.AreEqual(1, winners.Count);
            Assert.AreEqual("Player2", winners[0].Name);

            const string testcase7 = "Player1 8C 7H 9C 10C JC\r\nPlayer2 8C 7H 9S QC JD\r\nPlayer3 8C 7H 9S QH JD\r\n";
            builder = new RoundBuilder(new StringReader(testcase7));
            round = builder.Build();
            Assert.IsNotNull(round);
            Assert.AreEqual(3, round.Players.Count);
            winners = round.Play();
            Assert.AreEqual(2, winners.Count);
            Assert.AreEqual("Player2", winners[0].Name);
            Assert.AreEqual("Player3", winners[1].Name);

            const string testcase8 = "Player1 7C 7H 9C 10C JC\r\nPlayer2 8C 7H 9S QC JD\r\nPlayer3 8C 7H 9S QH JD\r\n";
            builder = new RoundBuilder(new StringReader(testcase8));
            round = builder.Build();
            Assert.IsNotNull(round);
            Assert.AreEqual(3, round.Players.Count);
            winners = round.Play();
            Assert.AreEqual(1, winners.Count);
            Assert.AreEqual("Player1", winners[0].Name);

            const string testcase9 = "Player1 7C 7H 9C 10C JC\r\nPlayer2 8C 9H 9S QC JD\r\nPlayer3 8C 7H 9S QH JD\r\n";
            builder = new RoundBuilder(new StringReader(testcase9));
            round = builder.Build();
            Assert.IsNotNull(round);
            Assert.AreEqual(3, round.Players.Count);
            winners = round.Play();
            Assert.AreEqual(1, winners.Count);
            Assert.AreEqual("Player2", winners[0].Name);

            const string testcase10 = "Player1 7C 7H 9C 10C JC\r\nPlayer2 8C 9H 9S QC JD\r\nPlayer3 6C 6H 6S QH JD\r\n";
            builder = new RoundBuilder(new StringReader(testcase10));
            round = builder.Build();
            Assert.IsNotNull(round);
            Assert.AreEqual(3, round.Players.Count);
            winners = round.Play();
            Assert.AreEqual(1, winners.Count);
            Assert.AreEqual("Player3", winners[0].Name);

            const string testcase11 = "Player1 7C 7H 7C 10C JC\r\nPlayer2 8C 9H 9S QC JD\r\nPlayer3 6C 6H 6S QH JD\r\n";
            builder = new RoundBuilder(new StringReader(testcase11));
            round = builder.Build();
            Assert.IsNotNull(round);
            Assert.AreEqual(3, round.Players.Count);
            winners = round.Play();
            Assert.AreEqual(1, winners.Count);
            Assert.AreEqual("Player1", winners[0].Name);

            const string testcase12 = "Player1 7C 7H 7C 10C JC\r\nPlayer2 8C 9H 9S QC JD\r\nPlayer3 7C 7H 7S QH JD\r\n";
            builder = new RoundBuilder(new StringReader(testcase12));
            round = builder.Build();
            Assert.IsNotNull(round);
            Assert.AreEqual(3, round.Players.Count);
            winners = round.Play();
            Assert.AreEqual(1, winners.Count);
            Assert.AreEqual("Player3", winners[0].Name);

            const string testcase13 = "Player1 7C 7H 7C 10C JC\r\nPlayer2 8C 9H 9S QC JD\r\nPlayer3 7C 7H 7S 7H 8D\r\n";
            builder = new RoundBuilder(new StringReader(testcase13));
            round = builder.Build();
            Assert.IsNotNull(round);
            Assert.AreEqual(3, round.Players.Count);
            winners = round.Play();
            Assert.AreEqual(1, winners.Count);
            Assert.AreEqual("Player1", winners[0].Name);

            const string testcase14 = "Player1 7C 7H 7C 10C JC\r\nPlayer2 8C 9C 9C QC JC\r\nPlayer3 7C 7H 7S 7H 8D\r\n";
            builder = new RoundBuilder(new StringReader(testcase14));
            round = builder.Build();
            Assert.IsNotNull(round);
            Assert.AreEqual(3, round.Players.Count);
            winners = round.Play();
            Assert.AreEqual(1, winners.Count);
            Assert.AreEqual("Player2", winners[0].Name);

            const string testcase15 = "Player1 6C 7C 7C 10C JC\r\nPlayer2 8C 9C 9C QC JC\r\nPlayer3 7C 7H 7S 7H 8D\r\n";
            builder = new RoundBuilder(new StringReader(testcase15));
            round = builder.Build();
            Assert.IsNotNull(round);
            Assert.AreEqual(3, round.Players.Count);
            winners = round.Play();
            Assert.AreEqual(1, winners.Count);
            Assert.AreEqual("Player2", winners[0].Name);

            const string testcase16 = "Player1 6C 7C 7C 10C JC\r\nPlayer2 8C 9C 9C QC JC\r\nPlayer3 7H 7H 7H 7H 8H\r\n";
            builder = new RoundBuilder(new StringReader(testcase16));
            round = builder.Build();
            Assert.IsNotNull(round);
            Assert.AreEqual(3, round.Players.Count);
            winners = round.Play();
            Assert.AreEqual(1, winners.Count);
            Assert.AreEqual("Player2", winners[0].Name);

            const string testcase17 = "Player1 6C 7C 7C 10C JC Player2 8C 9C 9C QC JC Player3 7H 7H 7H 7H 8H Player4 7H 7H 7H 7H KH Player5 7H 7H 7H 7H AC";
            builder = new RoundBuilder(new StringReader(testcase17));
            round = builder.Build();
            Assert.IsNotNull(round);
            Assert.AreEqual(5, round.Players.Count);
            winners = round.Play();
            Assert.AreEqual(1, winners.Count);
            Assert.AreEqual("Player4", winners[0].Name);
        }

        [TestMethod]
        public void TestBigRound()
        {
            var builder = new RoundBuilder(new StringReader(biground));
            var round = builder.Build();
            Assert.IsNotNull(round);
            Assert.AreEqual(100000, round.Players.Count);
            var winners = round.Play();
            Assert.AreEqual(100000, winners.Count);
        }

        public RoundTest()
        {
            StringBuilder sb = new StringBuilder();
            string testcase0 = "Player1 7C 8H 9C 10C JC ";
            for (int i = 0; i < 100000; ++i)
            {
                sb.Append(testcase0);
            }
            biground = sb.ToString();
        }

        private readonly string biground;
    }
}
