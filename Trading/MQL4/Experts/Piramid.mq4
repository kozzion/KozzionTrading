#property link      "https://www.mql5.com"
#property version   "1.00"
#property strict

bool isLong;
   
double SLB, TPB, SLS, TPS;
double limit  = 300 * Point;
double swap   = 100 * Point;
double startvolume = 0.03;
double volumeFactor = 1.6;
double volume;
double slippage    =   2.0;
int ticketnummer = 0; 
    

void OnInit()
  {
  }

void OnTick(){
      if(OrdersTotal() == 0)
      {
         if(DayOfWeek() == 1 || DayOfWeek() == 2){
            volume = startvolume;
            ticketnummer = BuyStart();
            isLong = true;
         }
      }
      else
      {
         OrderSelect(ticketnummer,SELECT_BY_TICKET);
         if(isLong == true){
            if ( Bid < OrderOpenPrice() - swap) {
               if(OrdersTotal() > 6) {
                  
               }
               Sell();
               isLong = false;
            } 
         }
         else {
            if ( OrderOpenPrice() + swap < Bid) {
               Buy();
               isLong = true;
            }
         }
      } 
   }
//+------------------------------------------------------------------+
//| BUY/SELL function                                                    |
//+------------------------------------------------------------------+
int BuyStart(){  
   SLB = Ask - limit;
   TPB = Ask + limit;       
   return OrderSend(Symbol(), OP_BUY, volume, Ask, slippage, SLB, TPB);
}


void Buy(){
   volume = volume * volumeFactor;
   OrderSelect(ticketnummer,SELECT_BY_TICKET);     
   OrderSend(Symbol(), OP_BUY, volume, Ask, slippage, OrderStopLoss(), OrderTakeProfit());
}

void Sell(){ 
   volume = volume * volumeFactor;
   OrderSelect(ticketnummer,SELECT_BY_TICKET);  
   OrderSend(Symbol(), OP_SELL, volume, Bid, slippage, OrderTakeProfit(), OrderStopLoss());
}