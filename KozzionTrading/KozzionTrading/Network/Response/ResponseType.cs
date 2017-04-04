using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KozzionTrading.Network.Response
{
 
    public enum ResponseType
    {
        ResponseConnectAccount,
        ResponseConnectProvideIndicator,
        ResponseConnectProvideExpert,
        ResponseAccountOnTimer,
        ResponseAccountOnTick,
        ResponseDisconnect
    }
 
}
