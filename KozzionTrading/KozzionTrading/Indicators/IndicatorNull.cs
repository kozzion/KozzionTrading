using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KozzionTrading.Market;

namespace KozzionTrading.Indicators
{
    class IndicatorNull : AIndicator
    {
        public IndicatorNull()
            : base("IndicatorNull", new string[] { })
        {
        }

        public override bool ComputeRBA(IMarketModelIndicator market_model, double[] result)
        {
            return true;
        }
    }
}
