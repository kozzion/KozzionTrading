using KozzionCore.Tools;
using KozzionPlotting.Tools;
using KozzionTrading.Indicators;
using KozzionTrading.Market;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KozzionTrading.Policy;

namespace KozzionTrading.Tools
{
    public class ToolsTradingPlotting
    {
        public static int SIZE_X = 3000;
        public static int SIZE_Y = 1200;

        public static Color COLOR_INDICATOR = Color.Black;
        public static Color COLOR_BID = Color.LightBlue;
        public static Color COLOR_ASK = Color.LightGreen;
        public static Color COLOR_LONG = Color.Red;
        public static Color COLOR_SHORT = Color.Purple;

        public static Color COLOR_CASH = Color.Blue;
        public static Color COLOR_EQUITY = Color.Black;


        public static void PlotIndicatorResult(string path, PriceSet price_set, IIndicator indicator)
        {
            IList<Color> color_list = new List<Color>();

            IList<int> indicator_list = new List<int>();
            for (int index = 0; index < indicator.SubIndicatorCount; index++)
            {
                color_list.Add(Color.Black);
                indicator_list.Add(index);
            }
            PlotIndicatorResult(path, price_set, indicator, color_list, indicator_list);
        }

        public static void PlotIndicatorResult(string path, PriceSet price_set, IIndicator indicator, IList<Color> color_list, IList<int> selected_subindicators,bool plot_bid = true)
        {
            PlotLine2D line_plot_2d = new PlotLine2D();
            AddIndicatorResult(line_plot_2d, price_set, indicator, color_list, selected_subindicators);
            if (plot_bid)
            {
                AddBid(line_plot_2d, price_set);
            }            
            ToolsPlotting.WriteToFile(path, line_plot_2d, SIZE_X, SIZE_Y);
        }



        public static void AddIndicatorResult(PlotLine2D line_plot_2d, PriceSet price_set, IIndicator indicator, IList<Color> color_list, IList<int> selected_subindicators)
        {
            MarketModelSimulation market = new MarketModelSimulation(10000, price_set);
            double[] time = new double[price_set.Prices.Count];
            for (int price_index = 0; price_index < price_set.Prices.Count; price_index++)
            {
                time[price_index] = price_set.Prices[price_index].Time.Ticks;
            }
            Tuple<double[,], bool[]> tuple = indicator.ComputeAll(market, price_set.Second1.Count);
            List<IList<int>> selections = CreateSections(tuple.Item2);
            for (int index = 0; index < selected_subindicators.Count; index++)
            {
                double[] signal = tuple.Item1.Select1DIndex1(selected_subindicators[index]);
                AddSignal(line_plot_2d, time, signal, selections, color_list[index]);
            }
        }


        public static void PlotPriceBid(string path, PriceSet price_set)
        {
            PlotLine2D line_plot_2d = new PlotLine2D();
            AddBid(line_plot_2d, price_set);
            ToolsPlotting.WriteToFile(path, line_plot_2d, SIZE_X, SIZE_Y);
        }



        public static void AddPrice(PlotLine2D line_plot_2d, PriceSet price_set, TimeScale time_scale, PriceType price_type, Color color)
        {
            if (time_scale == TimeScale.Spot)
            {
                AddPriceSpot(line_plot_2d, price_set, price_type, color);
            }
            else
            {
                AddPriceCandle(line_plot_2d, price_set, time_scale, price_type, color);
            }
        }

        public static void AddPriceSpot(PlotLine2D line_plot_2d, PriceSet price_set, PriceType price_type, Color color)
        {
            IReadOnlyList<Price> prices = price_set.Prices;
            IReadOnlyList<double> time = price_set.Ticks;
            double[] signal = new double[prices.Count];
            Parallel.For(0, prices.Count, index =>
            {
                signal[index] = prices[index].GetPrice(price_type);
            });
            AddSignal(line_plot_2d, time, signal, color);
        }

        public static void AddPriceCandle(PlotLine2D line_plot_2d, PriceSet price_set, TimeScale time_scale, PriceType price_type, Color color)
        {
            IReadOnlyList<PriceCandle> candles = price_set.GetCandles(time_scale);
            IReadOnlyList<double> time = price_set.Ticks;
            double[] signal = new double[candles.Count];
            Parallel.For(0, candles.Count, index =>
            {
                signal[index] = candles[index].GetPrice(price_type);
            });
            AddSignal(line_plot_2d, time, signal, color);
        }

