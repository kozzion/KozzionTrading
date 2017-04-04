using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KozzionTrading.Network.Response
{
    public abstract class AResponse
    {

        public ResponseType RequestType { get; private set; }

        protected AResponse(ResponseType request_type)
        {
            this.RequestType = request_type;
        }

        public void Write(BinaryWriter writer)
        {
            writer.WriteEnum(RequestType);
            WriteProtected(writer);
        }

        protected abstract void WriteProtected(BinaryWriter writer);

        public static AResponse Read(BinaryReader reader)
        {
            ResponseType request_type = reader.ReadEnum<ResponseType>();
            switch (request_type)
            {
                case ResponseType.ResponseConnectAccount:
                    return ResponseConnectAccount.ReadStatic(reader);
                case ResponseType.ResponseConnectProvideIndicator:
                    return ResponseConnectProvideIndicator.ReadStatic(reader);
                case ResponseType.ResponseConnectProvideExpert:
                    return ResponseConnectProvideExpert.ReadStatic(reader);
                case ResponseType.ResponseAccountOnTimer:
                    return ResponseAccountOnTimer.ReadStatic(reader);
                case ResponseType.ResponseAccountOnTick:
                    return ResponseAccountOnTick.ReadStatic(reader);
                case ResponseType.ResponseDisconnect:
                    return ResponseDisconnect.ReadStatic(reader);
                default:
                    throw new Exception("Unkown request type: " + request_type);
            }
        }
    }
}
