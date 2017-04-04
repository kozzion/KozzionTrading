using System;
using System.IO;

namespace KozzionTrading.Network.Response
{
    public class ResponseConnectProvideIndicator : AResponse
    {
        public ResponseConnectProvideIndicator() 
            : base(ResponseType.ResponseConnectProvideIndicator)
        {

        }

        public static AResponse ReadStatic(BinaryReader reader)
        {
            return new ResponseConnectProvideIndicator();
        }

        protected override void WriteProtected(BinaryWriter writer)
        {
        }
    }
}