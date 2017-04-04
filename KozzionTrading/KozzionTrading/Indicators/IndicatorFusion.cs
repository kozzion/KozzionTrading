using KozzionCore.Tools;
using KozzionTrading.Market;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KozzionTrading.Indicators
{
    public class IndicatorFusion : AIndicator
    {
        private IList<IIndicator> indicators;
        private int[] indicator_offsets;

        public IndicatorFusion(IList<IIndicator> indicators)
            : base("IndicatorFusion", GetSubIndicatorNames(indicators))
        {
            this.indicators = new List<IIndicator>(indicators);

            indicator_offsets = new int[indicators.Count];
            int all_indicator_count = this.SubIndicatorCount;
            if (0 < indicators.Count)
            {
                for (int indicator_index = 1; indicator_index < indicators.Count; indicator_index++)
                {
                    indicator_offsets[indicator_index] = indicator_offsets[indicator_index - 1] + indicators[indicator_index - 1].SubIndicatorCount;
                }
            }
        }

        private static IList<string> GetSubIndicatorNames(IList<IIndicator> indicators)
        {
            List<string> sub_indicator_names = new List<string>();
            foreach (IIndicator indicator in indicators)
            {
                sub_indicator_names.AddRange(indicator.SubIndicatorNames);
            }
            return sub_indicator_names;
        }

        public override bool ComputeRBA(IMarketModelIndicator market_model, double[] target)
        {
            bool all_valid = true;
            for (int indicator_index = 0; indicator_index < indicators.Count; indicator_index++)
            {
                IIndicator inidicator = indicators[indicator_index];
                Tuple<double[], bool> tuple = inidicator.Compute(market_model);
                ToolsCollection.CopyRBA(tuple.Item1, 0, target, indicator_offsets[indicator_index], inidicator.SubIndicatorCount);

                if (!tuple.Item2)
                {            
                    all_valid = false;
                }
            }    
            return all_valid;
        }


    }
}
