function [ fitness ] =  fitness_yield( output_data, label_data)
[ yields_all, yields_sum ] = simulate_0( output_data, label_data);
fitness = yields_sum(end);
end

