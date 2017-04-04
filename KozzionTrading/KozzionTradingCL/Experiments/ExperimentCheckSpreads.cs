using KozzionCore.Collections;
using KozzionCore.IO.CSV;
using KozzionTrading.Market;
using KozzionTrading.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Globalization;

namespace KozzionTradingCL.Experiments
{
    public class ExperimentCheckSpreads : IExperiment
    {
        public void DoExperiment()
        {
            IReadOnlyList<PriceCandle>  candles = ToolsPrice.GetPriceCandles(ToolsPrice.DefaultSymbolUSDEUR, TimeScale.Second1);
            DictionaryCount<int> dic = new DictionaryCount<int>();

            foreach (PriceCandle candle in candles)
            {
                dic.Increment((int)Math.Round((candle.OpenAsk - candle.OpenBid) / TradingConstants.POINT));
            }
            List<int> keys = new List<int>(dic.Keys);
            keys.Sort();

            string[,] spread_table = new string[keys.Count, 2];
            for (int i = 0; i < keys.Count; i++)
            {
                Console.WriteLine(keys[i] + " " + dic[keys[i]]);
                spread_table[i, 0] = keys[i].ToString(CultureInfo.InvariantCulture);
                spread_table[i, 1] = dic[keys[i]].ToString(CultureInfo.InvariantCulture);
            }
            ToolsIOCSV.WriteCSVFile(@"D:\GoogleDrive\TestData\Trading\Spreads.csv", spread_table);
        }
    }
}
