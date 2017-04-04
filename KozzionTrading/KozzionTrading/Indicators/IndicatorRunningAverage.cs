using KozzionTrading.Market;
using System;
using System.Collections.Generic;

namespace KozzionTrading.Indicators
{
    public class IndicatorRunningAverage : AIndicator
    {
        public int HistorySize { get; private set; }

        public IndicatorRunningAverage(int history) 
            : base("IndicatorRunningAverage_" + history, new string[] { "IndicatorRunningAverage_" + history } )
        {
            this.HistorySize = history;
        }

        public override bool ComputeRBA(IMarketModelIndicator market_model, double[] result)
        {
            bool is_valid = HistorySize <= market_model.Second1.HistoryCount;
            if (is_valid)
            {
                double average = market_model.CurrentBid;
                for (int index = 0; index < HistorySize; index++)
                {
                    average += market_model.Second1.GetHistory(index).OpenBid;
                }
                result[0] = average / (HistorySize + 1);
            }
            return is_valid;
        }
    }
}
