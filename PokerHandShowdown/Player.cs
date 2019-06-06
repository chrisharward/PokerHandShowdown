using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerHandShowdown
{
    /// <summary>
    /// PlayerBuilder is responsible for building a Player object.
    /// </summary>
    public class PlayerBuilder
    {
        public PlayerBuilder(Tokenizer tokenizer)
        {
            tokenizer_ = tokenizer;
            card_builder_ = new CardBuilder(tokenizer_);
        }

        public Player Build()
        {
            var name = tokenizer_.NextToken();
            if (name == null)
            {
                return null;
            }

            var player = new Player(name);
            for (int i = 0; i < 5; i++)
            {
                var card = card_builder_.Build();
                if (card == null)
                {
                    throw new Exception("Invalid player format");
                }
                player.AddCard(card);
            }

            return player;
        }

        private CardBuilder card_builder_;
        private Tokenizer tokenizer_;
    }

    /// <summary>
    /// Represents a player in the poker game, including a name and 5 cards.
    /// </summary>
    public class Player
    {
        public Player(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Add a card to a players hand.
        /// </summary>
        /// <param name="card">the card to add</param>
        internal void AddCard(Card card)
        {
            suit_indices_.Add(card.SuitIndex);
            rank_frequency_[card.RankIndex]++;
            Hand.Add(card);
        }

        /// <summary>
        /// The name of this player
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// A list containing this players cards
        /// </summary>
        public List<Card> Hand { get; } = new List<Card>();

        /// <summary>
        /// Indicates that this players hand is a flush
        /// </summary>
        public bool Flush
        {
            get
            {
                return suit_indices_.Count == 1;
            }
        }

        /// <summary>
        /// Returns a list of the number of cards the user has of each rank.
        /// </summary>
        public List<int> RankFrequency
        {
            get
            {
                return rank_frequency_;
            }
        }

        /// <summary>
        /// returns a list of tuples which contain the frequency of a particular rank and its index in the rank_ list.
        /// This is sorted to have the ranks with highest frequency at the top of the list. This is to identify pairs and
        /// triples quickly.
        /// </summary>
        public List<Tuple<int, int>> SortedRankFrequency
        {
            get
            {
                if (sorted_rank_frequency_ == null)
                {
                    sorted_rank_frequency_ = new List<Tuple<int, int>>();
                    int rank = 0;
                    foreach (var freq in RankFrequency)
                    {
                        if (freq > 0)
                        {
                            sorted_rank_frequency_.Add(new Tuple<int, int>(freq, rank));
                        }
                        ++rank;
                    }

                    sorted_rank_frequency_.Sort((x, y) =>
                    {
                        int cmp = y.Item1.CompareTo(x.Item1);
                        if (cmp == 0)
                        {
                            return y.Item2.CompareTo(x.Item2);
                        }
                        return cmp;
                    });
                }
                return sorted_rank_frequency_;
            }
        }

        private List<int> rank_frequency_ = new List<int>(new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
        private List<Tuple<int, int>> sorted_rank_frequency_;
        private HashSet<int> suit_indices_ = new HashSet<int>();
    }
}
