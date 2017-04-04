using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KozzionTrading.Market;
using KozzionTrading.Optimizer;
using KozzionTrading.Tools;

namespace KozzionTrading.Policy
{
    public class PolicyTemplateRunningAverageDual : APolicyTemplate
    {
        public PolicyTemplateRunningAverageDual() 
            : base("PolicyTemplateRunningAverageDual",
                new ParameterSet(new ParameterValue[]
                {
                    new ParameterValue("run_lenght_long", TypeCode.Int32,270),
                    new ParameterValue("gain_threshold_long", TypeCode.Double, 5 * TradingConstants.POINT),
                    new ParameterValue("run_lenght_short", TypeCode.Int32, 270),
                    new ParameterValue("gain_threshold_short", TypeCode.Double, 5 * TradingConstants.POINT)
                }))
        {
        }

        public override IPolicy Instance(ParameterSet parameter_set)
        {
            return new PolicyRunningAverageDual(parameter_set);
        }
    }
}
