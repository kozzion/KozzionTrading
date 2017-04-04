%return;
%%
close all;
clear all;

%% load data
%[background, ~, source_image, sample_times, ~, ~] = load_data_from_file('C:\TestDataDicom\', 'Series', [1 1 1]);
background = zeros(256,256,34);
source_image = zeros(256,256,34, 24);
sample_times = 1:24
%% reduce data 
sample_count = size(sample_times, 2)
size(background)
source_image = source_image(225:256,225:256,1:32,:) + repmat(background(225:256,225:256,1:32,:), [1 1 1 sample_count]);
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
filter_kernel = zeros(21,21,21)
% filter_kernel  = zeros([kernel_size kernel_size kernel_size]);
% offset = ((kernel_size + 1) / 2);
% filter_kernel(offset, offset, offset) = 1;
size(filter_kernel)
size(source_image)
% Dikke bak : 1417 seconden op 37 bij 37 bij 37 kernel  (64 slices, 25 frames)
%compile
%%
system('nvcc -ptx cuda_filter_tips.cu');
%status = system('nvcc --compiler-bindir "Program Files (x86)\Microsoft Visual Studio 10.0\VC\bin\" --ptx cuda_filter_tips.cu ');
%%


% filter_kernel(
% float *target_image,  
% float const *const source_image, 	
% float const *const filter_kernel,  
% unsigned int const *const filter_size,	 
% unsigned int const *const real_grid_dimensions)

% Create CUDAKernel object.
reset(gpuDevice);
cuda_filter_big = parallel.gpu.CUDAKernel(...
               'cuda_filter_tips.ptx',...
               'cuda_filter_tips.cu',...
               'cuda_filter_tips');

% Set object properties.
disp('prepping')
tic();
real_grid_dimensions            = uint32([4 4 4]);
cuda_filter_big.ThreadBlockSize = [8 8 8]; % typically 1- 512 maybe more depending on card
cuda_filter_big.GridSize        = [16 4];

pad_size                        = (size(filter_kernel) -1) / 2;
padded_inclusion_image          = single(padarray(ones([size(source_image, 1) size(source_image, 2) size(source_image, 3)]),[pad_size 0]));
padded_source_image             = single(padarray(source_image, [pad_size 0]));
filter_kernel                   = single(filter_kernel);
size(source_image)
size(padded_source_image)
% dimensions layout: 
% 1-3 target size               (incremental)
% 4-6 source and inclusion size (incremental)
% 7-9 filter size               (not incremental)
% 10  sample_count
dimensions = zeros(10,1);

dimensions(1,1)  = (kernel_size - 1) / 2;
dimensions(2,1)  = (kernel_size - 1) / 2;
dimensions(3,1)  = (kernel_size - 1) / 2;

dimensions(4,1)  = size(padded_source_image, 1);
dimensions(5,1)  = size(padded_source_image, 2) * dimensions(4,1);
dimensions(6,1)  = size(padded_source_image, 3) * dimensions(5,1);

dimensions(7,1)  = size(filter_kernel, 1);
dimensions(8,1)  = size(filter_kernel, 2);
dimensions(9,1)  = size(filter_kernel, 3);

dimensions(10,1) = size(source_image, 4);

dimensions = int32(dimensions);
toc();

%

% cuda_filter_big.ArgumentTypes
% this is usefull: filter_kernel.ArgumentTypes


% Call feval with defined inputs.
disp('copy to is next')
tic();


gpu_target_image     =  gpuArray.zeros(size(padded_source_image), 'single'); % Output gpuArray.
gpu_source_image     =  gpuArray(padded_source_image);
gpu_inclusion_image  =  gpuArray(padded_inclusion_image);
gpu_filter_kernel    =  gpuArray(filter_kernel);
gpu_filter_size      =  gpuArray(dimensions); 
gpu_max_error        =  gpuArray(max_error); 
gpu_real_grid_dimensions = gpuArray(real_grid_dimensions); 
wait(gpuDevice);
toc();

%
disp('run is next')
tic();
gpu_target_image = cuda_filter_big.feval(...
        gpu_target_image, ...
        gpu_source_image, ...
        gpu_inclusion_image, ...
        gpu_filter_kernel, ...
        gpu_filter_size, ...
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


%

close all
size(source_image)
size(result_cuda)
slice = 32;
time  = 25;
figure('Position', [300, 500, 100, 100])
imshow(source_image          (:,:,1,  1), [0 100]);
figure('Position', [500, 500, 100, 100])
imshow(padded_source_image   (:,:,16, 1),[0 100]);
figure('Position', [700, 500, 100, 100])
imshow(padded_inclusion_image(:,:,16),   [0 1]);
figure('Position', [900, 500, 100, 100])
imshow(result_cuda(:,:,16,1),   [0 100]);
%figure(2)
%imshow(result_matlab(:,:,slice), [0 200]);


