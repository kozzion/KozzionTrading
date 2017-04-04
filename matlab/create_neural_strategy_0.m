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
label_data   = zeros(point_count, 1);
for time_index = 1: point_count
    feature_data(time_index,:) = feature_generator_0(source_data, time_index, history_size);
end
label_data(1:end -1) = (source_data(2:end,1) - source_data(2:end,4)) /point ;


%% plot some features
bin_count  = 20;
percentile = 1;
feature_index = 1;

[ target_1, target_2 ] = sanitize_2d( feature_data(:,feature_index), label_data(:), percentile );
figure()
hist3([target_1,  target_2],[bin_count bin_count])

%% generate random NN

count_0_0 = history_size;
count_0_1 = 10;
count_1_2 = 1;
weigths_0 = rand(count_0_0 + 1, count_0_1) - 0.5;
weigths_1 = rand(count_0_1 + 1, count_1_2) - 0.5;
%
[feature_data_rescaled, slope, intercept] = rescale_features(feature_data, -1, 1);
%%
input_0   = f_linear_augment(feature_data_rescaled);
output_0  = input_0 * weigths_0;
input_1   = f_sigmoid_augment(output_0);
output_1  = input_1 * weigths_1;
input_2   = f_linear_augment(output_1);
prediction_data_rescaled = input_2(:,1);

prediction_data = descale_features(prediction_data_rescaled, slope, intercept);
residual_data = prediction_data - label_data;
error = mean(abs(residual_data))
%% update weights
error
iter = 1000
for i = 1:iter
    weigths_0_update = (rand(count_0_0 + 1, count_0_1) - 0.5) * 0.01;
    weigths_1_update = (rand(count_0_1 + 1, count_1_2) - 0.5) * 0.01;
    weigths_0_new = weigths_0 + weigths_0_update;
    weigths_1_new = weigths_1 + weigths_1_update;

    %
    output_0  = input_0 * weigths_0_new;
    input_1   = f_sigmoid_augment(output_0);
    output_1  = input_1 * weigths_1_new;
    input_2   = f_linear_augment(output_1);
    prediction_data_rescaled = input_2(:,1);
    prediction_data = descale_features(prediction_data_rescaled, slope, intercept);
    residual_data = prediction_data - label_data;
    error_new = mean(abs(residual_data));

    if(error_new < error)
        weigths_0 = weigths_0_new;
        weigths_1 = weigths_1_new;
        error = error_new;
    end
end
error

%% make this better, use it as fitness

short = prediction_data < -25;
long  = 25 < prediction_data;
sum(-label_data(short))
sum(label_data(long))
sum(short)
sum(long)
%% plot correleation and residual
close all
bin_count  = 20;
percentile = 1;
feature_index = 1;
residual = prediction_data - label_data;

figure()
[ target_1, target_2 ] = sanitize_2d( label_data, prediction_data, percentile );
hist3([target_1,  target_2],[bin_count bin_count])

figure()
[ target_1, target_2 ] = sanitize_2d(label_data, residual, percentile );
hist3([target_1,  target_2],[bin_count bin_count])