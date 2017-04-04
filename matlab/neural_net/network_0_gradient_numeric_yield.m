function [weigths_0_gradient, weigths_1_gradient] = network_0_gradient_numeric_yield(weigths_0, weigths_1, feature_data, label_data)
epsilon = 0.0001;

fitness  =  fitness_yield(network_0( weigths_0, weigths_1, feature_data), label_data);
weigths_0_gradient = zeros(size(weigths_0));
weigths_1_gradient = zeros(size(weigths_1));

for weight_0_index = 1:numel(weigths_0)
    weigths_0_new = weigths_0;
    weigths_0_new(weight_0_index) = weigths_0_new(weight_0_index) + epsilon;
    fitness_new =  fitness_yield(network_0( weigths_0_new, weigths_1, feature_data), label_data);
    weigths_0_gradient(weight_0_index) = (fitness_new - fitness) / epsilon;
end

for weight_1_index = 1:numel(weigths_1)
    weigths_1_new = weigths_1;
    weigths_1_new(weight_1_index) = weigths_1_new(weight_1_index) + epsilon;
    fitness_new =  fitness_yield(network_0( weigths_0, weigths_1_new, feature_data), label_data);
    weigths_1_gradient(weight_1_index) = (fitness_new - fitness) / epsilon;
end