using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KozzionTrading.Market;

namespace KozzionTrading.Policy
{
    public class PolicyPyramid : IPolicy
    {
        public string Title
        {
            get
            {
                return "PolicyPyramid";
            }
        }
        double SwapSize;
        double LimitSize;
        double InitialVolume;
        double Slippage;

        public PolicyPyramid(double swap_size, double limit_size, double initial_volume)
        {
            this.SwapSize = swap_size;
            this.LimitSize = limit_size;
            this.InitialVolume = initial_volume;
            this.Slippage = 2 * TradingOrderCommand.POINT;
        }

        public void GetTradeOrderCommand(IMarketModelSimulation market_model)
        {
            if (market_model.OpenOrders.Count == 0)
            {
                OpenFirst(market_model);
            }
            else
            {
                CheckSwap(market_model);
            }
        }

        public void OpenFirst(IMarketModelSimulation market_model)
        {
            //Long short criterion
            double upper_limit = market_model.CurrentAsk + 0;
            double lower_limit = market_model.CurrentAsk - 0;

            if (BuyLong())
            {            
                market_model.ProcessOrderCommand(new TradingOrderCommand(TradingOrderType.Long, market_model.CurrentAsk, InitialVolume, this.Slippage, lower_limit, upper_limit));
            }
            else
            {
                market_model.ProcessOrderCommand(new TradingOrderCommand(TradingOrderType.Short, market_model.CurrentBid, InitialVolume, this.Slippage, upper_limit, lower_limit));
            }
        }

        public void CheckSwap(IMarketModelIndicator market_model)
        {
        }

        private bool BuyLong()
        {
            return true;
        }
    }
}
