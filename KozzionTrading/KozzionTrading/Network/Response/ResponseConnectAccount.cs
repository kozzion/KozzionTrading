using System;
using System.IO;

namespace KozzionTrading.Network.Response
{
    public class ResponseConnectAccount :AResponse
    {
        public ResponseConnectAccount() 
            : base(ResponseType.ResponseConnectAccount)
        {

        }

        public static AResponse ReadStatic(BinaryReader reader)
        {
            return new ResponseConnectAccount();
        }

        protected override void WriteProtected(BinaryWriter writer)
        {

        }
    }
}