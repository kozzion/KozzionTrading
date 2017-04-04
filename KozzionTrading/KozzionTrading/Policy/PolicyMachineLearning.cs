using KozzionMachineLearning.DataSet;
using KozzionMachineLearning.Model;
using KozzionMathematics.Tools;
using KozzionTrading.Indicators;
using KozzionTrading.Market;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KozzionTrading.Policy
{
    public class PolicyMachineLearning :IPolicy
    {
        public string Symbol { get; private set; }

        public string Title
        {
            get
            {
                return "Machine Learning";
            }
        }

        private IIndicator          indicator;
        private IModelLabel<double, double> model;


        public PolicyMachineLearning(
            IIndicator indicator_ml, 
            ITemplateModelLikelihood<double, int> template)
        {
            //this.indicators = new List<IIndicator>(indicator_ml);

            ////Create Dataset
            //IDataSet<double, int> dataset = null;

            //// build model
            //this.model = template.GenerateModelLikelihood(dataset);


        }

 

        public void GetTradeOrderCommand(IMarketModelSimulation market)
        {
            Tuple<double[], bool> tuple = indicator.Compute(market);
  

        }
    }
}
