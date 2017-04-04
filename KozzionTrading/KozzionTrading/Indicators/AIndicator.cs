using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KozzionTrading.Market;

namespace KozzionTrading.Indicators
{
    public abstract class AIndicator : IIndicator
    {

        public string Description { get; private set; }
        public int SubIndicatorCount { get { return sub_indicator_names.Count; } }

        private List<string> sub_indicator_names;
        public IList<string> SubIndicatorNames { get { return new List<string>(sub_indicator_names); } }

        public AIndicator(string description, IList<string> sub_indicator_names)
        {
            this.Description = description;
            this.sub_indicator_names = new List<string>(sub_indicator_names);
        }

        public virtual Tuple<double[,], bool[]> ComputeAll(IMarketModelIndicator market_model, int price_count)
        {
            double[,] values = new double[price_count, SubIndicatorCount];
            bool[] is_valid = new bool[price_count];
            double[] result_temp = new double[SubIndicatorCount];   

            for (int price_index = 0; price_index < price_count - 1; price_index++)
            {
                is_valid[price_index] = ComputeRBA(market_model, result_temp);
                values.Set1DIndex0(price_index, result_temp);
                market_model.StepSecond();      
            }
            is_valid[price_count - 1] = ComputeRBA(market_model, result_temp);
            values.Set1DIndex0(price_count - 1, result_temp);
            return new Tuple<double[,], bool[]>(values, is_valid);
        }

        public Tuple<double[], bool> Compute(IMarketModelIndicator market_model)
        {
            double[] result = new double[SubIndicatorCount];
            bool is_valid = ComputeRBA(market_model, result);
            return new Tuple<double[], bool>(result, is_valid);
        }

        public abstract bool ComputeRBA(IMarketModelIndicator market_model, double[] result);

  
    }
}
