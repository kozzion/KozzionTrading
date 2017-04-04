using KozzionMathematics.Function;
using KozzionTrading.Market;
using System;

namespace KozzionTrading.Policy
{
    public class PolicyExpectation :IPolicy
    {
        private string symbol;
        private DayOfWeek day_of_week;
        private IFunction<Tuple<MarketModelSimulation, double>, double> expectation_function;
        private float[] raster;
        private float threshold_buy;
        private float threshold_sell;
        public string Title
        {
            get { return "PolicyExpectation";  }
        }

        public PolicyExpectation(string symbol, DayOfWeek day_of_week, IFunction<Tuple<MarketModelSimulation, double>, double> expectation_function, float[] raster, float threshold_buy, float threshold_sell) 
        {
            this.symbol = symbol;
            this.day_of_week = day_of_week;
            this.expectation_function = expectation_function;
            this.raster = raster;
            this.threshold_buy  = threshold_buy;
            this.threshold_sell = threshold_sell;
        }

        public void GetTradeOrderCommand(IMarketModelSimulation market_model)
        {
            //float expected_value = 0;
            //float desnisty_sum = 0;
            //for (int raster_index = 0; raster_index < this.raster.Length; raster_index++)
            //{
            //    float expected_density =  this.expectation_function.Compute(new Tuple<MarketModel,float>(data_set_slice, this.raster[raster_index]));
            //    desnisty_sum += expected_density;
            //    expected_value += this.raster[raster_index] * expected_density;
            //}
            //expected_value  /= desnisty_sum;
            //if(this.threshold_buy < expected_value)
            //{
            //    return new TradeOrderCommand(this.symbol, TradeOrderType.Long);
            //}
            //else if(expected_value < threshold_sell)
            //{
            //    return new TradeOrderCommand(this.symbol, TradeOrderType.Short);
            //}
            //else
            //{
            //    return new TradeOrderCommand(this.symbol, TradeOrderType.Hold);
            //}
        }

        public DateTime GetNextDateTime(DateTime current, MarketModelSimulation data_set)
        {
            DateTime previous = current;
            if (previous.Month == 1) //december
            {
                previous = new DateTime(current.Year - 1, 12, 1);
            }
            else
            {
                previous = new DateTime(current.Year, current.Month - 1, 1);
            }
            while (previous.DayOfWeek != this.day_of_week)
            {
                previous = previous.AddDays(1);
            }
            return previous;
        }
    }
}
