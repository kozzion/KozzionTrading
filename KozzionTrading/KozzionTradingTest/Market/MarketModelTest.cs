using KozzionCore.Tools;
using KozzionTrading.Market;
using KozzionTrading.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KozzionTradingTest.Market
{
    [TestClass]
    public class MarketModelTest
    {


        [TestMethod]
        public void TestSim1()
        {
            PriceSet price_set = ToolsPrice.GetPriceSet(ToolsPrice.DefaultSymbolGBPUSD);
            IMarketModelTest.TestSpeed(10000, price_set, 1);
        }


        [TestMethod]
        public void TestSim40()
        {
            PriceSet price_set = ToolsPrice.GetPriceSet(ToolsPrice.DefaultSymbolGBPUSD);
            IMarketModelTest.TestSpeed(10000, price_set, 40);
        }


        [TestMethod]
        public void TestStep()
        {
            List<Price> prices = new List<Price>();
            prices.Add(new Price(new DateTimeUTC(2000, 1, 1, 0, 0, 0), 1.0, 1.1));
            prices.Add(new Price(new DateTimeUTC(2000, 1, 1, 0, 0, 1), 0.9, 1.0));
            prices.Add(new Price(new DateTimeUTC(2000, 1, 1, 0, 0, 2), 1.1, 1.2));
            PriceSet price_set = new PriceSet(ToolsPrice.DefaultSymbolGBPUSD, prices.AsReadOnly());
            MarketModelSimulation market = new MarketModelSimulation(1000, price_set);
            Assert.AreEqual(1.0, market.CurrentBid, 0.0000001);
            Assert.AreEqual(1.1, market.CurrentAsk, 0.0000001);
            market.StepSecond();
            Assert.AreEqual(0.9, market.CurrentBid, 0.0000001);
            Assert.AreEqual(1.0, market.CurrentAsk, 0.0000001);
            market.StepSecond();
            Assert.AreEqual(1.1, market.CurrentBid, 0.0000001);
            Assert.AreEqual(1.2, market.CurrentAsk, 0.0000001);
        }

        [TestMethod]
        public void ManualCloseLongTest()
        {
            double initial_cash = 1000;
            double commission_per_unit = 0.01;
            List<Price> prices = new List<Price>();
            prices.Add(new Price(new DateTimeUTC(2000, 0, 0, 0, 0, 0), 1.0, 1.1));
            prices.Add(new Price(new DateTimeUTC(2000, 0, 0, 0, 0, 1), 2.0, 2.1));
            PriceSet price_set = new PriceSet(ToolsPrice.DefaultSymbolGBPUSD, prices.AsReadOnly());
            MarketModelSimulation market = new MarketModelSimulation(initial_cash, commission_per_unit, price_set);
            market.ProcessOrderCommand(new TradingOrderCommand(TradingOrderType.Long, 1.0, 1.0, 0.0));
            market.StepSecond();

            List<int> open_orders = new List<int>(market.OpenOrders.Keys);
            foreach (int open_order in open_orders)
            {
                market.CloseOrder(open_order);
            }
            Assert.AreEqual(1000.89, market.Cash);
        }


        [TestMethod]
        public void ManualCloseShortTest()
        {
            double initial_cash = 1000;
            double commission_per_unit = 0.01;
            List<Price> prices = new List<Price>();
            prices.Add(new Price(new DateTimeUTC(2000, 0, 0, 0, 0, 0), 1.0, 1.1));
            prices.Add(new Price(new DateTimeUTC(2000, 0, 0, 0, 0, 1), 2.0, 2.1));
            PriceSet price_set = new PriceSet(ToolsPrice.DefaultSymbolGBPUSD, prices.AsReadOnly());

            MarketModelSimulation market = new MarketModelSimulation(initial_cash, commission_per_unit, price_set);

            market.ProcessOrderCommand(new TradingOrderCommand(TradingOrderType.Short, 1.0, 1.0, 0.0));
            List<int> open_orders = new List<int>(market.OpenOrders.Keys);
            foreach (int open_order in open_orders)
            {
                market.CloseOrder(open_order);
            }
            Assert.AreEqual(998.89, market.Cash);            
        }


        [TestMethod]
        public void LimitCloseLongProfitTest()
        {
            double initial_cash = 1000;
            double commission_per_unit = 0.01;
            List<Price> prices = new List<Price>();
            prices.Add(new Price(new DateTimeUTC(2000, 0, 0, 0, 0, 0), 1.0, 1.1));
            prices.Add(new Price(new DateTimeUTC(2000, 0, 0, 0, 0, 1), 1.4, 1.5));
            prices.Add(new Price(new DateTimeUTC(2000, 0, 0, 0, 0, 2), 2.0, 2.1));
            PriceSet price_set = new PriceSet(ToolsPrice.DefaultSymbolGBPUSD, prices.AsReadOnly());
            MarketModelSimulation market = new MarketModelSimulation(initial_cash, commission_per_unit, price_set);

            market.ProcessOrderCommand(new TradingOrderCommand(TradingOrderType.Long, 1.0, 1.0, 0.0, 0.9, 1.6));

            market.StepSecond();
            List<int> open_orders_0 = new List<int>(market.OpenOrders.Keys);
            Assert.AreEqual(1, open_orders_0.Count);
            market.StepSecond();
            List<int> open_orders_1 = new List<int>(market.OpenOrders.Keys);
            Assert.AreEqual(0, open_orders_1.Count);

            Assert.AreEqual(1000.49, market.Cash);
        }

        [TestMethod]
        public void LimitCloseLongLossTest()
        {
            double initial_cash = 1000;
            double commission_per_unit = 0.01;
            List<Price> prices = new List<Price>();
            prices.Add(new Price(new DateTimeUTC(2000, 0, 0, 0, 0, 0), 1.0, 1.1));
            prices.Add(new Price(new DateTimeUTC(2000, 0, 0, 0, 0, 1), 1.4, 1.5));
            prices.Add(new Price(new DateTimeUTC(2000, 0, 0, 0, 0, 2), 0.5, 0.6));
            PriceSet price_set = new PriceSet(ToolsPrice.DefaultSymbolGBPUSD, prices.AsReadOnly());
            MarketModelSimulation market = new MarketModelSimulation(initial_cash, commission_per_unit, price_set);

            market.ProcessOrderCommand(new TradingOrderCommand(TradingOrderType.Long, 1.0, 1.1, 0.0, 0.9, 1.6));
            market.StepSecond();
            List<int> open_orders_0 = new List<int>(market.OpenOrders.Keys);
            Assert.AreEqual(1, open_orders_0.Count);
            market.StepSecond();
            List<int> open_orders_1 = new List<int>(market.OpenOrders.Keys);
            Assert.AreEqual(0, open_orders_1.Count);

            Assert.AreEqual(999.79, market.Cash);
        }
    }
}
