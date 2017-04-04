function [ fitness ] =  fitness_corr(output_data, label_data)
fitness = corr(output_data, label_data);
end