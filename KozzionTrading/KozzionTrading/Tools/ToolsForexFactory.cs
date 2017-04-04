using KozzionCore.Tools;
using KozzionTrading.Market;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace KozzionTrading.Tools
{

    public class ToolsForexFactory
    {
        //public static string example_url = "http://www.forexfactory.com/calendar.php?week=sep11.2016";

        public static DateTimeUTC InitialDate = new DateTimeUTC(2007, 1, 1);

        private static string url = "http://www.forexfactory.com/calendar.php";
        private static TimeZoneInfo time_zone_est = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

        public static List<TradingCalenderEvent> GetTradingCalendarEvents(DateTimeUTC week_start)
        {
            List<TradingCalenderEvent> trading_callender_event_list = new List<TradingCalenderEvent>();
            List<ForexFactoryEvent> forex_factory_event_list =  GetForexFactoryEvents(week_start);
            foreach (ForexFactoryEvent forex_factory_event in forex_factory_event_list)
            {
                DateTime event_time_local = DateTime.Now;
                bool is_all_day = false;

                if (!forex_factory_event.TimeString.Contains(":"))
                {
                    is_all_day = true;
                }

                int year = forex_factory_event.WeekStartDay.Year;
                int month = MonthToInt(forex_factory_event.DateString.Split(' ')[0]);
                int day = int.Parse(forex_factory_event.DateString.Split(' ')[1]);

                if (is_all_day)
                {
                    event_time_local = new DateTime(year, month, day, 0, 0, 0);
                }
                else
                {
                    int hour= int.Parse(forex_factory_event.TimeString.Split(':')[0]);
                    int minutes = int.Parse(forex_factory_event.TimeString.Split(':')[1].Substring(0,2));
                    if (forex_factory_event.TimeString.Split(':')[1].Substring(2).Equals("pm"))
                    {
                        hour += 12;
                    }
                    if (24 <= hour)
                    {
                        event_time_local = new DateTime(year, month, day, hour - 24, minutes, 0);
                        event_time_local.AddDays(1);
                    }
                    else
                    {
                        event_time_local = new DateTime(year, month, day, hour, minutes, 0);
                    }

               
                }
                Dictionary<string, string> tags = new Dictionary<string, string>();
                tags.Add(TradingCalenderEvent.TagImpact, forex_factory_event.Impact);
                tags.Add(TradingCalenderEvent.TagActual, forex_factory_event.Actual);
                tags.Add(TradingCalenderEvent.TagPrevious, forex_factory_event.Previous);
                tags.Add(TradingCalenderEvent.TagForecast, forex_factory_event.Forecast);




                //Here is some daylight savings time bullshit
                //if ("Daylight Saving Time Shift".Equals(forex_factory_event.Description))
               // {
               //     event_time_local = event_time_local.AddSeconds(-1);
                //}

                try
                {
                    TimeZoneInfo.ConvertTimeToUtc(event_time_local, time_zone_est);
                }
                catch (Exception)
                {
                    Console.WriteLine("Illigal Date time");
                    event_time_local = event_time_local.AddHours(1);
                    tags.Add(TradingCalenderEvent.TagIllegalDateTime, TradingCalenderEvent.TagIllegalDateTime);
                }
                DateTimeUTC event_time_utc = new DateTimeUTC(event_time_local, time_zone_est);

                if (event_time_utc.GetDayEqualOrBefore(DayOfWeek.Monday).Year < event_time_utc.Year)
                {
                    event_time_utc = event_time_utc.AddYears(1);
                }


                trading_callender_event_list.Add(new TradingCalenderEvent(
                    event_time_utc, 
                    forex_factory_event.Symbol, 
                    forex_factory_event.Description,
                    is_all_day,
                    tags));

            }
            return trading_callender_event_list;
        }

 

        public static List<ForexFactoryEvent> GetForexFactoryEvents(DateTimeUTC week_start)
        {
            List<string> day_html_list = GetTradingCalendarHTMLDays(week_start);
            List<ForexFactoryEvent> event_list = new List<ForexFactoryEvent>();
            foreach (string day_html in day_html_list)
            {
                Tuple<string, List<string>> tuple = GetTradingCalendarEventsHTML(day_html);
                foreach (string event_html in tuple.Item2)
                {
                    ParceEventRBA(week_start, tuple.Item1, event_html, event_list);
                }
            }
            return event_list;
        }

        public static string GetTradingCalendarHTMLLine(DateTimeUTC week_start)
        {

            List<string> lines = GetTradingCalendarHTMLLines(week_start);
            foreach (string line in lines)
            {
                if (line.Contains("calendar__row calendar__row--day-breaker"))
                {
                    return line;
                }
            }
            throw new Exception("html parce failure:  table not found");
        }
        public static List<string> GetTradingCalendarHTMLLines(DateTimeUTC week_start)
        {
            if (week_start.DayOfWeek != DayOfWeek.Monday)
            {
                throw new Exception("Day is not a monday");
            }



     


            string url = "http://www.forexfactory.com/calendar.php?week=" + IntToMonth(week_start.Month) + week_start.Day + "." + week_start.Year;
            Console.WriteLine(url);
            List<string> lines = new List<string>();
            WebRequest web_request = WebRequest.Create(url);
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072; //This enables SSL
            using (Stream stream = web_request.GetResponse().GetResponseStream())
            {
                StreamReader stream_reader = new StreamReader(stream);
                string line = stream_reader.ReadLine();
                while (line != null)
                {
                    lines.Add(line);
                    line = stream_reader.ReadLine();
                }
            }
            return lines;
        }
        public static List<string> GetTradingCalendarHTMLDays(DateTimeUTC week_start)
        {
            string html = GetTradingCalendarHTMLLine(week_start);
            List<string> days = new List<string>();
            int occurance = html.IndexOf("calendar__row calendar__row--day-breaker");
            while (occurance != -1)
            {
                int next_occurance = html.IndexOf("calendar__row calendar__row--day-breaker", occurance + 1);
                if (next_occurance == -1)
                {
                    days.Add(html.Substring(occurance));
                    break;
                }
                else
                {
                    days.Add(html.Substring(occurance, next_occurance - occurance));
                    occurance = next_occurance;
                }
            }
            return days;
        }


        public static Tuple<string, List<string>> GetTradingCalendarEventsHTML(string day_html)
        {
            int index_0 = day_html.IndexOf("<span>") + 6;
            int index_1 = day_html.IndexOf("</span>");
            string date_string = day_html.Substring(index_0, index_1 - index_0);  
            List<string> event_html_list = new List<string>();
            int occurance = day_html.IndexOf("calendar__cell calendar__date date");
            while (occurance != -1)
            {
                int next_occurance = day_html.IndexOf("calendar__cell calendar__date date", occurance + 1);
                if (next_occurance == -1)
                {
                    event_html_list.Add(day_html.Substring(occurance));
                    break;
                }
                else
                {
                    event_html_list.Add(day_html.Substring(occurance, next_occurance - occurance));
                    occurance = next_occurance;
                }
            }
            return new Tuple<string, List<string>>(date_string, event_html_list);
        }



        private static void ParceEventRBA(DateTimeUTC week_start_day, string date_string, string event_html, List<ForexFactoryEvent> list)
        {
            int time_string_start = event_html.IndexOf("calendar__cell calendar__time time") + 36;
            int time_string_end = event_html.IndexOf("</td>", time_string_start);
            string time_string = event_html.Substring(time_string_start, time_string_end - time_string_start);

            if (time_string.Contains("upnext"))
            {
                time_string_start = event_html.IndexOf("span class=", time_string_start) + 20;
                time_string_end = event_html.IndexOf("</span>", time_string_start);
                time_string = event_html.Substring(time_string_start, time_string_end - time_string_start);
            }
            if (time_string.Equals(""))
            {
                time_string = list[list.Count - 1].TimeString;
            }


            int symbol_start = event_html.IndexOf("calendar__cell calendar__currency currency") + 44;
            int symbol_end = event_html.IndexOf("</td>", symbol_start);
            string symbol = event_html.Substring(symbol_start, symbol_end - symbol_start);
            if (symbol.Equals(""))
            {
                return;
            }

            int impact_start = event_html.IndexOf("calendar__impact-icon calendar__impact-icon--screen") + 67;
            int impact_end = event_html.IndexOf("</span>", impact_start) - 14;
            string impact = event_html.Substring(impact_start, impact_end - impact_start);

            int description_start = event_html.IndexOf("calendar__event-title") + 23;
            int description_end = event_html.IndexOf("</span>", description_start);
            string description = event_html.Substring(description_start, description_end - description_start);

            int actual_start = event_html.IndexOf("calendar__cell calendar__actual actual") + 40;
            int actual_end = event_html.IndexOf("</td>", actual_start);
            string actual = event_html.Substring(actual_start, actual_end - actual_start);

            int forecast_start = event_html.IndexOf("calendar__cell calendar__forecast forecast") + 44;
            int forecast_end = event_html.IndexOf("</td>", forecast_start);
            string forecast = event_html.Substring(forecast_start, forecast_end - forecast_start);


            int previous_start = event_html.IndexOf("calendar__cell calendar__previous previous") + 44;
            int previous_end = event_html.IndexOf("</td>", previous_start);
            string previous = event_html.Substring(previous_start, previous_end - previous_start);

            if (previous.Contains("revised"))
            { 
                previous_start = event_html.IndexOf("revised", previous_start) + 37;
                previous_end = event_html.IndexOf("</span>", previous_start);
                previous = event_html.Substring(previous_start, previous_end - previous_start);
            }

   

            list.Add(new ForexFactoryEvent(week_start_day, date_string, time_string, symbol, impact, description, actual, forecast, previous));
        }

   

        public static int MonthToInt(string month_string)
        {
            switch (month_string)
            {
                case "jan":
                case "Jan":
                    return 1;
                case "feb":
                case "Feb":
                    return 2;
                case "mar":
                case "Mar":
                    return 3;
                case "apr":
                case "Apr":
                    return 4;
                case "may":
                case "May":
                    return 5;
                case "jun":
                case "Jun":
                    return 6;
                case "jul":
                case "Jul":
                    return 7;
                case "aug":
                case "Aug":
                    return 8;
                case "sep":
                case "Sep":
                    return 9;
                case "oct":
                case "Oct":
                    return 10;
                case "nov":
                case "Nov":
                    return 11;
                case "dec":
                case "Dec":
                    return 12;
                default:
                    throw new Exception("no a mont index");
            }
        }

        public static string IntToMonth(int month_number)
        {
            switch (month_number)
            {
                case 1:
                    return "jan";
                case 2:
                    return "feb";
                case 3:
                    return "mar";
                case 4:
                    return "apr";
                case 5:
                    return "may";
                case 6:
                    return "jun";
                case 7:
                    return "jul";
                case 8:
                    return "aug";
                case 9:
                    return "sep";
                case 10:
                    return "oct";
                case 11:
                    return "nov";
                case 12:
                    return "dec";
                default:
                    throw new Exception("no a mont index");
            }
        }

    }
}
