using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerHandShowdown
{
    public abstract class RepeatedRankRule : IRule
    {
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

        protected abstract bool ContainsType(Player player);
    }
}
