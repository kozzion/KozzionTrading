function [target] = f_linear_augment( source)
    target = zeros(size(source,1), size(source,2) + 1);
    target(:,1:end - 1) = source;
    target(:,end) = 1;
end

