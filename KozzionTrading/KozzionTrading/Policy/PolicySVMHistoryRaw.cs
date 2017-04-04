using System;
using System.Collections.Generic;
using KozzionTrading.Market;
using KozzionTrading.Policy;
using KozzionMachineLearning.Model;
using KozzionMachineLearning.Method.SupportVectorMachine;
using KozzionMachineLearning.DataSet;
using KozzionTrading.Indicators;

namespace KozzionTrading.Policy
{
    internal class PolicySVMHistoryRaw : IPolicy
    {
   
        private int history_count;
        private List<PriceCandle> training_data;
        IModelLikelihood<double, int, double> model;

        public PolicySVMHistoryRaw(List<PriceCandle> training_data,  int history_count, double c, double gamma)
        {
            this.history_count = history_count;
            this.training_data = training_data;
            ITemplateModelLikelihood<double, int> template = new TemplateModelLibSVMCSVC(c, gamma);
            IDataContext data_context_labeled = null;

            double[][] feature_data = null;

            IIndicator indicator = new IndicatorMagicProfit(60);
            //MarketModel model = new MarketModel(100000, training_data[0].Open, );
            //indicator.ComputeAll();
            int[][] label_data = null;
            IDataSet<double, int> training_set = new DataSet<double, int>(data_context_labeled, feature_data, label_data);
            model = template.GenerateModelLikelihood(training_set);
        }

        public string Title
        {
            get
            {
                return "PolicySVMHistoryRaw";
            }
        }

        public void GetTradeOrderCommand(IMarketModelSimulation market_model)
        {
        
            //double[] features = new double[history_count];
            //for (int index = 0; index < history_count; index++)
            //{
            //    features[index] = market_model.Second1[-index].CloseBid;
            //}
            //double [] likelyhoods = model.GetLikelihoods(features);


        }
    }
}