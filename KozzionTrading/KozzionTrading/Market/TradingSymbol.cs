using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KozzionTrading.Market
{
    public class TradingSymbol
    {
        public string Broker { get; private set; }
        public string Account { get; private set; }
        public string Symbol { get; private set; }
        public string Description { get; private set; }

        public TradingSymbol(string broker, string account, string symbol, string description)
        {
            this.Broker = broker;
            this.Account = account;
            this.Symbol = symbol;
            this.Description = description;
        }

        public override bool Equals(object other)
        {
            if (other is TradingSymbol)
            {
                TradingSymbol other_typed = (TradingSymbol)other;
                if (Broker != other_typed.Broker)
                {
                    return false;
                }
                if (Account != other_typed.Account)
                {
                    return false;
                }

                if (Symbol != other_typed.Symbol)
                {
                    return false;
                }

                if (Description != other_typed.Description)
                {
                    return false;
                }
                return true;

            }
            return false;
        }


        public override int GetHashCode()
        {
            return Broker.GetHashCode() + Account.GetHashCode() + Symbol.GetHashCode() + Description.GetHashCode();
        }
        public static TradingSymbol Read(BinaryReader reader)
        {
            string broker = reader.ReadString();
            string account = reader.ReadString();
            string symbol = reader.ReadString();
            string description = reader.ReadString();
            return new TradingSymbol(broker, account, symbol, description);
        }


        public void Write(BinaryWriter writer)
        {
            writer.Write(this.Broker);
            writer.Write(this.Account);
            writer.Write(this.Symbol);
            writer.Write(this.Description);
        }

        public override string ToString()
        {
            return Symbol;
        }
    }
}
