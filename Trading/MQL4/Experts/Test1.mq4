//+------------------------------------------------------------------+
//|                                                        Test1.mq4 |
//|                        Copyright 2016, MetaQuotes Software Corp. |
//|                                             https://www.mql5.com |
//+------------------------------------------------------------------+
#property copyright "Copyright 2016, MetaQuotes Software Corp."
#property link      "https://www.mql5.com"
#property version   "1.00"
#property strict



bool done = false;
bool Work = true;
bool Ans  = false;
int Lot = 10;

//+------------------------------------------------------------------+
//| Expert initialization function                                   |
//+------------------------------------------------------------------+
int OnInit()
  {
  
//--- create timer
   EventSetTimer(1);      
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
//---
   
  }
//+------------------------------------------------------------------+
//| Timer function                                                   |
//+------------------------------------------------------------------+
void OnTimer()
{     ReadAllTicket();
    /*   int Ticket =0;

            string  Symb=Symbol();            
            RefreshRates();
            //double SL = Bid - 1 * Point;     // Calculating SL of opened
            double SL = Bid - 1;
            double TP = Ask + 1; 
            double price = Bid;
            
            Alert("symbol " + Symb);
            Alert("oper   " + OP_BUY);
            Alert("loss   " + SL);
            Alert("take   " + TP);
            Alert("lot    " + Lot);
            Alert("price  " + price);
            
            Ticket = OrderSend(Symb, OP_BUY, Lot, price, 10, SL, TP); //Opening Buy
            
            if (Ticket < 0)                        // Success :)
            {
               Alert ("Opened order Buy ", Ticket);
               return;
            }          
            
    */                 
      
   
}

void ReadAllTicket()
{    
   int positie = 0;
   for (;positie <= OrdersTotal() - 1;) {
      OrderSelect(positie, SELECT_BY_POS);
      positie++;
      // ACTIES MET ORDER
      //OrderClosePrice(), OrderCloseTime(), OrderComment(), OrderCommission(), OrderExpiration(), OrderLots(), OrderMagicNumber(), 
      //OrderOpenPrice(), OrderOpenTime(), OrderPrint(), OrderProfit(), OrderStopLoss(), OrderSwap(), OrderSymbol(), OrderTakeProfit(), OrderTicket(), OrderType() 
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
         Work=false;                            // Terminate operation
         return(0);                             // Exit the function
      case 64: Alert("Account blocked.");
         Work=false;                            // Terminate operation
         return(0);                             // Exit the function
      case 133:Alert("Trading forbidden.");
         return(0);                             // Exit the function
      case 134:Alert("Not enough money to execute operation.");
         return(0);                             // Exit the function
      default: Alert("Error occurred: ",Error);  // Other variants   
         return(0);                             // Exit the function
     }
  }


//----------MAKE NEW TRADE

  
//----------CLOSE ALL
void CloseAll(int OrderNumber)
{
  OrderClose(OrderNumber,1,Ask,5);
}  