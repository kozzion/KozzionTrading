using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KozzionTrading.Market
{
    public enum PriceType
    {
        Bid, //open + close + high + low / 4 for bid
        Ask, //open + close + high + low / 4 for ask
        Mean, //open + close + high + low / 4 for mean

    }
}
