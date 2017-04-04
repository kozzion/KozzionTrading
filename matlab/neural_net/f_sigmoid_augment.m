function [target] = f_sigmoid_augment( source)
    target = zeros(size(source,1), size(source,2) + 1);
    target(:,1:end - 1) = 1.0 ./ ( 1.0 + exp(-source));
    target(:,end) = 1;
end

