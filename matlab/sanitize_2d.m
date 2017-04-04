function [ target_1, target_2 ] = sanitize_2d( source_1, source_2, percentile )
lower_1 = prctile(source_1, percentile);
upper_1 = prctile(source_1, 100 - percentile);
lower_2 = prctile(source_2, percentile);
upper_2 = prctile(source_2, 100 - percentile);
selection = (lower_1 <= source_1)  & logical(source_1 <= upper_1) & (lower_2 <= source_2) & (source_2 <= upper_2);
target_1 = source_1(selection);
target_2 = source_2(selection);
end

