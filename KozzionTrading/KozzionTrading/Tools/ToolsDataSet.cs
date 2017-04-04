using KozzionCore.IO.CSV;
using KozzionCore.Tools;
using KozzionMachineLearning.DataSet;
using KozzionTrading.Indicators;
using KozzionTrading.Market;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KozzionTrading.Tools
{


    public static class ToolsTradingDataSet
    {

        public static string GetPath()
        {
            string machine_name = Environment.MachineName;

            switch (machine_name)
            {
                case "DESKTOP-2O13C6P":
                    return @"D:\GoogleDrive\TestData\Trading\";
                case "DESKTOPJAAP2015":
                    return @"D:\GoogleDrive\TestData\Trading\";
                case "WM":
                    return @"C:\programming\trading\";
                default: throw new Exception("Unknown Machine Name: " + machine_name);
            }
        }

        public static DataSet<double, double> CreateDataSet(MarketModelSimulation market, IIndicator indicator_features)
        {
            return CreateDataSet(market, indicator_features, new IndicatorNull());
        }        

        public static DataSet<double, double> CreateDataSet(MarketModelSimulation market, IIndicator feature_indicator, IIndicator label_indicator)
        {   
            IList<double[]> feature_data = new List<double[]>();
            IList<bool[]> missing_data = new List<bool[]>();
            IList<double[]> label_data = new List<double[]>();

            AddIndicators(market, feature_indicator, label_indicator, feature_data, missing_data, label_data);
            while  (market.Second1.FutureCount != 0)
            {    
                market.StepSecond();
                AddIndicators(market, feature_indicator, label_indicator, feature_data, missing_data, label_data);
            }

            IDataContext data_context_labeled = new DataContext(DataLevel.INTERVAL, feature_indicator.SubIndicatorNames, DataLevel.INTERVAL, label_indicator.SubIndicatorNames);
            return new DataSet<double, double>(data_context_labeled, feature_data, missing_data, label_data);
        }

        private static void AddIndicators(MarketModelSimulation market, IIndicator feature_indicator, IIndicator label_indicators, IList<double[]> feature_data, IList<bool[]> missing_data, IList<double[]> label_data)
        {
            Tuple<double[], bool> tuple_features = feature_indicator.Compute(market);
            Tuple<double[], bool> tuple_labels = label_indicators.Compute(market);
            //Only add valid entries?
            if (tuple_features.Item2 && tuple_labels.Item2)
            {
                feature_data.Add(tuple_features.Item1);
                missing_data.Add(ToolsCollection.CreateArray(feature_indicator.SubIndicatorCount, false));
                label_data.Add(tuple_labels.Item1);
            }
        }

        //Layout: open, high, low, close, year, month, day_of_month, day of week, minute_of_day,
        public static void ExportDataSet(IList<PriceCandle> prices, DateTimeUTC first_day, DateTimeUTC last_day, string file_path)
        {



            IList<string[]> lines = new List<string[]>();
            foreach (PriceCandle price_candle in prices)
            {
                if ((first_day.Date <= price_candle.OpenTime.Date) && (price_candle.OpenTime.Date <= last_day.Date))
                {
                    string[] line = new string[9];
                    line[0] = price_candle.OpenBid.ToString(CultureInfo.InvariantCulture);
                    line[1] = price_candle.HighBid.ToString(CultureInfo.InvariantCulture);
                    line[2] = price_candle.LowBid.ToString(CultureInfo.InvariantCulture);
                    line[3] = price_candle.CloseBid.ToString(CultureInfo.InvariantCulture);
                    line[4] = price_candle.OpenTime.Year.ToString(CultureInfo.InvariantCulture);
                    line[5] = price_candle.OpenTime.Month.ToString(CultureInfo.InvariantCulture);
                    line[6] = price_candle.OpenTime.Day.ToString(CultureInfo.InvariantCulture);
                    line[7] = ((int)price_candle.OpenTime.DayOfWeek).ToString();
                    line[8] = ((price_candle.OpenTime.Hour * 60) + price_candle.OpenTime.Minute).ToString();
                    lines.Add(line);
                }
      
            }
            ToolsIOCSV.WriteCSVFile(file_path, ToolsCollection.ConvertToArray2D(lines));
        }
    }
}
