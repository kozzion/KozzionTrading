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
indicatoren  = {'Bid', 'Ask', 'accelerator_oscillator', 'accumulation_distribution',' average_directional_index1',' average_directional_index2',' average_directional_index3',...
    'alligatorjaw',' alligatorteeth',' alligatorlips',' awesome_oscillator',' average_true_range',' bears',' bands_main',' bands_upper',' bands_lower',' bulls', ...
    'commodity_channel_index',' moving_average_9','moving_average_20',' moving_average_oscillator',' macd',' balance_volume',' stop_reverse',' relative_strenght',...
    'relative_vigor','standard_deviation',' stochastic_main',' stochastic_signal',' williams','demarker',' envelopes',' force_index',' fractals',' gatorjaw',...
    'gatorteeth','gatorlips','ichimoku','bwmfi',' momentum',' money_flow'};

%%
goodIndicatorenBuy  = {'Bid', 'Ask', 'accelerator_oscillator', 'accumulation_distribution',' average_directional_index1',' average_directional_index2',' average_directional_index3',...
    'alligatorjaw',' alligatorteeth',' awesome_oscillator',' bears',' bands_lower',' bulls', ...
    'commodity_channel_index',' balance_volume',' stop_reverse',' relative_strenght',...
    'relative_vigor',' stochastic_main',' stochastic_signal',' williams','demarker',...
    'gatorteeth','bwmfi',' momentum',' money_flow'};
%%

weights = data(1:numel(buyCurve),:) \ sellCurve';
meta_linear = data(1:numel(buyCurve),:)* weights;
% sell : accumulation_distribution
figure(2)
plot(sellCurve)
hold on
plot(meta_linear)
hold off

figure(3)
scatter(buyCurve, meta_linear)

%%
contr = mean (data)'.*weights

for i = 1:numel(indicatoren)
   if (abs(contr(i)) > 0.0001)
    disp(indicatoren{i})
    disp(weights(i))
    disp(contr(i))
   end
end