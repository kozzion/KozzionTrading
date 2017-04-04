//+------------------------------------------------------------------+
//| Expert initialization function                                   |
//+------------------------------------------------------------------+
double verschil;
double gain_threshold_long  = 40 * Point;
int run_lenght_long         = 12;  
int run_lenght_short        = 12;
double gain_threshold_short = -40 * Point;

void OnInit()
  {
  }

//+------------------------------------------------------------------+
//| Expert tick function                                             |
//+------------------------------------------------------------------+
void OnTick()
{

   if ((OrdersTotal() == 0))
   {
         double gain_long = Ask - Open[run_lenght_long];
         if (gain_threshold_long < gain_long)
         {
            OrderSend(Symbol(),OP_BUY, 1.0, Ask, 2, Bid - gain_long, Ask + gain_long);
         }          
   }   

   if (OrdersTotal() == 0)
   {       
         double gain_short = Bid - Open[run_lenght_short];
         if (gain_short < gain_threshold_short)
         {
             OrderSend(Symbol(),OP_SELL, 1.0, Bid, 2, Ask + 100*Point, Bid - 100 * Point);
         }

   }
}
   
//+------------------------------------------------------------------+
//| Timer function                                                   |
//+------------------------------------------------------------------+
void OnTimer()
  {
//---
   
  }