using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KozzionTrading.Network.Requests
{
    public class RequestConnectProvideExpert : ARequest
    {
        public string ExpertID { get; private set; }

        public RequestConnectProvideExpert(string expert_id)
            : base(RequestType.RequestConnectProvideIndicator)
        {
            this.ExpertID = expert_id;
        }

        public static ARequest ReadStatic(BinaryReader reader)
        {
            string expert_id = reader.ReadString();
            return new RequestConnectProvideIndicator(expert_id);
        }

        protected override void WriteProtected(BinaryWriter writer)
        {
            writer.Write(ExpertID);
        }
    }
}
