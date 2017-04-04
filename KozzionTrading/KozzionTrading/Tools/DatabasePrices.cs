using KozzionCore.IO.CSV;
using KozzionCore.Tools;
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
    public class DatabasePrices
    {
        private static string database_path = Path.Combine(ToolsTradingDataSet.GetPath(),"DatabasePrices");
        private static string dump_path = Path.Combine(ToolsTradingDataSet.GetPath(), "History", "Dump");

        private Dictionary<TradingSymbol, Dictionary<int, PriceSet>> data_loaded;


        public DatabasePrices()
        {
            data_loaded = new Dictionary<TradingSymbol, Dictionary<int, PriceSet>>();
        }

        public void UnloadData()
        {
            data_loaded.Clear();
        }

        public void ClearData()
        {
            //TODO remove all folders
            
        }

        public void UpdateData()
        {
            UpdateSecondData();
            //UpdateHistoricData();
        }

        private void UpdateSecondData()
        {
            string[] dump_directories = Directory.GetDirectories(dump_path);
            //foreach (string dump_directory in dump_directories)
            //{


            //Dictionary<TradingSymbol, List<PriceSet>> data_read = new Dictionary<TradingSymbol, List<PriceSet>>();
            foreach (string dump_directory_name in dump_directories)
            {
                string[] dump_file_names = Directory.GetFiles(dump_directory_name);
                foreach (string dump_file_name in dump_file_names)
                {
                    if (!dump_file_name.Equals("desktop.ini"))
                    {
                        string symbol = Path.GetFileName(dump_file_name).Substring(0, 6);
                        if (symbol.Equals("EURUSD"))
                        {
                            Console.WriteLine("Reading" + dump_file_name);
                            PriceSet new_prices_set = ImportSecondData(dump_file_name);
                            Console.WriteLine("prices_set" + new_prices_set);


                            //TODO for each month in price set
                            IList<DateTimeUTC> month_date_list = CreateMonthRange(new_prices_set.OpenDateTime, new_prices_set.CloseDateTime);
                            foreach (DateTimeUTC month_date in month_date_list)
                            {
                                int year = new_prices_set.OpenDateTime.Year;
                                int month = new_prices_set.OpenDateTime.Month;

                                PriceSet prices_set = GetPrices(new_prices_set.Symbol, year, month);
                                prices_set.UpdatePrices(new_prices_set);                              
                            }
                        }
                    }
                }

            }
            SaveAllPrices();
        }

        private void SaveAllPrices()
        {
            foreach (TradingSymbol symbol in data_loaded.Keys)
            {
                foreach (int index in data_loaded[symbol].Keys)
                {
                    int year = index / 100;
                    int month = index % 100;
                    SavePrices(symbol, year, month);
                }                
            }
        }

        private IList<DateTimeUTC> CreateMonthRange(DateTimeUTC open, DateTimeUTC final_data_time)
        {
            open = new DateTimeUTC(open.Year, open.Month, 1);
            List<DateTimeUTC> months = new List<DateTimeUTC>();
            while (open < final_data_time)
            {
                months.Add(open);
                open = open.AddMonths(1);
            }
            return months;
        }

        private PriceSet ImportSecondData(string file_path)
        {
            string symbol = Path.GetFileName(file_path).Substring(0, 6);
            TradingSymbol trading_symbol = new TradingSymbol("TradersWay", "MT4.VAR.DEMO", symbol, "The one we mine on the VM");
            List<Price> price_list = new List<Price>();
            string[,] table = ToolsIOCSV.ReadCSVFile(file_path);
            for (int index_0 = 0; index_0 < table.GetLength(0); index_0++)
            {
                //TODO duplicate date_times can exist both on the server and on the client
                DateTimeUTC date_time = ToolsTime.UnixTimeStampToDateTimeUTC(int.Parse(table[index_0, 0]));
                double bid = double.Parse(table[index_0, 2], CultureInfo.InvariantCulture);
                double ask = double.Parse(table[index_0, 3], CultureInfo.InvariantCulture);
                price_list.Add(new Price(date_time, bid, ask));
            }

           return new PriceSet(trading_symbol, price_list);
        }

        public PriceSet GetPrices(TradingSymbol symbol, DateTimeUTC lower_inclusive, DateTimeUTC upper_exclusive)
        {
            List<PriceSet> month_price_set_list = new List<PriceSet>();
            //Move through months   
            DateTimeUTC initial_month = new DateTimeUTC(lower_inclusive.Year, lower_inclusive.Month, 1);
            int year = 0;
            int month = 0; 
            PriceSet price_set =  GetPrices(symbol, year, month);
            return new PriceSet(month_price_set_list);// new PriceSet(List<PriceSet> month_price_set_list)
        }

        public PriceSet GetPrices(TradingSymbol symbol, int year, int month)
        {
            int price_set_index = GetPriceSetIndex(year, month);
            string price_set_file_path = GetPriceSetFilePath(symbol, year, month);
            if (!data_loaded.ContainsKey(symbol))
            {
                data_loaded[symbol] = new Dictionary<int, PriceSet>();
            }

            if (data_loaded[symbol].ContainsKey(price_set_index))
            {
                return data_loaded[symbol][price_set_index];
            }
            else
            {
               
                if (File.Exists(price_set_file_path))
                {
                    data_loaded[symbol][price_set_index] = PriceSet.Read(new BinaryReader(File.Open(price_set_file_path, FileMode.Open)));
                }
                else
                {
                    data_loaded[symbol][price_set_index] = new PriceSet(symbol, new DateTimeUTC(year, month, 1), new DateTimeUTC(year, month + 1, 1));
                }

    
                return data_loaded[symbol][price_set_index];
            } 
        }

        public void SavePrices(TradingSymbol symbol, int year, int month)
        {
            int price_set_index = GetPriceSetIndex(year, month);
            string price_set_file_path = GetPriceSetFilePath(symbol, year, month);
            if (!data_loaded.ContainsKey(symbol))
            {
                throw new Exception("Cant save this data");
            }

            if (!data_loaded[symbol].ContainsKey(price_set_index))
            {
                throw new Exception("Cant save unloaded data");
            }

            if (!Directory.GetParent(price_set_file_path).Exists)
            {
                Directory.CreateDirectory(Directory.GetParent(price_set_file_path).ToString());
            }

            using (BinaryWriter writer = new BinaryWriter(File.Open(price_set_file_path, FileMode.OpenOrCreate)))
            {
                data_loaded[symbol][price_set_index].Write(writer);
            }
        }

        private int GetPriceSetIndex(int year, int month)
        {
            return (year * 100) + month;
        }

        private string GetPriceSetFilePath(TradingSymbol symbol, int year, int month)
        {
            return Path.Combine(database_path, symbol.ToString(), year + month.ToString("00") + ".dat");
        }
    }
}
