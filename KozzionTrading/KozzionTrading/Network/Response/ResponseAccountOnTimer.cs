using System;
using System.IO;

namespace KozzionTrading.Network.Response
{
    public class ResponseAccountOnTimer : AResponse
    {
        public ResponseAccountOnTimer() 
            : base(ResponseType.ResponseAccountOnTimer)
        {

        }

        public static AResponse ReadStatic(BinaryReader reader)
        {
            return new ResponseAccountOnTimer();
        }

        protected override void WriteProtected(BinaryWriter writer)
        {

        }
    }
}