        private static void AddSignal(PlotLine2D line_plot_2d, IReadOnlyList<double> time, IReadOnlyList<double> signal, Color color)
        {
             line_plot_2d.AddSeries(time, signal, color);
        }




        private static List<IList<int>> CreateSections(IList<bool> is_valid)
        {
            List<IList<int>> selections = new List<IList<int>>();
            if (is_valid[0])
            {
                selections.Add(new List<int>());
                selections.Last().Add(0);
            }
            for (int index = 1; index < is_valid.Count; index++)
            {
                if (is_valid[index])
                { 
                    if (!is_valid[index - 1])
                    {
                        selections.Add(new List<int>());
                    }
                    selections.Last().Add(index);
                }
            }
            return selections;
        }

        public static void PlotMarketResult(string path, MarketResult market_result)
        {
            PlotLine2D line_plot_2d = new PlotLine2D();
            AddBidAskTrades(line_plot_2d, market_result);
            ToolsPlotting.WriteToFile(path, line_plot_2d, SIZE_X, SIZE_Y);
        }



        public static void AddBidAsk(PlotLine2D line_plot_2d, PriceSet price_set)
        {
            AddBid(line_plot_2d, price_set);
            AddAsk(line_plot_2d, price_set);
        }

        public static void AddBid(PlotLine2D line_plot_2d, PriceSet price_set)
        {
            AddPrice(line_plot_2d, price_set, TimeScale.Spot, PriceType.Bid, COLOR_BID);
        }

        public static void AddAsk(PlotLine2D line_plot_2d, PriceSet price_set)
        {
            AddPrice(line_plot_2d, price_set, TimeScale.Spot, PriceType.Ask, COLOR_ASK);
        }

        public static void AddBidAskTrades(PlotLine2D line_plot_2d, MarketResult market_result)
        {
            AddBidAsk(line_plot_2d, market_result.PriceSet);
            AddTrades(line_plot_2d, market_result);
        }

        public static void AddCashEquity(PlotLine2D line_plot_2d, MarketResult market_result)
        {
            IReadOnlyList<double> time = market_result.Ticks;
            IReadOnlyList<double> cash = market_result.Cash;
            IReadOnlyList<double> equity = market_result.Equity;
            AddSignal(line_plot_2d, time, cash, COLOR_CASH);
            AddSignal(line_plot_2d, time, equity, COLOR_EQUITY);
        }

        public static void AddTrades(PlotLine2D line_plot_2d, MarketResult market_result)
        {
            List<Tuple<double[], double[], Color>> series_list = new List<Tuple<double[], double[], Color>>();
            for (int orderIndex = 0; orderIndex < market_result.Market.ClosedOrders.Count; orderIndex++)
            {
                TradingOrder closed_order = market_result.Market.ClosedOrders[orderIndex];
                double[] open_close_time = new double[] { closed_order.OpenDateTime.Ticks, closed_order.CloseDateTime.Ticks };
                double[] open_close_price = new double[] { closed_order.OpenPrice, closed_order.ClosePrice };            
                if (closed_order.OrderType == TradingOrderType.Long)
                {
                    series_list.Add(new Tuple<double[], double[], Color>(open_close_time, open_close_price, COLOR_LONG));
                }
                else
                {
                    series_list.Add(new Tuple<double[], double[], Color>(open_close_time, open_close_price, COLOR_SHORT));
                }             
            }
            line_plot_2d.AddSeries(series_list);
        }


        private static void AddSignal(PlotLine2D line_plot_2d, double[] time, double[] signal, List<IList<int>> selections, Color color)
        {
            List<Tuple<double[], double[], Color>> series_list = new List<Tuple<double[], double[], Color>>();

            foreach (IList<int> selection in selections)
            {
                series_list.Add(new Tuple<double[], double[], Color>(
                    time.Select(selection),
                    signal.Select(selection),
                    color));
            }

            line_plot_2d.AddSeries(series_list);
        }

        public static void WriteToFile(string path, PlotLine2D line_plot_2d)
        {
            ToolsPlotting.WriteToFile(path, line_plot_2d, SIZE_X, SIZE_Y);
        }
    }
}
