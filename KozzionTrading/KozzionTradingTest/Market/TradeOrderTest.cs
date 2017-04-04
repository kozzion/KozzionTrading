using KozzionCore.Tools;
using KozzionTrading.Market;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace KozzionTradingTest.Market
{
    [TestClass]
    public class TradeOrderTest
    {
        [TestMethod]
        public void CloseOrderNoLimitTest()
        {
            double spread = 0.1;
            Dictionary<int, TradingOrder> OpenOrders = new Dictionary<int, TradingOrder>();
            PriceCandle candle = new PriceCandle(DateTimeUTC.Now, TimeScale.Second1, 1, 1.2, 0.8, 1, spread);
     

            TradingOrder order_0 = new TradingOrder(TradingOrderType.Long, 0, DateTimeUTC.Now, 1, 1, 0.01);
            OpenOrders.Add(order_0.OrderTicket, order_0);

            List<TradingOrder> orders = MarketModelSimulation.CheckOrderLimits(OpenOrders, candle);
            Assert.AreEqual(orders.Count, 0);       
        }

        [TestMethod]
        public void CloseOrderLimit()
        {
            double spread = 0.05;
            Dictionary<int, TradingOrder> OpenOrders0 = new Dictionary<int, TradingOrder>();
            Dictionary<int, TradingOrder> OpenOrders1 = new Dictionary<int, TradingOrder>();
            Dictionary<int, TradingOrder> OpenOrders2 = new Dictionary<int, TradingOrder>();
            Dictionary<int, TradingOrder> OpenOrders3 = new Dictionary<int, TradingOrder>();
            Dictionary<int, TradingOrder> OpenOrders4 = new Dictionary<int, TradingOrder>();
            Dictionary<int, TradingOrder> OpenOrders5 = new Dictionary<int, TradingOrder>();
            PriceCandle candle = new PriceCandle(DateTimeUTC.Now, TimeScale.Second1, 1, 1.2, 0.8, 1, spread);
  

            TradingOrder order_0 = new TradingOrder(TradingOrderType.Long, 0, DateTimeUTC.Now, 1, 1, 0.01, true, 0.7, 1.1);
            OpenOrders0.Add(order_0.OrderTicket, order_0);
            TradingOrder order_1 = new TradingOrder(TradingOrderType.Long, 0, DateTimeUTC.Now, 1, 1, 0.01, true, 0.9, 1.3);
            OpenOrders1.Add(order_0.OrderTicket, order_1);
            TradingOrder order_2 = new TradingOrder(TradingOrderType.Short, 0, DateTimeUTC.Now, 1, 1, 0.01, true, 1.1, 0.7);
            OpenOrders2.Add(order_0.OrderTicket, order_2);
            TradingOrder order_3 = new TradingOrder(TradingOrderType.Short, 0, DateTimeUTC.Now, 1, 1, 0.01, true, 1.3, 0.9);
            OpenOrders3.Add(order_0.OrderTicket, order_3);
            TradingOrder order_4 = new TradingOrder(TradingOrderType.Long, 0, DateTimeUTC.Now, 1, 1, 0.01, true, 0.7, 1.3);
            OpenOrders4.Add(order_0.OrderTicket, order_4);
            TradingOrder order_5 = new TradingOrder(TradingOrderType.Short, 0, DateTimeUTC.Now, 1, 1, 0.01, true, 1.3, 0.7);
            OpenOrders5.Add(order_0.OrderTicket, order_5);

            Assert.AreEqual(1, MarketModelSimulation.CheckOrderLimits(OpenOrders0, candle).Count);
            Assert.AreEqual(1, MarketModelSimulation.CheckOrderLimits(OpenOrders1, candle).Count);
            Assert.AreEqual(1, MarketModelSimulation.CheckOrderLimits(OpenOrders2, candle).Count);
            Assert.AreEqual(1, MarketModelSimulation.CheckOrderLimits(OpenOrders3, candle).Count);
            Assert.AreEqual(0, MarketModelSimulation.CheckOrderLimits(OpenOrders4, candle).Count);
            Assert.AreEqual(0, MarketModelSimulation.CheckOrderLimits(OpenOrders5, candle).Count);

        }
    }
}
