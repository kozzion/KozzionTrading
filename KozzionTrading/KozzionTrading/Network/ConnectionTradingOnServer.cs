using KozzionCore.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;
using KozzionTrading.Network.Response;

namespace KozzionTrading.Network
{
    public class ConnectionTradingOnServer : IMessageHandler
    {
        private TcpClient client;
        private ConnectionReader connection_reader;
        private ConnectionWriter connection_writer;
        private TradingRequestHandler handler;

        public ConnectionTradingOnServer(TcpClient client, TradingRequestHandler handler)
        {
            this.client = client;
            this.connection_reader = new ConnectionReader(new BinaryReader(client.GetStream()), this);
            this.connection_writer = new ConnectionWriter(new BinaryWriter(client.GetStream()));
            this.handler = handler;
        }

        public void Start()
        {
            connection_reader.Start();
            connection_writer.Start();
        }

        public void Handle(Message message)
        {
            BinaryReader reader = new BinaryReader(new MemoryStream(message.Payload));
            handler.Enqueue(this, ARequest.Read(reader));
        }

        public void Send(AResponse reponse)
        {
            MemoryStream stream = new MemoryStream();
            reponse.Write(new BinaryWriter(stream));
            byte [] payload = stream.ToArray();
            connection_writer.SendMessage(new Message(MessageType.MessagePayload, payload));
        }
    }
}
