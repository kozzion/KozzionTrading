using KozzionCore.Tools;
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
    public class ExperimentOptimize
    {
        public void DoExperiment()
        {
            PriceSet price_set = ToolsPrice.GetPriceSet(ToolsPrice.DefaultSymbolGBPUSD).SubSet(new DateTimeUTC(2016, 10, 17), new DateTimeUTC(2016, 10, 20));
            IPolicyTemplate policy_template = new PolicyTemplateRunningAverageDual();
            double initial_cash = 10000;
            IEvaluator evaluator = new EvaluatorSingle(policy_template, initial_cash, price_set);
            ParameterSpaceGrid search_grid = new ParameterSpaceGrid(
                policy_template.DefaultParameters,
                new double[] { 10, 5 * TradingConstants.POINT,  10, 5 * TradingConstants.POINT},
                new int[] { 1, 1, 1, 1 },
                new int[] { 1, 1, 1, 1 }
            );
            IOptimizer optimizer = new OptimizerExaustive(search_grid, evaluator);
            OptimizationResult result = optimizer.Optimize(policy_template.DefaultParameters);
            Console.WriteLine(result.OptimalResult);

        }
    }
}
