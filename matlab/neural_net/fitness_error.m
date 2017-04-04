function [ fitness ] =  fitness_error(output_data, label_data)
fitness = -mean(abs(output_data - label_data));
end