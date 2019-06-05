using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerHandShowdown
{
    public class ThreeOfAKindRule : RepeatedRankRule
    {
        protected override bool ContainsType(Player player)
        {
            return player.SortedRankFrequency[0].Item1 > 2;
        }
    }
}
