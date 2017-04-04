close all;
clear all;
%%
%Layout: open, high, low, close, year, month, day_of_month, day of week, minute_of_day,
data = csvread('D:\GoogleDrive\TestData\Trading\EUR_USD_1M_20160101_20161231.csv');
point_count = size(data, 1);
future      = 60;
commission  = 0.00007;
spread      = 0.00004;
volume      = 100000;

open      = data(:,1);
high      = data(:,2);
low       = data(:,3);
%% get indicator
indicator = zeros(1,point_count);
indicator(3:end) = ((high(1:end - 2) - low(1:end - 2)) + (high(2:end - 1) - low(2:end - 1))+ (high(3:end) - low(3:end))) / 3;
indicator = indicator * 100000;
%%
%Convert data to values

long_profit     = zeros(1, point_count);
long_index      = zeros(1, point_count);
long_stop_loss  = zeros(1, point_count);
short_profit    = zeros(1, point_count);
short_index     = zeros(1, point_count);
short_stop_loss = zeros(1, point_count);

for i = 1: (point_count - future)  
    [max_val, max_index] = max(high(i:i + future));
    [min_val, min_index] = min(low(i:i + future));
    
    long_profit(i)  = ((max_val - (open(i) + spread + commission)) * 100000);
    long_index(i)  = (i + max_index - 1);
    long_stop_loss(i)    = (open(i) - min(low(i:long_index(i)))) * 100000;
    short_profit(i) = ((open(i) - (min_val + spread + commission)) * 100000);
    short_index(i) = (i + min_index - 1);
    short_stop_loss(i)  = (max(high(i:short_index(i))) -  open(i)) * 100000;
end

analysis_valid                      = zeros(1, point_count);
analysis_valid(4:(point_count - future)) = 1;
%% analyse_indicator

indicator_resolution    = 10;
curve_resolution        = 20;
max_profit              = 500;

indicator_lower         = prctile(indicator, 1);
indicator_upper         = prctile(indicator, 99);
indicator_step          = (indicator_upper - indicator_lower)/indicator_resolution;
indicator_bounds        = indicator_lower:indicator_step:indicator_upper;

long_profit_bounds      = 0:max_profit/curve_resolution:max_profit;
long_stop_loss_bounds   = 0:max_profit/curve_resolution:max_profit;
short_profit_bounds     = 0:max_profit/curve_resolution:max_profit;
short_stop_loss_bounds  = 0:max_profit/curve_resolution:max_profit;


long_profit_matix       = zeros(indicator_resolution, curve_resolution);
long_stop_loss_matrix   = zeros(indicator_resolution, curve_resolution);
short_profit_matix      = zeros(indicator_resolution, curve_resolution);
short_stop_loss_matrix  = zeros(indicator_resolution, curve_resolution);

for indicator_bin_index = 1:indicator_resolution
    selection = analysis_valid & (indicator_bounds(indicator_bin_index) <= indicator) & (indicator < indicator_bounds(indicator_bin_index + 1));
    for curve_bin_index = 1:curve_resolution
        long_profit_matix(indicator_bin_index, curve_bin_index)      = nnz(selection & (long_profit_bounds(curve_bin_index) <= long_profit)         & (long_profit < long_profit_bounds(curve_bin_index+ 1)));
        long_stop_loss_matrix(indicator_bin_index, curve_bin_index)  = nnz(selection & (long_stop_loss_bounds(curve_bin_index) <= long_stop_loss)   & (long_stop_loss < long_stop_loss_bounds(curve_bin_index+ 1)));
        short_profit_matix(indicator_bin_index, curve_bin_index)     = nnz(selection & (short_profit_bounds(curve_bin_index) <= short_profit)       & (short_profit < short_profit_bounds(curve_bin_index+ 1)));
        short_stop_loss_matrix(indicator_bin_index, curve_bin_index) = nnz(selection & (short_stop_loss_bounds(curve_bin_index) <= short_stop_loss) & (short_stop_loss < short_stop_loss_bounds(curve_bin_index+ 1)));
    end
    long_profit_matix(indicator_bin_index, :)      = long_profit_matix(indicator_bin_index, :) / sum(long_profit_matix(indicator_bin_index, :));
    long_stop_loss_matrix(indicator_bin_index, :)  = long_stop_loss_matrix(indicator_bin_index, :) / sum(long_stop_loss_matrix(indicator_bin_index, :));
    short_profit_matix(indicator_bin_index, :)     = short_profit_matix(indicator_bin_index, :) / sum(short_profit_matix(indicator_bin_index, :));
    short_stop_loss_matrix(indicator_bin_index, :) = short_stop_loss_matrix(indicator_bin_index, :) / sum(short_stop_loss_matrix(indicator_bin_index, :));
