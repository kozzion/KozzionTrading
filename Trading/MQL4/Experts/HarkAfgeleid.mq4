/*
alligatorjaw
alligatorteeth
bands_lower
stop_reverse
momentum
*/

//+------------------------------------------------------------------+
//|                                                 HarkAfgeleid.mq4 |
//|                        Copyright 2016, MetaQuotes Software Corp. |
//|                                             https://www.mql5.com |
//+------------------------------------------------------------------+
#property copyright "Copyright 2016, MetaQuotes Software Corp."
#property link      "https://www.mql5.com"
#property version   "1.00"
#property strict

//+------------------------------------------------------------------+
//| Expert initialization function                                   |
//+------------------------------------------------------------------+
void OnInit()
  {

  }
//+------------------------------------------------------------------+
//| Expert deinitialization function                                 |
//+------------------------------------------------------------------+

void OnTick()
  {
      double BBLow = iBands(Symbol(),PERIOD_CURRENT,14,2,0,PRICE_CLOSE,MODE_LOWER,0);
      double BBMid = iBands(Symbol(),PERIOD_CURRENT,14,2,0,PRICE_CLOSE,MODE_MAIN,0);
      double BBHigh = iBands(Symbol(),PERIOD_CURRENT,14,2,0,PRICE_CLOSE,MODE_UPPER,0);
      double verschilHigh = BBHigh - BBMid;
      if(verschilHigh<0.0006) { 
         verschilHigh = 0;
      }
      else {
         if(OrdersTotal() == 0){
            if(BBMid < Ask){
               OrderSend(Symbol(),OP_BUY, 1.0, Ask, 2, Bid - 50* Point, Ask + 50*Point);
           }
           else if (Bid < BBMid ){
               OrderSend(Symbol(),OP_SELL, 1.0, Ask, 2, Ask + 50*Point,Bid - 50* Point);
            }
         }
      }
  }
//+------------------------------------------------------------------+
