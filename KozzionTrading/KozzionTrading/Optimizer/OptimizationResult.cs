using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KozzionTrading.Optimizer
{
    public class OptimizationResult
    {
        public ParameterSpaceGrid SearchGrid { get; private set; }
        public Dictionary<ParameterSet, double> Results { get; private set; }
        public ParameterSet Optimal { get; private set; }
        public double OptimalResult { get; private set; }

        public OptimizationResult(ParameterSpaceGrid search_grid, Dictionary<ParameterSet, double> results, ParameterSet optimal, double optimal_result)
        {
            SearchGrid = search_grid;
            Results = results;
            Optimal = optimal;
            OptimalResult = optimal_result;
        }
    }
}
