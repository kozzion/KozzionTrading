function [ output_args ] = Write( responce, order, stop_loss, take_profit)
    output_file_name  = 'C:\Users\Jaap Oosterbroek\AppData\Roaming\MetaQuotes\Terminal\50CA3DFB510CC5A8F28B48D1BF2A5702\MQL4\Files\MetaTraderRead.csv';
    csvwrite(output_file_name, [responce order stop_loss take_profit]);
    disp('succes writing')
end

