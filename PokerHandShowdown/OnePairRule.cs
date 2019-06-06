using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerHandShowdown
{
    public class OnePairRule : RepeatedRankRule
    {
        /// <summary>
        /// Determines whether or not players hand contains a pair.
        /// </summary>
        protected override bool ContainsType(Player player)
        {
            return player.SortedRankFrequency[0].Item1 == 2;
        }
    }
}
