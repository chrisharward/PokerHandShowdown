using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerHandShowdown
{
    /// <summary>
    /// Responsible for building a round object, which is a list of players and their hands
    /// for this game.
    /// </summary>
    public class RoundBuilder
    {
        public RoundBuilder(TextReader reader)
        {
            tokenizer_ = new Tokenizer(reader);
        }

        public Round Build()
        {
            var round = new Round();
            var builder = new PlayerBuilder(tokenizer_);

            Player player;
            while ((player = builder.Build()) != null)
            {
                round.Players.Add(player);
            }

            return round;
        }

        private Tokenizer tokenizer_;
    }

    /// <summary>
    /// Represents a single round of poker, players and their hands for the round.
    /// </summary>
    public class Round
    {
        public List<Player> Players { get; } = new List<Player>();

        /// <summary>
        /// Play this round, calculate who the winners are
        /// </summary>
        /// <returns>a list of winners for this round</returns>
        public List<Player> Play()
        {
            if (Players.Count == 0)
            {
                return new List<Player>();
            }

            foreach (var rule in Round.rules_)
            {
                List<Player> winners = rule.Apply(this);
                if (winners.Count != 0)
                {
                    return winners;
                }
            }

            return new List<Player>();
        }

        private static List<IRule> rules_ = new List<IRule>(new IRule[] {
            new FlushRule(),
            new ThreeOfAKindRule(),
            new OnePairRule(),
            new HighCardRule()
        });
    }
}
