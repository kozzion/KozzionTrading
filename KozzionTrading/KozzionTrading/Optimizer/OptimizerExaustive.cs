using KozzionTrading.Market;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KozzionTrading.Optimizer
{
    public class OptimizerExaustive : IOptimizer
    {
        ParameterSpaceGrid SearchSpace;
        IEvaluator Evaluator;

        public OptimizerExaustive(ParameterSpaceGrid search_space, IEvaluator evaluator)
        {
            SearchSpace = search_space;
            Evaluator = evaluator;
        }

        public OptimizationResult Optimize(ParameterSet initial_guess)
        {
            HashSet<ParameterSet> black_list = new HashSet<ParameterSet>();
            Queue<ParameterSet> queue = new Queue<ParameterSet>();    

            ParameterSet best_set = initial_guess;
            double best_score = Evaluator.Evaluate(SearchSpace, initial_guess);
            black_list.Add(initial_guess);
            foreach (ParameterSet neighbor in SearchSpace.GetNeighborsArea(initial_guess))
            {
                queue.Enqueue(neighbor);
            }
            int count = 0;
            int reject_count = 0;
            Console.WriteLine(SearchSpace.Size);
            Console.WriteLine(SearchSpace);

            while (0 < queue.Count)
            {
                ParameterSet current = queue.Dequeue();
                black_list.Add(current);

                List<ParameterSet> neighbors = SearchSpace.GetNeighborsArea(initial_guess);
                foreach (ParameterSet neighbor in neighbors)
                {
                    if (!black_list.Contains(neighbor))
                    {
                        queue.Enqueue(neighbor);
                    }
                    else
                    {
                        reject_count++;
                    }
                }
                count++;
                double score = Evaluator.Evaluate(SearchSpace, current);
                Console.WriteLine(current);
                Console.WriteLine(score);
                Console.WriteLine(count);
                Console.WriteLine(reject_count);

            }


            return new OptimizationResult(SearchSpace, Evaluator.Results, best_set, best_score);
        }


    }
}
