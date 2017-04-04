using System;
using System.IO;

namespace KozzionTrading.Network.Response
{
    public class ResponseConnectProvideExpert : AResponse
    {
        public ResponseConnectProvideExpert() 
            : base(ResponseType.ResponseConnectProvideExpert)
        {

        }

        public static AResponse ReadStatic(BinaryReader reader)
        {
            return new ResponseConnectProvideExpert();
        }

        protected override void WriteProtected(BinaryWriter writer)
        {

        }
    }
}