using KozzionCore.Tools;
using KozzionTrading.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KozzionTrading.Market
{
    public struct ForexFactoryEvent
    {
        //Direct
        public DateTimeUTC WeekStartDay { get; private set; }
        public string DateString { get; private set; }
        public string TimeString { get; private set; }
        public string Symbol { get; private set; }
        public string Impact { get; private set; }
        public string Description { get; private set; }
        public string Actual { get; private set; }
        public string Forecast { get; private set; }
        public string Previous { get; private set; }

        //Inferred
        public DateTimeUTC QueuryTimeStamp{ get; private set; }
        

        public ForexFactoryEvent(DateTimeUTC week_start_day, string date_string, string time_string, string symbol, string impact,  string description, string actual, string forecast, string previous)
        {
            WeekStartDay = week_start_day;
            DateString = date_string;
            TimeString = time_string;
            Symbol = symbol;
            Impact = impact;    
            Description = description;
            Actual = actual;
            Forecast = forecast;
            Previous = previous;
            QueuryTimeStamp = DateTimeUTC.Now;
        }
    }
}
