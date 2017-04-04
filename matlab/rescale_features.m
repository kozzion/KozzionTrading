function [feature_data_rescaled, slope, intercept] = rescale_features(feature_data, target_min, target_max)
min_value = min(feature_data(:));
max_value = max(feature_data(:));
intercept = min_value - target_min;
slope = (max_value - min_value) / (target_max - target_min);
feature_data_rescaled = (feature_data - intercept) / slope;


end

