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
    public class MarketModelSimulation : IMarketModelSimulation
    {
        private int next_order_ticket;

        public double Cash { get; private set; }
        public double Equity { get; private set; }
        public double CommissionPerUnit { get; private set; }

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
        public MarketModelSimulation(double initial_cash, double commission_per_unit, PriceSet price_set)
        {
            this.next_order_ticket = 0; //TODO create and argument for this
            this.Cash = initial_cash;
            this.Equity = Cash;
            this.CommissionPerUnit = commission_per_unit;

            this.Prices = new HistoryListFast<Price>(price_set.Prices);
            this.Second1 = new HistoryListFast<PriceCandle>(price_set.Second1);
            this.Minute1 = new HistoryListFast<PriceCandle>(price_set.Minute1);
            this.Minute15 = new HistoryListFast<PriceCandle>(price_set.Minute15);
            this.Minute30 = new HistoryListFast<PriceCandle>(price_set.Minute30);
            this.Hour1= new HistoryListFast<PriceCandle>(price_set.Hour1);
            this.Hour4 = new HistoryListFast<PriceCandle>(price_set.Hour4);
            this.Day1 = new HistoryListFast<PriceCandle>(price_set.Day1);
            this.Week1 = new HistoryListFast<PriceCandle>(price_set.Week1);
            this.Month1 = new HistoryListFast<PriceCandle>(price_set.Month1);

            this.OpenOrders = new Dictionary<int, TradingOrder>();
            this.ClosedOrders = new List<TradingOrder>();
            //Make the first price availeble
            Prices.Step();
        }


        public MarketModelSimulation(double initial_cash, PriceSet price_set)
         : this(initial_cash, 0.0, price_set)
        {

        }

        public void StepSecond()
        {
            if (Second1.FutureCount == 0)
            {
                throw new Exception("No future to step to");
            }
            PriceCandle candle = Second1.GetFuture(0);

            // Close orders
            List<TradingOrder> ClosingOrders = CheckOrderLimits(OpenOrders, candle);
            foreach (TradingOrder closed_order in ClosingOrders)
            {
                this.OpenOrders.Remove(closed_order.OrderTicket);
                this.ClosedOrders.Add(closed_order);
                this.Cash += closed_order.Profit;
            }

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

            if ((0 <  Minute15.FutureCount) && (Prices.GetHistory(0).Time == Minute15.GetFuture(0).CloseTime))
            {
                Minute15.Step();
            }

            if ((0 < Minute30.FutureCount) && (Prices.GetHistory(0).Time == Minute30.GetFuture(0).CloseTime))
            {
                Minute30.Step();
            }

            if ((0 < Hour1.FutureCount) && (Prices.GetHistory(0).Time == Hour1.GetFuture(0).CloseTime))
            {
                Hour1.Step();
            }

            if ((0 < Hour4.FutureCount) && (Prices.GetHistory(0).Time == Hour4.GetFuture(0).CloseTime))
            {
                Hour4.Step();
            }

            if ((0 < Day1.FutureCount) && (Prices.GetHistory(0).Time == Day1.GetFuture(0).CloseTime))
            {
                Day1.Step();
            }

            if ((0 < Week1.FutureCount) && (Prices.GetHistory(0).Time == Week1.GetFuture(0).CloseTime))
            {
                Week1.Step();
            }

            if ((0 < Month1.FutureCount) && (Prices.GetHistory(0).Time == Month1.GetFuture(0).CloseTime))
            {
                Month1.Step();
            }
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

        public static List<TradingOrder> CheckOrderLimits(IDictionary<int, TradingOrder> open_orders, PriceCandle candle)
        {
            //TODO there is something nasty here if a candle hits the stoploss as well as the take profit. 
            //Conservatively we go for the stop loss but this should be configurable.
            //UPDATE for second candles this should not matter
            List<TradingOrder> closing_orders = new List<TradingOrder>();

            //TODO speed this up, cos most of the time no orders would close
            foreach (TradingOrder trade_order in open_orders.Values)
            {
                //Check stop loss and take profits     
                if (trade_order.UseLimits)
                {
                    switch (trade_order.OrderType)
                    {
                        case TradingOrderType.Long:
                            if (candle.LowBid < trade_order.StopLoss)
                            {
                                closing_orders.Add(trade_order.Close(candle.CloseTime, trade_order.StopLoss));
                            }
                            else if (trade_order.TakeProfit < candle.HighBid)
                            {
                                closing_orders.Add(trade_order.Close(candle.CloseTime, trade_order.TakeProfit));
                            }
                            break;

                        case TradingOrderType.Short:
                            if (trade_order.StopLoss < candle.HighAsk)
                            {
                                closing_orders.Add(trade_order.Close(candle.CloseTime, trade_order.StopLoss));
                            }
                            else if (candle.LowAsk < trade_order.TakeProfit)
                            {
                                closing_orders.Add(trade_order.Close(candle.CloseTime, trade_order.TakeProfit));
                            }
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
            }
            return closing_orders;
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
            switch (trade_order_command.OrderType)
            {
                case TradingOrderType.Long:
                    OpenOrders.Add(next_order_ticket, new TradingOrder(trade_order_command.OrderType, next_order_ticket, CurrentDateTime, CurrentAsk, trade_order_command.Volume, this.CommissionPerUnit * trade_order_command.Volume, trade_order_command.UseLimits, trade_order_command.StopLoss, trade_order_command.TakeProfit));
                    next_order_ticket++;
                    break;
                case TradingOrderType.Short:
                    OpenOrders.Add(next_order_ticket, new TradingOrder(trade_order_command.OrderType, next_order_ticket, CurrentDateTime, CurrentBid, trade_order_command.Volume, this.CommissionPerUnit * trade_order_command.Volume, trade_order_command.UseLimits, trade_order_command.StopLoss, trade_order_command.TakeProfit));
                    next_order_ticket++;
                    break;
                default:
                    throw new NotImplementedException();
            }

        }

        public void ModifyOrder(int order_ticket, double stop_loss, double take_profit)
        {
            if (!OpenOrders.ContainsKey(order_ticket))
            {
                throw new Exception("No such order ticket");
            }
            OpenOrders[order_ticket] = new TradingOrder(OpenOrders[order_ticket], stop_loss, take_profit);
        }

        public void AddStep(PriceCandle priceCandle)
        {
            throw new NotImplementedException("Not for fast mode");
        }

     
    }
}
