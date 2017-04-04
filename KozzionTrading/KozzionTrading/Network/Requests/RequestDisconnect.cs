using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KozzionTrading.Network.Requests
{
    public class RequestDisconnect : ARequest
    {
        public RequestDisconnect()
            :base(RequestType.RequestDisconnect)
        {

        }

        internal static ARequest ReadStatic(BinaryReader reader)
        {
            return new RequestDisconnect();
        }

        protected override void WriteProtected(BinaryWriter writer)
        {
            //Adds nothing
        }
    }
}
