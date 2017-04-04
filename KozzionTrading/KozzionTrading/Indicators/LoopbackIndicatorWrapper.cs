using KozzionTrading.Market;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KozzionTrading.Indicators
{
    public class LoopbackIndicatorWrapper
    {
        //private IIndicator indicator;
        //private IPAddress ip_address;
        //private int port_number;
        //private MarketModelSimulationSecond1 market_model;

        //public LoopbackIndicatorWrapper(IPAddress ip_address, int port_number, IIndicator indicator)
        //{
        //    this.ip_address = ip_address;
        //    this.port_number = port_number;
        //    this.indicator = indicator;
        //    //this.market_model = new MarketModelSimulationSecond1(1000, 1, 0.8, 0.4);
        //}

        //public LoopbackIndicatorWrapper(IIndicator indicator)
        //    : this(Dns.GetHostEntry("localhost").AddressList[0], 2007, indicator)
        //{
        //}


        //public void CreateListener()
        //{
        //    // Create an instance of the TcpListener class.
        //    // Set the listener on the local IP address 
        //    TcpListener tcpListener = new TcpListener(ip_address, port_number);
        //    tcpListener.Start();
        //    TcpClient tcp_client = tcpListener.AcceptTcpClient();
        //    while (true)
        //    {
        //        // Always use a Sleep call in a while(true) loop 
        //        // to avoid locking up your CPU.
        //        Thread.Sleep(10);
        //        // Read the data stream from the client.                 
        //        NetworkStream stream = tcp_client.GetStream();
        //        byte[] bytes_count = new byte[4];
        //        stream.Read(bytes_count, 0, bytes_count.Length);
        //        int candle_count = BitConverter.ToInt32(bytes_count, 0);

        //        byte[] bytes_candles = new byte[candle_count * 8 * 4];
        //        stream.Read(bytes_candles, 0, bytes_candles.Length);

        //        List<PriceCandle> candles = new List<PriceCandle>();
        //        for (int candle_index = 0; candle_index < candle_count; candle_index++)
        //        {

        //        }



        //        // Update market 
        //        market_model.Add(candles);

        //        // Call indicator
        //        byte[] bytes_indicator = BitConverter.GetBytes(indicator.ComputeFor(market_model));
        //        // Send  indicator bytes
        //        stream.Write(bytes_indicator, 0, bytes_indicator.Length);
        //    }
        //}

    }
}
