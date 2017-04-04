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
    public abstract class AIndicatorTemplate :IIndicatorTemplate
    {
        public ParameterSet DefaultParameters { get; private set; }
        public string IndicatorName { get; private set; }
        public IList<string> SubIndicatorNames { get; private set; }

        protected AIndicatorTemplate(ParameterSet default_parameters, string indicator_name, string[] subindicator_names)
        {
            this.DefaultParameters = default_parameters;
            this.IndicatorName = indicator_name;
            this.SubIndicatorNames = subindicator_names;
        }

        public IIndicator CreateIndicator()
        {
            return CreateIndicator(DefaultParameters);
        }

        public abstract IIndicator CreateIndicator(ParameterSet parameters);

    }
}
