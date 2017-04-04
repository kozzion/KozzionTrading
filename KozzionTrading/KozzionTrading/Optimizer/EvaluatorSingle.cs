using KozzionTrading.Market;
using KozzionTrading.Optimizer;
using KozzionTrading.Policy;
using System;
using System.Collections.Generic;

namespace KozzionTrading.Optimizer
{
    public class EvaluatorSingle : IEvaluator
    {
        private IPolicyTemplate PolicyTemplate;
        private double initial_cash;
        private PriceSet price_set;
        private Dictionary<ParameterSet, double> results;
        public Dictionary<ParameterSet, double> Results
        {
            get
            {
                return new Dictionary<ParameterSet, double>(results);
            }
        }

        public EvaluatorSingle(IPolicyTemplate policy_template, double initial_cash, PriceSet price_set) 
        {
            this.PolicyTemplate = policy_template;
            this.initial_cash = initial_cash;
            this.price_set = price_set;
            this.results = new Dictionary<ParameterSet, double>();
        }

        public double Evaluate(ParameterSpaceGrid search_grid, ParameterSet parameter_set)
        {
            if (results.ContainsKey(parameter_set))
            {
                return results[parameter_set];
            }
            else
            {
                MarketManagerSimulation exchange = new MarketManagerSimulation(initial_cash, price_set);
                MarketResult result = exchange.Run(PolicyTemplate.Instance(parameter_set));
                return result.EndCash;
            }
        }
    }
}
