using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerHandShowdown
{
    public class HighCardRule : IRule
    {
        /// <summary>
        /// Determines which players win using a high card rule.
        /// </summary>
        /// <returns>list of winning players</returns>
        public List<Player> Apply(Round round)
        {
            List<Player> winners = new List<Player>();
            if (round.Players.Count == 0)
            {
                return winners;
            }

            winners.Add(round.Players[0]);
            for (int i = 1; i < round.Players.Count; ++i)
            {
                int cmp = RuleUtils.HighCardCompare(winners[0], round.Players[i]);
                if (cmp == 1)
                {
                    continue;
                }
                else if (cmp == -1)
                {
                    winners.Clear();
                }
                winners.Add(round.Players[i]);
            }
            return winners;
        }
    }
}
