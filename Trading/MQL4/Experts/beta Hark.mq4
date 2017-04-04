//+------------------------------------------------------------------+
//|                                                indicatortest.mq4 |
//|                        Copyright 2016, MetaQuotes Software Corp. |
//|                                             https://www.mql5.com |
//+------------------------------------------------------------------+
#property copyright "Copyright 2016, MetaQuotes Software Corp."
#property link      "https://www.mql5.com"
#property version   "1.00"
#property strict

double indicator;
string symbol     = Symbol();
int timeframe     = Period();
int applied_price = PRICE_CLOSE;
int shift         = 0;
int ma_method     = MODE_SMA;
//+------------------------------------------------------------------+
//| Expert initialization function                                   |
//+------------------------------------------------------------------+
int OnInit()
  {
   return(INIT_SUCCEEDED);
  }
//+------------------------------------------------------------------+
//|ON TICK                                |
//+------------------------------------------------------------------+

void OnTick()
  {
  
      double bid = Bid;
      double weightedBid = bid * -8.8562e+03;
   
      double ask = Ask;
      double weightedAsk = ask * -1.5096e+04;

      //Accelerator Oscillator
      double accelerator_oscillator = iAC(symbol, timeframe, shift);
      double weightedAC = accelerator_oscillator * 1.4268e+05;
      
      //Accumulation/Distribution
      double  accumulation_distribution = iAD(symbol, timeframe, shift);
      double weightedAD = accumulation_distribution * 0.0018;
      
      //Average Directional Index - MODE_MAIN
      double average_directional_index1 = iADX(symbol, timeframe, 14, applied_price, MODE_MAIN, shift);
      double weightedADX1 = average_directional_index1 * 4.8052;
      
      //Average Directional Index - MODE_PLUSDI
      double average_directional_index2 = iADX(symbol, timeframe, 14, applied_price, MODE_PLUSDI, shift);
      double weightedADX2 = average_directional_index2 * 0.0079;
      
      //Average Directional Index - MODE_MINUSDI
      double average_directional_index3 =  iADX(symbol, timeframe, 14, applied_price, MODE_MINUSDI, shift);
      double weightedADX3 = average_directional_index3 * 1.9265;
            
      //Alligator MODE_GATORJAW
      double alligatorjaw = iAlligator(symbol, timeframe, 13,8,8,5,5,3,ma_method,applied_price,MODE_GATORJAW,shift);
      double weightedAlligatorjaw = alligatorjaw * -1.2319e+03;
   
      //Alligator MODE_GATORTEETH 
      double alligatorteeth = iAlligator(symbol, timeframe, 13,8,8,5,5,3,ma_method,applied_price,MODE_GATORTEETH ,shift);
      double weightedAlligatorteeth = alligatorteeth * 8.1045e+04;
        
      //Awesome Oscillator
      double awesome_oscillator = iAO(symbol, timeframe, shift);
      double weightedAO = awesome_oscillator * -1.0180e+05;
      
     //Bears Power
      double bears = iBearsPower(symbol, timeframe, 13, applied_price, shift);
      double weightedBearsPower =  bears * 4.0512e+03;
         
      //Bollinger Bands® MODE_LOWER
      double bands_lower = iBands(symbol,timeframe,20,2,0,applied_price,MODE_LOWER,shift);
      double weightedBands = bands_lower * -5.6712e+04;
      
      //Bulls Power
      double bulls = iBullsPower(symbol, timeframe, 13, applied_price, shift);
      double weightedBulls = bulls * -1.3384e+04;
      
      //Commodity Channel Index
      double commodity_channel_index = iCCI(symbol,timeframe,14,applied_price,shift); 
      double weightedCCI = commodity_channel_index * 0.7420;
     
      //On Balance Volume
      double balance_volume = iOBV(symbol,timeframe,applied_price,shift);
      double weightedOBV = balance_volume * 7.3873e-04;
       
      //Parabolic Stop And Reverse System
      double stop_reverse = iSAR(symbol,timeframe,0.02,0.2,shift);
      double weightedSAR = stop_reverse * -791.8813;
      
      //Relative Strength Index
      double relative_strenght = iRSI(symbol,timeframe,14,applied_price,shift);
      double weightedRSI = relative_strenght * -1.9265;
            
      //Relative Vigor Index
      double relative_vigor = iRVI(symbol,timeframe,10,MODE_SMA,shift);
      double weightedRVI = relative_vigor * 115.5604;
      
      //Stochastic Oscillator MODE_MAIN
      double stochastic_main = iStochastic(symbol,timeframe,5,3,3,MODE_SMA,0,MODE_MAIN,shift);
      double weightedsStochasticMain = stochastic_main * 3.9509;
            
      //Stochastic Oscillator MODE_SIGNAL
      double stochastic_signal = iStochastic(symbol,timeframe,5,3,3,MODE_SMA,0,MODE_SIGNAL,shift);
      double weightedStochasticSignal = stochastic_signal * -3.3808;
       
      //Williams' Percent Range
      double williams = iWPR(symbol,timeframe,14,shift);
      double weightedWPR = williams * -2.0530;

      //DeMarker
      double demarker = iDeMarker(symbol,timeframe,14,shift);
      double weigthedDeMarker = demarker * -54.1893;       
           
      //Force Index
      double force_index = iForce(symbol,timeframe,13,MODE_SMA,applied_price,shift);
      double weightedForce = force_index * 6.0778;

      //Gator Oscillator MODE_GATORTEETH
      double gatorteeth = iGator(symbol,timeframe,13, 8,8,5,5,3,MODE_SMA,applied_price,MODE_GATORTEETH ,shift);      
      double weightedGatorTeeth = gatorteeth * -6.3364e+04;
            
      //Market Facilitation Index by Bill Williams
      double bwmfi = iBWMFI(symbol,timeframe,shift);
      double weightedBWMFI = bwmfi * -3.1698;
      
      //Momentum
      double momentum = iMomentum(symbol,timeframe,14,applied_price,shift);
      double weightedMomentum = momentum * 24.3028;
      
      //Money Flow Index
      double money_flow = iMFI(symbol,timeframe,14,shift);      
      double weightedMFI = money_flow * 0.1111;

      double deHark = weightedBid + weightedAsk + weightedAC + weightedAD + weightedADX1 + weightedADX2 + weightedADX3 + weightedAlligatorjaw + weightedAlligatorteeth + weightedAO + weightedBearsPower + weightedBands + weightedBulls + weightedCCI  + weightedOBV + weightedSAR + weightedRSI + weightedRVI + weightedsStochasticMain + weightedStochasticSignal + weightedWPR + weigthedDeMarker + weightedForce + weightedGatorTeeth + weightedBWMFI + weightedMomentum + weightedMFI;
     
       Alert(deHark);
      if (OrdersTotal() == 0)
      {
         if (400 < deHark) 
         {
            OrderSend(Symbol(), OP_BUY, 1.0, Ask, 10, Ask-(100*Point), Ask+(400*Point));       
         }
     }  
}

//+------------------------------------------------------------------+
//| Tester function                                                  |
//+------------------------------------------------------------------+

