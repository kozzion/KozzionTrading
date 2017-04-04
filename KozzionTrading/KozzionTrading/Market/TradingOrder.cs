using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KozzionTrading.Market;
using KozzionTrading.Tools;
using KozzionCore.Tools;

namespace KozzionTrading.Market
{
    public struct TradingOrder
    {
        public TradingOrderType OrderType { get; private set; }
        public int OrderTicket { get; private set; }
        public DateTimeUTC OpenDateTime { get; private set; }
        public double OpenPrice { get; private set; }
        public double Volume { get; private set; }
        public double CommissionForOrder { get; private set; }


        public bool UseLimits { get; private set; }
        public double StopLoss { get; private set; }
        public double TakeProfit { get; private set; }


        public DateTimeUTC CloseDateTime { get; private set; }
        public double ClosePrice { get; private set; }

        public bool IsOpen { get; private set; }
        public double Profit { get; private set; }


        // Close contructor
        public TradingOrder(TradingOrderType order_type, int order_ticket, DateTimeUTC open_date_time, double open_price, double volume, double commission_for_order,
            bool use_limits, double stop_loss, double take_profit,
            DateTimeUTC close_date_time, double close_price)
        {
            this.OrderType = order_type;
            this.OrderTicket = order_ticket;
            this.OpenDateTime = open_date_time;
            this.OpenPrice = open_price;
            this.Volume = volume;
            this.CommissionForOrder = commission_for_order;


            this.UseLimits = use_limits;
            this.StopLoss = stop_loss;
            this.TakeProfit = take_profit;


            this.CloseDateTime = close_date_time;
            this.ClosePrice = close_price;
 
            this.IsOpen = false;

             switch (order_type)
            {
                case TradingOrderType.Long:
                    this.Profit = ((ClosePrice - OpenPrice) * Volume) - commission_for_order;
                    break;
                case TradingOrderType.Short:
                    this.Profit = ((OpenPrice - ClosePrice) * Volume) - commission_for_order;
                    break;
                default :
                    throw new Exception("Unknown order type");
            }
         
        }


       
        //Simple open contructor
        public TradingOrder(TradingOrderType order_type, int order_ticket, DateTimeUTC open_date_time, double open_price, double volume, double commission_for_order)
            : this(order_type, order_ticket, open_date_time, open_price, volume, commission_for_order, false, 0, 0, new DateTimeUTC(), 0)
        {
            this.IsOpen = true;
        }

        //limit open contructor
        public TradingOrder(TradingOrderType order_type, int order_ticket, DateTimeUTC open_date_time, double open_price, double volume, double commission_for_order,
            bool use_limits, double stop_loss, double take_profit)
            : this(order_type, order_ticket, open_date_time, open_price, volume, commission_for_order, use_limits, stop_loss, take_profit, new DateTimeUTC(), 0)
        {
            this.IsOpen = true;
        }

        //limit modify contructor
        public TradingOrder(TradingOrder other, double stop_loss, double take_profit) 
            : this(other.OrderType, other.OrderTicket, other.OpenDateTime, other.OpenPrice, other.Volume, other.CommissionForOrder, true, stop_loss, take_profit, new DateTimeUTC(), 0)
        {
            this.IsOpen = true;
        }

        public TradingOrder Close(DateTimeUTC close_date_time, double close_price)
        {
            return new TradingOrder(OrderType, OrderTicket, OpenDateTime, OpenPrice, Volume, CommissionForOrder, UseLimits, StopLoss, TakeProfit, close_date_time, close_price);
        }

        public double ComputeValue(Price price)
        {
            switch (this.OrderType)
            {
                case TradingOrderType.Long:
                    return ((price.Bid - OpenPrice) * Volume) - this.CommissionForOrder;
                case TradingOrderType.Short:
                    return ((OpenPrice - price.Ask) * Volume) - this.CommissionForOrder;
                default:
                    throw new Exception("Unknown order type");
            }
        }
    }
}
