close all;
clear all;

%Layout: open, high, low, close, year, month, day_of_month, day of week, minute_of_day,
source_data = csvread('D:\GoogleDrive\TestData\Trading\EUR_USD_1M_20160101_20161231.csv');
point_count = size(source_data, 1);
history_size = 10;

spread       = 0.00014;
point        = 0.00001;
volume       = 100000;
spread_point = spread * volume; 

open_values      = source_data(:,1);
high_values      = source_data(:,2);
low_values       = source_data(:,3);
close_values     = source_data(:,4);

%% create feature data set

feature_data = zeros(point_count, history_size);
yields       = zeros(point_count, 1);
for time_index = 1: point_count
    feature_data(time_index,:) = feature_generator_0(source_data, time_index, history_size);
end
yields(1:end -1) = (source_data(2:end,1) - source_data(2:end,4)) / point ;
yields(1:21)


%% plot some features
bin_count  = 20;
percentile = 1;
feature_index = 1;

[ target_1, target_2 ] = sanitize_2d( feature_data(:,feature_index), yields(:), percentile );
figure()
hist3([target_1,  target_2],[bin_count bin_count])

%% generate random NN
close all
count_0_0 = history_size;
count_0_1 = 10;
count_1_2 = 1;


weigths_0 = (rand(count_0_0 + 1, count_0_1) * 2) - 1;
weigths_1 = (rand(count_0_1 + 1, count_1_2) * 2) - 1;
output_data = network_0( weigths_0, weigths_1, feature_data);
lower  = prctile(output_data, 25);
centre = prctile(output_data, 50);
upper  = prctile(output_data, 75);

error = abs(lower - 0.25) + abs(centre - 0.50) + abs(upper - 0.75)
figure()
hist(output_data, 20)

%% precondition
iter = 100;

error = abs(prctile(output_data, 25) - 0.25) + abs(prctile(output_data, 50) - 0.50) + abs(prctile(output_data, 75) - 0.75)
for i = 1:iter
    weigths_0_update = (rand(count_0_0 + 1, count_0_1) - 0.5) * 0.1;
    weigths_1_update = (rand(count_0_1 + 1, count_1_2) - 0.5) * 0.1;
    weigths_0_new = weigths_0 + weigths_0_update;
    weigths_1_new = weigths_1 + weigths_1_update;
    output_data = network_0( weigths_0_new, weigths_1_new, feature_data);
    lower  = prctile(output_data, 25);
    centre = prctile(output_data, 50);
    upper  = prctile(output_data, 75);
    error_new = abs(prctile(output_data, 25) - 0.25) + abs(prctile(output_data, 50) - 0.50) + abs(prctile(output_data, 75) - 0.75);
    if(error_new < error)
        weigths_0 = weigths_0_new;
        weigths_1 = weigths_1_new;
        error = error_new;
    end
end

error
figure()
hist(output_data, 20)


%% sim
[ yields_all, yields_sum ] = simulate_0( output_data, yields);
fitness = yields_sum(end)

figure
plot(yields(1:200))
hold on
plot(yields_all(1:200))
plot(yields_sum(1:200))
hold off

%% update weights
iter = 1000;

for i = 1:iter
    weigths_0_update = (rand(count_0_0 + 1, count_0_1) - 0.5) * 0.01;
    weigths_1_update = (rand(count_0_1 + 1, count_1_2) - 0.5) * 0.01;
    weigths_0_new = weigths_0 + weigths_0_update;
    weigths_1_new = weigths_1 + weigths_1_update;

    output_data = network_0(weigths_0_new, weigths_1_new, feature_data);
    fitness_new = evaluate_1( yields, output_data);

    if(fitness < fitness_new)
        weigths_0 = weigths_0_new;
        weigths_1 = weigths_1_new;
        fitness = fitness_new;
         disp('improved');
    end
    
    if( mod(i, iter/10)== 0)
        disp(i/iter);
        disp(fitness);
    end
end
[ yields_all, yields_sum ] = simulate_0( output_data, yields);
yield = yields_sum(end)
fitness

%%
close all
[ yields_all, yields_sum ] = simulate_0( output_data, yields);
yield = yields_sum(end)
trades = sum(0.25 < abs(output_data - 0.5))

figure(2)
plot(yields(1:200))
hold on
plot(yields_all(1:200))
plot(yields_sum(1:200))
hold off

figure(3)
hist(output_data, 20)


[ target_1, target_2 ] = sanitize_2d( output_data, yields, 1 );
figure(4)
hist3([target_1,  target_2],[bin_count bin_count])

%% plot trades


