using KozzionTrading.Optimizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KozzionTrading.Policy
{
    public class PolicyTemplateJaapBands: APolicyTemplate
    {
        public PolicyTemplateJaapBands() 
            : base("PolicyJaapBands",
                new ParameterSet(new ParameterValue[]
                {
                    new ParameterValue("varriance_history_size", TypeCode.Int32,  270),
                    new ParameterValue("momentum_weight",  TypeCode.Double, 0.91),
                    new ParameterValue("expected_error_z", TypeCode.Double,50.0),
                    new ParameterValue("max_error_weight", TypeCode.Double,0.015),
                    new ParameterValue("stop_loss",  TypeCode.Double,210.0),
                    new ParameterValue("take_profit", TypeCode.Double,305.0),
                    new ParameterValue("threshold_long", TypeCode.Double, Math.Pow(10, -5)),
                    new ParameterValue("threshold_short", TypeCode.Double, -Math.Pow(10, -5))
                }))
        { 
        }

  
        public override IPolicy Instance(ParameterSet parameter_set)
        {
            return new PolicyJaapBands(parameter_set);
        }
    }
}
