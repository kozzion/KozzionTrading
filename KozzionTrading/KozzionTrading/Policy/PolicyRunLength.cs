using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KozzionTrading.Market;
using KozzionTrading.Tools;

namespace KozzionTrading.Policy
{
    public class PolicyRunLength : IPolicy
    {
        public string Title
        {
            get
            {
                return "PolicyRunLength";
            }
        }

        int run_length;
        int margin;

        public PolicyRunLength(int run_length, int margin)
        {
            if (run_length < 1)
            {
                throw new Exception("run_length must be at least 1 ");
            }

            if (margin < 1)
            {
                throw new Exception("margin must be at least 1 ");
            }

            this.run_length = run_length;
            this.margin = margin;
        }

        public void GetTradeOrderCommand(IMarketModelSimulation market)
        {
            if (market.Second1.HistoryCount < run_length)
            {
                return;
            }
            //Long side: Bid is rising
            if (market.Second1.GetHistory(1).CloseBid < market.Second1.GetHistory(0).CloseBid)
            {
                bool make_trade = true;
                for (int index = 1; index < run_length; index++)
                {
                    if (market.Second1.GetHistory(index + 1).CloseBid >= market.Second1.GetHistory(index).CloseBid)
                    {
                        make_trade = false;
                    }
                }

                if (make_trade)
                {
                    Long(market);
                }
            }

            //Short side: Ask is going doen
            if (market.Second1.GetHistory(0).CloseAsk < market.Second1.GetHistory(1).CloseAsk)
            {
                bool make_trade = true;
                for (int index = 1; index < run_length; index++)
                {
                    if (market.Second1.GetHistory(index).CloseAsk >= market.Second1.GetHistory(index + 1).CloseAsk)
                    {
                        make_trade = false;
                    }
                }

                if (make_trade)
                {
                    Short(market);
                }
            }
       
        }

        public void Long(IMarketModelSimulation market)
        {
            market.ProcessOrderCommand(new TradingOrderCommand(TradingOrderType.Long, 1, market.CurrentAsk, 2, market.CurrentBid - margin * TradingConstants.POINT, market.CurrentAsk + margin * TradingConstants.POINT));
        }


        public void Short(IMarketModelSimulation market)
        {
            market.ProcessOrderCommand(new TradingOrderCommand(TradingOrderType.Short, 1, market.CurrentBid, 2, market.CurrentAsk + margin * TradingConstants.POINT, market.CurrentBid - margin * TradingConstants.POINT));
        }
    }
}
