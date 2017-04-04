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
    public class Price
    {
        public DateTimeUTC Time { get; private set; }
        public double Bid { get; private set; }
        public double Ask { get; private set; }

        public Price(DateTimeUTC time, double bid, double ask)
        {
            Time = time;
            Bid = bid;
            Ask = ask;
        }

        public double GetPrice(PriceType price_type)
        {
            switch (price_type)
            {
                case PriceType.Bid:
                    return Bid;
                case PriceType.Ask:
                    return Ask;
                case PriceType.Mean:
                    return (Bid + Ask) / 2.0;
                default:
                    throw new Exception("Unknown Price type");
            }
        }

        public void Write(BinaryWriter writer)
        {
            writer.WriteDateTimeUTC(this.Time);
            writer.Write(this.Bid);
            writer.Write(this.Ask);
        }

        public static Price Read(BinaryReader reader)
        {
            DateTimeUTC time = reader.ReadDateTimeUTC();
            double bid = reader.ReadDouble();
            double ask = reader.ReadDouble();
            return new Price(time, bid, ask);
        }
    }

   
}
