using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KozzionTrading.Network.Requests
{
    public class RequestConnectAccount : ARequest
    {
        public string Broker { get; private set; }
        public string Account { get; private set; }
        public IList<string> Symbols { get; private set; }

        public RequestConnectAccount(string broker, string account, IList<string> symbols)
            : base(RequestType.RequestConnectAccount)
        {
            Broker = broker;
            Account = account;
            Symbols = new List<string>(symbols);
        }


        public static ARequest ReadStatic(BinaryReader reader)
        {
            string broker = reader.ReadString();
            string account = reader.ReadString();
            IList< string > symbols =  reader.ReadStringArray1D();
            return new RequestConnectAccount(broker, account, symbols);
        }

        protected override void WriteProtected(BinaryWriter writer)
        {
            writer.Write(Broker);
            writer.Write(Account);
            writer.WriteStringArray1D(Symbols);
        }
    }
}
