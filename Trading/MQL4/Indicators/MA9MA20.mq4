//+------------------------------------------------------------------+
//|                                                      MA9MA20.mq4 |
//|                        Copyright 2016, MetaQuotes Software Corp. |
//|                                             https://www.mql5.com |
//+------------------------------------------------------------------+
#property copyright "Copyright 2016, MetaQuotes Software Corp."
#property link      "https://www.mql5.com"
#property version   "1.00"
#property strict
#property indicator_chart_window
#property indicator_buffers 2
#property indicator_plots   2
//--- plot MA9
#property indicator_label1  "MA9"
#property indicator_type1   DRAW_LINE
#property indicator_color1  clrRed
#property indicator_style1  STYLE_SOLID
#property indicator_width1  1
//--- plot MA20
#property indicator_label2  "MA20"
#property indicator_type2   DRAW_LINE
#property indicator_color2  clrAqua
#property indicator_style2  STYLE_SOLID
#property indicator_width2  1
//--- indicator buffers
double         MA9Buffer[];
double         MA20Buffer[];
//+------------------------------------------------------------------+
//| Custom indicator initialization function                         |
//+------------------------------------------------------------------+
int OnInit()
  {
//--- indicator buffers mapping
   SetIndexBuffer(0,MA9Buffer);
   SetIndexBuffer(1,MA20Buffer);
   
//---
   return(INIT_SUCCEEDED);
  }
//+------------------------------------------------------------------+
//| Custom indicator iteration function                              |
//+------------------------------------------------------------------+
int OnCalculate(const int rates_total,
                const int prev_calculated,
                const datetime &time[],
                const double &open[],
                const double &high[],
                const double &low[],
                const double &close[],
                const long &tick_volume[],
                const long &volume[],
                const int &spread[])
  {
  int i,                           // Bar index
       Counted_bars;                // Number of counted bars
   //--------------------------------------------------------------------
   Counted_bars=IndicatorCounted(); // Number of counted bars
   i=Bars-Counted_bars-1;           // Index of the first uncounted
   while(i>=0)                      // Loop for uncounted bars
     {
      MA9Buffer[i]=iMA(symbol, PERIOD_M1, 9,  0, MODE_SMA, PRICE_OPEN[i], 0);             // Value of 0 buffer on i bar
      MA20Buffer[i]=iMA(symbol, PERIOD_M1, 20, 0, MODE_SMA, PRICE_OPEN[i], 0);              // Value of 1st buffer on i bar
      i--;                          // Calculating index of the next bar
     }
     
   return(rates_total);
  }
//+------------------------------------------------------------------+
