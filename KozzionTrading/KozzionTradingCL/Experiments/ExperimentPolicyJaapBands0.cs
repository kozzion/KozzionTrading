using KozzionTrading.Market;
using KozzionTrading.Policy;
using KozzionTrading.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KozzionTradingCL.Experiments
{
    public class ExperimentPolicyJaapBands0
    {
        public void DoExperiment()
        {
            PriceSet price_set = ToolsPrice.GetPriceSet(ToolsPrice.DefaultSymbolGBPUSD);

            List<IPolicy> policies = new List<IPolicy>();
            double long_threshold = 10 * Math.Pow(10, -6);
            double short_threshold = -10 * Math.Pow(10, -6);
            for (int index = 0; index < 1; index++)
            {
                policies.Add(new PolicyTemplateJaapBands().Instance());
            }

            MarketResult[] market_results = new MarketResult[policies.Count];

            Parallel.For(0, policies.Count, index =>
            {
                MarketManagerSimulation exchange = new MarketManagerSimulation(10000, price_set);
                market_results[index] = exchange.Run(policies[index]);
            });

            List<Tuple<MarketResult, IPolicy>> accepted_policy = new List<Tuple<MarketResult, IPolicy>>();
            for (int index = 0; index < market_results.Length; index++)
            {
                //    if(market_results[index].Market.ClosedOrders.Count > 9 && market_results[index].EndCash > 10000)
                //    {
                accepted_policy.Add(new Tuple<MarketResult, IPolicy>(market_results[index], policies[index]));
                //    }
            }

            for (int index = 0; index < accepted_policy.Count; index++)
            {
                var xx = accepted_policy[index];
                Console.WriteLine(xx.Item1.EndCash + " in: " + xx.Item1.Market.ClosedOrders.Count);
                ToolsTradingPlotting.PlotMarketResult(ToolsTradingDataSet.GetPath() + "Trades" + index + ".png", xx.Item1);
            }

        }
    }
}
