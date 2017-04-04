using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KozzionTrading.Network.Requests
{
    public class RequestConnectProvideIndicator : ARequest
    {
        public string IndicatorID { get; private set; }

        public RequestConnectProvideIndicator(string indicator_id)
            : base(RequestType.RequestConnectProvideIndicator)
        {
            this.IndicatorID = indicator_id;
        }

        public static ARequest ReadStatic(BinaryReader reader)
        {
            string indicator_id = reader.ReadString();
            return new RequestConnectProvideIndicator(indicator_id);
        }

        protected override void WriteProtected(BinaryWriter writer)
        {
            writer.Write(IndicatorID);
        }
    }
}
