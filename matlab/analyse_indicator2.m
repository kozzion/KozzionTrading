close all;
clear all;
%%
%Layout: open, high, low, close, year, month, day_of_month, day of week, minute_of_day,
data = csvread('D:\GoogleDrive\TestData\Trading\EUR_USD_1M_20160101_20161231.csv');
point_count = size(data, 1);
future      = 60;
commission  = 0.00007;
spread      = 0.00004;
point       = 0.00001;
volume      = 100000;

open      = data(:,1);
high      = data(:,2);
low       = data(:,3);
close_vals = data(:,4);

%%
%Indicator moving average
MA14 = zeros(1, point_count);
MA50 = zeros(1, point_count);

for time_index = 14:point_count  
    MA14(time_index) = mean(open (time_index - 13 : time_index));
end

for time_index = 50:point_count  
    MA50(time_index) = mean(open (time_index - 49 : time_index));
end

%%

MAdiv = zeros(1, point_count);
div = abs(MA50 - MA14);

for time_index = 64:point_count
    MAdiv(time_index) = mean(div (time_index - 13 : time_index));
end

figure(1)
plot(MAdiv(64:point_count))
%%
numel(MAdiv(64:end - future))
trade_indexes = find(MAdiv(64:end - future) < 0.0002);
trade_count   = numel(trade_indexes);

%%
%Convert data to values

take_profit_values = (20:20:260) * point;
stop_loss_values   = (20:20:260) * point;
take_profit_count  = numel(take_profit_values);
stop_loss_count    = numel(stop_loss_values);
profit_matrix      = zeros(trade_count, take_profit_count, stop_loss_count);
for time_index_index = 1:trade_count
    time_index = trade_indexes(time_index_index);
    for take_profit_index = 1:take_profit_count 
        take_profit = take_profit_values(take_profit_index);
        for stop_loss_index = 1:stop_loss_count 
        	stop_loss = stop_loss_values(stop_loss_index);
            found = false;
            for future_index = 0:future 
                if(open(time_index) + spread + take_profit < (high(time_index + future_index)))
                    profit = take_profit - spread - commission;
                    found = true;
                    break;                    
                else
                    if(low(time_index + future_index) < open(time_index) - stop_loss)
                        profit = -stop_loss - spread - commission;       
                        found = true;
                        break;  
                    end
                end
            end
            
            if (found == false)  
                profit = close_vals(time_index + future) - open(time_index) - spread - commission;
            end
            profit_matrix(time_index_index, take_profit_index, stop_loss_index) = profit;
        end
    end
end

%%
profit_mean = squeeze(mean(profit_matrix)) * volume;
profit_var = squeeze(var(profit_matrix)) * volume;



%% plot
close all;
figure(2)
imagesc(profit_mean, [min(profit_mean(:)) max(profit_mean(:))])
colorbar()
xlabel('Stop loss')
ylabel('Take profit')
set(gca,'YTick',[1, (take_profit_count + 1)/2,  take_profit_count]);
set(gca,'YtickLabels',[20,140 260]);
set(gca,'XTick',[1, (stop_loss_count + 1)/ 2, stop_loss_count]);
set(gca,'XtickLabels',[20,140 260]);


figure(3)
imagesc(profit_var, [min(profit_var(:)) max(profit_var(:))])
colorbar()
xlabel('Stop loss')
ylabel('Take profit')
set(gca,'YTick',[1, (take_profit_count + 1)/2,  take_profit_count]);
set(gca,'YtickLabels',[20,140 260]);
set(gca,'XTick',[1, (stop_loss_count + 1)/ 2, stop_loss_count]);
set(gca,'XtickLabels',[20,140 260]);

return
%%