using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerHandShowdown
{
    public class CardBuilder
    {
        public CardBuilder(Tokenizer tokenizer)
        {
            tokenizer_ = tokenizer;
        }

        public Card Build()
        {
            return Parse(tokenizer_.NextToken());
        }

        private Card Parse(string token)
        {
            if (token == null)
            {
                return null;
            }

            token = token.ToUpper();
            if (token.Length < 2)
            {
                throw new Exception(invalid_card_format_);
            }

            var suit = token[token.Length - 1];
            if (!Card.IsValidSuit(suit))
            {
                throw new Exception(invalid_card_format_);
            }

            var rank = token.Substring(0, token.Length - 1);
            if (!Card.IsValidRank(rank))
            {
                throw new Exception(invalid_card_format_);
            }

            return new Card(rank, suit);
        }

        private Tokenizer tokenizer_;
        private const string invalid_card_format_ = "Invalid card format";
    }

    public class Card
    {
        public Card(string rank, char suit)
        {
            RankIndex = Card.ranks_.IndexOf(rank);
            SuitIndex = Card.suits_.IndexOf(suit);
        }
 
        public static bool IsValidRank(string s)
        {
            return ranks_.Contains(s);
        }

        public static bool IsValidSuit(char c)
        {
            return suits_.Contains(c);
        }

        public int Compare(Card other)
        {
            if (RankIndex < other.RankIndex)
            {
                return -1;
            }
            else if (RankIndex > other.RankIndex)
            {
                return 1;
            }
            return 0; 
        }

        public string Rank
        {
            get
            {
                return Card.ranks_[RankIndex];
            }
        }

        public int RankIndex { get; }

        public char Suit
        {
            get
            {
                return Card.suits_[SuitIndex];
            }
        }

        public int SuitIndex { get; }

        private static List<char> suits_ = new List<char>(new char[]{ 'C', 'S', 'D', 'H' });
        private static List<string> ranks_ = new List<string>(
            new string[] { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" });
    }
}
