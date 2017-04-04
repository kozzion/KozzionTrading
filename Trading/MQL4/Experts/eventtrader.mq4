//+------------------------------------------------------------------+
//|                                                 event trader.mq4 |
//|                        Copyright 2016, MetaQuotes Software Corp. |
//|                                             https://www.mql5.com |
//+------------------------------------------------------------------+
#property copyright "Copyright 2016, MetaQuotes Software Corp."
#property link      "https://www.mql5.com"
#property version   "1.00"
#property strict

int event_index = 0;
int event_count = 0;
datetime event_date[];
string event_symbol[];
int event_impact[];
bool event_traded[];

//+------------------------------------------------------------------+
//| Expert initialization function                                   |
//+------------------------------------------------------------------+
int OnInit()
  {
//---
   int output_file_handle = FileOpen("events_small.csv", FILE_READ |FILE_CSV);
   if(output_file_handle==-1)
   {
      Alert("Invalid File");
      return(INIT_FAILED);
   }
   int index = 0;
   while(!FileIsEnding(output_file_handle))
   {
      string line_string = FileReadString(output_file_handle);
      string result[];
      StringSplit(line_string,',', result);         
      datetime date = StrToInteger(result[0]);      
      string symbol = result[1];
      int impact = StrToInteger(result[2]);      

      ArrayResize(event_date,   index + 1);
      ArrayResize(event_impact, index + 1);
      ArrayResize(event_symbol, index + 1);
      ArrayResize(event_traded, index + 1);
      event_date[index] = date;
      event_symbol[index] = symbol;
      event_impact[index] = impact;
      event_traded[index] = false;
      index++;      
   }
   event_count = index;
   EventSetTimer(2);
//---
   FileClose(output_file_handle);
   return(INIT_SUCCEEDED);
  }
//+------------------------------------------------------------------+
//| Expert deinitialization function                                 |
//+------------------------------------------------------------------+
void OnDeinit(const int reason)
  {
//---
   
  }
//+------------------------------------------------------------------+
//| Expert tick function                                             |
//+------------------------------------------------------------------+
void OnTick()
{
    //---
    datetime currentTime = TimeCurrent();    
    datetime time = event_date[event_index];
    while((currentTime < time) && (event_index < event_count))
    {
      event_index++;
      time = event_date[event_index];
    }
    if(event_index < event_count)
    {
      int trading_event_index = event_index;
         //Trading policiy
      
       while((time < currentTime + 120) && (trading_event_index < event_count))
       {
       
          int time_to_event = event_date[trading_event_index] - currentTime + 3600;
          if (time_to_event < 15 && event_traded[trading_event_index] == false && event_impact[trading_event_index] == 3 ) 
          {
            if (event_symbol[trading_event_index] == "USD"  || event_symbol[trading_event_index] == "EUR") 
            {
               //Buy(event_symbol[trading_event_index]);
               //Sell(event_symbol[trading_event_index]);
               Buy();
               Sell();
               event_traded[trading_event_index] = true;    
            }  
          }
          
          trading_event_index++;
       }    
    }      
}


//+------------------------------------------------------------------+
void OnTimer()
{
   OnTick();
}


//+------------------------------------------------------------------+
//| Buy function                                                     |
//+------------------------------------------------------------------+

void Buy()
{
   double SL = Ask - 40 * Point;
   double TP = Ask + 400 * Point; 
   double price = Ask;
   int slippage = 2; 
   double volume = 1.0;          
   OrderSend(NULL, OP_BUY, volume, price, slippage, SL, TP);
}

//+------------------------------------------------------------------+
//| Sell function                                                    |
//+------------------------------------------------------------------+

void Sell()
{
   double SL = Bid + 40 * Point;
   double TP = Bid - 400 * Point; 
   double price = Bid;
   int slippage = 2; 
   double volume = 1.0;      
   OrderSend(NULL, OP_SELL, volume, price, slippage, SL, TP);
}




//+------------------------------------------------------------------+
//| TrailingSL function                                                    |
//+------------------------------------------------------------------+

void TrailingSL(int ticket_number)
{
   int StopLossTrail = 100;
   OrderSelect(ticket_number, SELECT_BY_TICKET);
   if(OrderType() == OP_BUY)
   {
      if(Point*StopLossTrail < (Bid - OrderOpenPrice()))
      {
         if(OrderStopLoss() < (Bid - (Point * StopLossTrail)))
         {
            OrderModify(ticket_number, OrderOpenPrice(), NormalizeDouble(Bid - (Point*StopLossTrail), Digits), OrderTakeProfit(), 0, Blue);
         }
      }
   }
   if(OrderType() == OP_SELL)
   {
      if(Point*StopLossTrail < (OrderOpenPrice() - Bid))
      {
         if((Bid + (Point * StopLossTrail)) < OrderStopLoss())
         {
            OrderModify(ticket_number, OrderOpenPrice(), NormalizeDouble(Bid + (Point*StopLossTrail), Digits), OrderTakeProfit(), 0, Blue);
         }
      }
   }   
   
}