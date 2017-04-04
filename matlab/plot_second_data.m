close all;

data = csvread('D:\GoogleDrive\TestData\Trading\indicators1sclean.csv', 0, 2);

future = 3600;
commission = 7;

%Convert data to values
ask = data(:,1) * 10000;
bid = data(:,2) * 10000;


buyCurve  = zeros(1, numel(ask) - future);
sellCurve = zeros(1, numel(ask) - future);

for i = 1: numel(buyCurve)

    max_val = -100000;
    min_val = 100000;

    for j = 1:future    
        max_val = max(max_val, bid(i + j));
        min_val = min(min_val, ask(i + j));
    end

    buyCurve(i) = max_val - ask(i) - commission;
    sellCurve(i) = bid(i) - min_val  - commission;
end


%%
close all;
figure(1)
plot((bid - min(bid)) / (max(bid) - min(bid)))
hold on
plot((buyCurve - min(buyCurve))  / (max(buyCurve) - min(buyCurve)))
plot((sellCurve- min(sellCurve))  / (max(sellCurve) - min(sellCurve)))
hold off
%%
indicatoren  = {'accelerator_oscillator', 'accumulation_distribution',' average_directional_index1',' average_directional_index2',' average_directional_index3',...
    'alligatorjaw',' alligatorteeth',' alligatorlips',' awesome_oscillator',' average_true_range',' bears',' bands_main',' bands_upper',' bands_lower',' bulls', ...
    'commodity_channel_index',' moving_average_9','moving_average_20',' moving_average_oscillator',' macd',' balance_volume',' stop_reverse',' relative_strenght',...
    'relative_vigor','standard_deviation',' stochastic_main',' stochastic_signal',' williams','demarker',' envelopes',' force_index',' fractals',' gatorjaw',...
    'gatorteeth','gatorlips','ichimoku','bwmfi',' momentum',' money_flow'};
%%
for i = 3:size(data,2)
    figure(2)
    scatter(sellCurve, data(1:end - future, i))
    title(indicatoren{i -2})
    pause()
end
%%
i = size(data,2);
figure(3)
scatter(buyCurve, data(1:end - future, i))
  title(indicatoren{i -2})


% sell : accumulation_distribution