using System.Collections.Generic;
using System.Text;

namespace KozzionTrading.Market
{
    public class MarketResult
    { 

        public double StartCash { get; private set; }
        public double EndCash { get; private set; }
        public PriceSet PriceSet { get; private set; }
        public IMarketModelSimulation Market { get; private set; }
        public IReadOnlyList<double> Ticks { get { return PriceSet.Ticks; } }
        public IReadOnlyList<double> Cash { get; private set; }
        public IReadOnlyList<double> Equity { get; private set; }




        public MarketResult(double start_cash, PriceSet price_set, IMarketModelSimulation market, IReadOnlyList<double> cash, IReadOnlyList<double> equity)
        {
            this.StartCash = start_cash;
            this.EndCash = market.Cash;
            this.PriceSet = price_set;
            this.Market = market;
            this.Cash = cash;
            this.Equity = equity;
        }

        public override string ToString()
        {
            StringBuilder string_builder = new StringBuilder();
            string_builder.AppendLine("Start Cash: " + StartCash);
            string_builder.AppendLine("End Cash:   " + EndCash);
            string_builder.AppendLine("Orders:");
            foreach (TradingOrder order in Market.ClosedOrders)
            {
                string_builder.AppendLine("Open price: " + order.OpenPrice + " Open tick: " + order.OpenDateTime + " Close price: " + order.ClosePrice + " Close tick: " + order.CloseDateTime + " Commission: " + order.CommissionForOrder + " Profit: " + order.Profit);
            }
            return string_builder.ToString();
        }

        public void ResultReport()
        {
            int posLong = 0;
            int posLongWin = 0;
            int posShort = 0;
            int posShortWin = 0;
            double profitTotal = 0;

            foreach (TradingOrder tradingOrder in Market.ClosedOrders)
            {
                if(tradingOrder.OrderType == TradingOrderType.Long)
                {
                    posLong += 1;
                    if(0 < tradingOrder.Profit)
                    {
                        posLongWin += 1;
                    }
                }
                if (tradingOrder.OrderType == TradingOrderType.Short)
                {
                    posShort += 1;
                    if (0 < tradingOrder.Profit)
                    {
                        posShortWin += 1;
                    }
                }
                profitTotal += tradingOrder.Profit;
                
            }


            System.Console.WriteLine("Long positions: " + posLong);
            System.Console.WriteLine("Long positions won: " + posLongWin);
            System.Console.WriteLine("Short positions: " + posShort);
            System.Console.WriteLine("Short positions won: " + posShortWin);
            System.Console.WriteLine("Total positions: " + (posLong + posShort));
            //System.Console.WriteLine("Total win percentage: " + ((posLongWin + posShortWin) / (posLong + posShort)));
            System.Console.WriteLine("Total profit: " + profitTotal);
        }

    }
}