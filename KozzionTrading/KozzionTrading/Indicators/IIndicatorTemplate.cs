using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KozzionTrading.Optimizer;

namespace KozzionTrading.Indicators
{
    public interface IIndicatorTemplate
    {
        IIndicator CreateIndicator(ParameterSet parameters);
        IIndicator CreateIndicator();
        ParameterSet DefaultParameters { get; }
        string IndicatorName { get; }
    }
}
