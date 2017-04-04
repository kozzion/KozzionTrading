function [weigths_0_gradient, weigths_1_gradient] = network_0_gradient_backprop_error(weigths_0, weigths_1, feature_data, label_data)
%   Detailed explanation goes here
input_0   = f_linear_augment(feature_data);
output_0  = input_0 * weigths_0;
input_1   = f_sigmoid_augment(output_0);
output_1  = input_1 * weigths_1;
input_2   = f_linear_augment(output_1);
output_data = input_2(:,1:end-1);


input_2_error      = d_linear_augment(output_1) .* (label_data - output_data);
input_1_error      = d_sigmoid_augment(output_0) .*  (input_2_error * weigths_1(1:end -1)');

weigths_1_gradient = input_1'  * input_2_error;
weigths_0_gradient = input_0'  * input_1_error;