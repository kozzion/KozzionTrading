using System;
using System.IO;

namespace KozzionTrading.Network.Response
{
    public class ResponseDisconnect : AResponse
    {
        public ResponseDisconnect() 
            : base(ResponseType.ResponseDisconnect)
        {

        }

        public static AResponse ReadStatic(BinaryReader reader)
        {
            return null;
        }

        protected override void WriteProtected(BinaryWriter writer)
        {
        }
    }
}