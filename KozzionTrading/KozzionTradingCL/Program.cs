using KozzionCore.IO.CSV;
using KozzionCore.Tools;
using KozzionMachineLearning.DataSet;
using KozzionPlotting.Tools;
using KozzionTrading.Indicators;
using KozzionTrading.IO;
using KozzionTrading.Market;
using KozzionTrading.Policy;
using KozzionTrading.Tools;
using KozzionTradingCL.Experiments;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KozzionTradingCL
{
    class Program
    {
        static void Main(string[] args)
        {
            DatabasePrices database_prices = new DatabasePrices();
            database_prices.UpdateData();
            //List all nfp events
            //DatabaseTradingCalender calendar = new DatabaseTradingCalender();
            //IList<TradingCalenderEvent>  events = calendar.GetEvents(calendar.StartDateTime, calendar.EndDateTime, "USD", "Farm Employment Change");





            //calendar.ClearUpdate();
            //calendar.Update();
            //List<ForexFactoryEvent> events = ToolsForexFactory.GetForexFactoryEvents(ToolsForexFactory.default_date);
            //new ExperimentPlotEventNFP().DoExperiment();
            //new ExperimentRunBusClient().DoExperiment();
            //new ExperimentCreateDataSet().DoExperiment();
            //new ExperimentPolicyJaapBands0().DoExperiment();
            //new ExperimentOptimize().DoExperiment();
            Console.WriteLine("Done");
            Console.Read();
            //new ExperimentPolicyJaapBands1().DoExperiment();
        }


    



        private static void CreateEventCSV()
        {
            // read CSV
            string[,] source = ToolsIOCSV.ReadCSVFile(ToolsTradingDataSet.GetPath() + "events.csv", Delimiter.SemiColon);

            //Allocate new array
            List<string[]> result_list = new List<string[]>();

            string[] exclusion = new string[] { "day", "Day", "Data" };
            //For each other column
            for (int index_0 = 0; index_0 < source.GetLength(0); index_0++)
            {
                if (!ExtensionsString.ContainsAny(source[index_0, 2], exclusion))
                {
                    string[] result = new string[3]; // Date symbol impact


                    //Find dat
                    DateTime week_start = DateTime.Parse(source[index_0, 0]);
                    int year = week_start.Year;
                    int month = ToolsForexFactory.MonthToInt(source[index_0, 1].Split(' ')[0]);
                    int day = int.Parse(source[index_0, 1].Split(' ')[1]);

                    if ((week_start.Month == 12) && (month == 1))
                    {
                        year++;
                    }

                    int hour = int.Parse(source[index_0, 2].Split(':')[0]);
                    int minute = int.Parse(source[index_0, 2].Split(':')[1].Substring(0, 2));

                    if (source[index_0, 2].Contains("pm"))
                    {
                        hour = hour + 12;
                    }
                    if (hour == 24)
                    {
                        hour = 0;
                        DateTime date_time = new DateTime(year, month, day, hour, minute, 0);
                        date_time = date_time.AddDays(1);
                        result[0] = ToolsTime.DateTimeToUnixTimestampInt32(date_time).ToString();
                    }
                    else
                    {
                        DateTime date_time = new DateTime(year, month, day, hour, minute, 0);
                        result[0] = ToolsTime.DateTimeToUnixTimestampInt32(date_time).ToString();
                    }

                    result[1] = source[index_0, 3];
                    switch (source[index_0, 4])
                    {
                        case "Non-Economic":
                            result[2] = "0"; // Impacs       
                            break;

                        case "Low Impact Expected":
                            result[2] = "1"; // Impacs       
                            break;

                        case "Medium Impact Expected":
                            result[2] = "2"; // Impacs       
                            break;

                        case "High Impact Expected":
                            result[2] = "3"; // Impacs       
                            break;

                        default:
                            throw new Exception("Unknown impact");

                    }
                    result_list.Add(result);
                }
            }
            ToolsIOCSV.WriteCSVFile(ToolsTradingDataSet.GetPath() + "events_small.csv", ToolsCollection.ConvertToArray2D(result_list));
        }


    }
}
