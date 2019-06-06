using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerHandShowdown
{
    public class FlushRule : IRule
    {
        /// <summary>
        /// Determines if any players in this round have flush hands, and which ones are the winners if so.
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
                if (!player.Flush)
                {
                    continue;
                }

                if (winners.Count != 0)
                {
                    int cmp = RuleUtils.HighCardCompare(winners[0], player);
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
    }
}
