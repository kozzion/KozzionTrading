using KozzionMathematics.Tools;
using KozzionTrading.Market;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KozzionTrading.Indicators
{
    public class IndicatorSuperBollingerFast : AIndicator
    {
        double[] error_now_array;
        int index_error_now;
        double error_now_array_sqr_sum;
        double error_now_array_sum;
        double momentum_weight;
        double expected_error_z;
        double max_error_weight;

        double previous_1;
        double previous_2;

        public IndicatorSuperBollingerFast(int error_history, double momentum_weight, double expected_error_z, double max_error_weight) 
            : base("IndicatorSuperBollinger", new string[] {
            "Main",
            "Upper",
            "Lower",
            "error_now",
            "error_now_sdv",
            "error_now_sum",
            "error_now_z",
            "error_now_weight",
            "momentum"
           })
        {

            this.error_now_array = new double[error_history];
            this.index_error_now = 0;
            this.error_now_array_sqr_sum = 0;
            this.error_now_array_sum = 0;
            this.momentum_weight = momentum_weight;
            this.expected_error_z = expected_error_z;
            this.max_error_weight = max_error_weight;

      
            this.previous_1 = 0;
            this.previous_2 = 0;
        }

        public IndicatorSuperBollingerFast()
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
            error_now_array_sqr_sum = (error_now_array_sqr_sum + ToolsMath.Sqr(error_now) - ToolsMath.Sqr(this.error_now_array[index_error_now % this.error_now_array.Length])) / this.error_now_array.Length;
            error_now_array_sum = error_now_array_sum + error_now - this.error_now_array[index_error_now % this.error_now_array.Length];

            this.error_now_array[index_error_now % this.error_now_array.Length] = error_now;
            index_error_now++;
   

            double error_now_sdev = Math.Sqrt(error_now_array_sqr_sum);
            if (error_now_sdev == 0)
            {
                error_now_sdev = double.Epsilon;
            }
            double error_now_z = error_now / error_now_sdev;
            double error_now_weight = max_error_weight * (1 - Math.Exp(-ToolsMath.Sqr(error_now_z) / expected_error_z));

            result[0] = previous_1 + (momentum * momentum_weight) + (error_now * error_now_weight);
            result[1] = result[0] + (error_now_sdev * 2);
            result[2] = result[0] - (error_now_sdev * 2);
            result[3] = error_now;
            result[4] = error_now_sdev;
            result[5] = error_now_array_sum;
            result[6] = error_now_z;
            result[7] = error_now_weight;
            result[8] = momentum;

            previous_2 = previous_1;
            previous_1 = result[0];

            return this.error_now_array.Length < market_model.Second1.HistoryCount;
        }
    }
}
