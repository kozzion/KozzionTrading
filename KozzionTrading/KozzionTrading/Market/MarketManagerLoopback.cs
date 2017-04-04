using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace KozzionTrading.Market
{
    public class MarketManagerLoopbackCTrader : IMarketManager
    {
        private static int REQUEST_SIZE = 1;
        private static int RESPONSE_SIZE = 1;

        private const byte REQUEST_CODE_CONNECT = 0;
        private const byte REQUEST_CODE_ORDER = 1;
        private const byte REQUEST_CODE_CLOSE = 2;
        private const byte RESPONSE_CODE_CONNECT = 0;
        private const byte RESPONSE_CODE_ORDER = 1;
        private const byte RESPONSE_CODE_CLOSE = 2;


        private byte[] request_data = new byte[REQUEST_SIZE];
        private byte[] response_data = new byte[RESPONSE_SIZE];

        private string address;
        private int port;
        private TcpListener listener;
        private TcpClient client;

        private bool runnning;

        public MarketManagerLoopbackCTrader()
        {
            address = "127.0.0.1";
            port = 13000;
            listener = new TcpListener(IPAddress.Parse(address), port);
        }

        public void Start()
        {        
            listener.Start();
            runnning = true;
            client = listener.AcceptTcpClient();

            // Enter the listening loop.
            while (runnning)
            {     
                // Get a stream object for reading and writing
                client.GetStream().Read(request_data, 0, REQUEST_SIZE);
                HandleMessage();
                client.GetStream().Write(response_data, 0, RESPONSE_SIZE);                
            }
            client.GetStream().Close();
            client.Close();
        }

        private void HandleMessage()
        {
            uint request_code = request_data[0];
            switch (request_code)
            {
                case REQUEST_CODE_CONNECT:
                    Console.WriteLine("REQUEST_CODE_CONNECT: " + request_code);
                    response_data[0] = RESPONSE_CODE_CONNECT;
                    break;
                case REQUEST_CODE_ORDER:
                    Console.WriteLine("RESPONSE_CODE_ORDER: " + request_code);
                    response_data[0] = RESPONSE_CODE_ORDER;
                    break;
                case REQUEST_CODE_CLOSE:
                    Console.WriteLine("REQUEST_CODE_CONNECT: " + request_code);
                    response_data[0] = RESPONSE_CODE_CLOSE;
                    runnning = false;             
                    break;

                default:
                    throw new Exception("Unknown request code");
            }
        }
    }
}
