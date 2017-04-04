namespace KozzionTrading.Market
{
    public struct TradingOrderCommand
    {
        public static readonly double LOT   =  100000;
        public static readonly double POINT =  0.00001;

        public TradingOrderType OrderType { get; private set; }
        public double Volume { get; private set; }
        public double Price { get; private set; }
        public double Slippage { get; private set; }
        public double StopLoss { get; private set; }
        public double TakeProfit { get; private set; } 
        public bool UseLimits { get; private set; }

        private TradingOrderCommand(TradingOrderType trade_order_type, double volume, double price, double slippage, double stop_loss, double take_profit, bool use_limits)
        {
            this.OrderType = trade_order_type;
            this.Volume = volume;
            this.Price = price;
            this.Slippage = slippage;   
            this.StopLoss = stop_loss;
            this.TakeProfit = take_profit;
            this.UseLimits = use_limits;
        }

        public TradingOrderCommand(TradingOrderType trade_order_type, double volume, double price, double slippage, double stop_loss, double take_profit)
            : this(trade_order_type, volume, price, slippage, stop_loss, take_profit, true)
        {

        }

        public TradingOrderCommand(TradingOrderType trade_order_type, double volume, double price, double slippage)
            : this(trade_order_type, volume, price, slippage, 0,0, false)
        {

        }


    }
}
