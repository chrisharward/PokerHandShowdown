using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerHandShowdown
{
    public class ThreeOfAKindRule : RepeatedRankRule
    {
        /// <summary>
        /// Determines whether or not a players hand contains 3 of a kind.
        /// </summary>
        protected override bool ContainsType(Player player)
        {
            return player.SortedRankFrequency[0].Item1 > 2;
        }
    }
}
