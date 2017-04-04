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
    public class ExperimentOptimizePolicyRunningAverageDual : IExperiment
    {
        public void DoExperiment()
        {
            PriceSet price_set = ToolsPrice.GetPriceSet(ToolsPrice.DefaultSymbolUSDEUR);
            List<IPolicy> policies = new List<IPolicy>();
            for (int index_0 = 1; index_0 < 10; index_0++)
            {
                for (int index_1 = 1; index_1 < 10; index_1++)
                {
                    policies.Add(new PolicyRunningAverageDual(
                        index_0 * 30, index_1 * 5 * TradingConstants.POINT,
                        index_0 * 30, index_1 * -5 * TradingConstants.POINT));
                }
            }

            ExperimentPolicies experiment = new ExperimentPolicies(price_set, policies);
            experiment.DoExperiment();
        }
    }
}
