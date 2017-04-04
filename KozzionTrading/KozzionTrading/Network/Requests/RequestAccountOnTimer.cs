using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KozzionTrading.Network.Requests
{
    public class RequestAccountOnTimer : ARequest
    {
        public DateTime ServerTime { get; private set; }
        public DateTime AccountTime { get; private set; }
        public double Bid { get; private set; }
        public double Ask { get; private set; }

        public RequestAccountOnTimer(DateTime server_time, DateTime account_time, double bid, double ask) 
            : base(RequestType.RequestAccountOnTimer)
        {
            ServerTime = server_time;
        }

        protected override void WriteProtected(BinaryWriter writer)
        {
            writer.WriteDateTime(this.ServerTime);
            writer.WriteDateTime(this.AccountTime);
            writer.Write(Bid);
            writer.Write(Ask);
        }

        public static ARequest ReadStatic(BinaryReader reader)
        {
            DateTime server_time = reader.ReadDateTime();
            DateTime account_time = reader.ReadDateTime();
            double bid = reader.ReadDouble();
            double ask = reader.ReadDouble();
            return new RequestAccountOnTimer(server_time, account_time, bid, ask);
        }


    }
}
