using KozzionTrading.Market;
using KozzionTrading.Policy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KozzionTradingTest.Market
{
    public class IMarketModelTest
    {
        public static void TestSpeed(double initial_cash, PriceSet price_set, int count)
        {
            List<PolicyJaapBands> policies = new List<PolicyJaapBands>();
            for (int i = 0; i < count; i++)
            {
                policies.Add(new PolicyJaapBands(300, 0.85, 50, 0.015, 150, 130));
            }
            MarketResult[] market_results = new MarketResult[policies.Count];
            Parallel.For(0, policies.Count, index =>
            {
                MarketManagerSimulation exchange = new MarketManagerSimulation(10000, price_set);
                market_results[index] = exchange.Run(policies[index]);
            });
            Assert.AreEqual(10230.0, market_results[0].EndCash, 0.0000001);
        }
    }
}
