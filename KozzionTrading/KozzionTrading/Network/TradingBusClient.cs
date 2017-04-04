using KozzionCore.Networking;
using KozzionTrading.Network.Requests;
using KozzionTrading.Network.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace KozzionTrading.Network
{
    public class TradingBusClient
    {
        private ConnectionTradingOnClient connection;

        public TradingBusClient(string host_name, int port_number)
        {
            this.connection = new ConnectionTradingOnClient(new TcpClient(host_name, port_number));
        }

        public ResponseAccountOnTick OnTick(DateTime date_time_server, DateTime date_time_account, double bid, double ask)
        {
            return (ResponseAccountOnTick)connection.SendRequest(new RequestAccountOnTick( date_time_server, date_time_account, bid, ask));
        }
    }
}
