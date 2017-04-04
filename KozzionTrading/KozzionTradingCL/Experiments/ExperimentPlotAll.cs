using KozzionCore.Collections;
using KozzionCore.IO.CSV;
using KozzionTrading.Market;
using KozzionTrading.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Globalization;
using System.Drawing;
using KozzionPlotting.Tools;
using KozzionTrading.Indicators;
using KozzionTrading.Policy;
using KozzionCore.Tools;

namespace KozzionTradingCL.Experiments
{
    public class ExperimentPlotAll
    {
        public ExperimentPlotAll()
        {
      
        }

        internal void DoExperiment()
        {
            ToolsPrice.ComposeBinary(ToolsPrice.DefaultSymbolGBPUSD);
            PriceSet price_set = ToolsPrice.GetPriceSet(ToolsPrice.DefaultSymbolGBPUSD).SubSet(new DateTimeUTC(2016, 10, 17), new DateTimeUTC(2016, 10, 20));

            //PlotLine2D line_plot_2d_0 = new PlotLine2D();
            //ToolsTradingPlotting.AddPrice(line_plot_2d_0, price_set, TimeScale.Spot, PriceType.Bid, Color.Blue);
            //ToolsTradingPlotting.AddPrice(line_plot_2d_0, price_set, TimeScale.Spot, PriceType.Mean, Color.Green);
            //ToolsTradingPlotting.AddPrice(line_plot_2d_0, price_set, TimeScale.Spot, PriceType.Ask, Color.Red);
            //ToolsTradingPlotting.AddPrice(line_plot_2d_0, price_set, TimeScale.Day1, PriceType.Bid, Color.Black);
            //ToolsTradingPlotting.AddPrice(line_plot_2d_0, price_set, TimeScale.Day1, PriceType.Mean, Color.Green);
            //ToolsTradingPlotting.AddPrice(line_plot_2d_0, price_set, TimeScale.Day1, PriceType.Ask, Color.Red);      
            //ToolsTradingPlotting.WriteToFile(ToolsTradingDataSet.GetPath() + "TestPlot0.png", line_plot_2d_0);

            IIndicator indicator = new IndicatorMagicProfit(60);
            PlotLine2D line_plot_2d_1 = new PlotLine2D();
            ToolsTradingPlotting.AddIndicatorResult(line_plot_2d_1, price_set, indicator, new Color[] { Color.Red, Color.Green }, new int[] {0, 1 });
            ToolsTradingPlotting.WriteToFile(ToolsTradingDataSet.GetPath() + "TestPlot3.png", line_plot_2d_1);



            //ToolsTradingPlotting.PlotPriceBid(ToolsTradingDataSet.GetPath() + "prices.png", prices);
            //ToolsTradingPlotting.PlotIndicatorResult(ToolsTradingDataSet.GetPath() + "indicator_0.png", prices, indicator_0, color_list, index_list, true);
            //ToolsTradingPlotting.PlotIndicatorResult(ToolsTradingDataSet.GetPath() + "indicator_1.png", prices, indicator_1, color_list, index_list, true);


            //IPolicyTemplate policy = new PolicyTemplateJaapBands();
            //MarketManagerSimulation exchange = new MarketManagerSimulation(10000, price_set);
            //MarketResult market_result = exchange.Run(policy.Instance());
            //Console.WriteLine(market_result.EndCash + " in: " + market_result.Market.ClosedOrders.Count);

            //PlotLine2D line_plot_2d_0 = new PlotLine2D();
            //ToolsTradingPlotting.AddBidAskTrades(line_plot_2d_0, market_result);
            //ToolsTradingPlotting.WriteToFile(ToolsTradingDataSet.GetPath() + "TestPlot0.png", line_plot_2d_0);


            //PlotLine2D line_plot_2d_1 = new PlotLine2D();
            //ToolsTradingPlotting.AddCashEquity(line_plot_2d_1, market_result);
            //ToolsTradingPlotting.WriteToFile(ToolsTradingDataSet.GetPath() + "TestPlot1.png", line_plot_2d_1);
        }
    }
}