using KozzionCore.DataStructure.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KozzionTrading.Market
{
    public interface IMarketModel
    {
        double Cash { get; }
        IHistoryList<Price> Prices { get; }
        IHistoryList<PriceCandle> Second1 { get; }
        IHistoryList<PriceCandle> Minute1 { get; }
        IHistoryList<PriceCandle> Minute15 { get; }
        IHistoryList<PriceCandle> Minute30 { get; }
        IHistoryList<PriceCandle> Hour1 { get; }
        IHistoryList<PriceCandle> Hour4 { get; }
        IHistoryList<PriceCandle> Day1 { get; }
        IHistoryList<PriceCandle> Week1 { get; }
        IHistoryList<PriceCandle> Month1 { get; }
        double CurrentBid { get; }
        double CurrentAsk { get; }
    }
}
