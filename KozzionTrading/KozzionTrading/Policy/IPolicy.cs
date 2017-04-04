using System;
using KozzionTrading.Market;

namespace KozzionTrading.Policy
{
    public interface IPolicy
    {
        string Title {get;}

        void GetTradeOrderCommand(IMarketModelSimulation market_model);
    }
}
