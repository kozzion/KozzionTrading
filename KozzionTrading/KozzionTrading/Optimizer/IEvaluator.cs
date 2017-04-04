using System.Collections.Generic;

namespace KozzionTrading.Optimizer
{
    public interface IEvaluator
    {
        Dictionary<ParameterSet, double> Results { get; }

        double Evaluate(ParameterSpaceGrid search_grid, ParameterSet paramter_set);
    }
}