using KozzionCore.IO.CSV;
using KozzionCore.Tools;
using KozzionTrading.IO;
using KozzionTrading.Market;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KozzionTrading.Tools
{
    public static class ToolsPrice
    {
        public static TradingSymbol DefaultSymbolEURUSD = new TradingSymbol("TradersWay", "MT4.VAR.DEMO", "USDEUR", "The one we mine on the VM");
        public static TradingSymbol DefaultSymbolUSDEUR = new TradingSymbol("TradersWay", "MT4.VAR.DEMO", "USDEUR", "The one we mine on the VM");
        public static TradingSymbol DefaultSymbolGBPUSD = new TradingSymbol("TradersWay", "MT4.VAR.DEMO", "GBPUSD", "The one we mine on the VM");

        private static string merged_data_root_path = ToolsTradingDataSet.GetPath() + @"History\All\";
        private static string binary_data_root_path = ToolsTradingDataSet.GetPath() + @"History\Binary\";
        private static string source_data_root_path = ToolsTradingDataSet.GetPath() + @"History\Source\";

        public static void ComposeSource()
        {
            string[] file_paths = Directory.GetFiles(merged_data_root_path);
            foreach (string source_path in file_paths)
            {
                string file_name = Path.GetFileName(source_path);
                string symbol = file_name.Substring(0, 6);
                string target_dir = Path.Combine(binary_data_root_path, "TradersWay", "MT4.VAR.DEMO", symbol);
                string target_path = Path.Combine(binary_data_root_path, "TradersWay", "MT4.VAR.DEMO", symbol, file_name);
                if (!Directory.Exists(target_dir))
                {
                    Directory.CreateDirectory(target_dir);
                }

                if (File.Exists(target_path))
                {
                    throw new Exception("Duplicate: " + target_path);
                }
                else
                {
                    File.Move(source_path, target_path);
                }
            }
        }

        public static void ComposeBinary(TradingSymbol trading_symbol)
        {
            string source_data_path = Path.Combine(source_data_root_path, trading_symbol.Broker, trading_symbol.Account, trading_symbol.Symbol);
            string target_data_path = Path.Combine(binary_data_root_path, trading_symbol.Broker, trading_symbol.Account, trading_symbol.Symbol);

            if (!Directory.Exists(source_data_path))
            {
                throw new Exception("No data for trading symbol: " + trading_symbol);
            }
            string[] files = Directory.GetFiles(source_data_path);
            if (files.Length == 0)
            {
                throw new Exception("No data for trading symbol: " + trading_symbol);
            }
            Array.Sort(files);

            List<Price> price_list = new List<Price>();
            foreach (string file in files)
            {
                AddFileToPriceList(price_list, file);
            }
            PriceSet price_set = new PriceSet(ToolsPrice.DefaultSymbolGBPUSD, price_list);
            DateTimeUTC date_time_first = price_set.OpenDateTime;
            DateTimeUTC date_time_last = price_set.CloseDateTime;

            target_data_path = Path.Combine(target_data_path, date_time_first.ToString("yyyyMMddHHmmss") + "_" + date_time_last.ToString("yyyyMMddHHmmss") + ".blb");

            string target_data_path_dir = Path.GetDirectoryName(target_data_path);
            if (!Directory.Exists(target_data_path_dir))
            {
                Directory.CreateDirectory(target_data_path_dir);
            }

            using (BinaryWriter writer = new BinaryWriter(new FileStream(target_data_path, FileMode.Create)))
            {
                price_set.Write(writer);
            }
        }

        public static void AddFileToPriceList(List<Price> price_list, string file)
        {
            string[,] table = ToolsIOCSV.ReadCSVFile(file);
            for (int index_0 = 0; index_0 < table.GetLength(0); index_0++)
            {
                //TODO duplicate date_times can exist both on the server and on the client
                DateTimeUTC date_time = ToolsTime.UnixTimeStampToDateTimeUTC(int.Parse(table[index_0, 1]));
                double bid = double.Parse(table[index_0, 2], CultureInfo.InvariantCulture);
                double ask = double.Parse(table[index_0, 3], CultureInfo.InvariantCulture);
                price_list.Add(new Price(date_time, bid, ask));
            }
        }

        public static PriceSet GetPriceSet(TradingSymbol trading_symbol)
        {
            string data_path = Path.Combine(binary_data_root_path, trading_symbol.Broker, trading_symbol.Account, trading_symbol.Symbol);
            if (!Directory.Exists(data_path))
            {
                throw new Exception("No data for trading symbol: " + trading_symbol);
            }
            string[] files = Directory.GetFiles(data_path);
            if (files.Length != 1)
            {
                throw new Exception("No unique data for trading symbol: " + trading_symbol);
            }

            using (BinaryReader reader = new BinaryReader(new FileStream(files[0], FileMode.Open)))
            {
                return PriceSet.Read(reader);
            }
        }


        public static IReadOnlyList<PriceCandle> GetPriceCandles(TradingSymbol trading_symbol, TimeScale time_scale)
        {

            string data_path = Path.Combine(binary_data_root_path, trading_symbol.Broker, trading_symbol.Account, trading_symbol.Symbol, ToolsEnum.EnumToString(time_scale));
            if (!Directory.Exists(data_path))
            {
                throw new Exception("No data for trading symbol: " + trading_symbol);
            }
            string[] files = Directory.GetFiles(data_path);
            if (files.Length == 0)
            {
                throw new Exception("No data for trading symbol: " + trading_symbol);
            }

            using (BinaryReader reader = new BinaryReader(new FileStream(files[0], FileMode.Open)))
            {
                int count = reader.ReadInt32();
                List<PriceCandle> candles = new List<PriceCandle>(count);
                for (int candle_index = 0; candle_index < count; candle_index++)
                {
                    candles.Add(PriceCandle.Read(reader));
                }
                return candles;
            }
        }

        public static IReadOnlyList<Price> GetPriceList(TradingSymbol trading_symbol)
        {
            string file_path = Path.Combine(binary_data_root_path, trading_symbol.Broker, trading_symbol.Account, trading_symbol.Symbol, "Prices.bin");
            if (!File.Exists(file_path))
            {
                throw new Exception("No data for trading symbol: " + trading_symbol);
            }
            using (BinaryReader reader = new BinaryReader(new FileStream(file_path, FileMode.Open)))
            {
                int count = reader.ReadInt32();
                List<Price> price_list = new List<Price>(count);
                for (int candle_index = 0; candle_index < count; candle_index++)
                {
                    price_list.Add(Price.Read(reader));
                }
                return price_list;
            }
        }

        public static List<PriceCandle> GetPriceCandles(TradingSymbol trading_symbol, TimeScale time_scale, DateTime start_inclusive, DateTime end_exclusive)
        {
            string data_path = Path.Combine(binary_data_root_path, trading_symbol.Broker, trading_symbol.Account, trading_symbol.Symbol, ToolsEnum.EnumToString(time_scale));
            if (!Directory.Exists(data_path))
            {
                throw new Exception("No data for trading symbol: " + trading_symbol);
            }
            string[] files = Directory.GetFiles(data_path);
            if (files.Length == 0)
            {
                throw new Exception("No data for trading symbol: " + trading_symbol);
            }

            return ToolsIOSerialization.SerializeFromFile<List<PriceCandle>>(files[0]);
        }


        public static List<PriceCandle> GetPriceCandles()
        {
            return GetPriceCandles("EUR_USD", TimeScale.Minute1, new DateTimeUTC(2000, 1, 1), new DateTimeUTC(2016, 9, 3), 0.01);
        }

        public static List<PriceCandle> GetPriceCandles(string symbol, TimeScale time_scale, DateTimeUTC start_inclusive, DateTimeUTC end_exclusive, double spread)
        {
            if (symbol != "EUR_USD")
            {
                //TODO include other and reverse symbols
                throw new Exception("Data Unavaileble");
            }

            if (time_scale != TimeScale.Minute1)
            {
                //TODO build other time scales
                throw new Exception("Data Unavaileble");
            }


            //if ((new DateTime(2000, 1, 1) < start_inclusive) || (end_exclusive <=  new DateTime(2016, 9, 3)))
            //{
            //    //TODO make correct
            //    throw new Exception("Data Unavaileble");
            //}

            List<PriceCandle> selected_data = new List<PriceCandle>();
            List<PriceCandle> all_data = ToolsIOHST.ReadFileHST(@"D:\GoogleDrive\TestData\Trading\Simulation\EURUSD1.hst", spread); // File for 

            foreach (var item in all_data)
            {
                if ((start_inclusive <= item.OpenTime) && (item.OpenTime < end_exclusive))
                {
                    selected_data.Add(item);
                }
            }
            return selected_data;
        }


    }
}
