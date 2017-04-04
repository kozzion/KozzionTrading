close all;
clear all;


%% create data set

feature_data = [0 0; 0 1; 1 0;1 1; 0.25 0.25; 0.75 0.75];
label_data   = [0.0 1.0 1.0 0.0 0.0 0.0]';


feature_data_gird = [repmat(0:0.1:1,1,11); reshape(repmat(0:0.1:1,11,1),[1,121])]'

%% test numeric
% generate random NN
count_0_0 = 2;
count_0_1 = 4;
count_1_2 = 1;
learning_rate = 0.1;
iter = 3000;

weigths_0 = (rand(count_0_0 + 1, count_0_1) * 2) - 1;
weigths_1 = (rand(count_0_1 + 1, count_1_2) * 2) - 1;


tic()
for i = 1:iter
    [weigths_0_gradient, weigths_1_gradient] = network_0_gradient_numeric_error(weigths_0, weigths_1, feature_data, label_data);
    weigths_0 = weigths_0 + weigths_0_gradient * learning_rate;
    weigths_1 = weigths_1 + weigths_1_gradient * learning_rate;
end
toc()

output_data_grid = network_0(weigths_0, weigths_1, feature_data_gird);
figure(3)
surf(reshape(output_data_grid,[11,11])) 


%% test backprop
% generate random NN
count_0_0 = 2;
count_0_1 = 4;
count_1_2 = 1;
learning_rate = 0.1;
iter = 300;

weigths_0 = (rand(count_0_0 + 1, count_0_1) * 2) - 1;
weigths_1 = (rand(count_0_1 + 1, count_1_2) * 2) - 1;

tic()
for i = 1:iter
   for j  = 1:10
    [weigths_0_gradient, weigths_1_gradient] = network_0_gradient_backprop_error(weigths_0, weigths_1, feature_data, label_data);
    weigths_0 = weigths_0 + (weigths_0_gradient * learning_rate);
    weigths_1 = weigths_1 + (weigths_1_gradient * learning_rate);
   end
    output_data_grid = network_0(weigths_0, weigths_1, feature_data_gird);
    figure(3)
    surf(reshape(output_data_grid,[11,11])) 
    pause(0.1)
end
toc()

output_data_grid = network_0(weigths_0, weigths_1, feature_data_gird);
figure(4)
surf(reshape(output_data_grid,[11,11])) 

%%
 [weigths_0_gradient_n, weigths_1_gradient_n] = network_0_gradient_numeric_error(weigths_0, weigths_1, feature_data, label_data)
 [weigths_0_gradient_b, weigths_1_gradient_b] = network_0_gradient_backprop_error(weigths_0, weigths_1, feature_data, label_data)
 weigths_0_gradient_n ./ weigths_0_gradient_b
 weigths_1_gradient_n ./ weigths_1_gradient_b