using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KozzionTrading.Market;
using KozzionTrading.Indicators;
using KozzionTrading.Tools;
using KozzionTrading.Optimizer;

namespace KozzionTrading.Policy
{
    public class PolicyJaapBands : IPolicy
    {
        public string Title
        {
            get
            {
                return "JaapBands";
            }
        }

        IIndicator indicator_0;

        public double stop_loss { get; private set; }
        public double take_profit { get; private set; }
        public int varriance_history_size { get; private set; }
        public double momentum_weight { get; private set; }
        public double expected_error_z { get; private set; }
        public double max_error_weight { get; private set; }
        public double threshold_long { get; private set; }
        public double threshold_short { get; private set; }
   

        public PolicyJaapBands(int varriance_history_size, double momentum_weight, double expected_error_z, double max_error_weight, double stop_loss, double take_profit, double threshold_long, double threshold_short)
        {
            this.indicator_0 = new IndicatorSuperBollinger(varriance_history_size, momentum_weight, expected_error_z, max_error_weight);
            this.varriance_history_size = varriance_history_size;
            this.momentum_weight = momentum_weight;
            this.expected_error_z = expected_error_z;
            this.max_error_weight = max_error_weight;
            this.stop_loss = stop_loss;
            this.take_profit = take_profit;
            this.threshold_long = threshold_long;
            this.threshold_short = threshold_short;
        }

        public PolicyJaapBands(ParameterSet parameter_set)
            : this(
                  parameter_set["varriance_history_size"].Int32Value,
                  parameter_set["momentum_weight"].Float64Value,
                  parameter_set["expected_error_z"].Float64Value,
                  parameter_set["max_error_weight"].Float64Value,
                  parameter_set["stop_loss"].Float64Value,
                  parameter_set["take_profit"].Float64Value,
                  parameter_set["threshold_long"].Float64Value,
                  parameter_set["threshold_short"].Float64Value)
        {


        }

        public PolicyJaapBands(int varriance_history_size, double momentum_weight, double expected_error_z, double max_error_weight)
            :this(varriance_history_size, momentum_weight, expected_error_z, max_error_weight, 50, 50, 10 * Math.Pow(10, -6), -10 * Math.Pow(10, -6))
        {
           
        }

        public PolicyJaapBands(int varriance_history_size, double momentum_weight, double expected_error_z, double max_error_weight, double stop_loss, double take_profit)
      : this(varriance_history_size, momentum_weight, expected_error_z, max_error_weight, stop_loss, take_profit, 10 * Math.Pow(10, -6), -10 * Math.Pow(10, -6))
        {

        }

        public void GetTradeOrderCommand(IMarketModelSimulation market_model)
        {
            Tuple<double[],bool> indicator_0_Tuple = indicator_0.Compute(market_model);
            Tuple<double[], bool> indicator_0_Tuple_Prev = indicator_0_Tuple;

            if(indicator_0_Tuple.Item2 && market_model.OpenOrders.Count == 0)
            {
                //   if (market_model.CurrentAsk < indicator_0_Tuple.Item1[2] && market_model.Second1[-1].CloseAsk < indicator_0_Tuple_Prev.Item1[2])

                if (threshold_long < indicator_0_Tuple.Item1[7])
                {
                    market_model.ProcessOrderCommand(new TradingOrderCommand(TradingOrderType.Long, TradingConstants.LOT, market_model.CurrentAsk, 0
                        , market_model.CurrentAsk - stop_loss * TradingConstants.POINT, market_model.CurrentAsk + take_profit * TradingConstants.POINT));
                }
                if (threshold_short > indicator_0_Tuple.Item1[7])
                {
                    market_model.ProcessOrderCommand(new TradingOrderCommand(TradingOrderType.Short, TradingConstants.LOT, market_model.CurrentBid, 0
                        , market_model.CurrentBid + stop_loss * TradingConstants.POINT, market_model.CurrentBid - take_profit * TradingConstants.POINT));
                }
            }

            TrailingStopLoss(market_model, TradingConstants.POINT * 150);            
        }

        public void TrailingStopLoss(IMarketModelSimulation market_model,double stoplossTrail)
        {
            if (market_model.OpenOrders.Count > 0)
            {
                List<int> openOrders = new List<int>(market_model.OpenOrders.Keys);
                foreach (int selectedOrder in openOrders)
                {
                    var tradingOrder = market_model.OpenOrders[selectedOrder];

                    if (tradingOrder.OrderType == TradingOrderType.Long &&
                         stoplossTrail < market_model.CurrentBid - tradingOrder.StopLoss)
                    {
                        market_model.ModifyOrder(selectedOrder, market_model.CurrentBid - stoplossTrail, tradingOrder.TakeProfit);
                    }
                    if (tradingOrder.OrderType == TradingOrderType.Short &&
                        stoplossTrail < tradingOrder.StopLoss - market_model.CurrentAsk)
                    {
                        market_model.ModifyOrder(selectedOrder, market_model.CurrentAsk + stoplossTrail, tradingOrder.TakeProfit);
                    }
                }

            }
        }

        //public void OpenFirst(MarketModelSimulationSecond1 market_model)
        //{
        //    //Long short criterion
        //    double upper_limit = market_model.CurrentAsk + 0;
        //    double lower_limit = market_model.CurrentAsk - 0;

        //    if (BuyLong())
        //    {            
        //        market_model.SendOrder(new TradingOrderCommand(TradingOrderType.Long, market_model.CurrentAsk, TradingConstants.LOT, 0, lower_limit, upper_limit));
        //    }
        //    else
        //    {
        //        market_model.SendOrder(new TradingOrderCommand(TradingOrderType.Short, market_model.CurrentBid, TradingConstants.LOT, 0, upper_limit, lower_limit));
        //    }
        //}

        //public void CheckSwap(MarketModelSimulationSecond1 market_model)
        //{
        //}

        //private bool BuyLong()
        //{
        //    return true;
        //}
    }
}
