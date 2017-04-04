function [ output_data, input_0, output_0, input_1, output_1] = network_0( weigths_0, weigths_1, feature_data)
input_0   = f_linear_augment(feature_data);
output_0  = input_0 * weigths_0;
input_1   = f_sigmoid_augment(output_0);
output_1  = input_1 * weigths_1;
input_2   = f_linear_augment(output_1);
output_data = input_2(:,1:end-1);
end

