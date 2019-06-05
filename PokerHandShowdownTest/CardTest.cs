using Microsoft.VisualStudio.TestTools.UnitTesting;

using PokerHandShowdown;
using System;
using System.IO;

namespace PokerHandShowdownTest
{
    [TestClass]
    public class CardTest
    {
        [TestMethod]
        public void TestCardBuilder()
        {
            const string testcase0 = "";
            CardBuilder builder = new CardBuilder(new Tokenizer(new StringReader(testcase0)));
            Assert.IsNull(builder.Build());

            const string testcase1 = "2S";
            builder = new CardBuilder(new Tokenizer(new StringReader(testcase1)));
            var card = builder.Build();
            Assert.IsNotNull(card);
            Assert.AreEqual('S', card.Suit);
            Assert.AreEqual("2", card.Rank);
            Assert.AreEqual(1, card.SuitIndex);
            Assert.AreEqual(0, card.RankIndex);

            const string testcase2 = "10H";
            builder = new CardBuilder(new Tokenizer(new StringReader(testcase2)));
            card = builder.Build();
            Assert.IsNotNull(card);
            Assert.AreEqual('H', card.Suit);
            Assert.AreEqual("10", card.Rank);
            Assert.AreEqual(3, card.SuitIndex);
            Assert.AreEqual(8, card.RankIndex);

            const string testcase3 = "AD";
            builder = new CardBuilder(new Tokenizer(new StringReader(testcase3)));
            card = builder.Build();
            Assert.IsNotNull(card);
            Assert.AreEqual('D', card.Suit);
            Assert.AreEqual("A", card.Rank);
            Assert.AreEqual(2, card.SuitIndex);
            Assert.AreEqual(12, card.RankIndex);

            const string testcase4 = "JC";
            builder = new CardBuilder(new Tokenizer(new StringReader(testcase4)));
            card = builder.Build();
            Assert.IsNotNull(card);
            Assert.AreEqual('C', card.Suit);
            Assert.AreEqual("J", card.Rank);
            Assert.AreEqual(0, card.SuitIndex);
            Assert.AreEqual(9, card.RankIndex);

            const string testcase5 = "2";
            builder = new CardBuilder(new Tokenizer(new StringReader(testcase5)));
            var ex = Assert.ThrowsException<Exception>(() => builder.Build());
            Assert.AreEqual(ex.Message, "Invalid card format");

            const string testcase6 = "2A";
            builder = new CardBuilder(new Tokenizer(new StringReader(testcase6)));
            ex = Assert.ThrowsException<Exception>(() => builder.Build());
            Assert.AreEqual(ex.Message, "Invalid card format");

            const string testcase7 = "1C";
            builder = new CardBuilder(new Tokenizer(new StringReader(testcase7)));
            ex = Assert.ThrowsException<Exception>(() => builder.Build());
            Assert.AreEqual(ex.Message, "Invalid card format");

            const string testcase8 = "PC";
            builder = new CardBuilder(new Tokenizer(new StringReader(testcase8)));
            ex = Assert.ThrowsException<Exception>(() => builder.Build());
            Assert.AreEqual(ex.Message, "Invalid card format");

            const string testcase9 = "A";
            builder = new CardBuilder(new Tokenizer(new StringReader(testcase9)));
            ex = Assert.ThrowsException<Exception>(() => builder.Build());
            Assert.AreEqual(ex.Message, "Invalid card format");

            const string testcase10 = "as";
            builder = new CardBuilder(new Tokenizer(new StringReader(testcase10)));
            card = builder.Build();
            Assert.IsNotNull(card);
            Assert.AreEqual('S', card.Suit);
            Assert.AreEqual("A", card.Rank);
            Assert.AreEqual(1, card.SuitIndex);
            Assert.AreEqual(12, card.RankIndex);
        }
    }
}
