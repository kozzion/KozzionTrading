using KozzionCore.Networking;
using KozzionTrading.Network.Response;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace KozzionTrading.Network
{
    public class ConnectionTradingOnClient :IMessageHandler
    {
        private BlockingCollection<Message> message_queue;
        private Connection connection;
  
        public ConnectionTradingOnClient(TcpClient client)
        {
            message_queue = new BlockingCollection<Message>(new ConcurrentQueue<Message>());
            connection = new Connection(client, this);
            connection.Start();
        }

        public void Handle(Message message)
        {
            message_queue.Add(message);
        }

        public AResponse SendRequest(ARequest request)
        {
            byte[] payload = null;
            using (MemoryStream stream = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    request.Write(writer);
                    payload = stream.ToArray();
                }
            }
            connection.SendMessage(new Message(MessageType.MessagePayload, payload));

            Message response_message = message_queue.Take();          

            using (MemoryStream stream = new MemoryStream(response_message.Payload))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    return AResponse.Read(reader);
                }
            }

        }
    }
}
