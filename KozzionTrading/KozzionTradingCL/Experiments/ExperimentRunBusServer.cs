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
using KozzionTrading.Network;

namespace KozzionTradingCL.Experiments
{
    public class ExperimentRunBusServer : IExperiment
    {
        public void DoExperiment()
        {
            TradingBusServer server = new TradingBusServer(8128);
        }
    }
}
