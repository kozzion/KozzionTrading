using KozzionCore.Tools;
using KozzionMachineLearning.DataSet;
using KozzionMachineLearning.Method.NearestNeighbor;
using KozzionMachineLearning.Model;
using KozzionMathematics.Tools;
using KozzionTrading.Indicators;
using KozzionTrading.Market;
using KozzionTrading.Tools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KozzionTradingCL.Experiments
{
    public class ExperimentIndicatorML : IExperiment

    {
        public void DoExperiment()
        {
            PriceSet price_set = ToolsPrice.GetPriceSet(ToolsPrice.DefaultSymbolGBPUSD);
            List<IIndicator> indicators = new List<IIndicator>();
            indicators.Add(new IndicatorRunningAverage(4));
            indicators.Add(new IndicatorRunningAverage(6));
            indicators.Add(new IndicatorRunningAverage(8));
            indicators.Add(new IndicatorRunningAverage(10));
            indicators.Add(new IndicatorRunningAverage(12));
            IIndicator feature_indicator = new IndicatorFusion(indicators);
            IIndicator label_indicator = new IndicatorMagicProfit(60);



            MarketModelSimulation market = new MarketModelSimulation(1000, price_set);
            DataSet<double, double> dataset = ToolsTradingDataSet.CreateDataSet(market, feature_indicator, label_indicator);
            ITemplateModelLabel<double, double> template = null;//new TemplateModelLibSVMCSVC();
            IModelLabel<double, double> model = template.GenerateModel(dataset);

            //TODO change everyting into templates
            List<IIndicator> indicators_2 = new List<IIndicator>();
            indicators_2.Add(new IndicatorRunningAverage(4));
            indicators_2.Add(new IndicatorRunningAverage(6));
            indicators_2.Add(new IndicatorRunningAverage(8));
            indicators_2.Add(new IndicatorRunningAverage(10));
            indicators_2.Add(new IndicatorRunningAverage(12));
            IIndicator feature_indicator_2 = new IndicatorFusion(indicators_2);

            //Build actual incator

            IIndicator indicator_0 = new IndicatorMagicProfit(60);
            IIndicator indicator_1 = new IndicatorMachineLearning(feature_indicator_2, model, "Profit_long_ml");
            IList<Color> color_list = new Color[] { Color.Black };
            IList<int> index_list = new int[] { 0 };
            ToolsTradingPlotting.PlotIndicatorResult(ToolsTradingDataSet.GetPath() + "indicator_0.png", price_set, indicator_0, color_list, index_list, false);
            ToolsTradingPlotting.PlotIndicatorResult(ToolsTradingDataSet.GetPath() + "indicator_1.png", price_set, indicator_1, color_list, index_list, false);
        }
    }
}
