using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerHandShowdown
{
    public abstract class RepeatedRankRule : IRule
    {
        /// <summary>
        /// Determines if winners can be found using a repeated rank rule (one pair, three of a kind).
        /// </summary>
        /// <returns>list of winners, empty list if no one wins under this rule.</returns>
        public List<Player> Apply(Round round)
        {
            var winners = new List<Player>();
            if (round.Players.Count == 0)
            {
                return winners;
            }

            foreach (var player in round.Players)
            {
                if (!ContainsType(player))
                {
                    continue;
                }

                if (winners.Count != 0)
                {
                    int cmp = RuleUtils.MultiCompare(winners[0], player);
                    if (cmp == 1)
                    {
                        continue;
                    }
                    else if (cmp == -1)
                    {
                        winners.Clear();
                    }
                }
                winners.Add(player);
            }
            return winners;
        }

        /// <summary>
        /// Does players hand contain the type we're interested in
        /// </summary>
        /// <returns>true if present</returns>
        protected abstract bool ContainsType(Player player);
    }
}
