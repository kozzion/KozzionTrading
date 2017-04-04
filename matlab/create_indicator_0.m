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

open_values      = data(:,1)';
high_values      = data(:,2)';
low_values       = data(:,3)';
close_values     = data(:,4)';

%%
index_start_train = 60* 24 * 4;
index_end_train   = numel(open_values) - 60* 24 * 4;

train_values      = open_values(index_start_train:index_end_train);
train_size        = numel(train_values);
train_time        = 1:train_size;

test_values       = open_values(index_start_train:index_end_train);
test_size         = numel(train_values);
test_time         = 1:train_size;



%%
index_start_ind   = 60* 24;
index_end_ind     = train_size - 60 * 24;
indicator_error   = zeros(1,train_size);
indicator_error_s = zeros(1,train_size);
indicator_error_z = zeros(1,train_size);
indicator_error_w = zeros(1,train_size);
indicator         = zeros(1,train_size);

%
%Convert data to values

buyCurve  = zeros(1, train_size);
buyIndex  = zeros(1, train_size);
buyLow    = zeros(1, train_size);
sellCurve = zeros(1, train_size);
sellIndex = zeros(1, train_size);
sellHigh  = zeros(1, train_size);


% inti
indicator(index_start_ind - 2) = train_values(index_start_ind - 1);
indicator(index_start_ind - 1) = train_values(index_start_ind - 1);

momentum_weight  = 0.001;
expected_error_z = 30;
max_error_weight = 0.2;

% compute indicator
for i = index_start_ind :index_end_ind
    %indicator_0(i) = median(selected(i - window_0:i));    
    %indicator_1(i) = median(selected(i - window_1:i));   
 
    last           = indicator(i - 1);
    momentum       = indicator(i - 1) - indicator(i - 2);    
    
    error_now      = train_values(i) - (last + (momentum_weight * momentum));
    error_now_s    = var(indicator_error(i - 60:i))^0.5;
    error_now_z    = error_now / error_now_s;
    error_now_weight   = max_error_weight * (1 - exp(error_now_z^2/ - expected_error_z));   
  
    indicator_error(i)   = error_now;
    indicator_error_s(i) = error_now_s;
    indicator_error_z(i) = error_now_z;
    indicator_error_w(i) = error_now_weight;   
    %indicator(i)         = last + (momentum * momentum_weight) - (error * error_weight);
    indicator(i)         = last + (momentum * momentum_weight) + (error_now * error_now_weight);
end

%%

future = 200;
for i = 1: (train_size - future)  
    [max_val, max_index] = max(train_values(i:i + future));
    [min_val, min_index] = min(train_values(i:i + future));
    
    buyCurve(i)  = ((max_val - (train_values(i) + spread)) * 100000) - commission;
    buyIndex(i)  = (i + max_index - 1);
    buyLow(i)    = (train_values(i) - min(train_values(i:buyIndex(i)))) * 100000;
    sellCurve(i) = ((train_values(i) - (min_val + spread)) * 100000) - commission;
    sellIndex(i) = (i + min_index - 1);
    sellHigh(i)  = (max(train_values(i:sellIndex(i))) -  train_values(i)) * 100000;
end



%%
plot_start = index_start_ind + 10500;
plot_end   = index_start_ind + 11500;

figure(1)
subplot(5,1,1)
plot(train_time(plot_start :plot_end), train_values(plot_start :plot_end))
hold on
plot(train_time(plot_start :plot_end), indicator(plot_start :plot_end), 'k')
plot(train_time(plot_start :plot_end), indicator(plot_start :plot_end) + 2* indicator_error_s(plot_start :plot_end), 'k')
plot(train_time(plot_start :plot_end), indicator(plot_start :plot_end) - 2* indicator_error_s(plot_start :plot_end), 'k')
hold off

subplot(5,1,2)
plot(train_time(plot_start :plot_end), indicator_error(plot_start :plot_end), 'g')
hold on
hold off

title('error')

subplot(5,1,3)
plot(train_time(plot_start :plot_end), indicator_error_s(plot_start :plot_end), 'g')
hold on
hold off

title('error s')

subplot(5,1,4)
plot(train_time(plot_start :plot_end), indicator_error_z(plot_start :plot_end), 'r')
hold on
hold off

title('error Z')

subplot(5,1,5)
plot(train_time(plot_start :plot_end), indicator_error_w(plot_start :plot_end), 'g')
hold on
hold off

title('error weigth')

%%
figure(2)
subplot(2,1,1)
plot(train_time(plot_start :plot_end), train_values(plot_start :plot_end))
hold on
plot(train_time(plot_start :plot_end), indicator(plot_start :plot_end), 'k')
plot(train_time(plot_start :plot_end), indicator(plot_start :plot_end) + 2* indicator_error_s(plot_start :plot_end), 'k')
plot(train_time(plot_start :plot_end), indicator(plot_start :plot_end) - 2* indicator_error_s(plot_start :plot_end), 'k')
hold off

subplot(2,1,2)
plot(train_time(plot_start :plot_end), buyCurve(plot_start :plot_end), 'r')

%%
time  = train_time(plot_end - 200 :plot_end);
upper = indicator(plot_end - 200 :plot_end) + 2* indicator_error_s(plot_end- 200 :plot_end)
lower = indicator(plot_end - 200 :plot_end) - 2* indicator_error_s(plot_end- 200 :plot_end)

P = polyfit(time,upper,1);
upper_fit = P(1)*time+P(2);
 

P = polyfit(time,lower,1);
lower_fit = P(1)*time+P(2);

figure(3)
plot(train_time(plot_start :plot_end), train_values(plot_start :plot_end))
hold on
plot(train_time(plot_start :plot_end), indicator(plot_start :plot_end), 'k')
plot(train_time(plot_start :plot_end), indicator(plot_start :plot_end) + 2* indicator_error_s(plot_start :plot_end), 'k')
plot(train_time(plot_start :plot_end), indicator(plot_start :plot_end) - 2* indicator_error_s(plot_start :plot_end), 'k')
plot(time,upper_fit, 'b')
plot(time,lower_fit, 'b')
hold off
    

    
%% exsadf
s = 20;
d = 0:100
v = 1 - (normpdf(d, 0, s) / normpdf(0, 0, s))
figure(3)
plot(d,v)


%% exsadf

d = -1:0.01:1
s = 1;
maxv = 0.1
v = maxv * (1 - exp(- ((d * 0.2).^4/ 6)));
figure(3)
plot(d,v)