function [ request ] = Read()
    file_name  = 'C:\Users\Jaap Oosterbroek\AppData\Roaming\MetaQuotes\Terminal\50CA3DFB510CC5A8F28B48D1BF2A5702\MQL4\Files\MetaTraderWrite.csv';  
    % attempt to open file
    fileID = -1;
    while(fileID == -1)         
        fileID = fopen(file_name, 'r');
        if(fileID == -1)
            disp('attempt reading')
            pause(1);  
        else            
            fclose(fileID);            
            request = csvread(file_name);
            disp('succes reading')
            return;
        end
    end
end

