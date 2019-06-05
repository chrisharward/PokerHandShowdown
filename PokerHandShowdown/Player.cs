using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerHandShowdown
{
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

    public class Player
    {
        public Player(string name)
        {
            Name = name;
        }

        public void AddCard(Card card)
        {
            suit_indices_.Add(card.SuitIndex);
            rank_frequency_[card.RankIndex]++;
            Hand.Add(card);
        }

        public string Name { get; }

        public List<Card> Hand { get; } = new List<Card>();

        public bool Flush
        {
            get
            {
                return suit_indices_.Count == 1;
            }
        }

        public List<int> RankFrequency
        {
            get
            {
                return rank_frequency_;
            }
        }

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

                    sorted_rank_frequency_.Sort((x, y) => {
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
