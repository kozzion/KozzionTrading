using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.Concurrent;
using System.Net.Sockets;
using KozzionTrading.Network.Requests;
using KozzionCore.Concurrency;
using KozzionTrading.Network.Response;
using KozzionTrading.Market;

namespace KozzionTrading.Network
{
    public class TradingRequestHandler : ATaskCycling
    {
        public bool Running { get; private set; }

        private TradingBusServer server;
        private ConcurrentQueue<Tuple<ConnectionTradingOnServer, ARequest>> RequestQueue;

        public TradingRequestHandler(TradingBusServer server)
        {
            this.server = server;
            this.Running = false;
            this.RequestQueue = new ConcurrentQueue<Tuple<ConnectionTradingOnServer, ARequest>>();          
        }

        public void Enqueue(ConnectionTradingOnServer source, ARequest request)
        {
            RequestQueue.Enqueue(new Tuple<ConnectionTradingOnServer, ARequest>(source, request));
        }


        protected override void DoTask()
        {
            Tuple<ConnectionTradingOnServer, ARequest> tuple = null;
            if (RequestQueue.TryDequeue(out tuple))
            {
                switch (tuple.Item2.RequestType)
                {

                    case RequestType.RequestConnectAccount:
                        HandleRequestConnectAccount(tuple.Item1, (RequestConnectAccount)tuple.Item2);
                        break;
                    case RequestType.RequestConnectProvideIndicator:
                        HandleRequestConnectProvideIndicator(tuple.Item1, (RequestConnectProvideIndicator)tuple.Item2);
                        break;
                    case RequestType.RequestConnectProvideExpert:
                        HandleRequestConnectProvideExpert(tuple.Item1, (RequestConnectProvideExpert)tuple.Item2);
                        break;
                    case RequestType.RequestAccountOnTimer:
                        HandleRequestAccountOnTimer(tuple.Item1, (RequestAccountOnTimer)tuple.Item2);
                        break;
                    case RequestType.RequestAccountOnTick:
                        HandleRequestAccountOnTick(tuple.Item1, (RequestAccountOnTick)tuple.Item2);
                        break;
                    case RequestType.RequestDisconnect:
                        HandleRequestDisconnect(tuple.Item1, (RequestDisconnect)tuple.Item2);
                        break;
                    default:
                        throw new Exception("Unkown Request type: " + tuple.Item2.RequestType);
                }
            }
        }



        private void HandleRequestDisconnect(ConnectionTradingOnServer client, RequestDisconnect request)
        {
            throw new NotImplementedException();
        }

        private void HandleRequestAccountOnTick(ConnectionTradingOnServer client, RequestAccountOnTick request)
        {
            throw new NotImplementedException();
        }

        private void HandleRequestAccountOnTimer(ConnectionTradingOnServer client, RequestAccountOnTimer request)
        {
            //request.Bid, request.Ask
            //    ser
            Console.WriteLine(request.Bid + " " + request.Ask);
            client.Send(new ResponseAccountOnTick(new TradingOrderCommand(TradingOrderType.Long, 1.0,1.0,0.0,0.0,2.0)));
        }

        private void HandleRequestConnectProvideExpert(ConnectionTradingOnServer client, RequestConnectProvideExpert request)
        {
            throw new NotImplementedException();
        }

        private void HandleRequestConnectProvideIndicator(ConnectionTradingOnServer client, RequestConnectProvideIndicator request)
        {
            throw new NotImplementedException();
        }

        private void HandleRequestConnectAccount(ConnectionTradingOnServer client, RequestConnectAccount request)
        {
            throw new NotImplementedException();
        }

 
    }
}
