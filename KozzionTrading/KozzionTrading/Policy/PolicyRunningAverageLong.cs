using KozzionTrading.Market;
using System;

namespace KozzionTrading.Policy
{

    public class PolicyRunningAverageLong : IPolicy
    {

        private int run_lenght;
        private double gain_threshold;
        public string Title { get; private set; }

        public PolicyRunningAverageLong(int run_lenght, double gain_threshold)
        {
            this.run_lenght = run_lenght;
            this.gain_threshold = gain_threshold;
            this.Title = "PolicyRunningAverage " + run_lenght + " with threshold: " + gain_threshold;
        }

        public void GetTradeOrderCommand(IMarketModelSimulation market)
        {
            if ((market.OpenOrders.Count == 0) && (run_lenght < market.Second1.HistoryCount))
            {
                double gain = market.CurrentBid - market.Second1.GetHistory(run_lenght).CloseBid;
                if (gain_threshold < gain)
                {
                    market.ProcessOrderCommand(new TradingOrderCommand( TradingOrderType.Long, 100000, market.CurrentAsk, 0, market.CurrentAsk - gain, market.CurrentBid + gain));
                }
            }
        }
    }
}
