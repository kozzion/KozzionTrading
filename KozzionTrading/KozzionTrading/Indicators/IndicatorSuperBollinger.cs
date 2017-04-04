using KozzionMathematics.Tools;
using KozzionTrading.Market;
using KozzionTrading.Optimizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KozzionTrading.Indicators
{
    public class IndicatorSuperBollinger : AIndicator
    {

        double[] indicator_error_now;
        int index_indicator_error_now;
        double momentum_weight;
        double expected_error_z;
        double max_error_weight;

        double previous_1;
        double previous_2;

        public IndicatorSuperBollinger(ParameterSet parameter_set)
    : base("IndicatorSuperBollinger", new string[] {
            "Main",
            "Upper",
            "Lower",
            "error_now",
            "error_now_s",
            "error_now_z",
            "error_now_weight",
            "momentum"})
        {
            this.indicator_error_now = new double[parameter_set["varriance_history_size"].Int32Value];
            this.index_indicator_error_now = 0;
            this.momentum_weight = parameter_set["momentum_weight"].Float64Value;
            this.expected_error_z = parameter_set["expected_error_z"].Float64Value;
            this.max_error_weight = parameter_set["max_error_weight"].Float64Value;


            this.previous_1 = 0;
            this.previous_2 = 0;
        }

        public IndicatorSuperBollinger(int varriance_history_size, double momentum_weight, double expected_error_z, double max_error_weight) 
            : base("IndicatorSuperBollinger", new string[] {
            "Main",
            "Upper",
            "Lower",
            "error_now",
            "error_now_s",
            "error_now_z",
            "error_now_weight",
            "momentum"})
        {

            this.indicator_error_now = new double[varriance_history_size];
            this.index_indicator_error_now = 0;
            this.momentum_weight = momentum_weight;
            this.expected_error_z = expected_error_z;
            this.max_error_weight = max_error_weight;

      
            this.previous_1 = 0;
            this.previous_2 = 0;
        }

        public IndicatorSuperBollinger()
          : this(60, 0.001, 30, 0.2)
        {
        }

        public override bool ComputeRBA(IMarketModelIndicator market_model, double[] result)
        {
            if (previous_1 == 0)
            {
                previous_1 = market_model.CurrentBid;
                previous_2 = market_model.CurrentBid;
            }

            double momentum = previous_1 - previous_2;
            double error_now = market_model.CurrentBid - (previous_1 + (momentum_weight * momentum));            
            indicator_error_now[index_indicator_error_now % indicator_error_now.Length] = error_now;
            index_indicator_error_now++;

            double error_now_s = ToolsMathStatistics.StandardDeviation(indicator_error_now);
            if (error_now_s == 0)
            {
                error_now_s = double.Epsilon;
            }
            double error_now_z = error_now / error_now_s;
            double error_now_weight = max_error_weight * (1 - Math.Exp(-ToolsMath.Sqr(error_now_z) / expected_error_z));

            result[0] = previous_1 + (momentum * momentum_weight) + (error_now * error_now_weight);
            result[1] = result[0] + (error_now_s * 2);
            result[2] = result[0] - (error_now_s * 2);
            result[3] = error_now;
            result[4] = error_now_s;
            result[5] = error_now_z;
            result[6] = error_now_weight;
            result[7] = momentum;

            previous_2 = previous_1;
            previous_1 = result[0];


            return indicator_error_now.Length < market_model.Second1.HistoryCount;
        }
    }
}
