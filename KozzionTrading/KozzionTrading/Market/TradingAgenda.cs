using System;
using System.Collections.Generic;

namespace KozzionTrading.Market
{
    public class TradingAgenda
    {
        DateTime LastTradingDate { get { return this.trading_dates[this.trading_dates.Count - 1]; } }
        List<DateTime> trading_dates;
        public TradingAgenda(List<DateTime> trading_dates)
        {
            this.trading_dates = new List<DateTime>(trading_dates);
            //TODO make sure its only dates
            this.trading_dates.Sort();
        }


        public DateTime GetFirstDateAfter(DateTime current)
        {
            for (int date_index = 0; date_index < trading_dates.Count; date_index++)
            {
                DateTime next = trading_dates[date_index];
                if (current < next)
                {
                    return next;
                }
            }
            return LastTradingDate;
        }


        public DateTime GetFirstDateAfterWithDayofWeek(DateTime current, DayOfWeek day_of_week)
        {
            for (int date_index = 0; date_index < trading_dates.Count; date_index++)
            {
                DateTime next = trading_dates[date_index];
                if ((current < next) && (next.DayOfWeek == day_of_week))
                {
                    return next;
                }
            }
            return LastTradingDate;
        }

        public DateTime GetFirstDateAfterAtLeastWithDayofWeek(DateTime current, TimeSpan time_span, DayOfWeek day_of_week)
        {
            return GetFirstDateAfterWithDayofWeek(current + time_span, day_of_week);
        }


        public DateTime GetLastDateInNextMonthWithDayofWeek(DateTime current, DayOfWeek day_of_week)
        {
            //DateTime month_start
            for (int date_index = 0; date_index < trading_dates.Count; date_index++)
            {
                DateTime next = trading_dates[date_index];
                if ((current < next) && (next.DayOfWeek == day_of_week))
                {
                    return next;
                }
            }
            return LastTradingDate;
        }
    }
}
