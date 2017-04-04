using KozzionTrading.Market;
using System;

namespace KozzionTrading.Policy
{
    public class PolicyPassiveLong :IPolicy
    {
        public string Title { get { return "PolicyPassive"; } }

        public PolicyPassiveLong() 
        {
        }

        public void GetTradeOrderCommand(IMarketModelSimulation data_set_slice)
        {
            if (data_set_slice.OpenOrders.Count == 0)
            {
                data_set_slice.ProcessOrderCommand(new TradingOrderCommand(TradingOrderType.Long, 100000.0, data_set_slice.CurrentBid, 0));  
            }
     
        }
    }
}
