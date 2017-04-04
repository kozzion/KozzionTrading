close all;
clear all;
import_script
%%
%Layout: open, high, low, close, year, month, day_of_month, day of week, minute_of_day,
data = csvread('D:\GoogleDrive\TestData\Trading\EUR_USD_1M_20160101_20161231.csv');
point_count = size(data, 1);
history_size = 10;
future      = 60;
commission  = 0.00007;
spread      = 0.00004;
point       = 0.00001;
volume      = 100000;

open_values      = data(:,1);
high_values      = data(:,2);
low_values       = data(:,3);
close_values     = data(:,4);

%%
%Indicator moving average
%Convert data to values

long_profit     = zeros(1, point_count);
long_index      = zeros(1, point_count);
long_stop_loss  = zeros(1, point_count);
short_profit    = zeros(1, point_count);
short_index     = zeros(1, point_count);
short_stop_loss = zeros(1, point_count);

for i = 1: (point_count - future)  
    [max_val, max_index] = max(high_values(i:i + future));
    [min_val, min_index] = min(low_values(i:i + future));
    
    long_profit(i)  = ((max_val - (open_values(i) + spread + commission)) * volume);
    long_index(i)  = (i + max_index - 1);
    long_stop_loss(i)    = (open_values(i) - min(low_values(i:long_index(i)))) * volume;
    short_profit(i) = ((open_values(i) - (min_val + spread + commission)) * volume);
    short_index(i) = (i + min_index - 1);
    short_stop_loss(i)  = (max(high_values(i:short_index(i))) -  open_values(i)) * volume;
end

%% 
%create feature_data;

feature_data = zeros(point_count, 4 * history_size );
offsets_data = zeros(point_count, 4 * history_size );
for history_index =0:(history_size - 1)
    feature_data(history_size + 1:end,1 +(history_index * 4)) = open_values(history_index + 1:end - history_size + history_index );
    feature_data(history_size + 1:end,2 +(history_index * 4)) = high_values(history_index + 1:end - history_size + history_index);
    feature_data(history_size + 1:end,3 +(history_index * 4)) = low_values(history_index + 1:end - history_size + history_index);
    feature_data(history_size + 1:end,4 +(history_index * 4)) = close_values(history_index + 1:end - history_size + history_index);
    offsets_data(history_size + 1:end,1 +(history_index * 4)) = close_values(history_size:end - 1);
    offsets_data(history_size + 1:end,2 +(history_index * 4)) = close_values(history_size:end - 1);
    offsets_data(history_size + 1:end,3 +(history_index * 4)) = close_values(history_size:end - 1);
    offsets_data(history_size + 1:end,4 +(history_index * 4)) = close_values(history_size:end - 1);
end
feature_data = (feature_data - offsets_data) * volume;
% create label_data;
long_label_data = 100 < long_profit';
short_indexes  = 100 < short_profit';

% select training_data
training_data_count = 3000;
testing_data_count  = 3000;
valid_indexes = zeros(1, point_count);
valid_indexes(history_size + 1: point_count-future) = 1;
valid_index_indexes = find(valid_indexes == 1);
random_valid_indexes = valid_index_indexes(randperm(numel(valid_index_indexes)));

training_indexes = zeros(1, point_count);
training_indexes(random_valid_indexes(1:training_data_count)) = 1;
training_indexes = logical(training_indexes);


testing_indexes = zeros(1, point_count);
testing_indexes(random_valid_indexes(training_data_count + 1:training_data_count + 1 + testing_data_count)) = 1;
testing_indexes = logical(training_indexes);
%svmtrain(feature_data(training_indexes,:),long_label_data(training_indexes))
%%
c_value = 0.03;
g_value = 0.0012;
svm_model_long = svmtrain(double(long_label_data(training_indexes)), double(feature_data(training_indexes,:)), ['-b 1 -c ' num2str(c_value) ' -g ' num2str(g_value)]);

[predicted_label, accuracy, scores] = svmpredict(double(long_label_data(training_indexes)), feature_data(training_indexes,:), svm_model_long, '-b 1');
[roc_x, roc_y, ~, auc_d] = perfcurve(long_label_data(training_indexes), scores(:,2) , 1);


