using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KozzionTrading.Market;

namespace KozzionTrading.Indicators
{
    public class IndicatorMagicProfit : AIndicator
    {
        private int future;

        public IndicatorMagicProfit(int seconds_future) 
            : base("IndicatorMagicLongProfit", new string[] { "IndicatorMagicLongProfit_" + seconds_future, "IndicatorMagicShortProfit_" + seconds_future })
        {
            this.future = seconds_future;
        }


        public override bool ComputeRBA(IMarketModelIndicator market_model, double[] result)
        {
            if (market_model.Prices.FutureCount < future)
            {
                return false;
            }

            double long_profit  = market_model.CurrentBid - market_model.CurrentAsk;
            double short_profit = market_model.CurrentBid - market_model.CurrentAsk;
            for (int index = 0; index < future; index++)
            {
                long_profit = Math.Max(long_profit, market_model.Prices.GetFuture(index).Bid - market_model.CurrentAsk);
                short_profit = Math.Max(short_profit, market_model.CurrentBid - market_model.Prices.GetFuture(index).Ask);
            }

            result[0] = long_profit;
            result[1] = short_profit;
            return true;
        }

 
    }
}
