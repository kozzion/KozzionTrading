//+------------------------------------------------------------------+
//|                                                  WriteToFile.mq4 |
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
int file_handle   = 0;  
string symbol     = Symbol();
int timeframe     = Period();
int applied_price = PRICE_CLOSE;
int shift         = 0;
int ma_method     = MODE_SMA;


int OnInit()
  {
//--- create timer
   EventSetTimer(1);
   file_handle =  FileOpen("TestFILE_WRITE.txt", FILE_WRITE |FILE_CSV);
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
      double bid = Bid;    
      double spread = Ask - Bid;

      //Accelerator Oscillator
      double accelerator_oscillator = iAC(symbol, timeframe, shift);
      
      //Accumulation/Distribution
      double  accumulation_distribution = iAD(symbol, timeframe, shift);
      
      //Average Directional Index - MODE_MAIN
      double average_directional_index1 = iADX(symbol, timeframe, 14, applied_price, MODE_MAIN, shift);
      
      //Average Directional Index - MODE_PLUSDI
      double average_directional_index2 = iADX(symbol, timeframe, 14, applied_price, MODE_PLUSDI, shift);
      
      //Average Directional Index - MODE_MINUSDI
      double average_directional_index3 =  iADX(symbol, timeframe, 14, applied_price, MODE_MINUSDI, shift);
            
      //Alligator MODE_GATORJAW
      double alligatorjaw = iAlligator(symbol, timeframe, 13,8,8,5,5,3,ma_method,applied_price,MODE_GATORJAW,shift);

      //Alligator MODE_GATORTEETH 
      double alligatorteeth = iAlligator(symbol, timeframe, 13,8,8,5,5,3,ma_method,applied_price,MODE_GATORTEETH ,shift);
      
      //Alligator MODE_GATORLIPS 
      double alligatorlips = iAlligator(symbol, timeframe, 13,8,8,5,5,3,ma_method,applied_price,MODE_GATORLIPS ,shift);
            
      //Awesome Oscillator
      double awesome_oscillator = iAO(symbol, timeframe, shift);
         
      //Average True Range
      double average_true_range = iATR(symbol, timeframe, 14, shift); 
      
      //Bears Power
      double bears = iBearsPower(symbol, timeframe, 13, applied_price, shift);
   
      //Bollinger Bands® MODE_MAIN
      double bands_main = iBands(symbol,timeframe,20,2,0,applied_price,MODE_MAIN,shift);
   
      //Bollinger Bands® MODE_UPPER
      double bands_upper = iBands(symbol,timeframe,20,2,0,applied_price,MODE_UPPER,shift);
         
      //Bollinger Bands® MODE_LOWER
      double bands_lower = iBands(symbol,timeframe,20,2,0,applied_price,MODE_LOWER,shift);
 
      //Bulls Power
      double bulls = iBullsPower(symbol, timeframe, 13, applied_price, shift);
      
      //Commodity Channel Index
      double commodity_channel_index = iCCI(symbol,timeframe,14,applied_price,shift); 
  
      //DeMarker
      double demarker = iDeMarker(symbol,timeframe,14,shift);
      
      //Envelopes
      double envelopes = iEnvelopes(symbol,timeframe,14,MODE_SMA,0,applied_price,0.10,MODE_MAIN,shift);
           
      //Force Index
      double force_index = iForce(symbol,timeframe,13,MODE_SMA,applied_price,shift);
      
      //Fractals
      double fractals = iFractals(symbol,timeframe,MODE_SMA,shift);
      
      //Gator Oscillator MODE_GATORJAW
      double gatorjaw = iGator(symbol,timeframe,13, 8,8,5,5,3,MODE_SMA,applied_price,MODE_GATORJAW,shift);      
      
      //Gator Oscillator MODE_GATORTEETH
      double gatorteeth = iGator(symbol,timeframe,13, 8,8,5,5,3,MODE_SMA,applied_price,MODE_GATORTEETH ,shift);      
      
      //Gator Oscillator MODE_GATORLIPS
      double gatorlips = iGator(symbol,timeframe,13, 8,8,5,5,3,MODE_SMA,applied_price,MODE_GATORLIPS ,shift);
      
      //Ichimoku Kinko Hyo
      double ichimoku = iIchimoku(symbol,timeframe,9,26,52,MODE_SMA,shift);
      
      //Market Facilitation Index by Bill Williams
      double bwmfi = iBWMFI(symbol,timeframe,shift);
      
      //Momentum
      double momentum = iMomentum(symbol,timeframe,14,applied_price,shift);
      
      //Money Flow Index
      double money_flow = iMFI(symbol,timeframe,14,shift);      

      //Moving Average
      double moving_average_9 = iMA(symbol,timeframe,9,0,MODE_SMA,applied_price,0);
       
      //Moving Average
      double moving_average_20 = iMA(symbol,timeframe,20,0,MODE_SMA,applied_price,0);
                
      //Moving Average of Oscillator (MACD histogram)
      double moving_average_oscillator = iOsMA(symbol,timeframe,12,26,9,applied_price,shift);
      
      //Moving Averages Convergence-Divergence
      double macd = iMACD(symbol,timeframe,5,20,5,applied_price,MODE_SMA,shift);
      
      //On Balance Volume
      double balance_volume = iOBV(symbol,timeframe,applied_price,shift);
      
      //Parabolic Stop And Reverse System
      double stop_reverse = iSAR(symbol,timeframe,0.02,0.2,shift);
   
      //Relative Strength Index
      double relative_strenght = iRSI(symbol,timeframe,14,applied_price,shift);
            
      //Relative Vigor Index
      double relative_vigor = iRVI(symbol,timeframe,10,MODE_SMA,shift);
      
      //Standard Deviation
      double standard_deviation = iStdDev(symbol,timeframe,20,0,MODE_SMA,applied_price,shift);
           
      //Stochastic Oscillator MODE_MAIN
      double stochastic_main = iStochastic(symbol,timeframe,5,3,3,MODE_SMA,0,MODE_MAIN,shift);
            
      //Stochastic Oscillator MODE_SIGNAL
      double stochastic_signal = iStochastic(symbol,timeframe,5,3,3,MODE_SMA,0,MODE_SIGNAL,shift);
       
      //Williams' Percent Range
      double williams = iWPR(symbol,timeframe,14,shift);
      
         
      if(file_handle != INVALID_HANDLE)
      {
         FileWrite(file_handle, symbol, TimeCurrent(),  Bid , Ask, accelerator_oscillator, accumulation_distribution, average_directional_index1, average_directional_index2, average_directional_index3,
               alligatorjaw, alligatorteeth, alligatorlips, awesome_oscillator, average_true_range, bears, bands_main, bands_upper, bands_lower, bulls, commodity_channel_index, moving_average_9, 
               moving_average_20, moving_average_oscillator, macd, balance_volume, stop_reverse, relative_strenght, relative_vigor, standard_deviation, stochastic_main, stochastic_signal, williams,
               demarker, envelopes, force_index, fractals, gatorjaw, gatorteeth, gatorlips, ichimoku, bwmfi, momentum, money_flow);         
      }
      else
      {
         Alert("Fail");
      }
  }
//+------------------------------------------------------------------+
//| Timer function                                                   |
//+------------------------------------------------------------------+
void OnTimer()
{


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
