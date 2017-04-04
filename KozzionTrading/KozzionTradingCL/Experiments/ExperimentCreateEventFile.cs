using KozzionCore.IO.CSV;
using KozzionCore.Tools;
using KozzionTrading.Market;
using KozzionTrading.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KozzionTradingCL.Experiments
{
    public class ExperimentCreateEventFile : IExperiment
    {
        public void DoExperiment()
        {
            DateTimeUTC week_start = new DateTimeUTC(2007, 1, 1);
            List<ForexFactoryEvent> all_event_list = new List<ForexFactoryEvent>();
            while (week_start < DateTimeUTC.Now)
            {
                Console.WriteLine(week_start);
                all_event_list.AddRange(ToolsForexFactory.GetForexFactoryEvents(week_start));
                week_start = week_start.AddDays(7);
                Thread.Sleep(1000);

            }
            string[,] table = new string[all_event_list.Count, 9];
            for (int index = 0; index < all_event_list.Count; index++)
            {
                table[index, 0] = all_event_list[index].WeekStartDay.ToString();
                table[index, 1] = all_event_list[index].DateString;
                table[index, 2] = all_event_list[index].TimeString;
                table[index, 3] = all_event_list[index].Symbol;
                table[index, 4] = all_event_list[index].Impact;
                table[index, 5] = all_event_list[index].Description;
                table[index, 6] = all_event_list[index].Actual;
                table[index, 7] = all_event_list[index].Forecast;
                table[index, 8] = all_event_list[index].Previous;
            }
            ToolsIOCSV.WriteCSVFile(@"D:\GoogleDrive\TestData\Trading\events.csv", table, Delimiter.SemiColon);

        }
    }
}