end


%% plot
close all;

figure(1)
subplot(2,2,1)
imagesc(long_profit_matix, [0 0.5])
colorbar()
%set(gca,'Yscale','log','Ydir','normal');
title('Long profit')
xlabel('Profit')
ylabel('Indicator')
set(gca,'YTick',[1, (indicator_resolution + 1)/ 2, indicator_resolution]);
set(gca,'YtickLabels',[min(indicator_bounds),(min(indicator_bounds) + max(indicator_bounds)) / 2, max(indicator_bounds)]);
set(gca,'XTick',[1, (curve_resolution + 1)/ 2, curve_resolution]);
set(gca,'XtickLabels',[min(long_profit_bounds),(min(long_profit_bounds) + max(long_profit_bounds)) / 2, max(long_profit_bounds)]);

subplot(2,2,2)
imagesc(long_stop_loss_matrix, [0 0.5])
colorbar()
title('Long stop-loss')
xlabel('Stop-loss')
ylabel('Indicator')
set(gca,'YTick',[1, (indicator_resolution + 1)/ 2, indicator_resolution]);
set(gca,'YtickLabels',[min(indicator_bounds),(min(indicator_bounds) + max(indicator_bounds)) / 2, max(indicator_bounds)]);
set(gca,'XTick',[1, (curve_resolution + 1)/ 2, curve_resolution]);
set(gca,'XtickLabels',[min(long_profit_bounds),(min(long_profit_bounds) + max(long_profit_bounds)) / 2, max(long_profit_bounds)]);

subplot(2,2,3)
imagesc(short_profit_matix, [0 0.5])
colorbar()
title('Short profit')
xlabel('Profit')
ylabel('Indicator')
set(gca,'YTick',[1, (indicator_resolution + 1)/ 2, indicator_resolution]);
set(gca,'YtickLabels',[min(indicator_bounds),(min(indicator_bounds) + max(indicator_bounds)) / 2, max(indicator_bounds)]);
set(gca,'XTick',[1, (curve_resolution + 1)/ 2, curve_resolution]);
set(gca,'XtickLabels',[min(long_profit_bounds),(min(long_profit_bounds) + max(long_profit_bounds)) / 2, max(long_profit_bounds)]);

subplot(2,2,4)
imagesc(short_stop_loss_matrix, [0 0.5])
colorbar()
title('Short stop-loss')
xlabel('Stop-loss')
ylabel('Indicator')
set(gca,'YTick',[1, (indicator_resolution + 1)/ 2, indicator_resolution]);
set(gca,'YtickLabels',[min(indicator_bounds),(min(indicator_bounds) + max(indicator_bounds)) / 2, max(indicator_bounds)]);
set(gca,'XTick',[1, (curve_resolution + 1)/ 2, curve_resolution]);
set(gca,'XtickLabels',[min(long_profit_bounds),(min(long_profit_bounds) + max(long_profit_bounds)) / 2, max(long_profit_bounds)]);
%%
figure(3)
hist(indicator(analysis_valid == 1),100)
