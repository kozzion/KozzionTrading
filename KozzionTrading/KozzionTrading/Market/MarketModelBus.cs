using KozzionCore.DataStructure.Collections;
using KozzionCore.Tools;
using KozzionTrading.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KozzionTrading.Market
{
    public class MarketModelBus : IMarketModel
    {
        public double Cash { get; private set; }
        public double Equity { get; private set; }

        public IHistoryList<Price> Prices { get; private set; }
        public IHistoryList<PriceCandle> Second1 { get; private set; }
        public IHistoryList<PriceCandle> Minute1 { get; private set; }
        public IHistoryList<PriceCandle> Minute15 { get; private set; }
        public IHistoryList<PriceCandle> Minute30 { get; private set; }
        public IHistoryList<PriceCandle> Hour1 { get; private set; }
        public IHistoryList<PriceCandle> Hour4 { get; private set; }
        public IHistoryList<PriceCandle> Day1 { get; private set; }
        public IHistoryList<PriceCandle> Week1 { get; private set; }
        public IHistoryList<PriceCandle> Month1 { get; private set; }


        public Dictionary<int, TradingOrder> OpenOrders { get; private set; }
        public List<TradingOrder> ClosedOrders { get; private set; }

        public DateTimeUTC CurrentDateTime
        {
            get
            {
                if (Second1.HistoryCount == 0)
                {
                    return Second1.GetFuture(0).OpenTime;
                }
                else
                {
                    return Second1.GetHistory(0).CloseTime;
                }
            }
        }

        public double CurrentBid
        {
            get
            {
                if (Second1.HistoryCount == 0)
                {
                    return Second1.GetFuture(0).OpenBid;
                }
                else
                {
                    return Second1.GetHistory(0).CloseBid;
                }
            }
        }

        public double CurrentAsk
        {
            get
            {
                if (Second1.HistoryCount == 0)
                {
                    return Second1.GetFuture(0).OpenAsk;
                }
                else
                {
                    return Second1.GetHistory(0).CloseAsk;
                }
            }
        }


        public void CloseAll()
        {
            List<int> open_order_tickets = new List<int>(OpenOrders.Keys);
            foreach (int open_order_ticket in open_order_tickets)
            {
                CloseOrder(open_order_ticket);
            }
        }

        //Primary contructor
        public MarketModelBus(double initial_cash, PriceSet price_set)
        {

            this.Cash = initial_cash;
            this.Equity = Cash;

            this.Prices = new HistoryList<Price>(price_set.Prices);
            this.Second1 = new HistoryList<PriceCandle>(price_set.Second1);
            this.Minute1 = new HistoryList<PriceCandle>(price_set.Minute1);
            this.Minute15 = new HistoryList<PriceCandle>(price_set.Minute15);
            this.Minute30 = new HistoryList<PriceCandle>(price_set.Minute30);
            this.Hour1 = new HistoryList<PriceCandle>(price_set.Hour1);
            this.Hour4 = new HistoryList<PriceCandle>(price_set.Hour4);
            this.Day1 = new HistoryList<PriceCandle>(price_set.Day1);
            this.Week1 = new HistoryList<PriceCandle>(price_set.Week1);
            this.Month1 = new HistoryList<PriceCandle>(price_set.Month1);

            this.OpenOrders = new Dictionary<int, TradingOrder>();
            this.ClosedOrders = new List<TradingOrder>();
            //Make the first price availeble
            Prices.Step();
        }

        public void Add(DateTime date_time, double bid, double ask)
        {
            PriceCandle candle = Second1.GetFuture(0);

            // Compute equity
            this.Equity = Cash;
            foreach (TradingOrder open_order in OpenOrders.Values)
            {
                Equity += open_order.ComputeValue(Prices.GetHistory(0));
            }

            
            Prices.Step();
            Second1.Step();
            if ((0 < Minute1.FutureCount) && (Prices.GetHistory(0).Time == Minute1.GetFuture(0).CloseTime))
            {
                Minute1.Step();
            }
            else
            {
                // if it is not the end of a minute the others also do not nee to step
                return;
            }

            //if ((0 < Minute15.FutureCount) && (Prices.GetHistory(0).Time == Minute15.GetFuture(0).CloseTime))
            //{
            //    Minute15.Step();
            //}

            //if ((0 < Minute30.FutureCount) && (Prices.GetHistory(0).Time == Minute30.GetFuture(0).CloseTime))
            //{
            //    Minute30.Step();
            //}

            //if ((0 < Hour1.FutureCount) && (Prices.GetHistory(0).Time == Hour1.GetFuture(0).CloseTime))
            //{
            //    Hour1.Step();
            //}

            //if ((0 < Hour4.FutureCount) && (Prices.GetHistory(0).Time == Hour4.GetFuture(0).CloseTime))
            //{
            //    Hour4.Step();
            //}

            //if ((0 < Day1.FutureCount) && (Prices.GetHistory(0).Time == Day1.GetFuture(0).CloseTime))
            //{
            //    Day1.Step();
            //}

            //if ((0 < Week1.FutureCount) && (Prices.GetHistory(0).Time == Week1.GetFuture(0).CloseTime))
            //{
            //    Week1.Step();
            //}

            //if ((0 < Month1.FutureCount) && (Prices.GetHistory(0).Time == Month1.GetFuture(0).CloseTime))
            //{
            //    Month1.Step();
            //}
        }

        public void CloseOrder(int order_ticket)
        {
            TradingOrder trade_order = OpenOrders[order_ticket];
            switch (trade_order.OrderType)
            {
                case TradingOrderType.Long:
                    trade_order = trade_order.Close(CurrentDateTime, CurrentBid);
                    break;
                case TradingOrderType.Short:
                    trade_order = trade_order.Close(CurrentDateTime, CurrentAsk);
                    break;
                default: throw new Exception("Unknown order type");
            }

            this.ClosedOrders.Add(trade_order);
            this.OpenOrders.Remove(order_ticket);
            this.Cash += trade_order.Profit;
        }

        private static TradingOrder CloseOrder(TradingOrder trade_order, DateTimeUTC current_date_time, double current_bid, double current_ask)
        {
            switch (trade_order.OrderType)
            {
                case TradingOrderType.Long:
                    return trade_order.Close(current_date_time, current_bid);
                case TradingOrderType.Short:
                    return trade_order.Close(current_date_time, current_ask);
                default:
                    throw new NotImplementedException();
            }
        }



        public void ProcessOrderCommand(TradingOrderCommand trade_order_command)
        {
            //TODO check price and return error codes!!!
            //TODO merge close order and modify order in here
   

        }

        public void ProcessOrderEvent(TradingOrderCommand trade_order_command)
        {
            //TODO check price and return error codes!!!
            //TODO merge close order and modify order in here


        }
    }
}
