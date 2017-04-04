using KozzionTrading.Market;
using System;
using System.IO;

namespace KozzionTrading.Network.Response
{
    public class ResponseAccountOnTick : AResponse
    {
        public TradingOrderCommand Command { get; private set; }

        public ResponseAccountOnTick(TradingOrderCommand command) 
            : base(ResponseType.ResponseAccountOnTick)
        {

        }

        public static AResponse ReadStatic(BinaryReader reader)
        {
            TradingOrderType order_type = reader.ReadEnum<TradingOrderType>();
            double volume = reader.ReadDouble();
            double price = reader.ReadDouble();
            double slippage = reader.ReadDouble();
            double stop_loss = reader.ReadDouble();
            double take_profit = reader.ReadDouble();
            return new ResponseAccountOnTick(new TradingOrderCommand(order_type, volume, price, slippage, stop_loss, take_profit));
        }

        protected override void WriteProtected(BinaryWriter writer)
        {
            writer.WriteEnum(Command.OrderType);
            writer.Write(Command.Volume);
            writer.Write(Command.Price);
            writer.Write(Command.Slippage);
            writer.Write(Command.StopLoss);
            writer.Write(Command.TakeProfit);
        }
    }
}