[predicted_label, accuracy, scores] = svmpredict(double(long_label_data(testing_indexes)), feature_data(testing_indexes,:), svm_model_long, '-b 1');
[roc_x, roc_y, ~, auc_v] = perfcurve(long_label_data(training_indexes), scores(:,2) , 1);
%%
figure(1)
plot(roc_x, roc_y)
auc_d
auc_v
%% build meta_indicator
[predicted_label, accuracy, scores] = svmpredict(double(long_label_data), feature_data, svm_model_long, '-b 1');
%%
indicator = scores(:,2)';

figure(2)
plot(indicator)
hold on
plot(long_profit / max(long_profit))
hold off
%% strategy
trade_indexes = find(0.5 < indicator & valid_indexes);

%%
%Convert data to values

take_profit_values = (20:40:520) * point;
stop_loss_values   = (20:40:520) * point;
take_profit_count  = numel(take_profit_values);
stop_loss_count    = numel(stop_loss_values);
trade_count        = numel(trade_indexes);
profit_matrix      = zeros(trade_count, take_profit_count, stop_loss_count);
for time_index_index = 1:trade_count
    time_index = trade_indexes(time_index_index);
   
    for take_profit_index = 1:take_profit_count 
        take_profit = take_profit_values(take_profit_index);
        for stop_loss_index = 1:stop_loss_count 
        	stop_loss = stop_loss_values(stop_loss_index);
            found = false;
            
            for future_index = 0:future 
               
                if(open_values(time_index) + spread + take_profit < (high_values(time_index + future_index)))
                    
                    profit = take_profit - spread - commission;
                    found = true;
                    break;                    
                else
                       
                    if(low_values(time_index + future_index) < open_values(time_index) - stop_loss)
                        profit = -stop_loss - spread - commission;       
                        found = true;
                        break;  
                    end
                end
            end
          
            if (found == false)  
                profit = close_values(time_index + future) - open_values(time_index) - spread - commission;
            end
            profit_matrix(time_index_index, take_profit_index, stop_loss_index) = profit;
        end
    end
end

%%
profit_mean = squeeze(mean(profit_matrix)) * volume;
profit_var = squeeze(var(profit_matrix)) * volume;

take_profit_points = take_profit_values * volume;
stop_loss_points   =  stop_loss_values * volume;
% plot
close all;
figure(2)
imagesc(profit_mean, [min(profit_mean(:)) max(profit_mean(:))])
colorbar()
title('expected profit')
xlabel('Stop loss')
ylabel('Take profit')
set(gca,'YTick',[1, (take_profit_count + 1)/2,  take_profit_count]);
set(gca,'YtickLabels',[min(take_profit_points), (max(take_profit_points)- min(take_profit_points)) / 2, max(take_profit_points)]);
set(gca,'XTick',[1, (stop_loss_count + 1)/ 2, stop_loss_count]);
set(gca,'XtickLabels',[min(stop_loss_points), (max(stop_loss_points)- min(stop_loss_points)) / 2, max(stop_loss_points)]);


figure(3)
imagesc(profit_var, [min(profit_var(:)) max(profit_var(:))])
colorbar()
title('expected profit variance')
xlabel('Stop loss')
ylabel('Take profit')
set(gca,'YTick',[1, (take_profit_count + 1)/2,  take_profit_count]);
set(gca,'YtickLabels',[min(take_profit_points), (max(take_profit_points)- min(take_profit_points)) / 2, max(take_profit_points)]);
set(gca,'XTick',[1, (stop_loss_count + 1)/ 2, stop_loss_count]);
set(gca,'XtickLabels',[min(stop_loss_points), (max(stop_loss_points)- min(stop_loss_points)) / 2, max(stop_loss_points)]);

return
%%
REQUEST_RESET  = 0;
REQUEST_ORDER  = 1;

RESPONSE_READY = 0;
RESPONSE_ERROR = 1;
RESPONSE_ORDER = 2;

ORDER_NONE     = 0;
ORDER_BUY      = 1;
ORDER_SELL     = 2;

while(true)
    % attempt to open file
    [request, data] = Read();  

    if(request== REQUEST_RESET)    
       history = zeros(0,4)
       Write(RESPONSE_READY, 0, 0, 0);         
    end
    
    if(request == REQUEST_ORDER)  
        % add data
        % compute somthing
       %  [predicted_label, accuracy, scores] = svmpredict(double(long_label_data), feature_data, svm_model_long, '-b 1');
         scores = [0 2];
        if(0.5 < scores(2'))
            Write(RESPONSE_ORDER, ORDER_BUY, stop_loss, take_profit);     
        else
            Write(RESPONSE_ORDER, ORDER_NONe, 0, 0);    
        end        
    end

end