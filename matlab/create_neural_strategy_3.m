close all;
clear all;
%%
%Layout: open, high, low, close, year, month, day_of_month, day of week, minute_of_day,
source_data = csvread('D:\GoogleDrive\TestData\Trading\EUR_USD_1M_20160101_20161231.csv');
source_data = source_data(:,1:4);
point_count = size(source_data, 1);

source_data(1:end-1,4) = source_data(2:end,1);
trend                  = (source_data(end,1) - source_data(1,1)) / size(source_data,1)
trend_line             = ((0:size(source_data,1) - 1) * trend)';

figure(1)
plot(source_data(:,1))
hold on
plot(source_data(1,1) + trend_line)
plot(source_data(:,1) - trend_line)
hold off

source_data(:,1) = source_data(:,1) - trend_line;
source_data(:,2) = source_data(:,2) - trend_line;
source_data(:,3) = source_data(:,3) - trend_line;
source_data(:,4) = source_data(:,4) - trend_line;

spread       = 0.00014;
point        = 0.00001;
volume       = 100000;
spread_point = spread * volume; 

% create data set
history_size = 240;
future_size = 10;

feature_data = zeros(point_count, history_size);
label_data   = zeros(point_count, 1);
for time_index = 1: point_count
    feature_data(time_index,:) = feature_generator_0(source_data, time_index, history_size);
end
label_data(1:end - future_size) = (source_data(1 + future_size:end,4) - source_data(1:end - future_size, 4)) / point ;


label_min = min(label_data);
label_max = max(label_data);
label_data_scaled = (label_data - label_min) / (label_max - label_min)

%% generate random NN
count_0_0 = size(feature_data,2);
count_0_1 = 40;
count_1_2 = size(label_data,2);
learning_rate = 0.000001;
iter      = 1000;


weigths_0 = (rand(count_0_0 + 1, count_0_1) * 2) - 1;
weigths_1 = (rand(count_0_1 + 1, count_1_2) * 2) - 1;
%%

%% simulation gradient descent
close all
errors = zeros(1,iter);
learning_rate = 0.0000002;
for i = 1:iter
  
    [weigths_0_gradient, weigths_1_gradient] = network_0_gradient_backprop_error(weigths_0, weigths_1, feature_data, label_data_scaled);    
    weigths_0 = weigths_0 + weigths_0_gradient * learning_rate;
    weigths_1 = weigths_1 + weigths_1_gradient * learning_rate;
    output_data = network_0( weigths_0, weigths_1, feature_data);
    errors(i) = fitness_error(output_data, label_data_scaled) * (label_max - label_min);

    correl =  corr(output_data, label_data_scaled)
    errors(i)    
    figure(1)
    plot(errors(1:i))
    pause(0.01)
end


%% plot other results
close all
output_data = network_0( weigths_0, weigths_1, feature_data);
label_data_rescaled = (output_data * (label_max - label_min)) + label_min;
bin_count = 20;

[ yields_all, yields_sum ] = simulate_1(label_data_rescaled, label_data, -20, 20);
yield = yields_sum(end)
trades = sum(0.25 < abs(label_data_rescaled - 0.5))

figure(2)
plot(label_data(1:2000))
hold on
plot(yields_all(1:2000))
plot(yields_sum(1:2000))
hold off

figure(3)
subplot(2,1,1)
hist(label_data_rescaled, 100)
subplot(2,1,2)
hist(label_data, 20, 100)


[ target_1, target_2 ] = sanitize_2d( label_data_rescaled, label_data, 1 );
figure(4)
hist3([target_1,  target_2],[bin_count bin_count])


figure(5)
subplot(2,2,1)
plot((source_data(:,1) - source_data(1,1)) / point )

subplot(2,2,2)
plot(label_data)
hold on
plot(label_data_rescaled)
hold off

subplot(2,2,3)
plot(label_data_rescaled)

subplot(2,2,4)
plot(cumsum((source_data(:,4) - source_data(:,1))) / point )
hold on
plot(yields_sum)
hold off
%%
figure(5)
plot(cumsum((source_data(2:12,4) - source_data(2:12,1))) / point - 14)
hold on
plot((source_data(3:13,1) - source_data(1,1)) / point- 14, 'y' )
hold off
%%

spread_point = 14;
short = label_data_rescaled < - 20;
long  =  20 < label_data_rescaled;

yields_short = label_data .* -short;
yields_long  = label_data .* long;

cost_short = (0.5 < conv(double(short),[1, -1])) * -spread_point;
cost_long  = (0.5 < conv(double(long), [1, -1])) * -spread_point;
cost_short = cost_short(1:end - 1);
cost_long  = cost_long(1:end - 1);
yields_all = yields_short + yields_long + cost_short + cost_long;
yields_sum = cumsum(yields_all);
yields_sum(end)

%%
%write script



row_count = 1 + size(weigths_0, 1) + size(weigths_1, 1);
column_count = max([size(weigths_0, 2), size(weigths_1, 2), 5]);
table = zeros(row_count, column_count);
size(table)
layer_count = 2;
table(2:size(weigths_0, 1) +1 , 1:size(weigths_0, 2)) = weigths_0
table(2 + size(weigths_0, 1):1 + size(weigths_0, 1)+ size(weigths_1, 1), 1:size(weigths_1, 2)) = weigths_1

table(1,1) = layer_count;
table(1,2) = 2;
table(1,3) = 1 + size(weigths_0, 1);
table(1,4) = size(weigths_0, 2);
table(1,5) = 2 + size(weigths_0, 1);
table(1,6) = 1 + size(weigths_0, 1)+ size(weigths_1, 1);
table(1,7) = size(weigths_1, 2);

csvwrite('test.csv', table)
%% Read script
table = csvread('test.csv')
weigths_0 = table(table(1,2):table(1,3), 1:table(1,4));
weigths_1 = table(table(1,5):table(1,6), 1:table(1,7));


























