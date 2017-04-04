using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KozzionTrading.Market;
using KozzionTrading.Tools;
using KozzionTrading.Optimizer;

namespace KozzionTrading.Policy
{
    public class PolicyRunningAverageDual : IPolicy
    {
        private int run_lenght_long;
        private double gain_threshold_long;
        private int run_lenght_short;
        private double gain_threshold_short;
        public string Title { get; private set; }

        public PolicyRunningAverageDual(int run_lenght_long, double gain_threshold_long, int run_lenght_short, double gain_threshold_short)
        {
            this.run_lenght_long = run_lenght_long;
            this.gain_threshold_long = gain_threshold_long;
            this.run_lenght_short = run_lenght_short;
            this.gain_threshold_short = gain_threshold_short;
            this.Title = "PolicyRunningAverageDual";
        }

        public PolicyRunningAverageDual(ParameterSet parameter_set)
            : this(
                  parameter_set["run_lenght_long"].Int32Value,
                  parameter_set["gain_threshold_long"].Float64Value,
                  parameter_set["run_lenght_short"].Int32Value,
                  parameter_set["gain_threshold_short"].Float64Value)
        {

        }

        

        public void GetTradeOrderCommand(IMarketModelSimulation market)
        {
            if ((market.OpenOrders.Count == 0) && (run_lenght_long < market.Second1.HistoryCount))
            {
                double gain_long = market.CurrentBid - market.Second1.GetHistory(run_lenght_long).CloseAsk;
                if (gain_threshold_long < gain_long)
                {
                    market.ProcessOrderCommand(new TradingOrderCommand(TradingOrderType.Long, TradingConstants.LOT, market.CurrentAsk, 0, market.CurrentAsk - gain_long, market.CurrentBid + gain_long));
                }          
            }

            if ((market.OpenOrders.Count == 0) && (run_lenght_short < market.Second1.HistoryCount))
            {       
                double gain_short = market.CurrentAsk - market.Second1.GetHistory(run_lenght_short).CloseBid;
                if (gain_short < gain_threshold_short)
                {
                    market.ProcessOrderCommand(new TradingOrderCommand(TradingOrderType.Short, TradingConstants.LOT, market.CurrentBid, 0, market.CurrentBid - gain_short, market.CurrentAsk + gain_short));
                }
      
            }
        }
    }
}
