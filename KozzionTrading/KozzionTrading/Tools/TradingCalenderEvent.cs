using KozzionCore.Tools;
using System;
using System.Collections.Generic;

namespace KozzionTrading.Tools
{
    public class TradingCalenderEvent : IComparable<TradingCalenderEvent>
    {
        public static string TagIllegalDateTime = "IllegalDatime";
        public static string TagImpact = "Impact";
        public static string TagActual = "Actual";
        public static string TagForecast = "Forecast";
        public static string TagPrevious = "Previous";

        public DateTimeUTC EventTimeUTC { get; private set; }
        public string Symbol { get; private set; }
        public string Description { get; private set; }
        public bool IsAllDay { get; private set; }
        public Dictionary<string, string> Tags { get; private set; }

        public TradingCalenderEvent(DateTimeUTC event_time_utc, string symbol, string description, bool is_all_day,Dictionary<string, string> tags)
        {
            EventTimeUTC = event_time_utc;
            Symbol = symbol;
            Description = description;
            IsAllDay = is_all_day;
            Tags = tags;
        }

        public int CompareTo(TradingCalenderEvent other)
        {
            return EventTimeUTC.CompareTo(other.EventTimeUTC);
        }


 
    }
}