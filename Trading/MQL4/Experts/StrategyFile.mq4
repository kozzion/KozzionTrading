//+------------------------------------------------------------------+
//|                                                 StrategyFile.mq4 |
//|                        Copyright 2016, MetaQuotes Software Corp. |
//|                                             https://www.mql5.com |
//+------------------------------------------------------------------+
#property copyright "Copyright 2016, MetaQuotes Software Corp."
#property link      "https://www.mql5.com"
#property version   "1.00"
#property strict

int file_index         = 0;
string buySignal ="0";
string buySignalArray[];
int tradeInput[3];
//+------------------------------------------------------------------+
//| Expert initialization function                                   |
//+------------------------------------------------------------------+
int OnInit()
{
   FolderClean(".");
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

   // get latest data,
   
   // write to file
   string output_file_name  = "MetaTraderWrite" + file_index + ".csv";
   string input_file_name  = "MetaTraderRead" + file_index + ".csv";
   
   ResetLastError();
   int output_file_handle = FileOpen(output_file_name, FILE_WRITE |FILE_CSV);
   if(output_file_handle != INVALID_HANDLE)
   {
      FileWrite(output_file_handle, Open[0], High[0], Low[0], Close[0]);
      FileFlush(output_file_handle);
      FileClose(output_file_handle);
      Print("FileOpen OK");
   }
   else
   {
      Print("Operation FileOpen failed, error ", GetLastError());
   }
   
   
   
   // read orders from file
   bool file_not_read = true;
   while(file_not_read)
   {
      //Attemt to open file
      if(FileIsExist(input_file_name))
      {
         int input_file_handle = FileOpen(input_file_name, FILE_READ |FILE_CSV);
         //Alert(input_file_handle);
         //Alert(input_file_name);
         if(input_file_handle != INVALID_HANDLE)
         {
            ushort u_sep = StringGetCharacter(",",0);
            buySignal = FileReadString(input_file_handle);
            file_not_read = false;
            StringSplit(buySignal, u_sep, buySignalArray);
            int buy         = buySignalArray[0];
            if(buy == 1)
            {
               // Make trade`
               Alert('making trade')
               double stop_loss   = buySignalArray[1];
               double take_profit = buySignalArray[2];
            }
         }
         FileClose(input_file_handle);
      }   
   }  
   Alert(file_index);
   file_index++;
}
//+------------------------------------------------------------------+
