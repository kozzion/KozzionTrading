using KozzionCore.IO.CSV;
using KozzionCore.Tools;
using KozzionTrading.Indicators;
using KozzionTrading.Market;
using KozzionTrading.Tools;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KozzionTradingCL.Experiments
{
    public class ExperimentCreateDataSet : IExperiment
    {
        public void DoExperiment()
        {
            ToolsPrice.ComposeBinary(ToolsPrice.DefaultSymbolGBPUSD);
            PriceSet price_set = ToolsPrice.GetPriceSet(ToolsPrice.DefaultSymbolGBPUSD).SubSet(new DateTimeUTC(2016, 10, 17), new DateTimeUTC(2016, 10, 20));


            IIndicator indicator_feature = new IndicatorSuperBollinger();
            IIndicator indicator_label   = new IndicatorMagicProfit(60);
            MarketModelSimulation market0 = new MarketModelSimulation(10000, price_set);
            MarketModelSimulation market1 = new MarketModelSimulation(10000, price_set);
            double[] time = new double[price_set.Prices.Count];
            for (int price_index = 0; price_index < price_set.Prices.Count; price_index++)
            {
                time[price_index] = price_set.Prices[price_index].Time.Ticks;
            }

            Tuple<double[,], bool[]> tuple0 = indicator_feature.ComputeAll(market0, price_set.Second1.Count);
            Tuple<double[,], bool[]> tuple1 = indicator_label.ComputeAll(market1, price_set.Second1.Count);

            List<string[]> list_string = new List<string[]>();
            for (int index_0 = 0; index_0 < tuple0.Item2.Length; index_0++)
            {
                if (tuple0.Item2[index_0] && tuple1.Item2[index_0])
                {

                    double[] array_double = ToolsCollection.Append(tuple0.Item1.Select1DIndex0(index_0), tuple1.Item1.Select1DIndex0(index_0));
                    string[] array_string = new string[array_double.Length];
                    for (int index_1 = 0; index_1 < array_double.Length; index_1++)
                    {
                        array_string[index_1] = array_double[index_1].ToString(CultureInfo.InvariantCulture);
                    }
                    list_string.Add(array_string);
                }
            }

            ToolsIOCSV.WriteCSVFile(ToolsTradingDataSet.GetPath() + "data.csv", ToolsCollection.ConvertToArray2D(list_string));


        }
    }
}
