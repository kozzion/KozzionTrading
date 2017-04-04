//+------------------------------------------------------------------+
//|                                                    boilinger.mq4 |
//|                        Copyright 2016, MetaQuotes Software Corp. |
//|                                             https://www.mql5.com |
//+------------------------------------------------------------------+
#property copyright "Copyright 2016, MetaQuotes Software Corp."
#property link      "https://www.mql5.com"
#property version   "1.00"
#property strict

string symbol = Symbol();
double MA4HR9;
double MA4HR20;
double MA1HR9;
double MA1HR20;
double MA30MIN9;
double MA30MIN20;
double BoilingerMid;
double BoilingerTop;
double BoilingerBot;
double total_orders;
double Bears;
double Bulls;
double StandaardDeviation;
bool HigherThanMidOld;
bool HigherThanHighOld;
bool LowerThanMidOld;
bool LowerThanLowOld;
//+------------------------------------------------------------------+
//| Expert initialization function                                   |
//+------------------------------------------------------------------+
int OnInit()
  {
//--- create timer
   EventSetTimer(60);
      
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
   MA1HR9 = iMA(symbol, PERIOD_H1, 9, 0, MODE_SMA, PRICE_CLOSE, 0);
   MA1HR20 = iMA(symbol, PERIOD_H1, 20, 0, MODE_SMA, PRICE_CLOSE, 0);
   MA30MIN9 = iMA(symbol, PERIOD_M30, 9, 0, MODE_SMA, PRICE_CLOSE, 0);
   MA30MIN20 = iMA(symbol, PERIOD_M30, 20, 0, MODE_SMA, PRICE_CLOSE, 0);
   BoilingerMid = iBands(symbol,0,20,2,0,PRICE_CLOSE,MODE_MAIN,0);
   BoilingerTop = iBands(symbol,0,20,2,0,PRICE_CLOSE,MODE_UPPER,0);
   BoilingerBot = iBands(symbol,0,20,2,0,PRICE_CLOSE,MODE_LOWER,0);
   Bears = iBearsPower(symbol, Period(), 13, PRICE_CLOSE, 0);
   Bulls = iBullsPower(symbol, Period(), 13, PRICE_CLOSE, 0);
   StandaardDeviation = iStdDev(symbol,0,10,0,MODE_SMA,PRICE_CLOSE,0);
   
   
   
   total_orders = OrdersTotal();
   for(int i = 0; i < total_orders; i++)
      {
         OrderSelect(i,SELECT_BY_POS, MODE_TRADES);
         if(Symbol() == OrderSymbol()){ 
                  
                  //TrailingSL(OrderTicket());
                                   
         }
      }
      
   if (total_orders == 0) {

      if(MA1HR9 > MA1HR20 && MA30MIN9 > MA30MIN20 && Bulls > 0){ 
         if(LowerThanLowOld == true && StandaardDeviation > 0.0003 && Close[1] > BoilingerBot){
            Buy();            
         }
      }

      if(MA1HR9 < MA1HR20 && MA30MIN9 < MA30MIN20 && Bears < 0){  
         if(HigherThanHighOld == true && StandaardDeviation > 0.0003 && Close[1] < BoilingerTop){
            Sell();
         } 
      }

      if (Open[0] > BoilingerTop) {
         HigherThanMidOld = false;
         HigherThanHighOld = true;
         LowerThanMidOld = false;
         LowerThanLowOld = false;
      }     
   
      if (Open[0] > BoilingerMid && Open[0] < BoilingerTop) {    
         HigherThanMidOld = true;
         HigherThanHighOld = false;
         LowerThanMidOld = false;
         LowerThanLowOld = false;
      }   
   
      if (Open[0] < BoilingerMid && Open[0] > BoilingerBot) {
         HigherThanMidOld = false;
         HigherThanHighOld = false;
         LowerThanMidOld = true;
         LowerThanLowOld = false;
      }
      if (Open[0] < BoilingerBot) {
         HigherThanMidOld = false;
         HigherThanHighOld = false;
         LowerThanMidOld = false;
         LowerThanLowOld = true;
      }
    }
  }
  
//+------------------------------------------------------------------+
//| Buy function                                                     |
//+------------------------------------------------------------------+

void Buy()
{
   double SL = Ask - 40 * Point;
   double TP = Ask + 500 * Point; 
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
   double TP = Bid - 500 * Point; 
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
   int StopLossTrail = 300;
   OrderSelect(ticket_number, SELECT_BY_TICKET);
   if(OrderType() == OP_BUY)
   {
      if (OrderStopLoss() !=  (OrderOpenPrice() + (15*Point)))
      {
         if(Point*StopLossTrail < (Bid - OrderOpenPrice()))
         {
            if(OrderStopLoss() < (Bid - (Point * StopLossTrail)))
            {
               OrderModify(ticket_number, OrderOpenPrice(), NormalizeDouble(OrderOpenPrice() + (15*Point), Digits), OrderTakeProfit(), 0, Blue);
            }
         }
      }
   }
   if(OrderType() == OP_SELL)
   {
      if (OrderStopLoss() !=  (OrderOpenPrice() - (15*Point)))
      {
         if(Point*StopLossTrail < (OrderOpenPrice() - Bid))
         {
            if((Bid + (Point * StopLossTrail)) < OrderStopLoss())
            {
               OrderModify(ticket_number, OrderOpenPrice(), NormalizeDouble(OrderOpenPrice() - (15*Point), Digits), OrderTakeProfit(), 0, Blue);
            }
         }
      } 
   }    
}