using Microsoft.VisualStudio.TestTools.UnitTesting;

using PokerHandShowdown;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PokerHandShowdownTest
{
    [TestClass]
    public class PlayerTest
    {
        [TestMethod]
        public void TestPlayerBuilder()
        {
            const string testcase0 = "";
            var builder = new PlayerBuilder(new Tokenizer(new StringReader(testcase0)));
            Assert.IsNull(builder.Build());

            const string testcase1 = "player1 AH";
            builder = new PlayerBuilder(new Tokenizer(new StringReader(testcase1)));
            var ex = Assert.ThrowsException<Exception>(() => builder.Build());
            Assert.AreEqual(ex.Message, "Invalid player format");

            const string testcase2 = "player1 AH KH QH JH";
            builder = new PlayerBuilder(new Tokenizer(new StringReader(testcase2)));
            ex = Assert.ThrowsException<Exception>(() => builder.Build());
            Assert.AreEqual(ex.Message, "Invalid player format");

            const string testcase3 = "player1 AH KH QH JH 10a";
            builder = new PlayerBuilder(new Tokenizer(new StringReader(testcase3)));
            ex = Assert.ThrowsException<Exception>(() => builder.Build());
            Assert.AreEqual(ex.Message, "Invalid card format");

            const string testcase4 = "player1 AH KH QH JH 10H";
            builder = new PlayerBuilder(new Tokenizer(new StringReader(testcase4)));
            var player = builder.Build();
            Assert.IsNotNull(player);
            Assert.AreEqual("player1", player.Name);
            Assert.AreEqual("A", player.Hand[0].Rank);
            Assert.AreEqual('H', player.Hand[0].Suit);
            Assert.AreEqual("K", player.Hand[1].Rank);
            Assert.AreEqual('H', player.Hand[1].Suit);
            Assert.AreEqual("Q", player.Hand[2].Rank);
            Assert.AreEqual('H', player.Hand[2].Suit);
            Assert.AreEqual("J", player.Hand[3].Rank);
            Assert.AreEqual('H', player.Hand[3].Suit);
            Assert.AreEqual("10", player.Hand[4].Rank);
            Assert.AreEqual('H', player.Hand[4].Suit);
            var rankFrequencyExpected = new List<int>(new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1 });
            Assert.AreEqual(rankFrequencyExpected.Count, player.RankFrequency.Count);
            Assert.IsTrue(Enumerable.SequenceEqual(rankFrequencyExpected, player.RankFrequency));

            Assert.IsNull(builder.Build());
            Assert.IsNull(builder.Build());

            const string testcase5 = "player1 AH KH QH JH 10H player2 10C 9C 8C 7c 6c";
            builder = new PlayerBuilder(new Tokenizer(new StringReader(testcase5)));
            player = builder.Build();
            Assert.IsNotNull(player);
            Assert.AreEqual("player1", player.Name);
            Assert.AreEqual("A", player.Hand[0].Rank);
            Assert.AreEqual('H', player.Hand[0].Suit);
            Assert.AreEqual("K", player.Hand[1].Rank);
            Assert.AreEqual('H', player.Hand[1].Suit);
            Assert.AreEqual("Q", player.Hand[2].Rank);
            Assert.AreEqual('H', player.Hand[2].Suit);
            Assert.AreEqual("J", player.Hand[3].Rank);
            Assert.AreEqual('H', player.Hand[3].Suit);
            Assert.AreEqual("10", player.Hand[4].Rank);
            Assert.AreEqual('H', player.Hand[4].Suit);

            player = builder.Build();
            Assert.IsNotNull(player);
            Assert.AreEqual("player2", player.Name);
            Assert.AreEqual("10", player.Hand[0].Rank);
            Assert.AreEqual('C', player.Hand[0].Suit);
            Assert.AreEqual("9", player.Hand[1].Rank);
            Assert.AreEqual('C', player.Hand[1].Suit);
            Assert.AreEqual("8", player.Hand[2].Rank);
            Assert.AreEqual('C', player.Hand[2].Suit);
            Assert.AreEqual("7", player.Hand[3].Rank);
            Assert.AreEqual('C', player.Hand[3].Suit);
            Assert.AreEqual("6", player.Hand[4].Rank);
            Assert.AreEqual('C', player.Hand[4].Suit);

            Assert.IsNull(builder.Build());
            Assert.IsNull(builder.Build());


            const string testcase6 = "playerA AH \nKH QH \n\tJH 10H playerB 10C \r\n9C 8C 7c 6c";
            builder = new PlayerBuilder(new Tokenizer(new StringReader(testcase6)));
            player = builder.Build();
            Assert.IsNotNull(player);
            Assert.AreEqual("playerA", player.Name);
            Assert.AreEqual("A", player.Hand[0].Rank);
            Assert.AreEqual('H', player.Hand[0].Suit);
            Assert.AreEqual("K", player.Hand[1].Rank);
            Assert.AreEqual('H', player.Hand[1].Suit);
            Assert.AreEqual("Q", player.Hand[2].Rank);
            Assert.AreEqual('H', player.Hand[2].Suit);
            Assert.AreEqual("J", player.Hand[3].Rank);
            Assert.AreEqual('H', player.Hand[3].Suit);
            Assert.AreEqual("10", player.Hand[4].Rank);
            Assert.AreEqual('H', player.Hand[4].Suit);

            player = builder.Build();
            Assert.IsNotNull(player);
            Assert.AreEqual("playerB", player.Name);
            Assert.AreEqual("10", player.Hand[0].Rank);
            Assert.AreEqual('C', player.Hand[0].Suit);
            Assert.AreEqual("9", player.Hand[1].Rank);
            Assert.AreEqual('C', player.Hand[1].Suit);
            Assert.AreEqual("8", player.Hand[2].Rank);
            Assert.AreEqual('C', player.Hand[2].Suit);
            Assert.AreEqual("7", player.Hand[3].Rank);
            Assert.AreEqual('C', player.Hand[3].Suit);
            Assert.AreEqual("6", player.Hand[4].Rank);
            Assert.AreEqual('C', player.Hand[4].Suit);

            Assert.IsNull(builder.Build());
            Assert.IsNull(builder.Build());

            const string testcase7 = "playerA\nAH\nKH\nQH\nJH\n10H\nplayerB\n10C\n9C\n8C\n7c\n6c\n\n\n\n";
            builder = new PlayerBuilder(new Tokenizer(new StringReader(testcase7)));
            player = builder.Build();
            Assert.IsNotNull(player);
            Assert.AreEqual("playerA", player.Name);
            Assert.AreEqual("A", player.Hand[0].Rank);
            Assert.AreEqual('H', player.Hand[0].Suit);
            Assert.AreEqual("K", player.Hand[1].Rank);
            Assert.AreEqual('H', player.Hand[1].Suit);
            Assert.AreEqual("Q", player.Hand[2].Rank);
            Assert.AreEqual('H', player.Hand[2].Suit);
            Assert.AreEqual("J", player.Hand[3].Rank);
            Assert.AreEqual('H', player.Hand[3].Suit);
            Assert.AreEqual("10", player.Hand[4].Rank);
            Assert.AreEqual('H', player.Hand[4].Suit);

            player = builder.Build();
            Assert.IsNotNull(player);
            Assert.AreEqual("playerB", player.Name);
            Assert.AreEqual("10", player.Hand[0].Rank);
            Assert.AreEqual('C', player.Hand[0].Suit);
            Assert.AreEqual("9", player.Hand[1].Rank);
            Assert.AreEqual('C', player.Hand[1].Suit);
            Assert.AreEqual("8", player.Hand[2].Rank);
            Assert.AreEqual('C', player.Hand[2].Suit);
            Assert.AreEqual("7", player.Hand[3].Rank);
            Assert.AreEqual('C', player.Hand[3].Suit);
            Assert.AreEqual("6", player.Hand[4].Rank);
            Assert.AreEqual('C', player.Hand[4].Suit);

            Assert.IsNull(builder.Build());
            Assert.IsNull(builder.Build());

            const string testcase8 = "player1 10D KH JH QH AS";
            builder = new PlayerBuilder(new Tokenizer(new StringReader(testcase8)));
            player = builder.Build();
            Assert.IsNotNull(player);
            Assert.AreEqual("player1", player.Name);
            Assert.AreEqual("A", player.Hand[4].Rank);
            Assert.AreEqual('S', player.Hand[4].Suit);
            Assert.AreEqual("K", player.Hand[1].Rank);
            Assert.AreEqual('H', player.Hand[1].Suit);
            Assert.AreEqual("Q", player.Hand[3].Rank);
            Assert.AreEqual('H', player.Hand[3].Suit);
            Assert.AreEqual("J", player.Hand[2].Rank);
            Assert.AreEqual('H', player.Hand[2].Suit);
            Assert.AreEqual("10", player.Hand[0].Rank);
            Assert.AreEqual('D', player.Hand[0].Suit);

            const string testcase9 = "player1 10D 10H 10S QH QS";
            builder = new PlayerBuilder(new Tokenizer(new StringReader(testcase9)));
            player = builder.Build();
            Assert.IsNotNull(player);
            Assert.AreEqual("player1", player.Name);

            rankFrequencyExpected = new List<int>(new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 2, 0, 0 });
            Assert.AreEqual(rankFrequencyExpected.Count, player.RankFrequency.Count);
            Assert.IsTrue(Enumerable.SequenceEqual(rankFrequencyExpected, player.RankFrequency));
        }

        [TestMethod]
        public void TestRankFrequency()
        {
            const string testcase0 = "player1 10D 10H 10S QH QS";
            var builder = new PlayerBuilder(new Tokenizer(new StringReader(testcase0)));
            var player = builder.Build();
            Assert.IsNotNull(player);
            Assert.AreEqual("player1", player.Name);
            Assert.IsFalse(player.Flush);

            var rankFrequencyExpected = new List<int>(new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 2, 0, 0 });
            var sortedRankFrequencyExpected = new List<Tuple<int, int>>(new Tuple<int, int>[] { new Tuple<int, int>(3, 8), new Tuple<int, int>( 2, 10 ) });
            Assert.IsTrue(Enumerable.SequenceEqual(rankFrequencyExpected, player.RankFrequency));
            Assert.IsTrue(Enumerable.SequenceEqual(sortedRankFrequencyExpected, player.SortedRankFrequency));

            const string testcase1 = "player1 10D 9H 10S QH QS";
            builder = new PlayerBuilder(new Tokenizer(new StringReader(testcase1)));
            player = builder.Build();
            Assert.IsNotNull(player);
            Assert.AreEqual("player1", player.Name);
            Assert.IsFalse(player.Flush);

            rankFrequencyExpected = new List<int>(new int[] { 0, 0, 0, 0, 0, 0, 0, 1, 2, 0, 2, 0, 0 });
            sortedRankFrequencyExpected = new List<Tuple<int, int>>(new Tuple<int, int>[] { new Tuple<int, int>(2, 10), new Tuple<int, int>(2, 8), new Tuple<int, int>(1, 7), });
            Assert.IsTrue(Enumerable.SequenceEqual(rankFrequencyExpected, player.RankFrequency));
            Assert.IsTrue(Enumerable.SequenceEqual(sortedRankFrequencyExpected, player.SortedRankFrequency));

            const string testcase2 = "player1 10D 9H 9S QH QS";
            builder = new PlayerBuilder(new Tokenizer(new StringReader(testcase2)));
            player = builder.Build();
            Assert.IsNotNull(player);
            Assert.AreEqual("player1", player.Name);
            Assert.IsFalse(player.Flush);

            rankFrequencyExpected = new List<int>(new int[] { 0, 0, 0, 0, 0, 0, 0, 2, 1, 0, 2, 0, 0 });
            sortedRankFrequencyExpected = new List<Tuple<int, int>>(new Tuple<int, int>[] { new Tuple<int, int>(2, 10), new Tuple<int, int>(2, 7), new Tuple<int, int>(1, 8) });
            Assert.IsTrue(Enumerable.SequenceEqual(rankFrequencyExpected, player.RankFrequency));
            Assert.IsTrue(Enumerable.SequenceEqual(sortedRankFrequencyExpected, player.SortedRankFrequency));

            const string testcase3 = "player1 2D 9D QD QD QD";
            builder = new PlayerBuilder(new Tokenizer(new StringReader(testcase3)));
            player = builder.Build();
            Assert.IsNotNull(player);
            Assert.AreEqual("player1", player.Name);
            Assert.IsTrue(player.Flush);

            rankFrequencyExpected = new List<int>(new int[] { 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 3, 0, 0 });
            sortedRankFrequencyExpected = new List<Tuple<int, int>>(new Tuple<int, int>[] { new Tuple<int, int>(3, 10), new Tuple<int, int>(1, 7), new Tuple<int, int>(1, 0) });
            Assert.IsTrue(Enumerable.SequenceEqual(rankFrequencyExpected, player.RankFrequency));
            Assert.IsTrue(Enumerable.SequenceEqual(sortedRankFrequencyExpected, player.SortedRankFrequency));
        }
    }
}
