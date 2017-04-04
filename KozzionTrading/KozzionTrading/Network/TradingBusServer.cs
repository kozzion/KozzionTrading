using KozzionCore.Concurrency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace KozzionTrading.Network
{
    public class TradingBusServer
    {
        TradingRequestHandler request_handler;
        ConnectionTradingListener listener;

        public TradingBusServer(int listen_port_number)
        {
            request_handler = new TradingRequestHandler(this);
            request_handler.Start();
            listener = new ConnectionTradingListener(request_handler, listen_port_number);
            listener.Start();
        }


    }
}
