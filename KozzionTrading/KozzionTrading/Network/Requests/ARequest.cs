using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KozzionTrading.Network.Requests;
using System.IO;
using KozzionCore.Networking;

namespace KozzionTrading.Network
{

    public abstract class ARequest
    {
        public RequestType RequestType { get; private set;}

        protected ARequest(RequestType request_type)
        {
            this.RequestType = request_type;
        }

        public void Write(BinaryWriter writer)
        {
            writer.WriteEnum(RequestType);
            WriteProtected(writer);
        }

        protected abstract void WriteProtected(BinaryWriter writer);

        public static ARequest Read(BinaryReader reader)
        {
            RequestType request_type = reader.ReadEnum<RequestType>();
            switch (request_type)
            {
                case RequestType.RequestConnectAccount:
                    return RequestConnectAccount.ReadStatic(reader);
                case RequestType.RequestConnectProvideIndicator:
                    return RequestConnectProvideIndicator.ReadStatic(reader);
                case RequestType.RequestConnectProvideExpert:
                    return RequestConnectProvideExpert.ReadStatic(reader);
                case RequestType.RequestAccountOnTimer:
                    return RequestAccountOnTimer.ReadStatic(reader);
                case RequestType.RequestAccountOnTick:
                    return RequestAccountOnTick.ReadStatic(reader);
                case RequestType.RequestDisconnect:
                    return RequestDisconnect.ReadStatic(reader);
                default:
                    throw new Exception("Unkown request type: " + request_type);
            }
        }
    }
}
