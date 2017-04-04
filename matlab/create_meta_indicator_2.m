close all;
clear all;
%%
data = csvread('D:\GoogleDrive\TestData\Trading\EUR_USD_1M.csv');
point_count = size(data, 1);
future      = 30;
commission  = 7;
spread      = 0.00004;

open = data(:,1);
high = data(:,2);
low = data(:,3);

%%
%Convert data to values

buyCurve  = zeros(1, point_count - future);
buyIndex  = zeros(1, point_count - future);
buyLow    = zeros(1, point_count - future);
sellCurve = zeros(1, point_count - future);
sellIndex = zeros(1, point_count - future);
sellHigh  = zeros(1, point_count - future);


for i = 1: (point_count - future)  
    [max_val, max_index] = max(high(i:i + future));
    [min_val, min_index] = min(low(i:i + future));
    
    buyCurve(i)  = ((max_val - (open(i) + spread)) * 100000) - commission;
    buyIndex(i)  = (i + max_index - 1);
    buyLow(i)    = (open(i) - min(low(i:buyIndex(i)))) * 100000;
    sellCurve(i) = ((open(i) - (min_val + spread)) * 100000) - commission;
    sellIndex(i) = (i + min_index - 1);
    sellHigh(i)  = (max(high(i:sellIndex(i))) -  open(i)) * 100000;
end


%%
close all;
figure(1)
plot(open)

figure(2)
plot(buyCurve)


figure(3)
plot(buyLow)

figure(4)
scatter(buyCurve, buyLow)

%% sim strategy

