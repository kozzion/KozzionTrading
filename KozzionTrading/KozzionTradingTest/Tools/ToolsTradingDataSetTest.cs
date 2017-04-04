using KozzionCore.Tools;
using KozzionMachineLearning.DataSet;
using KozzionTrading.Indicators;
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
    public class ToolsTradingDataSetTest
    {


        [TestMethod]
        public void TestCreateSmall()
        {
            //Prices 1.0  0.9, 1.1, 1.2
            List<Price> prices = new List<Price>();
            prices.Add(new Price(new DateTimeUTC(2000, 1, 1, 0, 0, 0), 1.0, 1.1));
            prices.Add(new Price(new DateTimeUTC(2000, 1, 1, 0, 0, 1), 0.9, 1.0));
            prices.Add(new Price(new DateTimeUTC(2000, 1, 1, 0, 0, 2), 1.1, 1.2));
            prices.Add(new Price(new DateTimeUTC(2000, 1, 1, 0, 0, 3), 1.2, 1.3));
            PriceSet price_set = new PriceSet(ToolsPrice.DefaultSymbolGBPUSD, prices.AsReadOnly());

            List<IIndicator> indicators_features = new List<IIndicator>();
            indicators_features.Add(new IndicatorRunningAverage(0));
            indicators_features.Add(new IndicatorRunningAverage(1));
            List<IIndicator> indicators_labels = new List<IIndicator>();
            indicators_labels.Add(new IndicatorMagicProfit(0));
            indicators_labels.Add(new IndicatorMagicProfit(1));
            MarketModelSimulation market = new MarketModelSimulation(1000, price_set);
            IDataSet<double, double> dataset = ToolsTradingDataSet.CreateDataSet(market, new IndicatorFusion(indicators_features), new IndicatorFusion(indicators_labels));
            Assert.AreEqual(0.90, dataset.FeatureData[0][0], 0.0000001);
            Assert.AreEqual(0.95, dataset.FeatureData[0][1], 0.0000001);
            Assert.AreEqual(1.10, dataset.FeatureData[1][0], 0.0000001);
            Assert.AreEqual(1.00, dataset.FeatureData[1][1], 0.0000001);
            Assert.AreEqual(-0.1, dataset.LabelData[0][0], 0.0000001);
            Assert.AreEqual(-0.1, dataset.LabelData[0][1], 0.0000001);
            Assert.AreEqual(0.10, dataset.LabelData[0][2], 0.0000001);
            Assert.AreEqual(-0.1, dataset.LabelData[0][3], 0.0000001);
            Assert.AreEqual(-0.1, dataset.LabelData[1][0], 0.0000001);
            Assert.AreEqual(-0.1, dataset.LabelData[1][1], 0.0000001);
            Assert.AreEqual(0.0, dataset.LabelData[1][2], 0.0000001);
            Assert.AreEqual(0.0, dataset.LabelData[1][3], 0.0000001);
        }

        [TestMethod]
        public void TestCreateMedium()
        {
            PriceSet price_set = new PriceSet(ToolsPrice.DefaultSymbolGBPUSD, ToolsCollection.Select(ToolsPrice.GetPriceList(ToolsPrice.DefaultSymbolGBPUSD), 0, 40));
            List<IIndicator> indicators_features = new List<IIndicator>();
            indicators_features.Add(new IndicatorRunningAverage(0));
            indicators_features.Add(new IndicatorRunningAverage(1));
            List<IIndicator> indicators_labels = new List<IIndicator>();
            indicators_labels.Add(new IndicatorMagicProfit(0));
            indicators_labels.Add(new IndicatorMagicProfit(1));
            MarketModelSimulation market = new MarketModelSimulation(1000, price_set);
            IDataSet<double, double> dataset = ToolsTradingDataSet.CreateDataSet(market, new IndicatorFusion(indicators_features), new IndicatorFusion(indicators_labels));
            Assert.AreEqual(39, dataset.InstanceCount);
            Assert.AreEqual(2, dataset.FeatureCount);
            Assert.AreEqual(4, dataset.LabelCount);
        }
    }
}
