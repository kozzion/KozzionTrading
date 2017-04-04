using KozzionCore.IO.CSV;
using KozzionMathematics.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KozzionTradingCL.Experiments
{
    public class ExperimentOptimal : IExperiment
    {
        public void DoExperiment()
        {
            double commission = 7;
            //Load data
            string [,] table = ToolsIOCSV.ReadCSVFile(@"D:\GoogleDrive\TestData\Trading\spreads.csv", Delimiter.SemiColon);

            //Convert data to values
            double[] ask = new double [table.GetLength(0)];
            double[] bid = new double [table.GetLength(0)];
            for (int i = 0; i < table.GetLength(0); i++)
            {
                ask[i] = 10000 * double.Parse(table[i, 3].Replace(".",","));
                bid[i] = 10000 * double.Parse(table[i, 2].Replace(".", ","));

            }
            double mean1 = ToolsMathStatistics.Mean(ask);
            double mean2  = ToolsMathStatistics.Mean(bid);

            double[] buyCurve = new double[table.GetLength(0) - 3600];
            double[] sellCurve = new double[table.GetLength(0) - 3600];

            for (int i = 0; i < buyCurve.Length; i++)
            {
                double max = double.MinValue;
                double min = double.MaxValue;

                for (int j = 1; j < 3600; j++)
                {
                    max = Math.Max(max, bid[i + j]);
                    min = Math.Min(min, ask[i + j]);
                }

                buyCurve[i] = max - ask[i] - commission;
                sellCurve[i] = bid[i] - min  - commission;
            }

            double mean3 = ToolsMathStatistics.Mean(buyCurve);
            double mean4 = ToolsMathStatistics.Mean(sellCurve);
            /* 
             * Load data
             * for each time point find optimal buy order
             * create line of optimal buy orders
             */

        }
    }
}
