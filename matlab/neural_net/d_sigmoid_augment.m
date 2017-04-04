function [ target ] = d_sigmoid_augment( source )
%D_SIGMOID_AUGMENT Summary of this function goes here
%   Detailed explanation goes here
    target = 1.0 ./ ( 1.0 + exp(-source)).* (1 - 1.0 ./ ( 1.0 + exp(-source)));

end

