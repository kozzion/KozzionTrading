using KozzionCore.Tools;
using KozzionTrading.Market;
using KozzionTrading.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KozzionTrading.IO
{
    public class ToolsIOHST
    {
        public static List<PriceCandle> ReadFileHST(string file, double spread)
        {
            long file_size = new FileInfo(file).Length;      

            using (BinaryReader reader = new BinaryReader(new FileStream(file, FileMode.Open)))
            {
                //header is 148 bytes for both 400 and 401
                int version = reader.ReadInt32();    // database version - 400	4 bytes

                string copyright = reader.ReadString(64);   // copyright info	64 bytes
                string symbol = reader.ReadString(12);  // symbol name	12 bytes

                int period = reader.ReadInt32(); ; // symbol timeframe	4 bytes
                TimeScale time_scale = TimeScale.Minute1; //TODO
                switch (period)
                {
                    case 1:
                        time_scale = TimeScale.Minute1;
                        break;
                    default:
                        throw new Exception("Unknown Timescale");
                }
           

                int digits = reader.ReadInt32(); ; // the amount of digits after decimal point	4 bytes
                DateTime timesign = DateTime.Now;   // timesign of the database creation	4 bytes
                DateTime last_sync = DateTime.Now; ; // the last synchronization time	4 bytes
                //int unused; // to be used in future	52 bytes
                reader.ReadBytes(4 + 4 + 52);

                switch (version)
                {
                    case 400:
                        return ReadHST400(reader, file_size, time_scale, spread);
                    case 401:
                        return ReadHST401(reader, file_size, time_scale);
                    default:
                        throw new Exception("Unknown database version");
                }              
            }
        }

        private static List<PriceCandle> ReadHST400(BinaryReader reader, long file_size, TimeScale time_scale, double spread)
        {
            // bars array(single-byte justification) . . . total 44 bytes
            long line_count = (file_size - 148) / 44;
            List< PriceCandle> data = new List<PriceCandle>();
            for (int i = 0; i < line_count; i++)
            {
                reader.ReadBytes(4);
                DateTimeUTC open_date_time = ToolsTime.UnixTimeStampToDateTimeUTC(reader.ReadInt32());
                double open   = reader.ReadDouble(); // open price	8 bytes
                double low    = reader.ReadDouble(); // lowest price	8 bytes
                double high   = reader.ReadDouble(); // highest price	8 bytes
                double close  = reader.ReadDouble(); // close price	8 bytes
                double volume = reader.ReadDouble(); // tick count	8 bytes
                data.Add(new PriceCandle(open_date_time, time_scale, open, high, low, close, volume, spread));
            }  
            return data;
        }


        private static List<PriceCandle> ReadHST401(BinaryReader reader, long file_size, TimeScale time_scale)
        {
            //then the bars array(single-byte justification) . . . total 60 bytes
            long line_count = (file_size - 148) / 60;
            List<PriceCandle> data = new List<PriceCandle>();

            for (int i = 0; i < line_count; i++)
            {
                DateTimeUTC open_date_time = ToolsTime.UnixTimeStampToDateTimeUTC(reader.ReadInt64());    // bar start time	8 bytes
                double open = reader.ReadDouble();  // open price	8 bytes
                double high = reader.ReadDouble();    // highest price	8 bytes
                double low = reader.ReadDouble(); // lowest price	8 bytes
                double close = reader.ReadDouble();   // close price	8 bytes
                long volume = reader.ReadInt64();    // tick count	8 bytes
                int spread = reader.ReadInt32(); ; // spread	4 bytes
                long real_volume = reader.ReadInt64();   // real volume	8 bytes
                data.Add(new PriceCandle(open_date_time, time_scale, open, high, low, close, volume, spread, real_volume));
            }

            return data;
        }
    }
}
