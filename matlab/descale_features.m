function feature_data = descale_features(feature_data_rescaled, slope, intercept)
feature_data = (feature_data_rescaled * slope) + intercept;

end

