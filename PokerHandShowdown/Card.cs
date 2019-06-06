using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerHandShowdown
{
    /// <summary>
    /// CardBuilder is responsible for building a Card object using the 
    /// data available in the tokenizer.
    /// </summary>
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

    /// <summary>
    /// Represents a poker card.
    /// </summary>
    public class Card
    {
        public Card(string rank, char suit)
        {
            RankIndex = Card.ranks_.IndexOf(rank);
            SuitIndex = Card.suits_.IndexOf(suit);
        }

        /// <summary>
        /// Is the given card rank valid (between 2 and 10 inclusive, plus J, Q, K, A).
        /// </summary>
        /// <param name="rank">the rank to validate</param>
        /// <returns>true if valid</returns>
        public static bool IsValidRank(string rank)
        {
            return ranks_.Contains(rank);
        }

        /// <summary>
        /// Is the given suit character valid, valid values are C, S, D, H standing for
        /// clubs, spades, diamonds, hearts.
        /// </summary>
        /// <param name="suit">the suit to validate</param>
        /// <returns>true if valid</returns>
        public static bool IsValidSuit(char suit)
        {
            return suits_.Contains(suit);
        }

        /// <summary>
        /// The rank of this card.
        /// </summary>
        public string Rank
        {
            get
            {
                return Card.ranks_[RankIndex];
            }
        }

        /// <summary>
        /// The index of this cards rank in the ranks_ list.
        /// </summary>
        public int RankIndex { get; }

        /// <summary>
        /// The suit of this card
        /// </summary>
        public char Suit
        {
            get
            {
                return Card.suits_[SuitIndex];
            }
        }

        /// <summary>
        /// The index of this cards rank in the suits_ list.
        /// </summary>
        public int SuitIndex { get; }

        private static List<char> suits_ = new List<char>(new char[] { 'C', 'S', 'D', 'H' });
        private static List<string> ranks_ = new List<string>(
            new string[] { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" });
    }
}
