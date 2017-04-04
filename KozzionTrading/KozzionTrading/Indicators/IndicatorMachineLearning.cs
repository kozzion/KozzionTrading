using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KozzionTrading.Market;
using KozzionMachineLearning.Model;
using KozzionCore.Tools;

namespace KozzionTrading.Indicators
{
    public class IndicatorMachineLearning : AIndicator
    {
        IIndicator indicator;
        IModelLabel<double, double> model;

        public IndicatorMachineLearning(IIndicator indicator, IModelLabel<double, double> model, string output_name) 
            : base("IndicatorMagicLongProfit", new string[] { output_name })
        {
            this.indicator = indicator;
            this.model = model;
        }

        public override bool ComputeRBA(IMarketModelIndicator market_model, double[] target)
        {
            Tuple<double[], bool> tuple = indicator.Compute(market_model);
            if (!tuple.Item2)
            {
                return false;
            }
            target[0] = model.GetLabel(tuple.Item1);
            return true;
        }

 
    }
}
