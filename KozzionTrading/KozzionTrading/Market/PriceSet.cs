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

    // A valid coherent set of candles based on a un clean priceslist.
    // A price list is clean when every prices is in a new second and no seconds are missing.
    // A priceSet does not contain implete candle, e.a Candles that are are fromed from less than their
    // maximum set of sub-candles (e.g. 60 1 second candles for a 1minute candle)
    public class PriceSet :IComparable<PriceSet>
    {
        public TradingSymbol Symbol { get; private set; }

        public DateTimeUTC OpenDateTime{ get { return Prices[0].Time; } }
        public DateTimeUTC CloseDateTime { get { return Prices.Last().Time; } }


        public IReadOnlyList<double> Ticks { get; private set; }
        public IReadOnlyList<Price> Prices { get; private set; }

        public IReadOnlyList<PriceCandle> Second1 { get; private set; }
        public IReadOnlyList<PriceCandle> Minute1 { get; private set; }
        public IReadOnlyList<PriceCandle> Minute15 { get; private set; }
        public IReadOnlyList<PriceCandle> Minute30 { get; private set; }
        public IReadOnlyList<PriceCandle> Hour1 { get; private set; }
        public IReadOnlyList<PriceCandle> Hour4 { get; private set; }
        public IReadOnlyList<PriceCandle> Day1 { get; private set; }
        public IReadOnlyList<PriceCandle> Week1 { get; private set; }
        public IReadOnlyList<PriceCandle> Month1 { get; private set; }

        //Primary constucter
        public PriceSet(TradingSymbol symbol, IReadOnlyList<Price> prices, IReadOnlyList<PriceCandle> second_1)
        {
            this.Symbol = symbol;
            this.Prices = prices;
            this.Second1 = second_1;
            //Build rest from prices and second data
            this.Ticks = CreateTicks(Prices);
            this.Minute1 = CreatePriceCandleList(Second1, TimeScale.Minute1);
            this.Minute15 = CreatePriceCandleList(Second1, TimeScale.Minute15);
            this.Minute30 = CreatePriceCandleList(Second1, TimeScale.Minute30);
            this.Hour1 = CreatePriceCandleList(Second1, TimeScale.Hour1);
            this.Hour4 = CreatePriceCandleList(Second1, TimeScale.Hour4);
            this.Day1 = CreatePriceCandleList(Second1, TimeScale.Day1);
            this.Week1 = CreatePriceCandleList(Second1, TimeScale.Week1);
            this.Month1 = CreatePriceCandleList(Second1, TimeScale.Month1);
        }


        //Read initial constucter
        public PriceSet(TradingSymbol symbol, IReadOnlyList<Price> price_list_unclean)
            : this(symbol, CreatePriceListClean(price_list_unclean), CreateSecond1(price_list_unclean))
        {
  
        }

        //Read initial constucter
        public PriceSet(IList<PriceSet> price_set_list)
            : this(price_set_list[0].Symbol, CombinePrices(price_set_list))    
        {

        }

        //Primary constucter
        public PriceSet(TradingSymbol symbol, DateTimeUTC open_time, DateTimeUTC close_time)
           : this(symbol,CreatePrices(open_time, close_time), CreateSecond1(open_time, close_time))
        {
        }

        private static IReadOnlyList<Price> CombinePrices(IList<PriceSet> price_set_list)
        {
            TradingSymbol symbol = price_set_list[0].Symbol;
            for (int index = 1; index < price_set_list.Count; index++)
            {
                if (!price_set_list[index].Symbol.Equals(symbol))
                {
                    throw new Exception("Symbols do not match");
                }

                if (price_set_list[index].OpenDateTime < price_set_list[index - 1].CloseDateTime )
                {
                    throw new Exception("PriceSet show overlap");
                }
            }

            List<Price> price_unclean_list = new List<Price>(price_set_list[0].Prices);          
            for (int index = 1; index < price_set_list.Count; index++)
            {
                price_unclean_list.AddRange(price_set_list[index].Prices);
            }
            return price_unclean_list;
        }

        private static IReadOnlyList<Price> CreatePrices(DateTimeUTC open_time, DateTimeUTC final_time)
        {
            List<Price> price = new List<Price>();
            while (open_time <= final_time)
            {
                price.Add(new Price(open_time, 0, 0));
                open_time = open_time.AddSeconds(1);
            }
            return price;
        }
        private static IReadOnlyList<PriceCandle> CreateSecond1(DateTimeUTC open_time, DateTimeUTC final_time)
        {
            List<PriceCandle> candles = new List<PriceCandle>();
            while (open_time < final_time)
            {
                candles.Add(new PriceCandle(open_time, 0, 0));
                open_time = open_time.AddSeconds(1);
            }
            return candles;
        }

        public PriceSet UpdatePrices(PriceSet other)
        {
            if (!Symbol.Equals(other.Symbol))
            {
                throw new Exception("Symbols do not match");
            }


            IReadOnlyList<Price> prices_new = UpdatePrices(Prices, other.Prices);
            IReadOnlyList<PriceCandle> second_new = UpdateSecond1(Second1, other.Second1); ;
            return new PriceSet(Symbol, prices_new, second_new);


        }

        private IReadOnlyList<Price> UpdatePrices(IReadOnlyList<Price> prices_0, IReadOnlyList<Price> prices_1)
        {
            List<Price> prices_new = new List<Price>(prices_0.Count);
            int index_1 = 0;
            for (int index_0 = 0; index_0 < prices_0.Count; index_0++)
            {
                prices_new.Add(prices_0[index_0]);
                if (prices_new[index_0].Time == prices_1[index_1].Time)
                {
                    prices_new[index_0] = prices_1[index_1];
                    index_1++;
                }
            }
            return prices_new;
        }

        private IReadOnlyList<PriceCandle> UpdateSecond1(IReadOnlyList<PriceCandle> prices_0, IReadOnlyList<PriceCandle> prices_1)
        {
            List<PriceCandle> prices_new = new List<PriceCandle>(prices_0.Count);
            int index_1 = 0;
            for (int index_0 = 0; index_0 < prices_0.Count; index_0++)
            {
                prices_new.Add(prices_0[index_0]);
                if (prices_new[index_0].OpenTime == prices_1[index_1].OpenTime)
                {
                    prices_new[index_0] = prices_1[index_1];
                    index_1++;
                }
            }
            return prices_new;
        }

        private IReadOnlyList<double> CreateTicks(IReadOnlyList<Price> prices)
        {
            double[] ticks = new double[prices.Count];
            Parallel.For(0, prices.Count, index => {
                ticks[index] = prices[index].Time.Ticks;
            });
            return ticks;
        }

        public PriceSet SubSet(DateTimeUTC lower, DateTimeUTC upper)
        {
            List<Price> prices = new List<Price>();
            foreach (Price price in Prices)
            {
                if ((lower <= price.Time) && ( price.Time <= upper))
                {
                    prices.Add(price);
                }
            }
            List<PriceCandle> second_1 = new List<PriceCandle>();
            foreach (PriceCandle price_candle in Second1)
            {
                if ((lower <= price_candle.OpenTime) && (price_candle.CloseTime <= upper))
                {
                    second_1.Add(price_candle);
                }
            }
            return new PriceSet(Symbol, prices, second_1);
        }


        //TODO make unit tests
        private static List<Price> CreatePriceListClean(IReadOnlyList<Price> price_list_unclean)
        {
            //Build price
            List<Price> clean_price_list = new List<Price>();
            DateTimeUTC time = price_list_unclean[0].Time;
            double bid = price_list_unclean[0].Bid;
            double ask = price_list_unclean[0].Ask;
            int price_index = 0;
            while (price_index < price_list_unclean.Count)
            {
                if (time < price_list_unclean[price_index].Time)
                {
                    //Create new price
                    clean_price_list.Add(new Price(time, bid, ask));
                    time = time.AddSeconds(1);
                }
                else
                {
                    //Save and scroll   
                    ask = price_list_unclean[price_index].Ask;
                    bid = price_list_unclean[price_index].Bid;
                    price_index++;
                }
            }
            //TODO make last
            clean_price_list.Add(new Price(time, bid, ask));
            return clean_price_list;
        }

        public IReadOnlyList<PriceCandle> GetCandles(TimeScale time_scale)
        {
            switch (time_scale)
            {
                case TimeScale.Second1: return Second1;
                case TimeScale.Minute1: return Minute1;
                case TimeScale.Minute15: return Minute15;
                case TimeScale.Minute30: return Minute30;
                case TimeScale.Hour1: return Hour1;
                case TimeScale.Hour4: return Hour4;
                case TimeScale.Day1: return Day1;
                case TimeScale.Week1: return Week1;
                case TimeScale.Month1: return Month1;
                default:
                    throw new Exception("No candles for: " + time_scale);
            }
        }

        private static List<PriceCandle> CreateSecond1(IReadOnlyList<Price> price_list_unclean)
        {
            //Build second 1         

            List<PriceCandle> second1 = new List<PriceCandle>();

            DateTimeUTC current_time = price_list_unclean[0].Time;
            DateTimeUTC open_time = PriceCandle.GetOpenTime(current_time, TimeScale.Second1);
            DateTimeUTC close_time = PriceCandle.GetCloseTime(current_time, TimeScale.Second1);
            int price_index = 0;
            //if we do not start on a candle open:
            if (!current_time.Equals(open_time))
            {
                //Wind forward to first candle in the next candle
                while (price_list_unclean[price_index].Time != close_time)
                {
                    price_index++;
                }
            }

            double open_bid = price_list_unclean[price_index].Bid;
            double high_bid = price_list_unclean[price_index].Bid;
            double low_bid = price_list_unclean[price_index].Bid;
            double close_bid = price_list_unclean[price_index].Bid;
            double open_ask = price_list_unclean[price_index].Ask;
            double high_ask = price_list_unclean[price_index].Ask;
            double low_ask = price_list_unclean[price_index].Ask;
            double close_ask = price_list_unclean[price_index].Ask;


            while (price_index < price_list_unclean.Count)
            {
                //If it is our current price candle
                if (price_list_unclean[price_index].Time <= close_time)
                {
                    //Update data
                    //open_bid = close_bid;
                    high_bid = Math.Max(high_bid, price_list_unclean[price_index].Bid);
                    low_bid = Math.Min(low_bid, price_list_unclean[price_index].Bid);
                    close_bid = price_list_unclean[price_index].Bid;
                    //open_ask = close_bid;
                    high_ask = Math.Max(high_ask, price_list_unclean[price_index].Ask);
                    low_ask = Math.Min(low_ask, price_list_unclean[price_index].Ask);
                    close_ask = price_list_unclean[price_index].Ask;
                    price_index++;

                }
                else
                {
                    //Other wise append a new candle
                    second1.Add(new PriceCandle(open_time, TimeScale.Second1, open_bid, high_bid, low_bid, close_bid, open_ask, high_ask,low_ask,close_ask,0,0));
                    //And prepare the next one
                    open_time = close_time;
                    close_time = PriceCandle.GetCloseTime(open_time, TimeScale.Second1);
                    open_bid = close_bid;
                    high_bid = Math.Max(close_bid, price_list_unclean[price_index].Bid);
                    low_bid = Math.Min(close_bid, price_list_unclean[price_index].Bid);
                    close_bid = price_list_unclean[price_index].Bid;
                    open_ask = close_ask;
                    high_ask = Math.Max(close_ask, price_list_unclean[price_index].Ask);
                    low_ask = Math.Min(close_ask, price_list_unclean[price_index].Ask);
                    close_ask = price_list_unclean[price_index].Ask;
                    //current.Clear();
                    //current.Add(new Price(close_time, second1.Last().CloseBid, second1.Last().CloseAsk));
                }
            }
            //If last candle was complete
            if (price_list_unclean.Last().Time == close_time)
            {
                //Add last candle
                second1.Add(new PriceCandle(open_time, TimeScale.Second1, open_bid, high_bid, low_bid, close_bid, open_ask, high_ask, low_ask, close_ask, 0, 0));
            }
            return second1;
        }



        //Should yield the same results regardless of what chandles are used for input, but faster on closer candles
        private static List<PriceCandle> CreatePriceCandleList(IReadOnlyList<PriceCandle> price_candles, TimeScale time_scale)
        {
            if (price_candles.Count == 0)
            {
                return new List<PriceCandle>();
            }

            //Set times next candle
            DateTimeUTC open_time = PriceCandle.GetOpenTime(price_candles[0].OpenTime, time_scale);
            DateTimeUTC close_time = PriceCandle.GetCloseTime(open_time, time_scale);

            int current_candle_index = 0;

            //if we do not start on a candle open:
            if (price_candles[0].OpenTime != open_time)
            { 
                //Wind forward to first candle in the next candle
                while ( (current_candle_index < price_candles.Count) && (price_candles[current_candle_index].OpenTime != close_time))
                {
                    current_candle_index++;
                }
                //Set times next candle
                open_time = PriceCandle.GetOpenTime(close_time, time_scale);
                close_time = PriceCandle.GetCloseTime(open_time, time_scale);
            }

            if (price_candles.Count <= current_candle_index)
            {
                return new List<PriceCandle>();
            }

            //Work
            List<PriceCandle> new_price_candles = new List<PriceCandle>();
            double open_bid = price_candles[current_candle_index].OpenBid;
            double high_bid = price_candles[current_candle_index].HighBid;
            double low_bid = price_candles[current_candle_index].LowBid;
            double close_bid = price_candles[current_candle_index].CloseBid;
            double open_ask = price_candles[current_candle_index].OpenAsk;
            double high_ask = price_candles[current_candle_index].HighAsk;
            double low_ask = price_candles[current_candle_index].LowAsk;
            double close_ask = price_candles[current_candle_index].CloseAsk;
            while (current_candle_index < price_candles.Count)
            {
                //If it is the end of our current price candle
                if (price_candles[current_candle_index].CloseTime == close_time)
                {
                    // Update everything one last time 
                    //open_bid = 
                    high_bid = price_candles[current_candle_index].HighBid;
                    low_bid = price_candles[current_candle_index].LowBid;
                    close_bid = price_candles[current_candle_index].CloseBid;
                   //open_ask =
                    high_ask = price_candles[current_candle_index].HighAsk;
                    low_ask = price_candles[current_candle_index].LowAsk;
                    close_ask = price_candles[current_candle_index].CloseAsk;
                    // Create candle
                    new_price_candles.Add(new PriceCandle(
                        open_time,
                        time_scale,
                        open_bid,
                        high_bid,
                        low_bid,
                        close_bid,
                        open_ask,
                        high_ask,
                        low_ask,
                        close_ask, 0, 0));
                    //Set begin and end for our next candle
                    open_time = PriceCandle.GetOpenTime(close_time, time_scale);
                    close_time = PriceCandle.GetCloseTime(open_time, time_scale);
                    //scroll forward
                    current_candle_index++;
                    //And if there is a next candle:
                    if (current_candle_index < price_candles.Count)
                    {
                        // initialize
                        open_bid = price_candles[current_candle_index].OpenBid;
                        high_bid = price_candles[current_candle_index].HighBid;
                        low_bid = price_candles[current_candle_index].LowBid;
                        close_bid = price_candles[current_candle_index].CloseBid;
                        open_ask = price_candles[current_candle_index].OpenAsk;
                        high_ask = price_candles[current_candle_index].HighAsk;
                        low_ask = price_candles[current_candle_index].LowAsk;
                        close_ask = price_candles[current_candle_index].CloseAsk;
                    }
                }
                else
                {
                    //If we are not the end we must be some part so just update data
                    //open_bid = 
                    high_bid = Math.Max(high_bid, price_candles[current_candle_index].HighBid);
                    low_bid = Math.Min(low_bid, price_candles[current_candle_index].LowBid);
                    close_bid = price_candles[current_candle_index].CloseBid;
                    // open_ask = 
                    high_ask = Math.Max(high_ask, price_candles[current_candle_index].HighAsk);
                    low_ask = Math.Min(low_ask, price_candles[current_candle_index].LowAsk);
                    close_ask = price_candles[current_candle_index].CloseAsk;
                    //scroll forward
                    current_candle_index++;
                }
            }
            return new_price_candles;
        }


        public static PriceSet Read(BinaryReader reader)
        {
            TradingSymbol symbol = TradingSymbol.Read(reader);
            int price_list_clean_count = reader.ReadInt32();
            List<Price> price_list_clean = new List<Price>();
            for (int price_index = 0; price_index < price_list_clean_count; price_index++)
            {
                price_list_clean.Add(Price.Read(reader));
            }

            int second1_count = reader.ReadInt32();
            List<PriceCandle> second1 = new List<PriceCandle>();
            for (int price_index = 0; price_index < second1_count; price_index++)
            {
                second1.Add(PriceCandle.Read(reader));
            }
            return new PriceSet(symbol, price_list_clean, second1);
        }


        public void Write(BinaryWriter writer)
        {
            this.Symbol.Write(writer);
            writer.Write(this.Prices.Count); 
            for (int price_index = 0; price_index < this.Prices.Count; price_index++)
            {
                this.Prices[price_index].Write(writer);
            }

            writer.Write(this.Second1.Count);
            for (int price_index = 0; price_index < this.Second1.Count; price_index++)
            {
                this.Second1[price_index].Write(writer);
            }
        }

        public int CompareTo(PriceSet other)
        {
            return OpenDateTime.CompareTo(other.OpenDateTime);
        }

        public override string ToString()
        {
            return OpenDateTime.ToString() + " " + CloseDateTime.ToString();
        }
    }
}
