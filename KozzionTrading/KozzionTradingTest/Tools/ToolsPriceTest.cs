using KozzionTrading.Market;
using KozzionTrading.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KozzionTradingTest.Tools
{
    [TestClass]
    public class ToolsPriceTest
    {
       

        [TestMethod]
        public void TestComposeSource()
        {
            ToolsPrice.ComposeSource();
        }

        [TestMethod]
        public void TestComposeBinary()
        {
            ToolsPrice.ComposeBinary(ToolsPrice.DefaultSymbolGBPUSD);
        }

        [TestMethod]
        public void TestGetPriceCandles()
        {
            ToolsPrice.GetPriceCandles(ToolsPrice.DefaultSymbolGBPUSD, TimeScale.Second1);
        }

        [TestMethod]
        public void TestGetPriceSet()
        {
            PriceSet price_set = ToolsPrice.GetPriceSet(ToolsPrice.DefaultSymbolGBPUSD);
        }

    }
}
