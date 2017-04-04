%return;
%%
close all;
clear all;

%% load data
[background, ~, source_image, sample_times, ~, ~] = load_data_from_file('C:\TestDataDicom\', 'Series', [1 1 1]);

%% reduce data 
sample_count = size(sample_times, 2);
size(background)
source_image = source_image + repmat(background, [1 1 1 sample_count]);
input_image = source_image(225:256,225:256,1:32,:);
size(source_image)
numel(source_image)
%% matlab version
kernel_size = 31;
max_error   = single(200);


disp('matlab');
tic()
%result_matlab = convn(source_image, filter_kernel, 'same');
toc();
%%

%% cuda version
filter_kernel = fspecial3('gaussian', kernel_size);
% filter_kernel  = zeros([kernel_size kernel_size kernel_size]);
% offset = ((kernel_size + 1) / 2);
% filter_kernel(offset, offset, offset) = 1;
% Dikke bak : 1417 seconden op 37 bij 37 bij 37 kernel  (64 slices, 25 frames)
tic();
compile

% filter_kernel(
% float *target_image,  
% float const *const source_image, 	
% float const *const filter_kernel,  
% unsigned int const *const filter_size,	 
% unsigned int const *const real_grid_dimensions)

% Create CUDAKernel object.
reset(gpuDevice);
cuda_filter_big = parallel.gpu.CUDAKernel(...
               'cuda_filter_tips_fast.ptx',...
               'cuda_filter_tips_fast.cu',...
               'cuda_filter_tips_fast');

% Set object properties.
disp('prepping')

real_grid_dimensions            = uint32([4 4 4]);
cuda_filter_big.ThreadBlockSize = [8 8 8]; % typically 1- 512 maybe more depending on card
cuda_filter_big.GridSize        = [16 4];

pad_size                        = (size(filter_kernel) -1) / 2;
padded_inclusion_image          = single(padarray(ones([size(input_image, 1) size(input_image, 2) size(input_image, 3)]),[pad_size 0]));
padded_input_image              = single(padarray(input_image, [pad_size 0]));
filter_kernel                   = single(filter_kernel);
size(input_image)
size(padded_input_image)
% dimensions layout: 
% 1-3 target size               (incremental)
% 4-6 source and inclusion size (incremental)
% 7-9 filter size               (not incremental)
% 10  sample_count
dimensions = zeros(10,1);

dimensions(1,1)  = (kernel_size - 1) / 2;
dimensions(2,1)  = (kernel_size - 1) / 2;
dimensions(3,1)  = (kernel_size - 1) / 2;

dimensions(4,1)  = size(padded_input_image, 1);
dimensions(5,1)  = size(padded_input_image, 2) * dimensions(4,1);
dimensions(6,1)  = size(padded_input_image, 3) * dimensions(5,1);


dimensions(10,1) = size(source_image, 4);


toc();

%
input_offsets  = zeros(size(filter_kernel));
relative_offset = ((kernel_size - 1) / 2);
for index_x = 1:kernel_size
    for index_y = 1:kernel_size
        for index_z = 1:kernel_size
            relative_x = index_x - relative_offset;
            relative_y = index_y - relative_offset;
            relative_z = index_z - relative_offset;

            input_offsets(index_x, index_y, index_z) =  relative_x + ...
                                                       (relative_y * dimensions(4,1)) + ...
                                                       (relative_z * dimensions(5,1));          
        end
    end
end
%
rim = filter_kernel(1,16,16)

input_offsets = int32(input_offsets);
input_offsets = input_offsets(rim < filter_kernel);
filter_kernel = filter_kernel(rim < filter_kernel);
dimensions(7,1)  = numel(filter_kernel);
dimensions = int32(dimensions);

a = size(input_offsets)
b = size(filter_kernel)
% cuda_filter_big.ArgumentTypes
% this is usefull: filter_kernel.ArgumentTypes


% Call feval with defined inputs.
disp('copy to is next')
tic();


gpu_target_image     =  gpuArray.zeros(size(padded_input_image), 'single'); % Output gpuArray.
gpu_input_image      =  gpuArray(padded_input_image);
gpu_inclusion_image  =  gpuArray(padded_inclusion_image);
gpu_filter_kernel    =  gpuArray(filter_kernel);
gpu_dimensions       =  gpuArray(dimensions); 
gpu_input_offsets    =  gpuArray(input_offsets); 
gpu_max_error        =  gpuArray(max_error); 
gpu_real_grid_dimensions = gpuArray(real_grid_dimensions); 
wait(gpuDevice);
toc();

%
disp('run is next')
tic();
gpu_target_image = cuda_filter_big.feval(...
        gpu_target_image, ...
        gpu_input_image, ...
        gpu_inclusion_image, ...
        gpu_filter_kernel, ...
        gpu_dimensions, ...
        gpu_input_offsets, ...
        gpu_max_error,...
        gpu_real_grid_dimensions);
wait(gpuDevice);
toc();

%
disp('copy back is next')
tic();
result_cuda = gather(gpu_target_image);
wait(gpuDevice);
toc();


%%

close all
size(source_image)
size(result_cuda)
slice = 32;
time  = 25;
figure('Position', [300, 500, 100, 100])
imshow(input_image          (:,:,1,  1), [0 100]);
figure('Position', [500, 500, 100, 100])
imshow(padded_input_image   (:,:,16, 1),[0 100]);
figure('Position', [700, 500, 100, 100])
imshow(padded_inclusion_image(:,:,16),   [0 1]);
figure('Position', [900, 500, 100, 100])
imshow(result_cuda(:,:,16,1),   [0 100]);
%figure(2)
%imshow(result_matlab(:,:,slice), [0 200]);


