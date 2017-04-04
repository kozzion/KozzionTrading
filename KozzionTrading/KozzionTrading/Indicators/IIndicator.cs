using KozzionMathematics.Function;
using KozzionTrading.Market;
using System;
using System.Collections.Generic;

namespace KozzionTrading.Indicators
{
    public interface IIndicator
    {
        string Description { get;}
        int SubIndicatorCount { get; }
        IList<string> SubIndicatorNames { get; }

        Tuple<double[,], bool[]> ComputeAll(IMarketModelIndicator initial_market, int price_count);
        Tuple<double[], bool> Compute(IMarketModelIndicator current_market);
        bool ComputeRBA(IMarketModelIndicator current_market, double[] result);
    }
}
