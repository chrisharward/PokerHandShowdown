using Microsoft.VisualStudio.TestTools.UnitTesting;

using PokerHandShowdown;
using System.IO;

namespace PokerHandShowdownTest
{
    [TestClass]
    public class TokenizerTest
    {
        [TestMethod]
        public void TestTokenizerSingleLine()
        {
            const string testcase0 = "";
            Tokenizer t = new Tokenizer(new StringReader(testcase0));
            Assert.AreEqual(null, t.NextToken());

            const string testcase1 = "p1";
            t = new Tokenizer(new StringReader(testcase1));
            Assert.AreEqual("p1", t.NextToken());
            Assert.AreEqual(null, t.NextToken());

            const string testcase2 = "p1 p2";
            t = new Tokenizer(new StringReader(testcase2));
            Assert.AreEqual("p1", t.NextToken());
            Assert.AreEqual("p2", t.NextToken());
            Assert.AreEqual(null, t.NextToken());
        }

        [TestMethod]
        public void TestTokenizerMultiLine()
        {
            const string testcase0 = "p1\n2C 3C \n 4C 5C \t 6C ";
            var t = new Tokenizer(new StringReader(testcase0));
            Assert.AreEqual("p1", t.NextToken());
            Assert.AreEqual("2C", t.NextToken());
            Assert.AreEqual("3C", t.NextToken());
            Assert.AreEqual("4C", t.NextToken());
            Assert.AreEqual("5C", t.NextToken());
            Assert.AreEqual("6C", t.NextToken());
            Assert.AreEqual(null, t.NextToken());

            const string testcase1 = "\r\n\r\n";
            t = new Tokenizer(new StringReader(testcase1));
            Assert.AreEqual(null, t.NextToken());
        }
    }
}
