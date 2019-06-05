using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerHandShowdown
{
    public interface IRule
    {
        List<Player> Apply(Round round);
    }

    public class RuleUtils
    {
        public static int MultiCompare(Player player1, Player player2)
        {
            var p1_ri = player1.SortedRankFrequency[0].Item2;
            var p2_ri = player2.SortedRankFrequency[0].Item2;
            if (p1_ri > p2_ri)
            {
                return 1;
            }
            else if (p1_ri < p2_ri)
            {
                return -1;
            }

            return RuleUtils.HighCardCompare(player1, player2);
        }

        public static int HighCardCompare(Player player1, Player player2)
        {
            for (int i = player1.RankFrequency.Count - 1; i >= 0; --i)
            {
                var if1 = player1.RankFrequency[i];
                var if2 = player2.RankFrequency[i];

                if (if1 > if2)
                {
                    return 1;
                }
                else if (if1 < if2)
                {
                    return -1;
                }
            }
            return 0;
        }
    }
}
