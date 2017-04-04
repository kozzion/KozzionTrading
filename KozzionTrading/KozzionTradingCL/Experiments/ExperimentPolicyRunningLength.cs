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
    public class ExperimentPolicyRunLength
    {
        public void DoExperiment()
        {
            PriceSet price_set = ToolsPrice.GetPriceSet(ToolsPrice.DefaultSymbolUSDEUR);
            List<IPolicy> policies = new List<IPolicy>();
            for (int index_0 = 2; index_0 < 10; index_0++)
            {

                policies.Add(new PolicyRunLength(index_0, 300));
       
            }

            ExperimentPolicies experiment = new ExperimentPolicies(price_set, policies);
            experiment.DoExperiment();
        }
    }
}
