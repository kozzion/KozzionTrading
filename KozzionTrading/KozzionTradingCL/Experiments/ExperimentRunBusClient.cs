using KozzionTrading.Network;
using KozzionTrading.Network.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace KozzionTradingCL.Experiments
{
    public class ExperimentRunBusClient : IExperiment
    {
        public void DoExperiment()
        {
            TradingBusClient client = new TradingBusClient("127.0.0.1", 8128);
            ResponseAccountOnTick response_0 = client.OnTick(DateTime.Now, DateTime.Now, 1, 2);
            Console.WriteLine(response_0.Command);
        }
    }
}
