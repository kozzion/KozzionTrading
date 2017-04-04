function [feature_values] = feature_generator_0( source_data, time_index, history_size)
    point = 0.00001;
    if(time_index < history_size + 1)
        feature_values = zeros(1,history_size);
    else
        feature_values = (source_data((time_index - history_size + 1) : time_index, 1) - source_data(time_index, 4)) / point;
    end
end

