using KozzionTrading.IO;
using KozzionTrading.Market;
using KozzionTrading.Policy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KozzionTradingCL.Experiments
{
    public class ExperimentPolicies : IExperiment
    {
        private PriceSet price_set;
        private List<IPolicy> policies;
        public ExperimentPolicies(PriceSet price_set, IList<IPolicy> policies)
        {
            this.price_set = price_set;
            this.policies = new List<IPolicy>(policies);
        }

        public void DoExperiment()
        {
            MarketResult[] results = new MarketResult[policies.Count];
            Parallel.For(0, policies.Count, index =>
            {
                MarketManagerSimulation exchange = new MarketManagerSimulation(10000, price_set);
                results[index] = exchange.Run(policies[index]);
            });

            for (int index = 0; index < policies.Count; index++)
            {
                Console.WriteLine(policies[index].Title);
                Console.WriteLine(results[index].EndCash);
                Console.WriteLine("In: " + results[index].Market.ClosedOrders.Count);
            }
   
        }
    }
}
