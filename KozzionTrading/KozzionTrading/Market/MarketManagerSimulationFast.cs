using KozzionTrading.Policy;
using KozzionTrading.Market;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KozzionTrading.Market
{
    public class MarketManagerSimulation : IMarketManager
    {

        double initial_cash;
        PriceSet price_set;

        public MarketManagerSimulation(double initial_cash, PriceSet price_set)
        {
            this.price_set = price_set;
            this.initial_cash = initial_cash;
        }  

        public MarketResult Run(IPolicy policy)
        {
            MarketModelSimulation market = new MarketModelSimulation(initial_cash, price_set);
            double[] cash = new double[price_set.Prices.Count];
            double[] equity = new double[price_set.Prices.Count];

            cash[0] = market.Cash;
            equity[0] = market.Equity;
            for (int index = 1; index < price_set.Prices.Count - 1; index++)
            {              
                market.StepSecond();
                policy.GetTradeOrderCommand(market);
                cash[index] = market.Cash;
                equity[index] = market.Equity;
            }
            market.StepSecond();
            market.CloseAll();
            cash[price_set.Prices.Count - 1] = market.Cash;
            equity[price_set.Prices.Count - 1] = market.Equity;

            return new MarketResult(market.Cash, price_set, market, cash, equity); 
        }
    }
}
