%% load source
clear all;
close all;



%% load data
% load data 1
data_op = csvread('D:\GoogleDrive\TestData\Trading\Simulation\OpenPricesClean.csv');
% load data 2
data_cp = csvread('D:\GoogleDrive\TestData\Trading\Simulation\ControlPointsClean.csv');
% load data 3
data_et = csvread('D:\GoogleDrive\TestData\Trading\Simulation\EveryTickClean.csv');
% load source
data_sr = csvread('D:\GoogleDrive\TestData\Trading\Simulation\HistoryClean.csv');
data_sr(:,1) = data_sr(:,1) - 7200; %% off by 2 hours... don't ask
% toto
%% check alling
% figure(1)
% plot(data_op(:,1) - 1464747000, data_op(:,2), 'b')
% hold on
% plot(data_sr(:,1) - 1464747000 - 7200, data_sr(:,2), 'r')
% hold off
%% layout: Bid, Ask, Open[0], High[0], Low[0], Close[0], Open[1], High[1], Low[1], Close[1]

%% plot a little
lot = 100000;

col = 2;
lower_limit = data_op(1,1);
upper_limit = data_op(20,1);
figure(1)
plot(data_op(data_op(:, 1) <= upper_limit, 1), data_op(data_op(:,1) <= upper_limit, col), 'b')
hold on
plot(data_cp(data_cp(:, 1) <= upper_limit, 1), data_cp(data_cp(:,1) <= upper_limit, col), 'r')
plot(data_et(data_et(:, 1) <= upper_limit, 1), data_et(data_et(:,1) <= upper_limit, col), 'g')
plot(data_sr(data_sr(:, 1) <= upper_limit & lower_limit <= data_sr(:, 1), 1), data_sr(data_sr(:,1) <= upper_limit &  lower_limit <= data_sr(:, 1), col), 'k')
hold off
%plot(data_op(:,1), data_op(:,2), 'g')

%%
data_sit = 0
data_siv = 0
figure(1)
plot(data_op(data_op(:, 1) <= upper_limit, 1), data_op(data_op(:,1) <= upper_limit, col), 'b')
hold on
plot(data_cp(data_cp(:, 1) <= upper_limit, 1), data_cp(data_cp(:,1) <= upper_limit, col), 'r')
plot(data_et(data_et(:, 1) <= upper_limit, 1), data_et(data_et(:,1) <= upper_limit, col), 'g')
plot(data_sr(data_sr(:, 1) <= upper_limit & lower_limit <= data_sr(:, 1), 1), data_sr(data_sr(:,1) <= upper_limit &  lower_limit <= data_sr(:,1), 3), 'k')
plot(data_sr(data_sr(:, 1) <= upper_limit & lower_limit <= data_sr(:, 1), 1), data_sr(data_sr(:,1) <= upper_limit &  lower_limit <= data_sr(:,1), 4), 'k')
hold off

%%