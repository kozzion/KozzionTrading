using KozzionCore.Tools;
using KozzionPlotting.Tools;
using KozzionTrading.IO;
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
    public class ExperimentPlotEventNFP : IExperiment
    {
        public void DoExperiment()
        {
            List<PriceCandle> all_data = ToolsPrice.GetPriceCandles();

            // new DateTime(2016, 9, 3)
            PlotLine2D line_plot_2d = new PlotLine2D();
            

            for (int month_index = 1; month_index < 9; month_index++)
            {
                DateTimeUTC nfp_date = null;// ToolsTradingCalendarEvent.GetNFPDatetime(2016, month_index);

                DateTimeUTC nfp_date_begin = nfp_date.AddMinutes(-100);
                DateTimeUTC nfp_date_end = nfp_date.AddMinutes(100);
                List<double> nfp_time = new List<double>();
                List<double> nfp_bid = new List<double>();

                double candle_time = 0;
                double subtract = 0;
                foreach (PriceCandle candle in all_data)
                {

                    if ((nfp_date_begin <= candle.OpenTime) && (candle.OpenTime < nfp_date_end))
                    {
                        if (candle_time == 0)
                        {
                            subtract = candle.OpenBid;
                        }
                        nfp_time.Add(candle_time);
                        nfp_time.Add(candle_time);
                        nfp_time.Add(candle_time);
                        nfp_time.Add(candle_time);
                        nfp_bid.Add(candle.OpenBid);
                        nfp_bid.Add(candle.LowBid);
                        nfp_bid.Add(candle.HighBid);
                        nfp_bid.Add(candle.CloseBid);
                        candle_time++;
                    }
                }
                line_plot_2d.AddSeries(nfp_time, nfp_bid, Color.Black);
            }
            ToolsPlotting.WriteToFile(ToolsTradingDataSet.GetPath() + "nfp.png", line_plot_2d, 800, 800);

        }
    }
}
