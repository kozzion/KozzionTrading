using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KozzionTrading.Network.Requests
{
    public enum RequestType
    {
        RequestConnectAccount,
        RequestConnectProvideIndicator,
        RequestConnectProvideExpert,
        RequestAccountOnTimer,
        RequestAccountOnTick,
        RequestDisconnect
    }

}
