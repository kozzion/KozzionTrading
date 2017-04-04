using KozzionCore.IO.CSV;
using KozzionCore.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KozzionTrading.Tools
{
    public class DatabaseTradingCalender
    {
        private static string database_path = ToolsTradingDataSet.GetPath() + "DatabaseEvents//TradingCalender.csv";

        public DateTimeUTC StartDateTime  { get { return event_list[0].EventTimeUTC; } }
        public DateTimeUTC EndDateTime  { get{ return event_list[event_list.Count -1].EventTimeUTC.AddSeconds(1); }}
        private DateTimeUTC LastUpdateDateTime;

        private List<TradingCalenderEvent> event_list;

        public DatabaseTradingCalender()
        {
            event_list = new List<TradingCalenderEvent>();
            if (File.Exists(database_path))
            {
                LoadState();
            }
            else
            {
                LastUpdateDateTime = new DateTimeUTC(2000, 1, 1); //Before forex actory
            }
        }

  

        public void ClearData()
        {
            LastUpdateDateTime = new DateTimeUTC(2000, 1, 1); //Before forex actory
            event_list.Clear();
            SaveState();
        }

        public void UpdateData()
        {
            //TODO update other?
            UpdateForexFactory();
            LastUpdateDateTime = DateTimeUTC.Now;
            event_list.Sort();
            SaveState();
        }

        public void UpdateForexFactory()
        {
            List<DateTimeUTC> query_date_list = new List<DateTimeUTC>();
            if (LastUpdateDateTime < ToolsForexFactory.InitialDate)
            {
                LastUpdateDateTime = ToolsForexFactory.InitialDate;
            }
            DateTimeUTC current_time = DateTimeUTC.Now;

            query_date_list.Add(LastUpdateDateTime.GetDayEqualOrBefore(DayOfWeek.Monday));
            while (query_date_list[query_date_list.Count - 1] <= current_time)
            {
                query_date_list.Add(query_date_list[query_date_list.Count - 1].AddDays(7));
            }


            foreach (DateTimeUTC query_date in query_date_list)
            {
                List<TradingCalenderEvent> trading_calendar_event_list = ToolsForexFactory.GetTradingCalendarEvents(query_date);
                foreach (TradingCalenderEvent trading_calender_event in trading_calendar_event_list)
                {
                    event_list.Add(trading_calender_event);
                }
            }
        }

        public IList<TradingCalenderEvent> GetEvents(DateTimeUTC lower_inclusive, DateTimeUTC upper_exclusive)
        {
            return GetEvents(lower_inclusive, upper_exclusive, "", "", new Dictionary<string, string>());
        }
        public IList<TradingCalenderEvent> GetEvents(DateTimeUTC lower_inclusive, DateTimeUTC upper_exclusive, string required_symbol = "", string required_decription = "")
        {
            return GetEvents(lower_inclusive, upper_exclusive, required_symbol, required_decription, new Dictionary<string, string>());
        }

        public IList<TradingCalenderEvent> GetEvents(DateTimeUTC lower_inclusive, DateTimeUTC upper_exclusive, string symbol, string decription, Dictionary<string, string> required_tags)
        {
            List<TradingCalenderEvent> selected_event_list = new List<TradingCalenderEvent>();
            foreach (TradingCalenderEvent callender_event in event_list)
            {
                if (CheckMatch(callender_event, lower_inclusive, upper_exclusive, symbol, decription, required_tags))
                {
                    selected_event_list.Add(callender_event);
                }

            }
            return selected_event_list;
        }

        private bool CheckMatch(TradingCalenderEvent callender_event, DateTimeUTC lower_inclusive, DateTimeUTC upper_exclusive, string required_symbol, string required_decription, Dictionary<string, string> required_tags)
        {
            if ((callender_event.EventTimeUTC < lower_inclusive) || (upper_exclusive <= callender_event.EventTimeUTC ))
            {
                return false;
            }
            if (!required_symbol.Equals("") && !required_symbol.Equals(callender_event.Symbol))
            {
                return false;
            }

            if (!required_decription.Equals("") && !(callender_event.Description.Contains(required_decription)))
            {
                return false;
            }

            return true;
        }

        public void SaveState()
        {
            List<string[]> array_list = new List<string[]>();
            array_list.Add(new string[]
            {
                LastUpdateDateTime.ToString(),
                "",
                "",
                "",
                ""
            });

            foreach (TradingCalenderEvent trading_calender_event in event_list)
            {
                array_list.Add(new string[]
                {
                    trading_calender_event.EventTimeUTC.ToString(),
                    trading_calender_event.Symbol,
                    trading_calender_event.Description,
                    trading_calender_event.IsAllDay.ToString(),
                    ToolsString.DictionaryToString(trading_calender_event.Tags)
                });
            }


            ToolsIOCSV.WriteCSVFile(database_path,ToolsCollection.ConvertToArray2D(array_list));
        }

        public void LoadState()
        {
            string [,] table = ToolsIOCSV.ReadCSVFile(database_path);
            LastUpdateDateTime = DateTimeUTC.Parse(table[0, 0]);
            for (int index_0 = 1; index_0 < table.GetLength(0); index_0++)
            {
                event_list.Add(new TradingCalenderEvent(
                    DateTimeUTC.Parse(table[index_0, 0]),
                    table[index_0, 1],
                    table[index_0, 2],
                    bool.Parse(table[index_0, 3]),
                    ToolsString.StringToDictionary(table[index_0, 4])));
            }
        }
    }
}