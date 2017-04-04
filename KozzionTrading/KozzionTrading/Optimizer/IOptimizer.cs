namespace KozzionTrading.Optimizer
{
    public interface IOptimizer
    {
        OptimizationResult Optimize(ParameterSet initial_guess);
    }
}