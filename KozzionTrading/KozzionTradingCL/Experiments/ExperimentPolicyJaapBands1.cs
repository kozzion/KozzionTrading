using KozzionTrading.Market;
using KozzionTrading.Optimizer;
using KozzionTrading.Policy;
using KozzionTrading.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KozzionTradingCL.Experiments
{
    public class ExperimentPolicyJaapBands1
    {
        public void DoExperiment()
        {
            IPolicyTemplate policy_template = new PolicyTemplateJaapBands();
            double initial_cash = 10000;
            PriceSet price_set = ToolsPrice.GetPriceSet(ToolsPrice.DefaultSymbolGBPUSD);
            IEvaluator evaluator = new EvaluatorSingle(policy_template, initial_cash, price_set);
            ParameterSpaceGrid search_grid = new ParameterSpaceGrid(
                policy_template.DefaultParameters,
                new double[] { },
                new int[] { },
                new int[] { }
            );
            IOptimizer optimizer = new OptimizerExaustive(search_grid, evaluator);
            OptimizationResult result = optimizer.Optimize(policy_template.DefaultParameters);
            Console.WriteLine(result.OptimalResult);

        }
    }
}
