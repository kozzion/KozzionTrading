function [ yields_all, yields_sum ] = simulate_1(output_data, label_data, lower_bound, upper_bound)

spread_point = 14;
short =  lower_bound < output_data;
long  =    output_data < upper_bound;

yields_short = label_data .* -short;
yields_long  = label_data .* long;

cost_short = (0.5 < conv(double(short),[1, -1])) * -spread_point;
cost_long  = (0.5 < conv(double(long), [1, -1])) * -spread_point;
cost_short = cost_short(1:end - 1);
cost_long  = cost_long(1:end - 1);
yields_all = yields_short + yields_long + cost_short + cost_long;
yields_sum = cumsum(yields_all);

end

