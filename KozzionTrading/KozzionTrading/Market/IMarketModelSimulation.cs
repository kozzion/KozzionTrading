using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KozzionTrading.Market
{
    public interface IMarketModelSimulation : IMarketModelIndicator
    {
        Dictionary<int, TradingOrder> OpenOrders { get; }
        List<TradingOrder> ClosedOrders { get; }
        void AddStep(PriceCandle priceCandle);
        void CloseAll();
        void ProcessOrderCommand(TradingOrderCommand tradingOrderCommand);
        void ModifyOrder(int order_ticket, double stop_loss, double take_profit);
        void CloseOrder(int order_ticket);
    }
}
