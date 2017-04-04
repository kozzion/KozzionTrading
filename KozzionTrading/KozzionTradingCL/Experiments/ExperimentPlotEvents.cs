using KozzionCore.IO.CSV;
using KozzionCore.Tools;
using KozzionTrading.Market;
using KozzionTrading.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KozzionTradingCL.Experiments
{
    public class ExperimentPlotEvents : IExperiment
    {
        DateTimeUTC start_date;
        DateTimeUTC end_date;

        public ExperimentPlotEvents(DateTimeUTC start_date, DateTimeUTC end_date)
        {
            this.start_date = start_date;
            this.end_date = end_date;
        }


        public ExperimentPlotEvents()
            :this(new DateTimeUTC(2015, 1, 1), new DateTimeUTC(2016, 1, 1))
        {
        }

        public void DoExperiment()
        {
            string [,] event_table = ToolsIOCSV.ReadCSVFile(@"D:\GoogleDrive\TestData\Trading\events.csv", Delimiter.SemiColon);
            //Load events
            //ToolsTradingCalendarEvent.GetTradingCalendarEvents(new string[] { "EUR", "USD" }, start_date, end_date);
            //Load candles
            ToolsPrice.GetPriceCandles("EUR_USD", TimeScale.Minute1, start_date, end_date, 0.07);
            //
        }
    }
}
