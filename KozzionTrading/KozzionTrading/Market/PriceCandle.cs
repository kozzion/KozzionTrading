using KozzionCore.Tools;
using KozzionTrading.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KozzionTrading.Market
{
    public struct PriceCandle
    {


        public DateTimeUTC OpenTime { get; private set; }
        public DateTimeUTC CloseTime { get; private set; }
        public DateTimeUTC MidTime { get { return OpenTime.AddMilliseconds((CloseTime - OpenTime).TotalMilliseconds / 2.0); } }
        public TimeScale TimeScale { get; private set; }

        public double OpenBid { get; private set; }
        public double HighBid { get; private set; }
        public double LowBid { get; private set; }
        public double CloseBid { get; private set; }

        public double OpenAsk { get; private set; }
        public double HighAsk { get; private set; }
        public double LowAsk { get; private set; }
        public double CloseAsk { get; private set; }

        public long Volume { get; private set; }
        public long RealVolume { get; private set; }
     

        public double GetPrice(PriceType price_type)
        {
            switch (price_type)
            {
                case PriceType.Bid:
                    return (OpenBid + HighBid + LowBid + CloseBid) / 4;
                case PriceType.Ask:
                    return (OpenAsk + HighAsk + LowAsk + CloseAsk) / 4; 
                case PriceType.Mean:
                    return (OpenBid + HighBid + LowBid + CloseBid + OpenAsk + HighAsk + LowAsk + CloseAsk) / 8;
                default:
                    throw new Exception("Unknown Price type");
            }
        }

        //Model Second1
        public PriceCandle(DateTimeUTC date_time, double bid, double ask)
        {
            this.OpenTime = date_time;
            this.CloseTime = GetCloseTime(date_time, TimeScale.Second1);
            this.TimeScale = TimeScale.Second1;
            this.OpenBid = bid;
            this.HighBid = bid;
            this.LowBid = bid;
            this.CloseBid = bid;

            this.OpenAsk = ask;
            this.HighAsk = ask;
            this.LowAsk = ask;
            this.CloseAsk = ask;

            this.Volume = 0;
            this.RealVolume = 0;
        }
        //Model 400
        public PriceCandle(DateTimeUTC date_time, TimeScale time_scale, double open_bid, double high_bid, double low_bid, double close_bid, double volume, double spread)
        {
            this.OpenTime = date_time;
            this.CloseTime = GetCloseTime(date_time, time_scale);
            this.TimeScale = time_scale;

            this.OpenBid = open_bid;
            this.HighBid = high_bid;
            this.LowBid = low_bid;
            this.CloseBid = close_bid;

            this.OpenAsk = open_bid + spread;
            this.HighAsk = high_bid + spread;
            this.LowAsk = low_bid + spread;
            this.CloseAsk = close_bid + spread;

            this.Volume = (long)volume;
            this.RealVolume = 0;
        }

        //Model 401
        public PriceCandle(DateTimeUTC date_time, TimeScale time_scale, double open_bid, double high_bid, double low_bid, double close_bid, long volume, double spread, long real_volume)
        {
            this.OpenTime = date_time;
            this.CloseTime = GetCloseTime(date_time, time_scale);
            this.TimeScale = time_scale;

            this.OpenBid = open_bid;
            this.HighBid = high_bid;
            this.LowBid = low_bid;
            this.CloseBid = close_bid;

            this.OpenAsk = open_bid + spread;
            this.HighAsk = high_bid + spread;
            this.LowAsk = low_bid + spread;
            this.CloseAsk = close_bid + spread;

            this.Volume = volume;
            this.RealVolume = real_volume;
        }

        //Model simple
        public PriceCandle(DateTimeUTC date_time, TimeScale time_scale, double open_bid, double high_bid, double low_bid, double close_bid, double spread)
        {
            this.OpenTime = GetOpenTime(date_time, time_scale);
            this.CloseTime = GetCloseTime(date_time, time_scale);
            this.TimeScale = TimeScale.Second1;

            this.OpenBid = open_bid;
            this.HighBid = high_bid;
            this.LowBid = low_bid;
            this.CloseBid = close_bid;

            this.OpenAsk = open_bid + spread;
            this.HighAsk = high_bid + spread;
            this.LowAsk = low_bid + spread;
            this.CloseAsk = close_bid + spread;

            this.Volume = 0;
            this.RealVolume = 0;
        }


        //Model full
        public PriceCandle(DateTimeUTC open_time, TimeScale time_scale,
            double open_bid, double high_bid, double low_bid, double close_bid,
            double open_ask, double high_ask, double low_ask, double close_ask,
            long volume, long real_volume)
        {
            this.OpenTime = open_time;
            this.CloseTime = GetCloseTime(open_time, time_scale);
            this.TimeScale = time_scale;

            this.OpenBid = open_bid;
            this.HighBid = high_bid;
            this.LowBid = low_bid;
            this.CloseBid = close_bid;

            this.OpenAsk = open_ask;
            this.HighAsk = high_ask;
            this.LowAsk = low_ask;
            this.CloseAsk = close_ask;

            this.Volume = volume;
            this.RealVolume = real_volume;
        }

        public void Write(BinaryWriter writer)
        {
            writer.WriteDateTimeUTC(this.OpenTime);
            writer.WriteEnum(this.TimeScale);

            writer.Write(this.OpenBid);
            writer.Write(this.HighBid);
            writer.Write(this.LowBid);
            writer.Write(this.CloseBid);
            writer.Write(this.OpenAsk);
            writer.Write(this.HighAsk);
            writer.Write(this.LowAsk);
            writer.Write(this.CloseAsk);

            writer.Write(this.Volume);
            writer.Write(this.RealVolume);
        }

        public static PriceCandle Read(BinaryReader reader)
        {
            DateTimeUTC date_time_open = reader.ReadDateTimeUTC();
            TimeScale time_scale = reader.ReadEnum<TimeScale>();
            double open_bid = reader.ReadDouble();
            double high_bid = reader.ReadDouble();
            double low_bid = reader.ReadDouble();
            double close_bid = reader.ReadDouble();
            double open_ask = reader.ReadDouble();
            double high_ask = reader.ReadDouble();
            double low_ask = reader.ReadDouble();
            double close_ask = reader.ReadDouble();

            long volume = reader.ReadInt64();
            long real_volume = reader.ReadInt64();

            return new PriceCandle(date_time_open, time_scale,
            open_bid, high_bid, low_bid, close_bid,
            open_ask, high_ask, low_ask, close_ask,
            volume, real_volume);
        }

        //Model list price
        public PriceCandle(DateTimeUTC open_time, TimeScale time_scale, IList<Price> prices)
        {
            this.OpenTime = open_time;
            this.CloseTime = GetCloseTime(open_time, time_scale);
            this.TimeScale = TimeScale.Second1;
            this.OpenBid = prices[0].Bid; 
            this.CloseBid = prices.Last().Bid;
            this.OpenAsk = prices[0].Ask;
            this.CloseAsk = prices.Last().Ask;
            
            this.HighBid = Math.Max(OpenBid, CloseBid);
            this.LowBid = Math.Min(OpenBid, CloseBid);
            this.HighAsk = Math.Max(OpenAsk, CloseAsk);
            this.LowAsk = Math.Min(OpenAsk, CloseAsk);

            // skip first and last
            for (int price_index = 1; price_index < prices.Count - 1; price_index++)
            {
                this.HighBid = Math.Max(this.HighBid, prices[price_index].Bid);
                this.LowBid = Math.Min(this.LowBid, prices[price_index].Bid);
                this.HighAsk = Math.Max(this.HighAsk, prices[price_index].Ask);
                this.LowAsk = Math.Min(this.LowAsk, prices[price_index].Ask);
            }  

            this.Volume = 0;
            this.RealVolume = 0;
        }


        //Model list price
        public PriceCandle(DateTimeUTC open_time, TimeScale time_scale, IList<PriceCandle> prices)
        {
            this.OpenTime = open_time;
            this.CloseTime = GetCloseTime(open_time, time_scale);
            this.TimeScale = TimeScale.Second1;
            this.OpenBid = prices[0].OpenBid;
            this.CloseBid = prices.Last().CloseBid;
            this.OpenAsk = prices[0].OpenAsk;
            this.CloseAsk = prices.Last().CloseAsk;

            this.HighBid = Math.Max(OpenBid, CloseBid);
            this.LowBid = Math.Min(OpenBid, CloseBid);
            this.HighAsk = Math.Max(OpenAsk, CloseAsk);
            this.LowAsk = Math.Min(OpenAsk, CloseAsk);

            // skip first and last
            for (int price_index = 0; price_index < prices.Count; price_index++)
            {
                this.HighBid = Math.Max(this.HighBid, prices[price_index].HighBid);
                this.LowBid = Math.Min(this.LowBid, prices[price_index].LowBid);
                this.HighAsk = Math.Max(this.HighAsk, prices[price_index].HighAsk);
                this.LowAsk = Math.Min(this.LowAsk, prices[price_index].LowAsk);
            }

            this.Volume = 0;
            this.RealVolume = 0;
        }



        public static DateTimeUTC GetOpenTime(DateTimeUTC time, TimeScale time_scale)
        {
            switch (time_scale)
            {
                case TimeScale.Second1:
                    return new DateTimeUTC(time.Year, time.Month, time.Day, time.Hour, time.Minute, time.Second);
                case TimeScale.Minute1:
                    return new DateTimeUTC(time.Year, time.Month, time.Day, time.Hour, time.Minute, 0);
                case TimeScale.Minute15:
                    return new DateTimeUTC(time.Year, time.Month, time.Day, time.Hour, time.Minute - time.Minute % 15, 0);
                case TimeScale.Minute30:
                    return new DateTimeUTC(time.Year, time.Month, time.Day, time.Hour, time.Minute - time.Minute % 30, 0);
                case TimeScale.Hour1:
                    return new DateTimeUTC(time.Year, time.Month, time.Day, time.Hour, 0, 0);
                case TimeScale.Hour4:
                    return new DateTimeUTC(time.Year, time.Month, time.Day, time.Hour - time.Hour % 4, 0, 0);
                case TimeScale.Day1:
                    return new DateTimeUTC(time.Year, time.Month, time.Day, 0, 0, 0);
                case TimeScale.Week1:
                    DateTimeUTC temp = new DateTimeUTC(time.Year, time.Month, time.Day, 0, 0, 0);
                    return temp.AddDays(-(int)time.DayOfWeek);
                case TimeScale.Month1:
                    return new DateTimeUTC(time.Year, time.Month, 1, 0, 0, 0);
                default:
                    throw new Exception("Unknown Timescale");
            }
        }

        public static DateTimeUTC GetCloseTime(DateTimeUTC time, TimeScale time_scale)
        {
            DateTimeUTC open_time = GetOpenTime(time, time_scale);
            switch (time_scale)
            {
                case TimeScale.Second1:
                    return open_time.AddSeconds(1);
                case TimeScale.Minute1:
                    return open_time.AddMinutes(1);
                case TimeScale.Minute15:
                    return open_time.AddMinutes(15);
                case TimeScale.Minute30:
                    return open_time.AddMinutes(30);
                case TimeScale.Hour1:
                    return open_time.AddHours(1);
                case TimeScale.Hour4:
                    return open_time.AddHours(4);
                case TimeScale.Day1:
                    return open_time.AddDays(1);
                case TimeScale.Week1:
                    return open_time.AddDays(7);
                case TimeScale.Month1:
                    return open_time.AddMonths(1);
                default:
                    throw new Exception("Unknown Timescale");
            }
        }
    }
}
