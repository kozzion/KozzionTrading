//+------------------------------------------------------------------+
//|                                   StrategyMovingAverageCross.mq4 |
//|                        Copyright 2016, MetaQuotes Software Corp. |
//|                                             https://www.mql5.com |
//+------------------------------------------------------------------+
#property copyright "Copyright 2016, MetaQuotes Software Corp."
#property link      "https://www.mql5.com"
#property version   "1.00"
#property strict

bool do_buy = false;

//+------------------------------------------------------------------+
//| Expert initialization function                                   |
//+------------------------------------------------------------------+
int OnInit()
  {
//--- create timer
//   EventSetTimer(1);
      
//---
   return(INIT_SUCCEEDED);
  }
//+------------------------------------------------------------------+
//| Expert deinitialization function                                 |
//+------------------------------------------------------------------+
void OnDeinit(const int reason)
  {
//--- destroy timer
   EventKillTimer();
      
  }
//+------------------------------------------------------------------+
//| Expert tick function                                             |
//+------------------------------------------------------------------+
void OnTick()
  { 
   OnTimer();
   
  }
//+------------------------------------------------------------------+
//| Timer function                                                   |
//+------------------------------------------------------------------+
void OnTimer()
  {
//---
   //compute movinbg averate 9
   //compute movinbg averate 20
   
   string symbol = Symbol();

   
   //Check if we have no open order
   if(OrdersTotal() == 0)
   {  
      double m20 = iMA(symbol, PERIOD_M1, 50, 0, MODE_SMA, PRICE_OPEN, 0);
      double m9  = iMA(symbol, PERIOD_M1, 20,  0, MODE_SMA, PRICE_OPEN, 0);
      
      if ((m20 < m9) && do_buy)   
      {  
               
         if((m9) < Low[0])
         {
            // buy
            //double SL = Bid - 1 * Point;     // Calculating SL of opened
            double volume = 1.0;
            double SL = Bid - 10 * Point;
            double TP = Ask + 20 * Point; 
            double price = Bid;
            int slippage = 10;            
            OrderSend(symbol, OP_BUY, volume, price, slippage, SL, TP);
            do_buy = false;
         }
      }
      else if ((m9 < m20) && !do_buy)   
      {  do_buy = true;    
         if( (High[0]) < m9)
         {
            // Sell
            //double SL = Bid - 1 * Point;     // Calculating SL of opened
            double volume = 1.0;
            double SL = Bid - 10 * Point;
            double TP = Ask + 20 * Point; 
            double price = Bid;
            int slippage = 10;            
            OrderSend(symbol, OP_SELL, volume, price, slippage, SL, TP);
            do_buy = true;
         }
      }      
   }
   else
   {
      //Check if we do have an open order`
    
      // Keep trailing the take loss of this order:
      OrderSelect(0, SELECT_BY_POS);
      if (OrderType()==OP_BUY){
         if (OrderStopLoss() + 10 * Point < Ask)
         {
            OrderModify(OrderTicket(), OrderOpenPrice(), OrderStopLoss() + 1 * Point, OrderTakeProfit(), OrderOpenTime());
         }
      }
      else{
         if (Ask < OrderStopLoss() - 10 * Point)
         {
            OrderModify(OrderTicket(), OrderOpenPrice(), OrderStopLoss() - 1 * Point, OrderTakeProfit(), OrderOpenTime());
         }
      }
      //TODO ajust
   }
   
  }
//+------------------------------------------------------------------+
//| Tester function                                                  |
//+------------------------------------------------------------------+
double OnTester()
  {
//---
   double ret=0.0;
//---
  
//---
   return(ret);
  }
//+------------------------------------------------------------------+




//+------------------------------------------------------------------+
int ReportError(int Error)                        // Function of processing errors
  {
   switch(Error)
     {                                          // Not crucial errors            
      case  4: Alert("Trade server is busy. Trying once again..");
         Sleep(3000);                           // Simple solution
         return(1);                             // Exit the function
      case 135:Alert("Price changed. Trying once again..");
         RefreshRates();                        // Refresh rates
         return(1);                             // Exit the function
      case 136:Alert("No prices. Waiting for a new tick..");
         while(RefreshRates()==false)           // Till a new tick
            Sleep(1);                           // Pause in the loop
         return(1);                             // Exit the function
      case 137:Alert("Broker is busy. Trying once again..");
         Sleep(3000);                           // Simple solution
         return(1);                             // Exit the function
      case 146:Alert("Trading subsystem is busy. Trying once again..");
         Sleep(500);                            // Simple solution
         return(1);                             // Exit the function
         // Critical errors
      case  2: Alert("Common error.");
         return(0);                             // Exit the function
      case  5: Alert("Old terminal version.");
         //Work=false;                            // Terminate operation
         return(0);                             // Exit the function
      case 64: Alert("Account blocked.");
         //Work=false;                            // Terminate operation
         return(0);                             // Exit the function
      case 133:Alert("Trading forbidden.");
         return(0);                             // Exit the function
      case 134:Alert("Not enough money to execute operation.");
         return(0);                             // Exit the function
      default: Alert("Error occurred: ",Error);  // Other variants   
         return(0);                             // Exit the function
     }
  }