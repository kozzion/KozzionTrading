using KozzionMathematics.Tools;
using KozzionTrading.Market;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KozzionTrading.Optimizer;

namespace KozzionTrading.Indicators
{
    public class IndicatorTemplateSuperBollinger : AIndicatorTemplate
    {
        public IndicatorTemplateSuperBollinger(int varriance_history_size, double momentum_weight, double expected_error_z, double max_error_weight) 
            : base(
                new ParameterSet(new ParameterValue [] 
                {
                    new ParameterValue("varriance_history_size", TypeCode.Int32, 270),
                    new ParameterValue("momentum_weight", TypeCode.Double, 0.91),
                    new ParameterValue("expected_error_z", TypeCode.Double, 50.0),
                    new ParameterValue("max_error_weight", TypeCode.Double, 0.015)
                }),
                "IndicatorSuperBollinger",                 
                new string[] {
                    "Main",
                    "Upper",
                    "Lower",
                    "error_now",
                    "error_now_sdv",
                    "error_now_sum",
                    "error_now_z",
                    "error_now_weight",
                    "momentum" })
        {
            
        }

        public IndicatorTemplateSuperBollinger()
          : this(60, 0.001, 30, 0.2)
        {
        }

        public override IIndicator CreateIndicator(ParameterSet parameters)
        {
            return new IndicatorSuperBollinger(parameters);
        }
    }
}
