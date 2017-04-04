using KozzionCore.IO.CSV;
using KozzionMathematics.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KozzionTradingCL.Experiments
{
    public class ExperimentHistory : IExperiment

    {
        public void DoExperiment()
        {

            double commission = 7;
            double spread = 5;
            int lookAhead = 60;
            //Load data
            string[,] table = ToolsIOCSV.ReadCSVFile(@"D:\GoogleDrive\TestData\Trading\eurusddata.csv", Delimiter.Comma);

            //Convert data to values
            double[] ask = new double[table.GetLength(0)];
            double[] bid = new double[table.GetLength(0)];
            for (int i = 0; i < table.GetLength(0); i++)
            {
                
                bid[i] = 100000 * double.Parse(table[i, 2].Replace(".", ","));
                ask[i] = bid[i] + spread;

            }
            double mean1 = ToolsMathStatistics.Mean(ask);
            double mean2 = ToolsMathStatistics.Mean(bid);

            double[] buyCurve = new double[table.GetLength(0) - lookAhead];
            double[] sellCurve = new double[table.GetLength(0) - lookAhead];
            string[] buyCurveString = new string[buyCurve.Length];

            for (int i = 0; i < buyCurve.Length; i++)
            {
                double max = double.MinValue;
                double min = double.MaxValue;

                for (int j = 1; j < lookAhead; j++)
                {
                    max = Math.Max(max, bid[i + j]);
                    min = Math.Min(min, ask[i + j]);
                }

                buyCurve[i] = max - ask[i] - commission;
                buyCurveString[i] = bid[i] + ";" + buyCurve[i];
                sellCurve[i] = bid[i] - min - commission;
            }

            double mean3 = ToolsMathStatistics.Mean(buyCurve);
            double mean4 = ToolsMathStatistics.Mean(sellCurve);

            File.AppendAllLines(@"D:\GoogleDrive\TestData\Trading\buyCurveOut.csv", buyCurveString);
            /* 
             * Load data
             * for each time point find optimal buy order
             * create line of optimal buy orders
             */
        }
    }
}
