using KozzionCore.Concurrency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace KozzionTrading.Network
{
    public class ConnectionTradingListener : ATaskCycling
    {
        TradingRequestHandler Handler;
        TcpListener listener;


        public ConnectionTradingListener(TradingRequestHandler handler, int port_number)
        {
            this.Handler = handler;
            this.listener = new TcpListener(IPAddress.Parse("127.0.0.1"), port_number);
            this.listener.Start();
        }

        protected override void DoTask()
        {
            TcpClient client = listener.AcceptTcpClient();
            ConnectionTradingOnServer connection = new ConnectionTradingOnServer(client, Handler);
            connection.Start();
        }
    }
}
